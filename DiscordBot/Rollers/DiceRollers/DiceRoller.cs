using System.Text;
using DiscordBot.Handlers;
using DiscordBot.Models;
using DiscordBot.Services;

namespace DiscordBot.Rollers.DiceRollers;

public static class DiceRoller
{
    public static string ParseAndRollDice(MessageDto messageDto)
    {
        try
        {
            var sb = new StringBuilder();

            var rollDiceCommands = DiceRollParser.Parse(messageDto);
            foreach (var rollDiceCommand in rollDiceCommands)
            {
                if (rollDiceCommand.ValidCommand)
                    return ErrorMessages.InvalidRollCommand;

                // Roll the dice
                for (var i = 0; i < rollDiceCommand.DiceCount; i++)
                {
                    rollDiceCommand.Rolls[i] = Random.Shared.Next(1, rollDiceCommand.DiceType + 1);
                }

                sb.Append(DiceRollerMessages.GetResultMessage(rollDiceCommand, messageDto.HiddenDice));
            }
                
            return sb.ToString();
        }
        catch (Exception)
        {
            return ErrorMessages.InvalidRollCommand;
        }
    }
}