namespace DiscordBot;

public class RollDiceCommand
{
    public int DiceCount { get; init; }
    public int DiceType { get; init; }
    public int DicesToKeep { get; init; }
    public int Modifier { get; init; }
    public bool ValidCommand { get; set; }
    public required string Command { get; init; }
}
