using Discord;
using Discord.Net;
using Discord.WebSocket;
using DiscordBot.Models;
using DiscordBot.ResponseMessages;
using DiscordBot.Services.Rollers;

namespace DiscordBot.Handlers;

public class DiscordCommandHandler(string token)
{
    private const string DiceOptionName = "dice";
    private const string HiddenDiceOptionName = "hiddendice";
    private const string HelpOptionName = "help";
    private const string RollOptionName = "roll";
    private const string RollOptionHiddenName = "roll_hidden";
    private const string RollOptionDevilsLuckName = "roll_devils_luck";
    private const string RollOptionWoundName = "roll_wound";

    private readonly DiscordSocketClient _client = new(new DiscordSocketConfig
    {
        GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent,
    });

    public async Task Start()
    {
        await _client.StartAsync();
        await _client.LoginAsync(TokenType.Bot, token);
        _client.Ready += ReadyAsync;
        _client.SlashCommandExecuted += MessageHandler;

        await Task.Delay(-1);
    }

    private async Task ReadyAsync()
    {
        try
        {
            var commands = CreateSlashCommands();
            await _client.BulkOverwriteGlobalApplicationCommandsAsync(commands.Select(c => c.Build()).ToArray<ApplicationCommandProperties>());
        }
        
        catch (HttpException ex)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(ex.Errors, Newtonsoft.Json.Formatting.Indented);
            Console.WriteLine(json);
        }
    }
    
    private static List<SlashCommandBuilder> CreateSlashCommands()
    {
        const string diceDescription = "[# of dice]d[dice type]k[keep amount][h/l][modifier] e.g. 2d20k1h+5";
    
        return
        [
            new SlashCommandBuilder()
                .WithName(RollOptionName)
                .WithDescription("Roll some dice!")
                .AddOption(DiceOptionName, ApplicationCommandOptionType.String, diceDescription, isRequired: true),

            new SlashCommandBuilder()
                .WithName(RollOptionHiddenName)
                .WithDescription("Roll some hidden dice!")
                .AddOption(HiddenDiceOptionName, ApplicationCommandOptionType.String, diceDescription,
                    isRequired: true),

            new SlashCommandBuilder()
                .WithName(RollOptionDevilsLuckName)
                .WithDescription("Roll devils luck!"),

            new SlashCommandBuilder()
                .WithName(RollOptionWoundName)
                .WithDescription("Roll wound!"),

            new SlashCommandBuilder()
                .WithName(HelpOptionName)
                .WithDescription("Explanation and examples"),
        ];
    }

    private static async Task MessageHandler(SocketSlashCommand command)
    {
        var userGlobalName = (command.User as SocketGuildUser)?.DisplayName;
        switch (command.Data.Name)
        {
            case RollOptionName:
            {
                var diceOption = command.Data.Options.First(x => x.Name == DiceOptionName).Value.ToString();
                var response = DiceRoller.ParseAndRollDice(new MessageDto
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
                var response = DiceRoller.ParseAndRollDice(new MessageDto
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
            case RollOptionWoundName:
            {
                var rollWound = WoundRoller.Roll();
                await command.RespondAsync(rollWound, ephemeral: false);
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
}