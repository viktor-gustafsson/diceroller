using DiscordBot.Equipment;
using DiscordBot.Rollers.CharacterRollers.Enums;
using DiscordBot.Rollers.CharacterRollers.Models;

namespace DiscordBot.Rollers.CharacterRollers;

public abstract class PractitionerCharacterRoller : NewCharacterRollerBase, ICharacterRoller<PractitionerSubType>
{
    private static readonly Dictionary<int, string> Secrets = new()
    {
        [1] = "You murdered your superior after you saw what they had done.",
        [2] = "You stole this Practitioner's identity after they died at your feet.",
        [3] = "Deep down, you lost your faith. But you carry on anyway.",
        [4] = "You have worked as a spy for years.",
        [5] = "Your real good deeds will not absolve the sins of your past.",
        [6] = "You have seen the terrible secret of the Sanguine Church.",
    };

    private static readonly Dictionary<PractitionerSubType, string> SubTypeInformation = new()
    {
        [PractitionerSubType.VowOfWar] =
            "⚔️ VOW OF WAR\n\n" +
            "Combat is inevitable in these\n" +
            "troubling times. You trained in the\n" +
            "arts of war to defend the full from\n" +
            "any threat, human and demonic alike.\n\n" +
            "🎯 Special Ability:\n" +
            "When your health drops to 0\n" +
            "in combat, you may roll DR12\n" +
            "Toughness to regain d4 hit\n" +
            "points for one round. If you do\n" +
            "not heal by the next round you\n" +
            "fall unconscious at 0 HP.\n\n",
        [PractitionerSubType.VowOfHealing] =
            "⛑️ VOW OF HEALING\n\n" +
            "Some practice their faith through\n" +
            "blood and fire; you practice yours\n" +
            "with the needle, bandage and poultice.\n\n" +
            "🎯 Special Ability:\n" +
            "When not in combat, you may\n" +
            "heal a character for 2d4 HP, up\n" +
            "to their max HP.\n\n" +
            "🏥 Surgical Precision:\n" +
            "You may declare a called attack\n" +
            "on a creature at DR14. On a\n" +
            "success, the creature suffers d4\n" +
            "internal bleeding damage for\n" +
            "2 rounds.\n\n",
        [PractitionerSubType.VovOfSustenance] =
            "🍞 VOW OF SUSTENANCE\n\n" +
            "It is written that the Torn Prophet fed a\n" +
            "thousand people with nought but a grain\n" +
            "of rice and an eel. You cannot boast such\n" +
            "talents but you try your best.\n\n" +
            "🎯 Special Ability:\n" +
            "Provided you have some\n" +
            "ingredients, you may make a\n" +
            "hearty meal for you and your\n" +
            "companions that recovers d8 HP\n" +
            "and gives a +2 to Agility, Strength\n" +
            "and Toughness rolls for the rest\n" +
            "of the day.\n\n" +
            "🍳 Cast Iron Wallop:\n" +
            "Succeed a DR12 Strength roll to\n" +
            "knock a human sized creature\n" +
            "unconscious for d4 rounds.\n" +
            "The pan can also be used to\n" +
            "parry like a sword. Not bad for a\n" +
            "humble skillet.\n\n",
    };

    public static string Roll(PractitionerSubType subType)
    {
        var strength = GetStat(modifier: 1);
        var agility = GetStat(modifier: -1);
        var presence = GetStat(modifier: -1);
        var toughness = GetStat(modifier: 1);
        var hp = GetHp(toughness: GetAbilityModifier(toughness), modifier: 0, dice: 12);
        var secret = GetSecret();
        var gold = GetGold(numberOfd6: 1);

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
            ClassSpecificEvent = secret,
            SpecificInfo = "",
            Gold = gold,
            SubTypeSpecificInfo = SubTypeInformation[subType],
            Equipment = equipment,
        };
        var newCharacterTemplate = GetNewCharacter(newCharacterDto);

        return GetCharacterResponseString(newCharacterTemplate);
    }

    private static string GetSecret() => $"You have a secret: {Secrets[Random.Shared.Next(1, Secrets.Count + 1)]}";
}
