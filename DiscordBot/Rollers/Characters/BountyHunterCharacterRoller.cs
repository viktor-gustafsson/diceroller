namespace DiscordBot.Rollers.Characters;

public class BountyHunterCharacterRoller : NewCharacterRollerBase
{
    private static readonly Dictionary<int, string> Memories = new()
    {
        [1] = "The screams in the night as wild laughter rang out.",
        [2] = "The thrill of your first kill, your father's proud hand on your shoulder.",
        [3] = "Eyes watching you from the forest, burning and hungry.",
        [4] = "The glimmer of the coins handed to you for your first bounty.",
        [5] = "The blood as it dripped down the bars of the black iron cage.",
        [6] = "The warm touch of their hand on a cold winter's night.",
    };

    public static string Roll()
    {
        var strength = GetStat(modifier: 1);
        var agility = GetStat(modifier: 0);
        var presence = GetStat(modifier: 2);
        var toughness = GetStat(modifier: 0);
        var hp = GetHp(toughness: GetAbilityModifier(toughness), modifier: 0, dice: 8);
        var memory = GetMemory();
        var gold = RollForGold(numberOfd6: 3);
        
        var newCharacterTemplate = GetNewCharacterTemplate(strength: strength, agility: agility, presence: presence, toughness: toughness, hp: hp, gold: gold, classSpecificEvent: memory, specificInfo: "");

        return GetCharacterResponseString(newCharacterTemplate);
    }
    
    private static string GetMemory()
    {
        var random = new Random();
        return $"You will always remember: {Memories[random.Next(1, 7)]}";
    }
}