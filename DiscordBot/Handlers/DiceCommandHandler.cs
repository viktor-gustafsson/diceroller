using Discord.WebSocket;
using DiscordBot.Models;
using DiscordBot.Rollers.DiceRollers;
using DiscordBot.Statistics;

namespace DiscordBot.Handlers;

public static class DiceCommandHandler
{
    public static async Task Handle(SocketSlashCommand command, bool hidden, IRollRecorder recorder)
    {
        var userGlobalName = (command.User as SocketGuildUser)?.DisplayName;

        var diceOption = hidden
            ? command.Data.Options.First(x => x.Name == Constants.HiddenDiceOptionName).Value.ToString()
            : command.Data.Options.First(x => x.Name == Constants.DiceOptionName).Value.ToString();

        var result = DiceRoller.ParseAndRollDice(new MessageDto
        {
            Command = diceOption!,
            UserDisplayName = userGlobalName!,
            HiddenDice = hidden,
        });
        await command.RespondAsync(result.Message, ephemeral: hidden);

        if (result.Rolls.Count > 0)
            await recorder.RecordStandardRollsAsync(command, result.Rolls, result.Message);
    }
}
