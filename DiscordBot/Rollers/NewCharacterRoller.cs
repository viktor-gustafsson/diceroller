namespace DiscordBot.Rollers;

public static class NewCharacterRoller
{
    private static readonly Dictionary<int, int> AbilityStartValue = new()
    {
        [1] = -3,
        [2] = -3,
        [3] = -3,
        [4] = -3,
        [5] = -2,
        [6] = -2,
        [7] = -1,
        [8] = -1,
        [9] = 0,
        [10] = 0,
        [11] = 0,
        [12] = 0,
        [13] = 1,
        [14] = 1,
        [15] = 2,
        [16] = 2,
        [17] = 3,
        [18] = 3,
    };
    
    public static string Roll()
    {
        var strength = GetStat();
        var agility = GetStat();
        var presence = GetStat();
        var toughness = GetStat();

        var hp = GetHp(AbilityStartValue[toughness]);

        return
            $"```\n" +
            $"🎲 NEW CHARACTER STATS\n\n" +
            $"💪 Strength:  [{AbilityStartValue[strength],3}]  (Rolled: {strength,2})\n" +
            $"🏃 Agility:   [{AbilityStartValue[agility],3}]  (Rolled: {agility,2})\n" +
            $"👑 Presence:  [{AbilityStartValue[presence],3}]  (Rolled: {presence,2})\n" +
            $"🛡️ Toughness: [{AbilityStartValue[toughness],3}]  (Rolled: {toughness,2})\n\n" +
            $"❤️ Hit Points: {hp}\n" +
            $"```";;
    }

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