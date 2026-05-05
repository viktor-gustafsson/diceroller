using Discord.WebSocket;
using DiscordBot.Models;
using DiscordBot.Statistics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DiscordBot.Statistics;

public class RollRecorder(
    IDbContextFactory<StatisticsDbContext> contextFactory,
    ILogger<RollRecorder> logger) : IRollRecorder
{
    public async Task RecordStandardRollsAsync(
        SocketSlashCommand command,
        IReadOnlyList<RollDiceCommand> rolls,
        string resultText)
    {
        try
        {
            var (userId, displayName, guildId, channelId) = ExtractContext(command);
            var timestamp = DateTime.UtcNow;

            await using var context = await contextFactory.CreateDbContextAsync();

            foreach (var rollCommand in rolls)
            {
                var keptIndices = GetKeptIndices(rollCommand);
                var roll = new Roll
                {
                    GuildId = guildId,
                    ChannelId = channelId,
                    UserId = userId,
                    UserDisplayName = displayName,
                    RollType = RollType.Standard,
                    RollSubType = null,
                    Command = rollCommand.Command,
                    Total = rollCommand.GetTotal(),
                    Modifier = rollCommand.Modifier,
                    ResultText = resultText,
                    Timestamp = timestamp,
                    Dice = rollCommand.Rolls
                        .Select((value, index) => new RollDie
                        {
                            DieType = rollCommand.DiceType,
                            Value = value,
                            Kept = keptIndices.Contains(index),
                            Position = index,
                        })
                        .ToList(),
                };
                context.Rolls.Add(roll);
            }

            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to record standard roll(s) for user {User}", command.User.Username);
        }
    }

    public Task RecordEffectRollAsync(SocketSlashCommand command, string subType, string resultText) =>
        RecordSimpleAsync(command, RollType.Effect, subType, resultText);

    public Task RecordCharacterRollAsync(SocketSlashCommand command, string subType, string resultText) =>
        RecordSimpleAsync(command, RollType.Character, subType, resultText);

    private async Task RecordSimpleAsync(
        SocketSlashCommand command,
        RollType rollType,
        string subType,
        string resultText)
    {
        try
        {
            var (userId, displayName, guildId, channelId) = ExtractContext(command);

            await using var context = await contextFactory.CreateDbContextAsync();
            context.Rolls.Add(new Roll
            {
                GuildId = guildId,
                ChannelId = channelId,
                UserId = userId,
                UserDisplayName = displayName,
                RollType = rollType,
                RollSubType = subType,
                Command = command.Data.Name,
                Total = null,
                Modifier = null,
                ResultText = resultText,
                Timestamp = DateTime.UtcNow,
            });
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to record {RollType}/{SubType} for user {User}",
                rollType, subType, command.User.Username);
        }
    }

    private static (ulong UserId, string DisplayName, ulong? GuildId, ulong ChannelId) ExtractContext(
        SocketSlashCommand command)
    {
        var guildUser = command.User as SocketGuildUser;
        var displayName = guildUser?.DisplayName ?? command.User.GlobalName ?? command.User.Username;
        var guildId = guildUser?.Guild.Id;
        return (command.User.Id, displayName, guildId, command.ChannelId ?? 0UL);
    }

    private static HashSet<int> GetKeptIndices(RollDiceCommand rollCommand)
    {
        var ordered = rollCommand.Rolls
            .Select((value, index) => (value, index));

        var kept = rollCommand.KeepHigh
            ? ordered.OrderByDescending(t => t.value).Take(rollCommand.DicesToKeep)
            : ordered.OrderBy(t => t.value).Take(rollCommand.DicesToKeep);

        return kept.Select(t => t.index).ToHashSet();
    }
}
