using Discord.WebSocket;
using DiscordBot.Rollers.DiceRollers;

namespace DiscordBot.Handlers;

public static class HelpCommandHandler
{
    public static async Task Handle(SocketSlashCommand command)
    {
        var helpMessage = DiceRollerMessages.GetHelpMessage();
        await command.RespondAsync(helpMessage, ephemeral: true);
    }
}