namespace DiscordBot.Rollers.Characters;

public abstract class GenericNewCharacterRoller: NewCharacterRollerBase
{
    public static string Roll()
    {
        var strength = GetStat();
        var agility = GetStat();
        var presence = GetStat();
        var toughness = GetStat();
        var hp = GetHp(GetAbilityModifier(toughness), modifier: 2, dice: 8);
        var gold = RollForGold(4);
        
        var newCharacterTemplate = GetNewCharacterTemplate(strength: strength, agility: agility, presence: presence, toughness: toughness, hp: hp, gold: gold, classSpecificEvent: "", specificInfo: "");

        return GetCharacterResponseString(newCharacterTemplate);
    }
}