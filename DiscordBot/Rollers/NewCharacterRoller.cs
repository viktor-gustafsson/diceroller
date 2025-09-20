namespace DiscordBot.Rollers;

public static class NewCharacterRoller
{
    public static string Roll()
    {
        var strength = GetStat();
        var agility = GetStat();
        var presence = GetStat();
        var toughness = GetStat();

        var hp = GetHp(GetAbilityModifier(toughness));

        return
            $"```\n" +
            $"🎲 NEW CHARACTER STATS\n\n" +
            $"💪 Strength:  [{GetAbilityModifier(strength),3}]  (Rolled: {strength,2})\n" +
            $"🏃 Agility:   [{GetAbilityModifier(agility),3}]  (Rolled: {agility,2})\n" +
            $"👑 Presence:  [{GetAbilityModifier(presence),3}]  (Rolled: {presence,2})\n" +
            $"🛡️ Toughness: [{GetAbilityModifier(toughness),3}]  (Rolled: {toughness,2})\n\n" +
            $"❤️ Hit Points: {hp}\n" +
            $"```";;
    }
    
    private static int GetAbilityModifier(int abilityScore) => (abilityScore - 10) / 2;

    private static int GetHp(int toughness)
    {
        var random = new Random();

        var d8 = random.Next(1,9);
        var hp = d8+toughness+2;
        
        return hp < 1 ? 1 : hp;
    }

    private static int GetStat()
    {
        var random = new Random();
        var sum = 0;
        for (var i = 0; i < 3; i++)
        {
            sum += random.Next(1,7);
        }

        return sum;
    }
}