using Discord.WebSocket;
using DiscordBot.Rollers.CharacterRollers;
using DiscordBot.Rollers.CharacterRollers.Enums;
using DiscordBot.Utilities;

namespace DiscordBot.Handlers.CharacterCreationHandlers;

public abstract class WitchCharacterCreationCommandHandler
{
    public static async Task Handle(SocketSlashCommand command)
    {
        var subType = CharacterSubTypeParser.Parse<WitchSubType>(command);
        var newWitchCharacter = WitchCharacterRoller.Roll(subType);
        await command.RespondAsync(newWitchCharacter, ephemeral: false);
    }
}