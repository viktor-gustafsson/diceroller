using System.Text;
using Discord.WebSocket;
using DiscordBot.Statistics;
using DiscordBot.Statistics.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordBot.Handlers;

public static class StatsCommandHandler
{
    private const int BarWidth = 15;
    private const char BarChar = '█';

    public static async Task Handle(SocketSlashCommand command, IDbContextFactory<StatisticsDbContext> contextFactory)
    {
        if (command.GuildId is not { } guildId)
        {
            await command.RespondAsync("📊 Stats are only available inside a server.", ephemeral: true);
            return;
        }

        var sub = command.Data.Options.First();
        var subOptions = sub.Options;
        var makePublic = subOptions.FirstOrDefault(o => o.Name == Constants.StatsPublicOptionName)?.Value as bool? ?? false;

        var response = sub.Name switch
        {
            Constants.StatsSubDistribution => await BuildDistribution(contextFactory, guildId,
                (long)(subOptions.First(o => o.Name == Constants.StatsDieOptionName).Value)),
            Constants.StatsSubCrits => await BuildCrits(contextFactory, guildId),
            Constants.StatsSubTop => await BuildTop(contextFactory, guildId),
            Constants.StatsSubStreaks => await BuildStreaks(contextFactory, guildId),
            Constants.StatsSubHours => await BuildHours(contextFactory, guildId),
            _ => "📊 Unknown stats subcommand.",
        };

        await command.RespondAsync(Trim(response), ephemeral: !makePublic);
    }

    private static async Task<string> BuildDistribution(
        IDbContextFactory<StatisticsDbContext> contextFactory, ulong guildId, long dieType)
    {
        await using var ctx = await contextFactory.CreateDbContextAsync();

        var rows = await (
            from d in ctx.RollDice
            join r in ctx.Rolls on d.RollId equals r.Id
            where r.GuildId == guildId && d.DieType == (int)dieType
            group d by d.Value into g
            select new { Value = g.Key, Count = g.Count() }).ToListAsync();

        var total = rows.Sum(r => r.Count);
        if (total == 0)
            return $"🎲 No d{dieType} rolls recorded yet on this server.";

        var byValue = rows.ToDictionary(r => r.Value, r => r.Count);
        var max = rows.Max(r => r.Count);

        var sb = new StringBuilder();
        sb.AppendLine("```");
        sb.AppendLine($"🎲 d{dieType} Distribution — this server");
        sb.AppendLine($"Total rolls: {total}");
        sb.AppendLine();

        for (var v = 1; v <= dieType; v++)
        {
            var count = byValue.GetValueOrDefault((int)v, 0);
            var bar = new string(BarChar, ScaleBar(count, max));
            var pct = (double)count / total * 100;
            sb.AppendLine($"{v,3}: {bar,-BarWidth} {count} ({pct:0.0}%)");
        }

        sb.AppendLine("```");
        return sb.ToString();
    }

    private static async Task<string> BuildCrits(
        IDbContextFactory<StatisticsDbContext> contextFactory, ulong guildId)
    {
        await using var ctx = await contextFactory.CreateDbContextAsync();

        var rows = await (
            from d in ctx.RollDice
            join r in ctx.Rolls on d.RollId equals r.Id
            where r.GuildId == guildId && d.DieType == 20
            select new { r.UserId, d.Value }).ToListAsync();

        if (rows.Count == 0)
            return "🎯 No d20 rolls recorded yet on this server.";

        var names = await GetLatestNames(ctx, guildId);

        var perUser = rows
            .GroupBy(x => x.UserId)
            .Select(g => new
            {
                UserId = g.Key,
                Total = g.Count(),
                Nat20 = g.Count(x => x.Value == 20),
                Nat1 = g.Count(x => x.Value == 1),
            })
            .OrderByDescending(x => x.Total)
            .ToList();

        var sb = new StringBuilder();
        sb.AppendLine("```");
        sb.AppendLine("🎯 Critical Rolls — d20 (this server)");
        sb.AppendLine();

        foreach (var u in perUser)
        {
            var name = SafeName(names.GetValueOrDefault(u.UserId, u.UserId.ToString()));
            var nat20Pct = (double)u.Nat20 / u.Total * 100;
            var nat1Pct = (double)u.Nat1 / u.Total * 100;
            sb.AppendLine($"{name}");
            sb.AppendLine($"  rolls: {u.Total}  nat20: {u.Nat20} ({nat20Pct:0.0}%)  nat1: {u.Nat1} ({nat1Pct:0.0}%)");
        }

        sb.AppendLine("```");
        return sb.ToString();
    }

