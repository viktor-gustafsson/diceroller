using Discord.WebSocket;
using DiscordBot.Rollers.CharacterRollers;
using DiscordBot.Rollers.CharacterRollers.Enums;
using DiscordBot.Utilities;

namespace DiscordBot.Handlers.CharacterCreationHandlers;

public abstract class PractitionerCharacterCommandHandler
{
    public static async Task Handle(SocketSlashCommand command)
    {
        var subType = CharacterSubTypeParser.Parse<PractitionerSubType>(command);
        var newPractitionerCharacter = PractitionerCharacterRoller.Roll(subType);
        await command.RespondAsync(newPractitionerCharacter, ephemeral: false);
    }
}