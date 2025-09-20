using System.Text;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using DiscordBot.Models;
using DiscordBot.ResponseMessages;

namespace DiscordBot.Services;

public class DiceRoller(string token)
{
    private const string DiceOptionName = "dice";
    private const string HiddenDiceOptionName = "hiddendice";
    private const string HelpOptionName = "help";
    private const string DevilsLuckName = "devils_luck";
    private const string RollOptionName = "roll";
    private const string RollOptionHiddenName = "roll_hidden";
    private const string RollOptionDevilsLuckName = "roll_devils_luck";

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
        
        var rollHiddenCommand = new SlashCommandBuilder()
            .WithName(RollOptionHiddenName)
            .WithDescription("Roll some hidden dice!")
            .AddOption(HiddenDiceOptionName, ApplicationCommandOptionType.String,
                "[# of dice]d[dice type]k[keep amount][h/l][modifier] e.g. 2d20k1h+5", isRequired: true);

        var devilsLuckCommand = new SlashCommandBuilder()
            .WithName(RollOptionDevilsLuckName)
            .WithDescription("Roll devils luck!");

        var helpCommand = new SlashCommandBuilder()
            .WithName(HelpOptionName)
            .WithDescription("Explanation and examples");

        try
        {
            await _client.BulkOverwriteGlobalApplicationCommandsAsync([
                rollCommand.Build(),
                rollHiddenCommand.Build(),
                devilsLuckCommand.Build(),
                helpCommand.Build(),
            ]);

        }
        catch (HttpException ex)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(ex.Errors, Newtonsoft.Json.Formatting.Indented);
            Console.WriteLine(json);
        }
    }

    private static async Task MessageHandler(SocketSlashCommand command)
    {
        var userGlobalName = (command.User as SocketGuildUser)?.DisplayName;
        switch (command.Data.Name)
        {
            case RollOptionName:
            {
                var diceOption = command.Data.Options.First(x => x.Name == DiceOptionName).Value.ToString();
                var response = ParseAndRollDice(new MessageDto
                {
                    Command = diceOption!,
                    UserDisplayName = userGlobalName!,
                    HiddenDice = false,
                });
                await command.RespondAsync(response, ephemeral: false);
                break;
            }
            case RollOptionHiddenName:
            {
                var diceOption = command.Data.Options.First(x => x.Name == HiddenDiceOptionName).Value.ToString();
                var response = ParseAndRollDice(new MessageDto
                {
                    Command = diceOption!,
                    UserDisplayName = userGlobalName!,
                    HiddenDice = true,
                });
                await command.RespondAsync(response, ephemeral: true);
                break;
            }
            case RollOptionDevilsLuckName:
            {
                var rollDevilsLuck = DevilsLuckRoller.Roll();
                await command.RespondAsync(rollDevilsLuck, ephemeral: false);
                break;
            }
            case HelpOptionName:
            {
                var helpMessage = Messages.GetHelpMessage();
                await command.RespondAsync(helpMessage, ephemeral: true);
                break; 
            }
            default:
            {
                await command.RespondAsync(ErrorMessages.FallbackErrorMessage, ephemeral: true);
                break;
            }
        }
    }

    private static string ParseAndRollDice(MessageDto messageDto)
    {
        try
        {
            var sb = new StringBuilder();

            var rollDiceCommands = DiceRollParser.Parse(messageDto);
            foreach (var rollDiceCommand in rollDiceCommands)
            {
                if (rollDiceCommand.ValidCommand)
                    return ErrorMessages.InvalidRollCommand;

                // Roll the dice
                var rand = new Random();
                for (var i = 0; i < rollDiceCommand.DiceCount; i++)
                {
                    rollDiceCommand.Rolls[i] = rand.Next(1, rollDiceCommand.DiceType + 1);
                }

                sb.Append(Messages.GetResultMessage(rollDiceCommand, messageDto.HiddenDice));
            }
                
            return sb.ToString();
        }
        catch (Exception)
        {
            return ErrorMessages.InvalidRollCommand;
        }
    }
}