namespace DiscordBot.Rollers;

public abstract class MagicMisHapRoller
{
    private static readonly Dictionary<int, string> MagicMishaps = new()
    {
        { 1, "You have pierced the veil. If you look up at the night sky for too long, roll DR12 Presence. On a failure you scream in horror at the terrible truth." },
        { 2, "The air cracks with lightning. Three capering imps appear and wreak havoc and chaos until killed or driven away. See page 144." },
        { 3, "The spell has the opposite effect." },
        { 4, "The flesh of your mouth and jaw rots away revealing the white bones beneath. Better wear a mask from now on." },
        { 5, "If a spell was intended for a foe, it strikes a friend, and vice versa." },
        { 6, "You vomit a black orb that grows and swells into an exact clone of yourself. The clone is everything you are not, and it wants you dead." },
        { 7, "The sunlight burns you. Direct sunlight on your skin burns you for d4 damage per minute." },
        { 8, "The moon knows what you did. When in sight of the moon, take a -1 to all rolls." },
        { 9, "You see all about tall spindly figures. Only the mad and the damned can see them too. If they suspect that you see them, they will attack." },
        { 10, "Crack! Lightning strikes you for d4 damage." },
        { 11, "Nature tries to reclaim you. d8 vines spring from the earth and begin dragging you down. Succeed DR12 Strength or be buried alive." },
        { 12, "Never again. Performing the spell again deals d4 damage to you." },
        { 13, "Reality tears. A rift in space opens and terrible creatures spill out and attack everything in sight." },
        { 14, "A mischievous shade. Your shadow now has a will of its own; it will try to undermine you at every opportunity." },
        { 15, "Who am I? Your stats including HP are shuffled. (A max HP of 0 or less becomes 1)" },
        { 16, "Food and water sickens you. Only blood and raw flesh will sate and sustain you. You must imbibe fresh blood daily to survive. Best you not be seen eating." },
        { 17, "Black as the night. Your eyes go black and anyone who looks at them is filled with a chilling dread. If you suspect the townsfolk or inquisitors are on to you, they are now." },
        { 18, "Mist and moonbeams. You turn into a vapour and cannot move or interact with the world for d4 rounds." },
        { 19, "A hunter's mark you alone can see. A symbol burns above your head. In d4 days, a Shadow Hunter will find you and try to slay you." },
        { 20, "A bell tolls. At a time of rest, you will be approached by a charming and mysterious figure. They will challenge you to a game of chance; if you win you get a free DL. Lose and they will snatch you away." }
    };
    
    public static string Roll()
    {
        var random = new Random();
        var d20 = random.Next(1, 21);

        return
            $"```🎲 MAGIC MISHAP ROLL 🎲\nD20: {d20}\n\n🔮 MAGIC GOES AWRY 🔮\nThe arcane energies twist and corrupt...\n\n🌙 MAGICAL MISHAP:\n\n>>> {MagicMishaps[d20]} <<<\n\nThe consequences of dabbling in dark magic manifest.```";
    }
}