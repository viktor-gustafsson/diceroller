using DiscordBot.Handlers;
using DiscordBot.Services;

var discordBotToken = Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN");

ArgumentException.ThrowIfNullOrEmpty(discordBotToken);

var discordCommandHandler = new DiscordCommandHandler(discordBotToken);

await discordCommandHandler.Start();