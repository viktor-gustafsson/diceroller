using Discord;
using Discord.Net;
using Discord.WebSocket;
using DiscordBot.Rollers.CharacterRollers;
using DiscordBot.Rollers.CharacterRollers.Enums;
using DiscordBot.Rollers.EffectRollers;
using DiscordBot.Statistics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace DiscordBot.Handlers;

public class DiscordCommandHandler(
    string token,
    IRollRecorder recorder,
    IDbContextFactory<StatisticsDbContext> statsContextFactory) : IHostedService
{
    private readonly DiscordSocketClient _client = new(new DiscordSocketConfig
    {
        GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent,
    });

    private readonly Dictionary<string, Func<SocketSlashCommand, bool, Task>> _diceCommandHanders = new()
    {
        [Constants.RollOptionName] = (command, _) => DiceCommandHandler.Handle(command, hidden: false, recorder),
        [Constants.RollOptionHiddenName] = (command, _) => DiceCommandHandler.Handle(command, hidden: true, recorder),
    };

    private readonly Dictionary<string, Func<SocketSlashCommand, Task>> _effectCommandHandlers = new()
    {
        [Constants.RollOptionDevilsLuckName] = c => EffectCommandHandler.Handle<DevilsLuckRoller>(c, recorder),
        [Constants.RollOptionWoundName] = c => EffectCommandHandler.Handle<WoundRoller>(c, recorder),
        [Constants.RollOptionMagicMisHapName] = c => EffectCommandHandler.Handle<MagicMisHapRoller>(c, recorder),
    };

    private readonly Dictionary<string, Func<SocketSlashCommand, Task>> _utilityCommandHandlers = new()
    {
        [Constants.HelpOptionName] = HelpCommandHandler.Handle,
        [Constants.StatsOptionName] = c => StatsCommandHandler.Handle(c, statsContextFactory),
    };

    private readonly Dictionary<string, Func<SocketSlashCommand, Task>> _characterCommandHandlers = new()
    {
        [Constants.NewWitchCharacter] = c => NewCharacterCommandHandler.Roll<WitchCharacterRoller, WitchSubType>(c, recorder),
        [Constants.NewBountyHunterCharacter] = c => NewCharacterCommandHandler.Roll<BountyHunterCharacterRoller, BountyHunterSubType>(c, recorder),
        [Constants.NewMercenaryCharacter] = c => NewCharacterCommandHandler.Roll<MercenaryCharacterRoller, MercenarySubType>(c, recorder),
        [Constants.NewOpportunistCharacter] = c => NewCharacterCommandHandler.Roll<OpportunistCharacterRoller, OpportunistSubType>(c, recorder),
        [Constants.NewPractitionerCharacter] = c => NewCharacterCommandHandler.Roll<PractitionerCharacterRoller, PractitionerSubType>(c, recorder),
    };

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _client.Ready += ReadyAsync;
        _client.SlashCommandExecuted += CommandHandler;
        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _client.LogoutAsync();
        await _client.StopAsync();
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
            new SlashCommandBuilder()
                .WithName(Constants.StatsOptionName)
                .WithDescription("View dice roll statistics for this server")
                .AddOption(new SlashCommandOptionBuilder()
                    .WithName(Constants.StatsSubDistribution)
                    .WithDescription("Show value distribution for a die type")
                    .WithType(ApplicationCommandOptionType.SubCommand)
                    .AddOption(Constants.StatsDieOptionName, ApplicationCommandOptionType.Integer,
                        "Die type, e.g. 20", isRequired: true, minValue: 2, maxValue: 100)
                    .AddOption(Constants.StatsPublicOptionName, ApplicationCommandOptionType.Boolean,
                        "Show to everyone in the channel (default: only you)", isRequired: false))
                .AddOption(new SlashCommandOptionBuilder()
                    .WithName(Constants.StatsSubCrits)
                    .WithDescription("Show nat 1 / nat 20 rates per user (d20)")
                    .WithType(ApplicationCommandOptionType.SubCommand)
                    .AddOption(Constants.StatsPublicOptionName, ApplicationCommandOptionType.Boolean,
                        "Show to everyone in the channel (default: only you)", isRequired: false))
                .AddOption(new SlashCommandOptionBuilder()
                    .WithName(Constants.StatsSubTop)
                    .WithDescription("Show roll counts per user")
                    .WithType(ApplicationCommandOptionType.SubCommand)
                    .AddOption(Constants.StatsPublicOptionName, ApplicationCommandOptionType.Boolean,
                        "Show to everyone in the channel (default: only you)", isRequired: false))
                .AddOption(new SlashCommandOptionBuilder()
                    .WithName(Constants.StatsSubStreaks)
                    .WithDescription("Show longest hot/cold d20 streaks per user")
                    .WithType(ApplicationCommandOptionType.SubCommand)
                    .AddOption(Constants.StatsPublicOptionName, ApplicationCommandOptionType.Boolean,
                        "Show to everyone in the channel (default: only you)", isRequired: false))
                .AddOption(new SlashCommandOptionBuilder()
                    .WithName(Constants.StatsSubHours)
                    .WithDescription("Show roll activity by hour of day (UTC)")
                    .WithType(ApplicationCommandOptionType.SubCommand)
                    .AddOption(Constants.StatsPublicOptionName, ApplicationCommandOptionType.Boolean,
                        "Show to everyone in the channel (default: only you)", isRequired: false)),
        ];
    }

    private async Task CommandHandler(SocketSlashCommand command)
    {
        var commandHandler = command.Data.Name switch
        {
            var name when _characterCommandHandlers.TryGetValue(name, out var characterHandler)
                => characterHandler(command),
            var name when _effectCommandHandlers.TryGetValue(name, out var effectHandler)
                => effectHandler(command),
            var name when _diceCommandHanders.TryGetValue(name, out var diceHandler)
                => diceHandler(command, name == Constants.RollOptionHiddenName),
            var name when _utilityCommandHandlers.TryGetValue(name, out var utilityHandler)
                => utilityHandler(command),
            _ => command.RespondAsync(ErrorMessages.FallbackErrorMessage, ephemeral: true),
        };

        await commandHandler;
    }
}
