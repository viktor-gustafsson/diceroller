using DiscordBot.Rollers.CharacterRollers.Models;
using DiscordBot.Rollers.Enums;

namespace DiscordBot.Rollers.CharacterRollers;

public abstract class PractitionerCharacterRoller : NewCharacterRollerBase
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
            "fall unconscious at 0 HP.\n\n" +
            "🎒 Starting Equipment:\n" +
            "• Heavy Armour\n" +
            "• Short Sword\n" +
            "• Badge of Honour:\n" +
            "\tYou wear a badge of honour and\n" +
            "\trespect in combat. This will\n" +
            "\tcommand respect from the faithful.\n",
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
            "2 rounds.\n\n" +
            "🎒 Starting Equipment:\n" +
            "• Needle\n" +
            "• Catgut thread\n" +
            "• Gauze and bandages\n" +
            "• Surgical tongs\n" +
            "• Ointment\n" +
            "• Smelling salts\n" +
            "• Poppy extract\n" +
            "• Small sharp knife\n" +
            "• Cauterising iron\n" +
            "• Tincture\n",
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
            "humble skillet.\n\n"+
            "🎒 Starting Equipment:\n" +
            "• 8 rations and two water skins\n" +
            "• Box of salt\n" +
            "• Cooking pot\n" +
            "• Skillet\n" +
            "• Cutting knife and spoon\n" +
            "• Enough bowls for the rest of the party\n" +
            "• A precious cache of herbs and spices\n",
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
        };
        var newCharacterTemplate = GetNewCharacter(newCharacterDto);

        return GetCharacterResponseString(newCharacterTemplate);
    }
    
    private static string GetSecret()
    {
        var random = new Random();
        return $"You have a secret: {Secrets[random.Next(1, 7)]}";
    }
}