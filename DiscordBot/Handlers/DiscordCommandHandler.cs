using Discord;
using Discord.Net;
using Discord.WebSocket;
using DiscordBot.Handlers.CharacterCreationHandlers;
using DiscordBot.Handlers.DiceHandlers;
using DiscordBot.Handlers.EffectHandlers;

namespace DiscordBot.Handlers;

public class DiscordCommandHandler(string token)
{
    private readonly DiscordSocketClient _client = new(new DiscordSocketConfig
    {
        GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent,
    });

    private static readonly Dictionary<string, Func<SocketSlashCommand, Task>> CommandHandlers = new()
    {
        [Constants.RollOptionName] = DiceRollCommandHandler.Handle,
        [Constants.RollOptionHiddenName] = HiddenDiceRollCommandHandler.Handle,
        [Constants.RollOptionDevilsLuckName] = DevilsLuckCommandHandler.Handle,
        [Constants.RollOptionWoundName] = WoundCommandHandler.Handle,
        [Constants.RollOptionMagicMisHapName] = MagicMisHapCommandHandler.Handle,
        [Constants.NewWitchCharacter] = WitchCharacterCreationCommandHandler.Handle,
        [Constants.NewBountyHunterCharacter] = BountyHunterCharacterCreationCommandHandler.Handle,
        [Constants.NewMercenaryCharacter] = MercenaryCharacterCommandHandler.Handle,
        [Constants.NewOpportunistCharacter] = OpportunistCharacterCommandHandler.Handle,
        [Constants.NewPractitionerCharacter] = PractitionerCharacterCommandHandler.Handle,
        [Constants.HelpOptionName] = HelpCommandHandler.Handle,
    };

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
            await _client.BulkOverwriteGlobalApplicationCommandsAsync(commands.Select(c => c.Build())
                .ToArray<ApplicationCommandProperties>());
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
                .WithName(Constants.RollOptionName)
                .WithDescription("Roll some dice!")
                .AddOption(Constants.DiceOptionName, ApplicationCommandOptionType.String, diceDescription,
                    isRequired: true),
            new SlashCommandBuilder()
                .WithName(Constants.RollOptionHiddenName)
                .WithDescription("Roll some hidden dice!")
                .AddOption(Constants.HiddenDiceOptionName, ApplicationCommandOptionType.String, diceDescription,
                    isRequired: true),
            new SlashCommandBuilder()
                .WithName(Constants.RollOptionDevilsLuckName)
                .WithDescription("Roll devil's luck!"),
            new SlashCommandBuilder()
                .WithName(Constants.RollOptionWoundName)
                .WithDescription("Roll wound!"),
            new SlashCommandBuilder()
                .WithName(Constants.RollOptionMagicMisHapName)
                .WithDescription("Roll magic mis-hap!"),
            new SlashCommandBuilder()
                .WithName(Constants.NewWitchCharacter)
                .WithDescription("Roll a new witch character!")
                .AddOption(Constants.SubTypeOptionName, ApplicationCommandOptionType.Integer,
                    "1 = Wood Witch, 2 = Herbalist, 3 = Hexen",
                    minValue: 1, maxValue: 3, isRequired: true),
            new SlashCommandBuilder()
                .WithName(Constants.NewBountyHunterCharacter)
                .WithDescription("Roll a new bounty hunter character!")
                .AddOption(Constants.SubTypeOptionName, ApplicationCommandOptionType.Integer,
                    "1 = Pistolier, 2 = Master Trapper, 3 = Beast Hunter",
                    minValue: 1, maxValue: 3, isRequired: true),
            new SlashCommandBuilder()
                .WithName(Constants.NewMercenaryCharacter)
                .WithDescription("Roll a new mercenary deserter character!")
                .AddOption(Constants.SubTypeOptionName, ApplicationCommandOptionType.Integer,
                    "1 = Rifleman, 2 = GreatSwordsman, 3 = Grenadier",
                    minValue: 1, maxValue: 3, isRequired: true),
            new SlashCommandBuilder()
                .WithName(Constants.NewOpportunistCharacter)
                .WithDescription("Roll a new opportunist character!")
                .AddOption(Constants.SubTypeOptionName, ApplicationCommandOptionType.Integer,
                    "1 = Adventurer, 2 = Sneak Thief, 3 = Silver-Tongued Trickster",
                    minValue: 1, maxValue: 3, isRequired: true),
            new SlashCommandBuilder()
                .WithName(Constants.NewPractitionerCharacter)
                .WithDescription("Roll a new practitioner character!")
                .AddOption(Constants.SubTypeOptionName, ApplicationCommandOptionType.Integer,
                    "1 = Vow of War, 2 = Vow of Healing, 3 = Vow of Sustenance",
                    minValue: 1, maxValue: 3, isRequired: true),
            new SlashCommandBuilder()
                .WithName(Constants.HelpOptionName)
                .WithDescription("Explanation and examples"),
        ];
    }

    private static async Task MessageHandler(SocketSlashCommand command)
    {
        if (CommandHandlers.TryGetValue(command.Data.Name, out var handler))
        {
            await handler(command);
        }
        else
        {
            await command.RespondAsync(ErrorMessages.FallbackErrorMessage, ephemeral: true);
        }
    }
}