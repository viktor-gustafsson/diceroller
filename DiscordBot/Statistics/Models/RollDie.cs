namespace DiscordBot.Statistics.Models;

public class RollDie
{
    public long Id { get; set; }
    public long RollId { get; set; }
    public int DieType { get; set; }
    public int Value { get; set; }
    public bool Kept { get; set; }
    public int Position { get; set; }
}
