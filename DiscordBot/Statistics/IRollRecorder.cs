using Discord.WebSocket;
using DiscordBot.Models;

namespace DiscordBot.Statistics;

public interface IRollRecorder
{
    Task RecordStandardRollsAsync(SocketSlashCommand command, IReadOnlyList<RollDiceCommand> rolls, string resultText);
    Task RecordEffectRollAsync(SocketSlashCommand command, string subType, string resultText);
    Task RecordCharacterRollAsync(SocketSlashCommand command, string subType, string resultText);
}
