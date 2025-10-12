using Discord.WebSocket;
using DiscordBot.Models;
using DiscordBot.Rollers.DiceRollers;

namespace DiscordBot.Handlers;

public static class DiceCommandHandler
{
    public static async Task Handle(SocketSlashCommand command, bool hidden)
    {
        var userGlobalName = (command.User as SocketGuildUser)?.DisplayName;
        
        var diceOption = hidden
            ? command.Data.Options.First(x => x.Name == Constants.HiddenDiceOptionName).Value.ToString()
            : command.Data.Options.First(x => x.Name == Constants.DiceOptionName).Value.ToString();

        var response = DiceRoller.ParseAndRollDice(new MessageDto
        {
            Command = diceOption!,
            UserDisplayName = userGlobalName!,
            HiddenDice = hidden,
        });
        await command.RespondAsync(response, ephemeral: hidden);
    }
}