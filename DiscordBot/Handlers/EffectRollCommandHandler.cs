using Discord.WebSocket;
using DiscordBot.Rollers.EffectRollers;

namespace DiscordBot.Handlers;

public abstract class EffectRollCommandHandler
{
    public static async Task Handle<T>(SocketSlashCommand command) where T : IEffectRoller
    {
        var effect = T.Roll();
        await command.RespondAsync(effect, ephemeral: false);
    }
}