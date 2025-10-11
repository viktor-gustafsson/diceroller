using Discord.WebSocket;
using DiscordBot.Rollers.EffectRollers;

namespace DiscordBot.Handlers.EffectHandlers;

public static class DevilsLuckCommandHandler
{
    public static async Task Handle(SocketSlashCommand command)
    {
        var rollDevilsLuck = DevilsLuckRoller.Roll();
        await command.RespondAsync(rollDevilsLuck, ephemeral: false);
    }
}