    private static async Task<string> BuildTop(
        IDbContextFactory<StatisticsDbContext> contextFactory, ulong guildId)
    {
        await using var ctx = await contextFactory.CreateDbContextAsync();

        var rolls = await ctx.Rolls
            .Where(r => r.GuildId == guildId)
            .Select(r => new { r.UserId, r.RollType })
            .ToListAsync();

        if (rolls.Count == 0)
            return "🏆 No rolls recorded yet on this server.";

        var names = await GetLatestNames(ctx, guildId);

        var leaderboard = rolls
            .GroupBy(x => x.UserId)
            .Select(g => new
            {
                UserId = g.Key,
                Total = g.Count(),
                Standard = g.Count(x => x.RollType == RollType.Standard),
                Effect = g.Count(x => x.RollType == RollType.Effect),
                Character = g.Count(x => x.RollType == RollType.Character),
            })
            .OrderByDescending(x => x.Total)
            .ToList();

        var sb = new StringBuilder();
        sb.AppendLine("```");
        sb.AppendLine("🏆 Roll Leaderboard — this server");
        sb.AppendLine();

        for (var i = 0; i < leaderboard.Count; i++)
        {
            var l = leaderboard[i];
            var name = SafeName(names.GetValueOrDefault(l.UserId, l.UserId.ToString()));
            sb.AppendLine($"{i + 1,2}. {name}");
            sb.AppendLine($"    total: {l.Total}  standard: {l.Standard}  effect: {l.Effect}  char: {l.Character}");
        }

        sb.AppendLine("```");
        return sb.ToString();
    }

    private static async Task<string> BuildStreaks(
        IDbContextFactory<StatisticsDbContext> contextFactory, ulong guildId)
    {
        await using var ctx = await contextFactory.CreateDbContextAsync();

        var rolls = await (
            from d in ctx.RollDice
            join r in ctx.Rolls on d.RollId equals r.Id
            where r.GuildId == guildId && d.DieType == 20
            orderby r.Timestamp, d.Position
            select new { r.UserId, d.Value }).ToListAsync();

        if (rolls.Count == 0)
            return "🔥 No d20 rolls recorded yet on this server.";

        var names = await GetLatestNames(ctx, guildId);

        var perUser = rolls
            .GroupBy(x => x.UserId)
            .Select(g => new
            {
                UserId = g.Key,
                Hot = LongestRun(g.Select(x => x.Value), v => v >= 11),
                Cold = LongestRun(g.Select(x => x.Value), v => v <= 10),
            })
            .OrderByDescending(x => Math.Max(x.Hot, x.Cold))
            .ToList();

        var sb = new StringBuilder();
        sb.AppendLine("```");
        sb.AppendLine("🔥 d20 Streaks — this server (consecutive ≥11 hot, ≤10 cold)");
        sb.AppendLine();

        foreach (var u in perUser)
        {
            var name = SafeName(names.GetValueOrDefault(u.UserId, u.UserId.ToString()));
            sb.AppendLine($"{name}");
            sb.AppendLine($"    hot: {u.Hot}  cold: {u.Cold}");
        }

        sb.AppendLine("```");
        return sb.ToString();
    }

    private static async Task<string> BuildHours(
        IDbContextFactory<StatisticsDbContext> contextFactory, ulong guildId)
    {
        await using var ctx = await contextFactory.CreateDbContextAsync();

        var times = await ctx.Rolls
            .Where(r => r.GuildId == guildId)
            .Select(r => r.Timestamp)
            .ToListAsync();

        if (times.Count == 0)
            return "⏰ No rolls recorded yet on this server.";

        var byHour = times
            .GroupBy(t => t.Hour)
            .ToDictionary(g => g.Key, g => g.Count());

        var max = byHour.Values.Max();

        var sb = new StringBuilder();
        sb.AppendLine("```");
        sb.AppendLine("⏰ Activity by Hour (UTC) — this server");
        sb.AppendLine($"Total rolls: {times.Count}");
        sb.AppendLine();

        for (var h = 0; h < 24; h++)
        {
            var count = byHour.GetValueOrDefault(h, 0);
            var bar = new string(BarChar, ScaleBar(count, max));
            sb.AppendLine($"{h:00}: {bar,-BarWidth} {count}");
        }

        sb.AppendLine("```");
        return sb.ToString();
    }

    private static async Task<Dictionary<ulong, string>> GetLatestNames(StatisticsDbContext ctx, ulong guildId)
    {
        var pairs = await ctx.Rolls
            .Where(r => r.GuildId == guildId)
            .Select(r => new { r.UserId, r.UserDisplayName, r.Timestamp })
            .ToListAsync();

        return pairs
            .GroupBy(p => p.UserId)
            .ToDictionary(g => g.Key, g => g.OrderByDescending(p => p.Timestamp).First().UserDisplayName);
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

    private static int ScaleBar(int count, int max) =>
        max == 0 ? 0 : (int)Math.Round((double)count / max * BarWidth);

    private static string SafeName(string name) => name.Replace("`", "'");

    private static string Trim(string text) =>
        text.Length <= 1900 ? text : text[..1900] + "\n…(truncated)\n```";
}
