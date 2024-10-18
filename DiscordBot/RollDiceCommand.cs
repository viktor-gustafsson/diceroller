using System.Text.RegularExpressions;

namespace DiscordBot;

public class RollDiceCommand
{
    public int DiceCount { get; set; }
    public int DiceType { get; set; }
    public int DicesToKeep { get; set; }
    public int Modifier { get; set; }
    public bool ValidCommand { get; set; }
    public string Command { get; set; }

    public RollDiceCommand(string command)
    {
        var modifier = 0;
        var modifierMatch = Regex.Match(command, @"[+-]\d+$");
        if (modifierMatch.Success)
        {
            modifier = int.Parse(modifierMatch.Value);
            command = command[..^modifierMatch.Value.Length];
        }

        var parts = command.ToLower().Split('d', 'k', 'h', 'l');
        
        DiceCount = int.Parse(parts[0]);
        DiceType = int.Parse(parts[1]);
        DicesToKeep = parts.Length > 2 ? int.Parse(parts[2]) : DiceCount;
        Modifier = modifier;
        Command = command;
        
        if (parts.Length < 2) 
            ValidCommand = false;
    }
}
