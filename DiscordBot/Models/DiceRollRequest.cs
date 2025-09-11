namespace DiscordBot.Models;

public class DiceRollRequest
{
    public bool HiddenRoll { get; init; }
    public List<RollDiceCommand> Commands { get; init; } = [];
}