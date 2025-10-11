using Discord.WebSocket;
using DiscordBot.Rollers.CharacterRollers;
using DiscordBot.Rollers.CharacterRollers.Enums;
using DiscordBot.Utilities;

namespace DiscordBot.Handlers.CharacterCreationHandlers;

public abstract class BountyHunterCharacterCreationCommandHandler
{
    public static async Task Handle(SocketSlashCommand command)
    {
        var subType = CharacterSubTypeParser.Parse<BountyHunterSubType>(command);
        var newBountyHunterCharacter = BountyHunterCharacterRoller.Roll(subType);
        await command.RespondAsync(newBountyHunterCharacter, ephemeral: false);
    }
}