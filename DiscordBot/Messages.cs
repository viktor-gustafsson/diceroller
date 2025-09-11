namespace DiscordBot;

public static class Messages
{
    public static string GetResultMessage(RollDiceCommand rollDiceCommand, bool hiddenRoll)
    {
        return
            $"```" +
            $"\n{GetHiddenMessage(hiddenRoll)}\n{rollDiceCommand.UserDisplayName} \n{GetRollingMessage(rollDiceCommand)}\n{GetDiceNumberToKeepMessage(rollDiceCommand)}{GetModifierMessage(rollDiceCommand)}\n{GetRollsMessage(rollDiceCommand)}{GetKeepMessage(rollDiceCommand)}\n{GetSumMessage(rollDiceCommand)}" +
            $"```";
    }
    
    public static string GetHelpMessage() =>
        " \n" +
        "### Basic Command: `/roll [number_of_dice]d[number_of_sides]`\n" +
        " - **number_of_dice**: The number of dice to roll.\n" +
        " - **number_of_sides**: The number of sides on each dice (e.g., `d4`, `d6`, `d8`, `d10`, `d12`, `d20`, `d100`).\n" +
        "   - Supports non-standard dice as well, such as `d3`, `d5`, `d7`, `d9`, etc.\n" +
        "### Hidden rolls:\n" +
        "   - Append '**hidden**' to your roll command to get a roll only visible to you\n" +
        "### Optional Extras:\n" +
        " - **k[number_to_keep]**: Keep only the highest or lowest dice.\n" +
        "   - Add `h` to keep the highest (default).\n" +
        "   - Add `l` to keep the lowest.\n" +
        " - **+/-[modifier]**: Add or subtract a number to the total roll.\n" +
        "### Examples:\n" +
        " - `/roll 3d20`: Roll 3 d20 dice.\n" +
        " - `/roll 3d20 hidden`: Roll 3 d20 dice. With hidden result\n" +
        " - `/roll 4d6k3h`: Roll 4 d6 dice and keep the highest 3.\n" +
        " - `/roll 6d6k2l+2`: Roll 6 d6 dice, keep the lowest 2, and add 2 to the result.\n" +
        "### Chain Dice Examples:\n" +
        " - `/roll 3d20 & 2d6`: Roll 3 d20 dice and 2 d6 dice\n" +
        " - `/roll 3d20 & 2d6k1l`: Roll 3 d20 dice and 2 d6 dice keep the lowest 1 dice\n" +
        " - `/roll 3d20 & 2d6k1l & 5d10k2h+5`: Roll 3 d20 dice and 2 d6 dice keep the lowest 1 dice and 5 d10 dice keep 2 highest dice and add +5 modifier\n" +
        "### Notes:\n" +
        " - If you don't specify `h` or `l`, the bot will keep the highest dice by default.\n" +
        " - You can use modifiers like `+5` or `-3` to adjust the total after rolling.";

    private static string GetHiddenMessage(bool hiddenRoll)
    {
       return hiddenRoll ? "🔒 Hidden roll: only you can see this message." : "";
    }
    private static string GetModifierMessage(RollDiceCommand rollDiceCommand)
    {
        return rollDiceCommand.Modifier == 0 ? "" : $"\n\ud83d\udfe6 Modifier: [ {rollDiceCommand.Modifier} ]";
    }

    private static string GetKeepMessage(RollDiceCommand rollDiceCommand)
    {
        return rollDiceCommand.DiceCount == rollDiceCommand.DicesToKeep
            ? ""
            : $"\n\ud83d\udcbe Keeping: [ {string.Join(", ", rollDiceCommand.GetKeptDice())} ]";
    }

    private static string GetSumMessage(RollDiceCommand rollDiceCommand)
    {
        var sumOfKeptDice = rollDiceCommand.GetKeptDice().Sum();

        var modifierMessage = rollDiceCommand.Modifier != 0
            ? $"{(rollDiceCommand.Modifier > 0 ? "+" : "")}{rollDiceCommand.Modifier}"
            : "";

        return rollDiceCommand.Modifier != 0
            ? $"\u2728 Sum: {sumOfKeptDice}{modifierMessage} = {rollDiceCommand.GetTotal()}"
            : $"\u2728 Sum: {sumOfKeptDice}{modifierMessage}";
    }

    private static string GetRollsMessage(RollDiceCommand rollDiceCommand)
        => $"\ud83c\udfb2 You rolled: [ {string.Join(", ", rollDiceCommand.Rolls)} ]";

    private static string GetDiceNumberToKeepMessage(RollDiceCommand rollDiceCommand) =>
        rollDiceCommand.KeepHigh switch
        {
            true => $"\ud83d\udee1\ufe0f Keeping: Highest {rollDiceCommand.DicesToKeep} dice",
            false => $"\ud83d\udee1\ufe0f Keeping: Lowest {rollDiceCommand.DicesToKeep} dice",
        };

    private static string GetRollingMessage(RollDiceCommand rollDiceCommand)
        => $"\ud83c\udfaf Rolling: {rollDiceCommand.DiceCount} d{rollDiceCommand.DiceType}";
}