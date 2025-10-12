using Discord.WebSocket;
using DiscordBot.Rollers.EffectRollers;

namespace DiscordBot.Handlers;

public abstract class EffectCommandHandler
{
    public static async Task Handle<TEffectRoller>(SocketSlashCommand command) where TEffectRoller : IEffectRoller
    {
        var effect = TEffectRoller.Roll();
        await command.RespondAsync(effect, ephemeral: false);
    }
}