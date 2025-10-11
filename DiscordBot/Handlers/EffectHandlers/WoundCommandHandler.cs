using Discord.WebSocket;
using DiscordBot.Rollers.EffectRollers;

namespace DiscordBot.Handlers.EffectHandlers;

public static class WoundCommandHandler
{
    public static async Task Handle(SocketSlashCommand command)
    {
        var rollWound = WoundRoller.Roll();
        await command.RespondAsync(rollWound, ephemeral: false);
    }
}