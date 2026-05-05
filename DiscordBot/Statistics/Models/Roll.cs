namespace DiscordBot.Statistics.Models;

public class Roll
{
    public long Id { get; set; }
    public ulong? GuildId { get; set; }
    public ulong ChannelId { get; set; }
    public ulong UserId { get; set; }
    public required string UserDisplayName { get; set; }
    public RollType RollType { get; set; }
    public string? RollSubType { get; set; }
    public required string Command { get; set; }
    public int? Total { get; set; }
    public int? Modifier { get; set; }
    public required string ResultText { get; set; }
    public DateTime Timestamp { get; set; }

    public List<RollDie> Dice { get; set; } = [];
}
