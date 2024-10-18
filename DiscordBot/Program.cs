using DiscordBot;

var token = Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN");

if (string.IsNullOrWhiteSpace(token))
    throw new ArgumentNullException(token, "Missing discord bot token");

var diceRoller = new DiceRoller(token);

await diceRoller.StartBot();