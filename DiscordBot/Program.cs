using DiscordBot;

var token = Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN");
var diceRoller = new DiceRoller(token);

await diceRoller.StartBot();