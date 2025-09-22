namespace DiscordBot.Rollers.EffectRollers;

public static class DevilsLuckRoller
{
    private static readonly Dictionary<int, string> Effects = new()
    {
        [1] = "Small bony horns sprout from your forehead, you feel them grow when you do violence. (d6 headbutt)",
        [2] = "Your hands are permanently blackened.",
        [3] = "The colour of your eyes invert.",
        [4] = "Your teeth elongate, sharp and vicious like that of a beast. (d4 bite)",
        [5] = "The hair on your forearms hardens into dark spikes like that of a hedgehog.",
        [6] = "Your skin becomes scaly and rigid devoid of feeling. (Natural 1 armour)",
        [7] = "Your feet fuse to become cloven hooves.",
        [8] = "You grow a tail. (It can be used to do simple actions)",
        [9] = "Each hand grows another finger.",
        [10] = "Your skin turns wine red.",
        [11] = "Your tongue forks at the tip... like that of a serpent.",
        [12] = "Small, horn-like spikes grow over your jaw and chin.",
        [13] = "A pair of small bats wings sprout from your back. (unfortunately they are flightless)",
        [14] = "Your fingernails harden into talons. (d4 slashing attack.)",
        [15] = "In the dark your eyes glow like coals. (You can see in complete darkness up to ten feet.)",
        [16] = "Your spine grows spikes that erupt from your back splitting the skin.",
        [17] = "When eating, test DR12 Toughness. Fail and you belch flames.",
        [18] = "Your nose hardens to a bone-like spike.",
        [19] = "Your blood runs black like tar.",
        [20] = "Direct sunlight to exposed skin burns you for 1d4 damage per minute.",
    };

    public static string Roll()
    {
        var rand = new Random();
        var d6 = rand.Next(1,7);
        var d20 = rand.Next(1, 21);

        return d6 == 1
            ? $"```🎲 DEVIL'S LUCK ROLL 🎲\nD6: {d6} | D20: {d20}\n\n⚖️ A DEVIL'S BARGAIN ⚖️\nThe figure with the ledger marks something in their book with a knowing smile...\n\n🔥 THE COST OF YOUR SECOND CHANCE MANIFESTS:\n\n>>> {Effects[d20]} <<<\n\nThe transformation begins... there is no going back.```"
            : $"```🎲 DEVIL'S LUCK ROLL 🎲\nD6: {d6}\n\n📜 The figure with the staff nods approvingly.\nYour debt remains... unpaid. For now.```";
    }
}