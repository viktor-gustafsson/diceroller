using Discord;
using Discord.Net;
using Discord.WebSocket;
using DiscordBot.Models;
using DiscordBot.ResponseMessages;
using DiscordBot.Rollers;
using DiscordBot.Rollers.Characters;

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
    private const string RollOptionMagicMisHapName = "roll_magic_mis_hap";
    private const string NewGenericCharacter = "new_generic_char";
    private const string NewWitchCharacter = "new_witch_char";
    private const string NewBountyHunterCharacter = "new_bounty_hunter_char";
    private const string NewMercenaryCharacter = "new_mercenary_deserter_char";
    private const string NewOpportunistCharacter = "new_opportunist_char";
    private const string NewPractitionerCharacter = "new_practitioner_char";

    private readonly DiscordSocketClient _client = new(new DiscordSocketConfig
    {
        GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent,
    });

    private static readonly Dictionary<string, Func<SocketSlashCommand, Task>> CommandHandlers = new()
    {
        [RollOptionName] = HandleDiceRoll,
        [RollOptionHiddenName] = HandleHiddenDiceRoll,
        [RollOptionDevilsLuckName] = HandleDevilsLuckRoll,
        [RollOptionWoundName] = HandleWoundRoll,
        [RollOptionMagicMisHapName] = HandleMagicMisHapRoll,
        [NewGenericCharacter] = HandleGenericCharacterCreation,
        [NewWitchCharacter] = HandleWitchCharacterCreation,
        [NewBountyHunterCharacter] = HandleBountyHunterCharacterCreation,
        [NewMercenaryCharacter] = HandleMercenaryCharacterCreation,
        [NewOpportunistCharacter] = HandleOpportunistCharacterCreation,
        [NewPractitionerCharacter] = HandlePractitionerCharacterCreation,
        [HelpOptionName] = HandleHelp,
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
                .WithName(RollOptionMagicMisHapName)
                .WithDescription("Roll magic mis-hap!"),
            new SlashCommandBuilder()
                .WithName(NewGenericCharacter)
                .WithDescription("Roll a new generic character!"),
            new SlashCommandBuilder()
                .WithName(NewWitchCharacter)
                .WithDescription("Roll a new witch character!"),
            new SlashCommandBuilder()
                .WithName(NewBountyHunterCharacter)
                .WithDescription("Roll a new bounty hunter character!"),
            new SlashCommandBuilder()
                .WithName(NewMercenaryCharacter)
                .WithDescription("Roll a new mercenary deserter character!"),
            new SlashCommandBuilder()
                .WithName(NewOpportunistCharacter)
                .WithDescription("Roll a new opportunist character!"),
            new SlashCommandBuilder()
                .WithName(NewPractitionerCharacter)
                .WithDescription("Roll a new practitioner character!"),
            new SlashCommandBuilder()
                .WithName(HelpOptionName)
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

    private static async Task HandleDiceRoll(SocketSlashCommand command)
    {
        var userGlobalName = (command.User as SocketGuildUser)?.DisplayName;
        var diceOption = command.Data.Options.First(x => x.Name == DiceOptionName).Value.ToString();
        var response = DiceRoller.ParseAndRollDice(new MessageDto
        {
            Command = diceOption!,
            UserDisplayName = userGlobalName!,
            HiddenDice = false,
        });
        await command.RespondAsync(response, ephemeral: false);
    }

    private static async Task HandleHiddenDiceRoll(SocketSlashCommand command)
    {
        var userGlobalName = (command.User as SocketGuildUser)?.DisplayName;
        var diceOption = command.Data.Options.First(x => x.Name == HiddenDiceOptionName).Value.ToString();
        var response = DiceRoller.ParseAndRollDice(new MessageDto
        {
            Command = diceOption!,
            UserDisplayName = userGlobalName!,
            HiddenDice = true,
        });
        await command.RespondAsync(response, ephemeral: true);
    }

    private static async Task HandleDevilsLuckRoll(SocketSlashCommand command)
    {
        var rollDevilsLuck = DevilsLuckRoller.Roll();
        await command.RespondAsync(rollDevilsLuck, ephemeral: false);
    }

    private static async Task HandleMagicMisHapRoll(SocketSlashCommand command)
    {
        var rollMagicMisHap = MagicMisHapRoller.Roll();
        await command.RespondAsync(rollMagicMisHap, ephemeral: false);
    }
    private static async Task HandleWoundRoll(SocketSlashCommand command)
    {
        var rollWound = WoundRoller.Roll();
        await command.RespondAsync(rollWound, ephemeral: false);
    }

    private static async Task HandleGenericCharacterCreation(SocketSlashCommand command)
    {
        var newGenericCharacter = GenericNewCharacterRoller.Roll();
        await command.RespondAsync(newGenericCharacter, ephemeral: false);
    }

    private static async Task HandleWitchCharacterCreation(SocketSlashCommand command)
    {
        var newWitchCharacter = WitchCharacterRoller.Roll();
        await command.RespondAsync(newWitchCharacter, ephemeral: false);
    }

    private static async Task HandleBountyHunterCharacterCreation(SocketSlashCommand command)
    {
        var newBountyHunterCharacter = BountyHunterCharacterRoller.Roll();
        await command.RespondAsync(newBountyHunterCharacter, ephemeral: false);
    }

    private static async Task HandleMercenaryCharacterCreation(SocketSlashCommand command)
    {
        var newMercenaryDeserter = MercenaryDeserterCharacterRoller.Roll();
        await command.RespondAsync(newMercenaryDeserter, ephemeral: false);
    }

    private static async Task HandleOpportunistCharacterCreation(SocketSlashCommand command)
    {
        var newOpportunistCharacter = OpportunistCharacterRoller.Roll();
        await command.RespondAsync(newOpportunistCharacter, ephemeral: false);
    }

    private static async Task HandlePractitionerCharacterCreation(SocketSlashCommand command)
    {
        var newPractitionerCharacter = PractitionerCharacterRoller.Roll();
        await command.RespondAsync(newPractitionerCharacter, ephemeral: false);
    }

    private static async Task HandleHelp(SocketSlashCommand command)
    {
        var helpMessage = Messages.GetHelpMessage();
        await command.RespondAsync(helpMessage, ephemeral: true);
    }
}