using System.Text.RegularExpressions;

namespace DiscordBot;

public static class RollDiceCommandFactory
{
    public static List<RollDiceCommand> GetRollDiceCommands(string input)
    {
        var rollDiceCommands = new List<RollDiceCommand>();
        var commandSegments = input.Replace(" ","").Split('&');
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
            };
            
            rollDiceCommands.Add(rollDiceCommand);
        }

        return rollDiceCommands;
    } 
    
}