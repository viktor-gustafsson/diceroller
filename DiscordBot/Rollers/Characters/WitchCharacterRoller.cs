namespace DiscordBot.Rollers.Characters;

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
    
    public static string Roll()
    {
        var strength = GetStat(modifier: -2);
        var agility = GetStat(modifier: 0);
        var presence = GetStat(modifier: 2);
        var toughness = GetStat(modifier: 0);
        var hp = GetHp(toughness: GetAbilityModifier(toughness), modifier: 0, dice: 6);
        var runningFrom = GetRunningFrom();
        var gold = RollForGold(1);

        var newCharacterTemplate = GetNewCharacterTemplate(strength: strength, agility: agility, presence: presence, toughness: toughness, hp: hp, gold: gold, classSpecificEvent: runningFrom, specificInfo: Info);

        return GetCharacterResponseString(newCharacterTemplate);
    }
    
    private static string GetRunningFrom()
    {
        var random = new Random();
        return $"You are running from: {RunningFrom[random.Next(1, 7)]}";
    }
}