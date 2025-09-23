using DiscordBot.Rollers.CharacterRollers.Models;
using DiscordBot.Rollers.Enums;

namespace DiscordBot.Rollers.CharacterRollers;

public abstract class WitchCharacterRoller : NewCharacterRollerBase
{
    private static readonly Dictionary<int, string> RunningFrom = new()
    {
        [1] = "The Inquisition.",
        [2] = "A killing in self defense.",
        [3] = "A dark pursuer.",
        [4] = "A man who thinks you owe him.",
        [5] = "A deal gone wrong.",
        [6] = "Your fate.",
    };

    private const string Info = 
        "You were blessed/cursed with the gifts of the wyrd,\n" +
        "and as such can access great and terrible magics.\n" +
        "But beware, those seen or merely suspected of using magic are more oft than not\n" +
        "lynched and burned, lest the witch hunters of the\n" +
        "inquisition come to town.\n" +
        "You start with two spells from the spell page 84.";

    private static readonly Dictionary<WitchSubType, string> SubTypeInformation = new()
    {
        [WitchSubType.WoodsWitch] =
            "🌲 WOODS WITCH\n\n" +
            "Many rightly fear the woods, but not you; they have always been your home.\n" +
            "No matter where you are, you feel the cold dew of a blue-grey morning under\n" +
            "dark branches.\n\n" +
            "🎯 Special Ability:\n" +
            "Forest through the trees\n" +
            "Roll Presence +2 to find your way in the woods.\n\n" +
            "🦅 Trusty Bird:\n" +
            "Your bird is loyal but only to you and despises\n" +
            "everyone else. You have a primeval link to its\n" +
            "thoughts and it will do as you command. It can scout,\n" +
            "keep watch and even attack your foes.\n\n" +
            "HP 4\n" +
            "Claws/bite d4\n\n" +
            "🎒 Starting Equipment:\n" +
            "• Talisman:\n" +
            "\tA small wood and stone item that can be used to\n" +
            "\tbless others. You can give +1 to any roll made by\n" +
            "\tyourself once per round.\n",
        [WitchSubType.Herbalist] =
            "🌿 HERBALIST\n\n" +
            "Some believe that magic is bestowed by the gods, others by\n" +
            "devils. But you know the truth is that magic is where\n" +
            "you find it, and how you brew it.\n\n" +
            "🎯 Special Ability:\n" +
            "You may brew d6 of the potions or poisons on\n" +
            "page 86-87 as long as you can scrounge up some\n" +
            "ingredients. Potions made in the wild\n" +
            "lose vitality after 24 hours.\n\n" +
            "🎒 Starting Equipment:\n" +
            "• Brewers Kit consisting of:\n" +
            "\t- Brass pot\n" +
            "\t- Weights and scales\n" +
            "\t- Flint and steel\n" +
            "\t- Six glass bottles with stops\n" +
            "\t- One pint of pure ethanol\n" +
            "• Healers Mask:\n" +
            "\tThis mask has glass lenses and a long beak stuffed\n" +
            "\twith healing herbs. This will protect you\n" +
            "\tfrom most airborne foul smells and airborne\n" +
            "\tmaladies. It also makes people uncomfortable.\n",
        [WitchSubType.Hexen] =
            "🕯️ HEXEN\n\n" +
            "Whether you made a pact with a demon or not, it matters none to those who\n" +
            "have already made up their minds about you. Your existence is a crime, why not\n" +
            "do more?\n\n" +
            "🎯 Special Ability:\n" +
            "You may curse one creature a day that has done you wrong.\n" +
            "All tests are an additional -2 on behalf of the creature and all\n" +
            "attacks upon it gain +2.\n\n" +
            "🎒 Starting Equipment:\n" +
            "• Black Candles:\n" +
            "\tThese inky black candles once lit, can\n" +
            "\toffer protection from malevolent spirits\n" +
            "\tand entities.\n" +
            "• Deck of Cards:\n" +
            "\tThese esoteric cards can be shuffled and drawn to\n" +
            "\tdivine the future, however the future is always dark\n" +
            "\tand vague.\n",
    };
    
    public static string Roll(WitchSubType subType)
    {
        var strength = GetStat(modifier: -2);
        var agility = GetStat(modifier: 0);
        var presence = GetStat(modifier: 2);
        var toughness = GetStat(modifier: 0);
        var hp = GetHp(toughness: GetAbilityModifier(toughness), modifier: 0, dice: 6);
        var runningFrom = GetRunningFrom();
        var gold = GetGold(numberOfd6: 1);

        var newCharacterDto = new NewCharacterDto
        {
            Strength = strength,
            Agility = agility,
            Presence = presence,
            Toughness = toughness,
            Hp = hp,
            ClassSpecificEvent = runningFrom,
            SpecificInfo = Info,
            Gold = gold,
            SubTypeSpecificInfo = SubTypeInformation[subType],
        };
        var newCharacterTemplate = GetNewCharacter(newCharacterDto);

        return GetCharacterResponseString(newCharacterTemplate);
    }
    
    private static string GetRunningFrom() => $"You are running from: {RunningFrom[Random.Shared.Next(1, 7)]}";
}