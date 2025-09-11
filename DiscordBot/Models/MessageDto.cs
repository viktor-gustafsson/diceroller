namespace DiscordBot.Models;

public class MessageDto
{
    public required string UserDisplayName { get; init; }
    public required string Command { get; set; }
}