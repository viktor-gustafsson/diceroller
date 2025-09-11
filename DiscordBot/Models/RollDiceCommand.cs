namespace DiscordBot.Models;

public class RollDiceCommand
{
    public int DiceCount { get; init; }
    public int DiceType { get; init; }
    public int DicesToKeep { get; init; }
    public int Modifier { get; init; }
    public bool ValidCommand { get; init; }
    public required string Command { get; init; }
    public int[] Rolls { get; init; } = [];
    public bool KeepHigh { get; init; }
    public required string UserDisplayName { get; init; }
    
    /// <summary>
    /// Retrieves the list of dice results to be kept based on the number specified by DicesToKeep.
    /// If the KeepHigh property is true, keeps the highest values; otherwise, keeps the lowest values.
    /// </summary>
    /// <returns>A list of integers representing the dice results to be kept.</returns>
    public List<int> GetKeptDice() =>
        KeepHigh
            ? Rolls.OrderByDescending(x => x).Take(DicesToKeep).ToList()
            : Rolls.OrderBy(x => x).Take(DicesToKeep).ToList();

    public int GetTotal() => GetKeptDice().Sum() + Modifier;
}
