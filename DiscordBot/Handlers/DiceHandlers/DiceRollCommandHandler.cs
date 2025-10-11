using Discord.WebSocket;
using DiscordBot.Models;
using DiscordBot.Rollers.DiceRollers;

namespace DiscordBot.Handlers.DiceHandlers;

public static class DiceRollCommandHandler
{
    public static async Task Handle(SocketSlashCommand command)
    {
        var userGlobalName = (command.User as SocketGuildUser)?.DisplayName;
        var diceOption = command.Data.Options.First(x => x.Name == Constants.DiceOptionName).Value.ToString();
        var response = DiceRoller.ParseAndRollDice(new MessageDto
        {
            Command = diceOption!,
            UserDisplayName = userGlobalName!,
            HiddenDice = false,
        });
        await command.RespondAsync(response, ephemeral: false);
    }
}