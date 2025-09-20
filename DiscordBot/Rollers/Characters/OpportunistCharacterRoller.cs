namespace DiscordBot.Rollers.Characters;

public class OpportunistCharacterRoller : NewCharacterRollerBase
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

    private const string Info = "Agility tests are DR10"; 
    
    public static string Roll()
    {
        var strength = GetStat(modifier: -2);
        var agility = GetStat(modifier: 0);
        var presence = GetStat(modifier: 2);
        var toughness = GetStat(modifier: 0);
        var hp = GetHp(toughness: GetAbilityModifier(toughness), modifier: 0, dice: 6);
        var desire = GetDesire();
        var gold = RollForGold(2);

        var newCharacterTemplate = GetNewCharacterTemplate(strength: strength, agility: agility, presence: presence, toughness: toughness, hp: hp, gold: gold, classSpecificEvent: desire, specificInfo: Info);

        return GetCharacterResponseString(newCharacterTemplate);
    }
    
    private static string GetDesire()
    {
        var random = new Random();
        return $"You desire more than anything: {Desires[random.Next(1, 7)]}";
    }
}