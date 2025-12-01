using Discord;
using Discord.Net;
using Discord.WebSocket;
using DiscordBot.Rollers.CharacterRollers;
using DiscordBot.Rollers.CharacterRollers.Enums;
using DiscordBot.Rollers.EffectRollers;

namespace DiscordBot.Handlers;

public class DiscordCommandHandler(string token)
{
    private readonly DiscordSocketClient _client = new(new DiscordSocketConfig
    {
        GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent,
    });

    private static readonly Dictionary<string, Func<SocketSlashCommand, bool, Task>> DiceCommandHanders = new()
    {
        [Constants.RollOptionName] = (command, _) => DiceCommandHandler.Handle(command: command, hidden: false),
        [Constants.RollOptionHiddenName] = (command, _) => DiceCommandHandler.Handle(command: command, hidden: true),
    };

    private static readonly Dictionary<string, Func<SocketSlashCommand, Task>> EffectCommandHandlers = new()
    {
        [Constants.RollOptionDevilsLuckName] = EffectCommandHandler.Handle<DevilsLuckRoller>,
        [Constants.RollOptionWoundName] = EffectCommandHandler.Handle<WoundRoller>,
        [Constants.RollOptionMagicMisHapName] = EffectCommandHandler.Handle<MagicMisHapRoller>,
    };

    private static readonly Dictionary<string, Func<SocketSlashCommand, Task>> UtilityCommandHandlers = new()
    {
        [Constants.HelpOptionName] = HelpCommandHandler.Handle,
    };

    private static readonly Dictionary<string, Func<SocketSlashCommand, Task>> CharacterCommandHandlers = new()
    {
        [Constants.NewWitchCharacter] = NewCharacterCommandHandler.Roll<WitchCharacterRoller, WitchSubType>,
        [Constants.NewBountyHunterCharacter] = NewCharacterCommandHandler.Roll<BountyHunterCharacterRoller, BountyHunterSubType>,
        [Constants.NewMercenaryCharacter] = NewCharacterCommandHandler.Roll<MercenaryCharacterRoller, MercenarySubType>,
        [Constants.NewOpportunistCharacter] = NewCharacterCommandHandler.Roll<OpportunistCharacterRoller, OpportunistSubType>,
        [Constants.NewPractitionerCharacter] = NewCharacterCommandHandler.Roll<PractitionerCharacterRoller, PractitionerSubType>,
    };

    public async Task Start()
    {
        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();
        _client.Ready += ReadyAsync;
        _client.SlashCommandExecuted += CommandHandler;
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

    private static async Task CommandHandler(SocketSlashCommand command)
    {
        var commandHandler = command.Data.Name switch
        {
            var name when CharacterCommandHandlers.TryGetValue(name, out var characterHandler)
                => characterHandler(command),
            var name when EffectCommandHandlers.TryGetValue(name, out var effectHandler)
                => effectHandler(command),
            var name when DiceCommandHanders.TryGetValue(name, out var diceHandler)
                => diceHandler(command, name == Constants.RollOptionHiddenName),
            var name when UtilityCommandHandlers.TryGetValue(name, out var utilityHandler)
                => utilityHandler(command),
            _ => command.RespondAsync(ErrorMessages.FallbackErrorMessage, ephemeral: true),
        };

        await commandHandler;
    }
}