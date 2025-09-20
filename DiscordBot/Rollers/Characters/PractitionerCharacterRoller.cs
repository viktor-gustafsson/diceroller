namespace DiscordBot.Rollers.Characters;

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
    
    public static string Roll()
    {
        var strength = GetStat(modifier: 1);
        var agility = GetStat(modifier: -1);
        var presence = GetStat(modifier: -1);
        var toughness = GetStat(modifier: 1);
        var hp = GetHp(toughness: GetAbilityModifier(toughness), modifier: 0, dice: 12);
        var secret = GetSecret();
        var gold = GetGold(1);

        var newCharacterTemplate = GetNewCharacter(strength: strength, agility: agility, presence: presence, toughness: toughness, hp: hp, gold: gold, classSpecificEvent: secret, specificInfo: "");

        return GetCharacterResponseString(newCharacterTemplate);
    }
    
    private static string GetSecret()
    {
        var random = new Random();
        return $"You have a secret: {Secrets[random.Next(1, 7)]}";
    }
}