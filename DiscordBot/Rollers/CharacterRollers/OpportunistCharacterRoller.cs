using DiscordBot.Equipment;
using DiscordBot.Rollers.CharacterRollers.Enums;
using DiscordBot.Rollers.CharacterRollers.Models;

namespace DiscordBot.Rollers.CharacterRollers;

public abstract class OpportunistCharacterRoller : NewCharacterRollerBase, ICharacterRoller<OpportunistSubType>
{
    private static readonly Dictionary<int, string> Desires = new()
    {
        [1] = "To drown in gold.",
        [2] = "To buy back your family's estate.",
        [3] = "To pay off your debt to a dangerous criminal.",
        [4] = "To never return to the gutter.",
        [5] = "To prove yourself worthy of their love.",
        [6] = "To die before you become old and frail.",
    };

    private static readonly Dictionary<OpportunistSubType, string> SubTypeInformation = new()
    {
        [OpportunistSubType.Adventurer] =
            "⚔️ ADVENTURER\n\n" +
            "You seek a family heirloom that will restore\n" +
            "honour to your house and put to rest the\n" +
            "angry ghosts of your ancestors. At least...\n" +
            "that's what you tell everyone.\n\n" +
            "🎯 Special Ability:\n" +
            "Agility rolls for performing acrobatics\n" +
            "gain +2.\n\n",
        [OpportunistSubType.SneakThief] =
            "🗡️ SNEAK THIEF\n\n" +
            "Fast hands make for light pockets. No\n" +
            "trinket or treasure is safe from you,\n" +
            "and your mark was never the wiser.\n" +
            "You make enemies faster than you can\n" +
            "count, because you are already counting\n" +
            "their coin.\n\n" +
            "🎯 Special Ability:\n" +
            "Picking pockets and locks gains\n" +
            "+2 to a roll.\n\n",
        [OpportunistSubType.SilverTonguedTrickster] =
            "🎭 SILVER-TONGUED TRICKSTER\n\n" +
            "You are a lover, not a fighter. That being\n" +
            "said, you are sure you could fight off a\n" +
            "dozen armed men to save your beloved\n" +
            "for that night.\n\n" +
            "🎯 Special Ability:\n" +
            "Gain +2 to rolls pertaining to convincing\n" +
            "or lying to others.\n\n",
    };

    private const string Info = "Agility tests are DR10";

    public static string Roll(OpportunistSubType subType)
    {
        var strength = GetStat(modifier: -2);
        var agility = GetStat(modifier: 0);
        var presence = GetStat(modifier: 0);
        var toughness = GetStat(modifier: 0);
        var hp = GetHp(toughness: GetAbilityModifier(toughness), modifier: 0, dice: 6);
        var desire = GetDesire();
        var gold = GetGold(numberOfd6: 2);

        var equipment = new List<EquipmentItem>();
        equipment.AddRange(CharacterEquipmentSets.CommonStartingGear);
        equipment.AddRange(CharacterEquipmentSets.GetEquipmentFor(subType));

        var newCharacterDto = new NewCharacterDto
        {
            Strength = strength,
            Agility = agility,
            Presence = presence,
            Toughness = toughness,
            Hp = hp,
            ClassSpecificEvent = desire,
            SpecificInfo = Info,
            Gold = gold,
            SubTypeSpecificInfo = SubTypeInformation[subType],
            Equipment = equipment,
        };
        var newCharacterTemplate = GetNewCharacter(newCharacterDto);

        return GetCharacterResponseString(newCharacterTemplate);
    }

    private static string GetDesire() => $"You desire more than anything: {Desires[Random.Shared.Next(1, Desires.Count + 1)]}";
}
