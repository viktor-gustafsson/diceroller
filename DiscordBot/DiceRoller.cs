using Discord;
using Discord.Net;
using Discord.WebSocket;

namespace DiscordBot;

public class DiceRoller(string token)
{
    private const string DiceOptionName = "dice";
    private const string HelpOptionName = "help";
    private const string RollOptionName = "roll";

    private readonly DiscordSocketClient _client = new(new DiscordSocketConfig
    {
        GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent,
    });

    public async Task StartBot()
    {
        await _client.StartAsync();
        await _client.LoginAsync(TokenType.Bot, token);
        _client.Ready += ReadyAsync;
        _client.SlashCommandExecuted += MessageHandler;

        await Task.Delay(-1);
    }

    private async Task ReadyAsync()
    {
        // Define the slash command builder
        var rollCommand = new SlashCommandBuilder()
            .WithName(RollOptionName)
            .WithDescription("Roll some dice!")
            .AddOption(DiceOptionName, ApplicationCommandOptionType.String,
                "[# of dice]d[dice type]k[keep amount][h/l][modifier] e.g. 2d20k1h+5", isRequired: true);

        var helpCommand = new SlashCommandBuilder()
            .WithName(HelpOptionName)
            .WithDescription("Explanation and examples");

        try
        {
            await _client.CreateGlobalApplicationCommandAsync(rollCommand.Build());
            await _client.CreateGlobalApplicationCommandAsync(helpCommand.Build());
        }
        catch (HttpException ex)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(ex.Errors, Newtonsoft.Json.Formatting.Indented);
            Console.WriteLine(json);
        }
    }

    private static async Task MessageHandler(SocketSlashCommand command)
    {
        var userGlobalName = (command.User as SocketGuildUser).DisplayName;
        switch (command.Data.Name)
        {
            case RollOptionName:
            {
                var diceOption = command.Data.Options.FirstOrDefault(x => x.Name == DiceOptionName)?.Value?.ToString();
                await command.DeferAsync();
                var response = ParseAndRollDice(userGlobalName, diceOption);
                await command.FollowupAsync(response);
                break;
            }
            case HelpOptionName:
                await command.DeferAsync();
                var helpMessage = Messages.GetHelpMessage();
                await command.FollowupAsync(helpMessage, ephemeral: true);
                break;
            default:
                await command.RespondAsync(ErrorMessages.GetFallbackErrorMessage());
                break;
        }
    }

    private static string ParseAndRollDice(string userDisplayName, string command)
    {
        try
        {
            var result = string.Empty;

            var rollDiceCommands = RollDiceCommandFactory.GetRollDiceCommands(command, userDisplayName);
            foreach (var rollDiceCommand in rollDiceCommands)
            {
                if (rollDiceCommand.ValidCommand)
                    return ErrorMessages.GetInvalidRollCommandMessage();

                // Roll the dice
                var rand = new Random();
                for (var i = 0; i < rollDiceCommand.DiceCount; i++)
                {
                    rollDiceCommand.Rolls[i] = rand.Next(1, rollDiceCommand.DiceType + 1);
                }

                result += Messages.GetResultMessage(rollDiceCommand);
            }

            return result;
        }
        catch (Exception)
        {
            return ErrorMessages.GetInvalidRollCommandMessage();
        }
    }
}