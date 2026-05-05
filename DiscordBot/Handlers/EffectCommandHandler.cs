using Discord.WebSocket;
using DiscordBot.Rollers.EffectRollers;
using DiscordBot.Statistics;

namespace DiscordBot.Handlers;

public abstract class EffectCommandHandler
{
    public static async Task Handle<TEffectRoller>(SocketSlashCommand command, IRollRecorder recorder)
        where TEffectRoller : IEffectRoller
    {
        var effect = TEffectRoller.Roll();
        await command.RespondAsync(effect, ephemeral: false);
        await recorder.RecordEffectRollAsync(command, typeof(TEffectRoller).Name, effect);
    }
}
