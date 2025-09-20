namespace DiscordBot.Rollers.Characters;

public abstract class MercenaryDeserterCharacterRoller : NewCharacterRollerBase
{
    private static readonly Dictionary<int, string> Memories = new()
    {
        [1] = "The savage ransacking of a town by you and your fellows.",
        [2] = "The massacre of your company.",
        [3] = "Your beloved commander ripped apart by a demon.",
        [4] = "Stacks of corpses set alight, the flames billowing to the sky.",
        [5] = "A field of crows picking at the dead, beneath black banners.",
        [6] = "Your brother, executed for cowardice and desertion.",
    };
    
    private const string Info = "Normal agility tests are DR14";
    
    public static string Roll()
    {
        var strength = GetStat(modifier: 2);
        var agility = GetStat(modifier: -1);
        var presence = GetStat(modifier: -1);
        var toughness = GetStat(modifier: 2);
        var hp = GetHp(toughness: GetAbilityModifier(toughness), modifier: 0, dice: 10);
        var memory = GetMemory();
        var gold = RollForGold(numberOfd6: 2);

        var newCharacterTemplate = GetNewCharacter(strength: strength, agility: agility, presence: presence, toughness: toughness, hp: hp, gold: gold, classSpecificEvent: memory, specificInfo: Info);

        return GetCharacterResponseString(newCharacterTemplate);
    }
    
    private static string GetMemory()
    {
        var random = new Random();
        return $"You saw something that will haunt you forever: {Memories[random.Next(1, 7)]}";
    }
}