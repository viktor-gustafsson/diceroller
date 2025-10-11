using Discord.WebSocket;
using DiscordBot.Models;
using DiscordBot.Rollers.DiceRollers;

namespace DiscordBot.Handlers.DiceHandlers;

public static class HiddenDiceRollCommandHandler
{
    public static async Task Handle(SocketSlashCommand command)
    {
        var userGlobalName = (command.User as SocketGuildUser)?.DisplayName;
        var diceOption = command.Data.Options.First(x => x.Name == Constants.HiddenDiceOptionName).Value.ToString();
        var response = DiceRoller.ParseAndRollDice(new MessageDto
        {
            Command = diceOption!,
            UserDisplayName = userGlobalName!,
            HiddenDice = true,
        });
        await command.RespondAsync(response, ephemeral: true);
    }
}