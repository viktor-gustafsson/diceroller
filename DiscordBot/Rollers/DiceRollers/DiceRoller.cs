using System.Text;
using DiscordBot.Handlers;
using DiscordBot.Models;
using DiscordBot.Parsers;

namespace DiscordBot.Rollers.DiceRollers;

public static class DiceRoller
{
    public static DiceRollResult ParseAndRollDice(MessageDto messageDto)
    {
        try
        {
            var sb = new StringBuilder();

            var rollDiceCommands = DiceRollParser.Parse(messageDto);
            foreach (var rollDiceCommand in rollDiceCommands)
            {
                if (!rollDiceCommand.IsValid)
                    return new DiceRollResult(ErrorMessages.InvalidRollCommand, []);

                // Roll the dice
                for (var i = 0; i < rollDiceCommand.DiceCount; i++)
                {
                    rollDiceCommand.Rolls[i] = Random.Shared.Next(1, rollDiceCommand.DiceType + 1);
                }

                sb.Append(DiceRollerMessages.GetResultMessage(rollDiceCommand, messageDto.HiddenDice));
            }

            return new DiceRollResult(sb.ToString(), rollDiceCommands);
        }
        catch (Exception)
        {
            return new DiceRollResult(ErrorMessages.InvalidRollCommand, []);
        }
    }
}
