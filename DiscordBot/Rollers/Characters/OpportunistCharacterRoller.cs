using DiscordBot.Rollers.Enums;

namespace DiscordBot.Rollers.Characters;

public abstract class OpportunistCharacterRoller : NewCharacterRollerBase
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
            "gain +2.\n\n" +
            "🎒 Starting Equipment:\n" +
            "• A grappling hook and rope\n" +
            "• Crossbow with ten bolts\n" +
            "• The Broken Compass:\n" +
            "\tThis compass does not point north,\n" +
            "\tit does however point to the thing\n" +
            "\tyou most desire.\n\n" +
            "• Sportsman's Sword d6+2:\n" +
            "\tDR10 to disarm an opponent of\n" +
            "\ttheir held weapon rather than\n" +
            "\tinflict harm.\n",
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
            "+2 to a roll.\n\n" +
            "🎒 Starting Equipment:\n" +
            "• Grappling Hook\n" +
            "• Crossbow:\n" +
            "\tThis custom made crossbow will shoot\n" +
            "\ta grappling hook and rope up to\n" +
            "\t30ft. The rope is light and can only\n" +
            "\tsupport one person at a time.\n" +
            "• Smoke bombs x 5:\n" +
            "\tOnce thrown, these will cover a ten\n" +
            "\tfoot sphere and dissipate after a\n" +
            "\tcouple of seconds.\n",
        [OpportunistSubType.SilverTonguedTrickster] =
            "🎭 SILVER-TONGUED TRICKSTER\n\n" +
            "You are a lover, not a fighter. That being\n" +
            "said, you are sure you could fight off a\n" +
            "dozen armed men to save your beloved\n" +
            "for that night.\n\n" +
            "🎯 Special Ability:\n" +
            "Gain +2 to rolls pertaining to convincing\n" +
            "or lying to others.\n\n" +
            "🎒 Starting Equipment:\n" +
            "• Lucky Coin:\n" +
            "\tA large brilliantly shiny gold coin\n" +
            "\tfrom a foreign land. It will return\n" +
            "\tto your hands in a few minutes.\n" +
            "\tPerfect for bribery.\n" +
            "• Perfume:\n" +
            "\tThe smell of a rose is irresistible to all that smell\n" +
            "\tit. Perfect for those with no time to bathe.\n",
    };
        
    private const string Info = "Agility tests are DR10"; 
    
    public static string Roll(OpportunistSubType subType)
    {
        var strength = GetStat(modifier: -2);
        var agility = GetStat(modifier: 0);
        var presence = GetStat(modifier: 2);
        var toughness = GetStat(modifier: 0);
        var hp = GetHp(toughness: GetAbilityModifier(toughness), modifier: 0, dice: 6);
        var desire = GetDesire();
        var gold = GetGold(2);

        var newCharacterTemplate = GetNewCharacter(strength: strength, agility: agility, presence: presence, toughness: toughness, hp: hp, gold: gold, classSpecificEvent: desire, specificInfo: Info);
        newCharacterTemplate.SubTypeSpecificInfo = SubTypeInformation[subType];

        return GetCharacterResponseString(newCharacterTemplate);
    }
    
    private static string GetDesire()
    {
        var random = new Random();
        return $"You desire more than anything: {Desires[random.Next(1, 7)]}";
    }
}