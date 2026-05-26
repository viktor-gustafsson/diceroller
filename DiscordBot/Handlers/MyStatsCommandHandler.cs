using System.Text;
using Discord.WebSocket;
using DiscordBot.Statistics;
using DiscordBot.Statistics.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordBot.Handlers;

public static class MyStatsCommandHandler
{
    private const int TopDieTypes = 5;

    public static async Task Handle(SocketSlashCommand command, IDbContextFactory<StatisticsDbContext> contextFactory)
    {
        if (command.GuildId is not { } guildId)
        {
            await command.RespondAsync("📊 Stats are only available inside a server.", ephemeral: true);
            return;
        }

        var makePublic = command.Data.Options
            .FirstOrDefault(o => o.Name == Constants.StatsPublicOptionName)?.Value as bool? ?? false;

        var userId = command.User.Id;
        var displayName = (command.User as SocketGuildUser)?.DisplayName ?? command.User.GlobalName ?? command.User.Username;

        var response = await Build(contextFactory, guildId, userId, displayName);
        await command.RespondAsync(response, ephemeral: !makePublic);
    }

    private static async Task<string> Build(
        IDbContextFactory<StatisticsDbContext> contextFactory, ulong guildId, ulong userId, string displayName)
    {
        await using var ctx = await contextFactory.CreateDbContextAsync();

        var rolls = await ctx.Rolls
            .Where(r => r.GuildId == guildId && r.UserId == userId)
            .Select(r => new { r.Id, r.RollType, r.Timestamp })
            .ToListAsync();

        if (rolls.Count == 0)
            return $"📊 No rolls recorded yet for {displayName} on this server.";

        var dice = await (
            from d in ctx.RollDice
            join r in ctx.Rolls on d.RollId equals r.Id
            where r.GuildId == guildId && r.UserId == userId
            orderby r.Timestamp, d.Position
            select new { d.DieType, d.Value }).ToListAsync();

        var sb = new StringBuilder();
        sb.AppendLine("```");
        sb.AppendLine($"📊 Personal stats — {displayName} (this server)");
        sb.AppendLine();

        AppendTotals(sb, rolls.Count,
            rolls.Count(r => r.RollType == RollType.Standard),
            rolls.Count(r => r.RollType == RollType.Effect),
            rolls.Count(r => r.RollType == RollType.Character));

        var d20s = dice.Where(d => d.DieType == 20).Select(d => d.Value).ToList();
        if (d20s.Count > 0)
        {
            sb.AppendLine();
            AppendD20Crits(sb, d20s);
            sb.AppendLine();
            AppendD20Streaks(sb, d20s);
        }

        if (dice.Count > 0)
        {
            sb.AppendLine();
            AppendTopDieTypes(sb, dice.GroupBy(d => d.DieType)
                .Select(g => (DieType: g.Key, Count: g.Count()))
                .OrderByDescending(x => x.Count)
                .ToList());
        }

        sb.AppendLine();
        AppendMostActiveHour(sb, rolls.Select(r => r.Timestamp.Hour));

        sb.AppendLine("```");
        return sb.ToString();
    }

    private static void AppendTotals(StringBuilder sb, int total, int standard, int effect, int character)
    {
        sb.AppendLine($"Total rolls: {total}");
        sb.AppendLine($"  standard: {standard}  effect: {effect}  char: {character}");
    }

    private static void AppendD20Crits(StringBuilder sb, IReadOnlyCollection<int> d20s)
    {
        var nat20 = d20s.Count(v => v == 20);
        var nat1 = d20s.Count(v => v == 1);
        var nat20Pct = (double)nat20 / d20s.Count * 100;
        var nat1Pct = (double)nat1 / d20s.Count * 100;
        sb.AppendLine($"🎯 d20 crits ({d20s.Count} rolls)");
        sb.AppendLine($"  nat20: {nat20} ({nat20Pct:0.0}%)  nat1: {nat1} ({nat1Pct:0.0}%)");
    }

    private static void AppendD20Streaks(StringBuilder sb, IEnumerable<int> d20s)
    {
        var values = d20s.ToList();
        var hot = LongestRun(values, v => v >= 12);
        var cold = LongestRun(values, v => v <= 11);
        sb.AppendLine("🔥 d20 streaks");
        sb.AppendLine($"  hot (≥12): {hot}  cold (≤11): {cold}");
    }

    private static void AppendTopDieTypes(StringBuilder sb, IReadOnlyList<(int DieType, int Count)> ordered)
    {
        sb.AppendLine("🎲 Top die types");
        foreach (var (dieType, count) in ordered.Take(TopDieTypes))
            sb.AppendLine($"  d{dieType}: {count}");
    }

    private static void AppendMostActiveHour(StringBuilder sb, IEnumerable<int> hours)
    {
        var grouped = hours.GroupBy(h => h)
            .Select(g => (Hour: g.Key, Count: g.Count()))
            .OrderByDescending(x => x.Count)
            .First();
        sb.AppendLine($"⏰ Most active hour (UTC): {grouped.Hour:00}:00 ({grouped.Count} rolls)");
    }

    private static int LongestRun(IEnumerable<int> values, Func<int, bool> predicate)
    {
        var longest = 0;
        var current = 0;
        foreach (var v in values)
        {
            if (predicate(v))
            {
                current++;
                if (current > longest) longest = current;
            }
            else
            {
                current = 0;
            }
        }
        return longest;
    }
}
