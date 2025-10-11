using Discord.WebSocket;
using DiscordBot.Rollers.CharacterRollers;
using DiscordBot.Utilities;

namespace DiscordBot.Handlers;

public abstract class RollNewCharacterCommandHandler
{
    public static async Task Roll<T, TK>(SocketSlashCommand command) where T : ICharacterRoller<TK> where TK : Enum
    {
        var subType = CharacterSubTypeParser.Parse<TK>(command);
        var newBountyHunterCharacter = T.Roll(subType);
        await command.RespondAsync(newBountyHunterCharacter, ephemeral: false);
    }
}