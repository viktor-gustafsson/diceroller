using DiscordBot.Models;

namespace DiscordBot.Rollers.DiceRollers;

public record DiceRollResult(string Message, IReadOnlyList<RollDiceCommand> Rolls);
