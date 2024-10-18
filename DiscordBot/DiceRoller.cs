using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace DiscordBot;

public class DiceRoller
{
    private readonly string _token;

    private readonly DiscordSocketClient _client = new(new DiscordSocketConfig
    {
        GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
    });

    public DiceRoller(string token)
    {
        _token = token;
        _client.MessageReceived += MessageHandler;
    }
    
    public async Task StartBot()
    {
        await _client.StartAsync();
        await _client.LoginAsync(TokenType.Bot, _token);

        await Task.Delay(-1);
    }

    private static async Task MessageHandler(SocketMessage message)
    {
        if (message.Author.IsBot)
            return;

        if (message.Content.StartsWith("!roll"))
        {
            var response = ParseAndRollDice(message.Content);
            await Reply(message, response);
        }
        else
        {
            await Reply(message, "I'm sorry, I don't know what to do.");
        }
    }

    private static string ParseAndRollDice(string command)
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
            var keptDiceMessage = rollDiceCommand.DiceCount == rollDiceCommand.DicesToKeep ? "" : GetKeepMessage(keptDice);
            
            var rollResult =
                $"{GetRollingMessage(rollDiceCommand)}\n{GetDiceNumberToKeepMessage(rollDiceCommand, keepHigh)}{modifierMessage}\n{GetRollsMessage(rolls)}{keptDiceMessage}\n{GetSumMessage(keptDice, rollDiceCommand.Modifier, total)}";
            return rollResult;
        }
        catch (Exception)
        {
            return "Invalid roll command. Please use a format like '!roll 2d20k1h+5' or '!roll 6d6k3l-3'.";
        }
    }

    private static string GetModifierMessage(RollDiceCommand rollDiceCommand)
        => $"\nModifier: {rollDiceCommand.Modifier}";

    private static string GetSumMessage(IEnumerable<int> keptDice, int modifier, int total)
        => $"Sum: {keptDice.Sum()}{(modifier != 0 ? $"{(modifier > 0 ? "+" : "")}{modifier}" : "")} = {total}";
    
    private static string GetKeepMessage(IEnumerable<int> keptDice)
        => $"\nKeeping: {string.Join(", ", keptDice)}";

    private static string GetRollsMessage(IEnumerable<int> rolls)
        => $"You rolled: {string.Join(", ", rolls)}";

    private static string GetDiceNumberToKeepMessage(RollDiceCommand rollDiceCommand, bool keepHighest) =>
        keepHighest switch
        {
            true => $"Keep highest {rollDiceCommand.DicesToKeep} dice",
            false => $"Keep lowest {rollDiceCommand.DicesToKeep} dice"
        };

    private static string GetRollingMessage(RollDiceCommand rollDiceCommand)
        => $"Rolling {rollDiceCommand.DiceCount} d{rollDiceCommand.DiceType}";

    private static async Task Reply(SocketMessage message, string response) =>
        await message.Channel.SendMessageAsync(response);
}