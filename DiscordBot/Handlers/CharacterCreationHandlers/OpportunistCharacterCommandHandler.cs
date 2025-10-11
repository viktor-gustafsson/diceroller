using Discord.WebSocket;
using DiscordBot.Rollers.CharacterRollers;
using DiscordBot.Rollers.CharacterRollers.Enums;
using DiscordBot.Utilities;

namespace DiscordBot.Handlers.CharacterCreationHandlers;

public abstract class OpportunistCharacterCommandHandler
{
    public static async Task Handle(SocketSlashCommand command)
    {
        var subType = CharacterSubTypeParser.Parse<OpportunistSubType>(command);
        var newOpportunistCharacter = OpportunistCharacterRoller.Roll(subType);
        await command.RespondAsync(newOpportunistCharacter, ephemeral: false);
    }
}