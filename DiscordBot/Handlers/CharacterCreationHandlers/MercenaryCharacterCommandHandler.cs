using Discord.WebSocket;
using DiscordBot.Rollers.CharacterRollers;
using DiscordBot.Rollers.CharacterRollers.Enums;
using DiscordBot.Utilities;

namespace DiscordBot.Handlers.CharacterCreationHandlers;

public abstract class MercenaryCharacterCommandHandler
{
    public static async Task Handle(SocketSlashCommand command)
    {
        var subType = CharacterSubTypeParser.Parse<MercenarySubType>(command);
        var newMercenaryDeserter = MercenaryDeserterCharacterRoller.Roll(subType);
        await command.RespondAsync(newMercenaryDeserter, ephemeral: false);
    }
}