using Discord.WebSocket;

namespace DiscordBot.Utilities;

public static class CharacterSubTypeParser
{
    public static T Parse<T>(SocketSlashCommand command)
    {
        var socketSlashCommandDataOption = Convert.ToInt32(command.Data.Options.First().Value);
        return (T)Enum.ToObject(typeof(T), socketSlashCommandDataOption);
    }
}