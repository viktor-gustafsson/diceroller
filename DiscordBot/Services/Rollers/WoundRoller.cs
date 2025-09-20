namespace DiscordBot.Services.Rollers;

public class WoundRoller
{
    private static readonly Dictionary<int, string> Wounds = new()
    {
        { 1, "Small Scar - The type they say the ladies like." },
        { 2, "Missing Leg - You take a permanent -1 to your agility stat. But fear not, you can get a dashing prosthetic to replace it." },
        { 3, "Missing Hand or Arm - From now on using a two handed weapon is DR14 to hit." },
        { 4, "Large Scar - People are nervous when talking to you. +1 to intimidate someone." },
        { 5, "Missing Eye - You take a permanent -1 to all non magic Presence tests. If you lose both eyes you are blind and take another -1 to all non magic Presence tests." },
        { 6, "Missing Nose - Without a prosthetic, people will be uneasy talking to you. You also better pray you don't get a cold." },
        { 7, "Broken Limbs - Until you are healed, which can take up to a week, you take a -1 to all physical rolls." },
        { 8, "Missing Teeth - You can get a gold replacement for 20G each." },
        { 9, "Broken Ribs - You find it difficult to breathe, and your movement is reduced by ten feet per round for one week. You cannot use black powder weapons until you are healed, or you will take 1d4 damage every time you fire your weapon." },
        { 10, "A Bone Deep Ache - After rolling an Agility test, you take a -1 penalty for every other Agility test that day." }
    };

    public static string Roll()
    {
        var rand = new Random();
        var d10 = rand.Next(1, 11);

        return
            $"```🎲 WOUND ROLL 🎲\nD10: {d10}\n\n⚔️ THE BATTLEFIELD TAKES ITS TOLL ⚔️\nYour body bears the scars of combat...\n\n🩸 WOUND SUSTAINED:\n\n>>> {Wounds[d10]} <<<\n\nThe injury will serve as a reminder of this battle.```";
    }
}