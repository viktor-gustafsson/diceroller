using DiscordBot.Services;

var discordBotToken = Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN");

ArgumentException.ThrowIfNullOrEmpty(discordBotToken);

var diceRoller = new DiceRoller(discordBotToken);

await diceRoller.StartBot();