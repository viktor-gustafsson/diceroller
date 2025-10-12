using Discord.WebSocket;
using DiscordBot.Parsers;
using DiscordBot.Rollers.CharacterRollers;

namespace DiscordBot.Handlers;

public abstract class NewCharacterCommandHandler
{
    public static async Task Roll<TCharacterRoller, TKSubType>(SocketSlashCommand command)
        where TCharacterRoller : ICharacterRoller<TKSubType> where TKSubType : Enum
    {
        var subType = CharacterSubTypeParser.Parse<TKSubType>(command);
        var character = TCharacterRoller.Roll(subType);
        await command.RespondAsync(character, ephemeral: false);
    }
}