using Discord;
using Discord.Net;
using Discord.WebSocket;

namespace DiscordBot;

public class DiceRoller(string token)
{
    private const string OptionName = "dice";
    private readonly DiscordSocketClient _client = new(new DiscordSocketConfig
    {
        GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
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
            .WithName("roll")
            .WithDescription("Roll some dice!")
            .AddOption(OptionName, ApplicationCommandOptionType.String, "[# of dice]d[dice type]k[keep amount][h/l][modifier] e.g. 2d20k1h+5", isRequired: true);

        try
        {
            await _client.CreateGlobalApplicationCommandAsync(rollCommand.Build());
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
        if (command.Data.Name == "roll")
        {
            var diceOption = command.Data.Options.FirstOrDefault(x => x.Name == OptionName)?.Value?.ToString();
            var response = ParseAndRollDice(userGlobalName,diceOption);
            await command.RespondAsync(response);
        }
        else
        {
            await command.RespondAsync("I'm sorry, I don't know what to do.");
        }
    }

    private static string ParseAndRollDice(string userDisplayName, string command)
    {
        try
        {
            var rollDiceCommand = new RollDiceCommand(command);
            if (rollDiceCommand.ValidCommand) 
                return "Invalid roll command.";

            // By default, keep highest dice if no 'h' or 'l' is provided
            var keepHigh = !rollDiceCommand.Command.Contains('l');

            // Roll the dice
            var rand = new Random();
            var rolls = new int[rollDiceCommand.DiceCount];
            for (var i = 0; i < rollDiceCommand.DiceCount; i++)
            {
                rolls[i] = rand.Next(1, rollDiceCommand.DiceType + 1);
            }

            // Sort and select the dice to keep
            var keptDice = keepHigh
                ? rolls.OrderByDescending(x => x).Take(rollDiceCommand.DicesToKeep).ToList()
                : rolls.OrderBy(x => x).Take(rollDiceCommand.DicesToKeep).ToList();

            // Apply the modifier to the total of the kept dice
            var total = keptDice.Sum() + rollDiceCommand.Modifier;

            var modifierMessage = rollDiceCommand.Modifier == 0 ? "" : GetModifierMessage(rollDiceCommand);
            var keptDiceMessage =
                rollDiceCommand.DiceCount == rollDiceCommand.DicesToKeep ? "" : GetKeepMessage(keptDice);
            
            var rollResult =
                $"```" +
                $"\n{userDisplayName} \n{GetRollingMessage(rollDiceCommand)}\n{GetDiceNumberToKeepMessage(rollDiceCommand, keepHigh)}{modifierMessage}\n{GetRollsMessage(rolls)}{keptDiceMessage}\n{GetSumMessage(keptDice, rollDiceCommand.Modifier, total)}" +
                $"```";
            return rollResult;
        }
        catch (Exception)
        {
            return "Invalid roll command. Please use a format like '2d20k1h+5' or '6d6k3l-3'.";
        }
    }

    private static string GetModifierMessage(RollDiceCommand rollDiceCommand)
        => $"\n\ud83d\udfe6 Modifier: [ {rollDiceCommand.Modifier} ]";

    private static string GetSumMessage(IEnumerable<int> keptDice, int modifier, int total)
    {
        var sumOfKeptDice = keptDice.Sum();

        var modifierMessage = modifier != 0 
            ? $"{(modifier > 0 ? "+" : "")}{modifier}" 
            : "";

        return modifier != 0
            ? $"\u2728 Sum: {sumOfKeptDice}{modifierMessage} = {total}"
            : $"\u2728 Sum: {sumOfKeptDice}{modifierMessage}";
    }

    private static string GetKeepMessage(IEnumerable<int> keptDice)
        => $"\n\ud83d\udcbe Keeping: [ {string.Join(", ", keptDice)} ]";

    private static string GetRollsMessage(IEnumerable<int> rolls)
        => $"\ud83c\udfb2 You rolled: [ {string.Join(", ", rolls)} ]";

    private static string GetDiceNumberToKeepMessage(RollDiceCommand rollDiceCommand, bool keepHighest) =>
        keepHighest switch
        {
            true => $"\ud83d\udee1\ufe0f Keeping: Highest {rollDiceCommand.DicesToKeep} dice",
            false => $"\ud83d\udee1\ufe0f Keeping: Lowest {rollDiceCommand.DicesToKeep} dice"
        };

    private static string GetRollingMessage(RollDiceCommand rollDiceCommand)
        => $"\ud83c\udfaf Rolling: {rollDiceCommand.DiceCount} d{rollDiceCommand.DiceType}";
}