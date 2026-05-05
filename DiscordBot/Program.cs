using DiscordBot.Handlers;
using DiscordBot.Statistics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var discordBotToken = Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN");
ArgumentException.ThrowIfNullOrEmpty(discordBotToken);

var dbPath = Environment.GetEnvironmentVariable("DICE_BOT_DB_PATH") ?? "dicestats.db";

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContextFactory<StatisticsDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddSingleton<IRollRecorder, RollRecorder>();
builder.Services.AddHostedService(sp =>
    new DiscordCommandHandler(
        discordBotToken,
        sp.GetRequiredService<IRollRecorder>(),
        sp.GetRequiredService<IDbContextFactory<StatisticsDbContext>>()));

var host = builder.Build();

await using (var scope = host.Services.CreateAsyncScope())
{
    var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<StatisticsDbContext>>();
    await using var ctx = await contextFactory.CreateDbContextAsync();
    await ctx.Database.EnsureCreatedAsync();
}

await host.RunAsync();
