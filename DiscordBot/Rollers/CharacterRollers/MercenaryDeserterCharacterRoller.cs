using DiscordBot.Rollers.CharacterRollers.Models;
using DiscordBot.Rollers.Enums;

namespace DiscordBot.Rollers.CharacterRollers;

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

    private static readonly Dictionary<MercenarySubType, string> SubTypeInformation = new()
    {
        [MercenarySubType.Rifleman] =
            "🔫 RIFLEMAN\n\n" +
            "Never annoy someone who can blow your head off at one hundred paces.\n" +
            "The future of war, and you know it. The world will likely end at the\n" +
            "barrel of a gun, and you will ensure it.\n\n" +
            "🎯 Special Ability:\n" +
            "Shots with a musket get +2 at range.\n\n" +
            "🎒 Starting Equipment:\n" +
            "• Musket, 10 bullets\n" +
            "• One gunpowder pouch\n" +
            "• Bullet Forge:\n" +
            "\tAt rest, you can make your own bullets easily. In a test, you can\n" +
            "\targue about with the use of this small forge.\n",
        [MercenarySubType.GreatSwordsman] =
            "⚔️ GREATSWORDSMAN\n\n" +
            "The strength of your arm and the reach of your sword has gotten\n" +
            "you this far. Not all of life's problems can be solved by the\n" +
            "edge of a blade, but until then your trusty Zweihander will do.\n\n" +
            "🎯 Special Ability:\n" +
            "Zweihander: Roll an extra d4 damage on a successful hit.\n\n" +
            "🎒 Starting Equipment:\n" +
            "• Zweihander\n\n" +
            "👑 Flamboyant Clothes:\n" +
            "\tThese garments are a sign of your martial skill and so will\n" +
            "\tcommand respect amongst other men and women of war.\n",
        [MercenarySubType.Grenadier] =
            "💣 GRENADIER\n\n" +
            "Gunpowder and fire call to your soul, explosions and shockwaves\n" +
            "dance in your dreams. One day, you will build a bomb that will\n" +
            "shake the world. Or turn you into a fine mist trying.\n\n" +
            "🎯 Special Ability:\n" +
            "You may reroll a bomb malfunction dice, but you must\n" +
            "keep the second roll.\n\n" +
            "🎒 Starting Equipment:\n" +
            "• 6 bombs\n" +
            "• 2 gunpowder pouches\n" +
            "• Bomb Maker's Kit:\n" +
            "\tWhile at rest, you may roll d4 to see how many bombs you make with\n" +
            "\tthe materials you have. This includes the gunpowder and\n" +
            "\twick. Best do this away from an open flame.\n",
    };

    private const string Info = "Normal agility tests are DR14";

    public static string Roll(MercenarySubType subType)
    {
        var strength = GetStat(modifier: 2);
        var agility = GetStat(modifier: -1);
        var presence = GetStat(modifier: -1);
        var toughness = GetStat(modifier: 2);
        var hp = GetHp(toughness: GetAbilityModifier(toughness), modifier: 0, dice: 10);
        var memory = GetMemory();
        var gold = GetGold(numberOfd6: 2);

        var newCharacterDto = new NewCharacterDto
        {
            Strength = strength,
            Agility = agility,
            Presence = presence,
            Toughness = toughness,
            Hp = hp,
            ClassSpecificEvent = memory,
            SpecificInfo = Info,
            Gold = gold,
            SubTypeSpecificInfo = SubTypeInformation[subType],
        };
        var newCharacterTemplate = GetNewCharacter(newCharacterDto);

        return GetCharacterResponseString(newCharacterTemplate);
    }

    private static string GetMemory()
    {
        var random = new Random();
        return $"You saw something that will haunt you forever: {Memories[random.Next(1, 7)]}";
    }
}