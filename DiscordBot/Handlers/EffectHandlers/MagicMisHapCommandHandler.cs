using Discord.WebSocket;
using DiscordBot.Rollers.EffectRollers;

namespace DiscordBot.Handlers.EffectHandlers;

public static class MagicMisHapCommandHandler
{
    public static async Task Handle(SocketSlashCommand command)
    {
        var rollMagicMisHap = MagicMisHapRoller.Roll();
        await command.RespondAsync(rollMagicMisHap, ephemeral: false);
    }
}