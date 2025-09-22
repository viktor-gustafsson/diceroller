using DiscordBot.Models;
using DiscordBot.Rollers.DiceRollers;
using DiscordBot.Services;
using Shouldly;

namespace DiscordBot.Tests;

public class DiceRollParserTests
{
    [Fact]
    public void Parses_SingleCommand_WithKeepHighAndPositiveModifier()
    {
        // Arrange
        var input = "4d6k3h+2";

        // Act
        var diceRollRequest = DiceRollParser.Parse(new MessageDto
        {
            Command = input,
            UserDisplayName = "foobar",
        });

        // Assert
        diceRollRequest.Count.ShouldBe(1);
        var cmd = diceRollRequest.Single();
        cmd.DiceCount.ShouldBe(4);
        cmd.DiceType.ShouldBe(6);
        cmd.DicesToKeep.ShouldBe(3);
        cmd.Modifier.ShouldBe(2);
        cmd.Command.ShouldBe("4d6k3h");
        cmd.ValidCommand.ShouldBeFalse("parts length should be >= 2 for a valid command");
    }

    [Fact]
    public void Parses_SingleCommand_KeepLowest_WithNegativeModifier()
    {
        // Arrange
        var input = "6d6k2l-1";

        // Act
        var diceRollRequest = DiceRollParser.Parse(new MessageDto
        {
            Command = input,
            UserDisplayName = "foobar",
        });

        // Assert
        diceRollRequest.Count.ShouldBe(1);
        var cmd = diceRollRequest.Single();
        cmd.DiceCount.ShouldBe(6);
        cmd.DiceType.ShouldBe(6);
        cmd.DicesToKeep.ShouldBe(2);
        cmd.Modifier.ShouldBe(-1);
        cmd.Command.ShouldBe("6d6k2l");
        cmd.ValidCommand.ShouldBeFalse();
    }

    [Fact]
    public void Parses_ChainedCommands_WithSpaces()
    {
        // Arrange
        var input = "3d20 & 2d6k1l+5";

        // Act
        var diceRollRequest = DiceRollParser.Parse(new MessageDto
        {
            Command = input,
            UserDisplayName = "foobar",
        });

        // Assert
        diceRollRequest.Count.ShouldBe(2);

        var first = diceRollRequest[0];
        first.DiceCount.ShouldBe(3);
        first.DiceType.ShouldBe(20);
        first.DicesToKeep.ShouldBe(3, "default keep is dice count when k is not provided");
        first.Modifier.ShouldBe(0);
        first.Command.ShouldBe("3d20");
        first.ValidCommand.ShouldBeFalse();

        var second = diceRollRequest[1];
        second.DiceCount.ShouldBe(2);
        second.DiceType.ShouldBe(6);
        second.DicesToKeep.ShouldBe(1);
        second.Modifier.ShouldBe(5);
        second.Command.ShouldBe("2d6k1l");
        second.ValidCommand.ShouldBeFalse();
    }

    [Theory]
    [InlineData("3d")]        // Missing dice type details to parse into parts[1]
    [InlineData("d20")]       // Missing dice count
    [InlineData("3")]         // Missing 'd' part
    [InlineData("xyz")]       // Nonsense
    public void Marks_Invalid_When_PartsLengthLessThan2(string input)
    {
        Should.Throw<Exception>(() => DiceRollParser.Parse(new MessageDto
        {
            Command = input,
            UserDisplayName = "foobar",
        }));
    }
}

public class MessagesFormattingTests
{
    [Fact]
    public void ResultMessage_DefaultKeepHighest_NoModifier_HidesModifierAndKeepSection()
    {
        // Arrange
        var user = "PlayerOne";
        var roll = new RollDiceCommand
        {
            DiceCount = 4,
            DiceType = 6,
            DicesToKeep = 3,
            Modifier = 0,
            Command = "4d6k3h",
            ValidCommand = false,
            KeepHigh = true,
            Rolls = [1, 2, 3, 4],
            UserDisplayName = user,
        };

        // Act
        var msg = DiceRollerMessages.GetResultMessage(roll, false);

        // Assert (structure and key content, not exact formatting)
        msg.ShouldContain("```");
        msg.ShouldContain(user);
        msg.ShouldContain("🎯 Rolling: 4 d6");
        msg.ShouldContain("🛡️ Keeping: Highest 3 dice");
        msg.ShouldContain("🎲 You rolled: [ 1, 2, 3, 4 ]");

        // No explicit "💾 Keeping:" section when keeping all dice would be hidden,
        // but here we keep 3 of 4, so it should be present:
        msg.ShouldContain("💾 Keeping: [ 4, 3, 2 ]");

        // No modifier line displayed when modifier == 0
        msg.ShouldNotContain("🟦 Modifier:");

        // Sum line with no modifier should not include "= total"
        msg.ShouldSatisfyAllConditions(s =>
        {
            s.ShouldContain("✨ Sum: 9");
            s.ShouldNotContain(" = ");
        });
    }

    [Fact]
    public void ResultMessage_KeepLowest_WithPositiveModifier_ShowsModifier_And_Equation()
    {
        // Arrange
        var user = "PlayerTwo";
        var roll = new RollDiceCommand
        {
            DiceCount = 3,
            DiceType = 20,
            DicesToKeep = 1,
            Modifier = 2,
            Command = "3d20k1l+2",
            ValidCommand = false,
            KeepHigh = false,
            Rolls = [5, 7, 10],
            UserDisplayName = user,
        };

        // Act
        var msg = DiceRollerMessages.GetResultMessage(roll, false);

        // Assert
        msg.ShouldContain("```");
        msg.ShouldContain(user);
        msg.ShouldContain("🎯 Rolling: 3 d20");
        msg.ShouldContain("🛡️ Keeping: Lowest 1 dice");
        msg.ShouldContain("🎲 You rolled: [ 5, 7, 10 ]");
        msg.ShouldContain("💾 Keeping: [ 5 ]");

        // Modifier line shown and equation present
        msg.ShouldContain("🟦 Modifier: [ 2 ]");
        msg.ShouldContain("✨ Sum: 5+2 = 7");
    }

    [Fact]
    public void HelpMessage_Contains_CoreSections()
    {
        // Act
        var help = DiceRollerMessages.GetHelpMessage();

        // Assert
        help.ShouldContain("Basic Command");
        help.ShouldContain("Optional Extras");
        help.ShouldContain("Examples");
        help.ShouldContain("Chain Dice Examples");
        help.ShouldContain("Notes");
        help.ShouldContain("/roll 3d20");
        help.ShouldContain("/roll 4d6k3h");
        help.ShouldContain("/roll 6d6k2l+2");
    }
}