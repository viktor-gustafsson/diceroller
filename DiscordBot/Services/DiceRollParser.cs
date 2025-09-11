using System.Text.RegularExpressions;
using DiscordBot.Models;

namespace DiscordBot.Services;

public static class DiceRollParser
{
    public static DiceRollRequest Parse(MessageDto messageDto)
    {
        var hiddenRoll = messageDto.Command.Contains("hidden");
        messageDto.Command = messageDto.Command.Replace("hidden", "");
        
        var rollDiceCommands = new List<RollDiceCommand>();
        var commandSegments = messageDto.Command.Replace(" ","").Split('&');
        foreach (var commandSegment in commandSegments)
        {
            var command = commandSegment;
            var modifier = 0;
            var modifierMatch = Regex.Match(command, @"[+-]\d+$");
            if (modifierMatch.Success)
            {
                modifier = int.Parse(modifierMatch.Value);
                command = command[..^modifierMatch.Value.Length];
            }

            var parts = command.ToLower().Split('d', 'k', 'h', 'l');
            
            var rollDiceCommand = new RollDiceCommand
            {
                DiceCount = int.Parse(parts[0]),
                DiceType = int.Parse(parts[1]),
                DicesToKeep = parts.Length > 2 ? int.Parse(parts[2]) : int.Parse(parts[0]),
                Modifier = modifier,
                Command = command,
                ValidCommand = parts.Length < 2,
                Rolls = new int[int.Parse(parts[0])],
                KeepHigh = !command.Contains('l'),
                UserDisplayName = messageDto.UserDisplayName,
            };
            
            rollDiceCommands.Add(rollDiceCommand);
        }

        var rollDiceCommandWrapper = new DiceRollRequest
        {
            Commands = rollDiceCommands,
            HiddenRoll = hiddenRoll,
        };

        return rollDiceCommandWrapper;
    } 
    
}