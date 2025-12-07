using DiscordBot.Equipment;
using DiscordBot.Rollers.CharacterRollers.Enums;
using DiscordBot.Rollers.CharacterRollers.Models;

namespace DiscordBot.Rollers.CharacterRollers;

public abstract class MercenaryCharacterRoller : NewCharacterRollerBase, ICharacterRoller<MercenarySubType>
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
            "Shots with a musket get +2 at range.\n\n",
        [MercenarySubType.GreatSwordsman] =
            "⚔️ GREATSWORDSMAN\n\n" +
            "The strength of your arm and the reach of your sword has gotten\n" +
            "you this far. Not all of life's problems can be solved by the\n" +
            "edge of a blade, but until then your trusty Zweihander will do.\n\n" +
            "🎯 Special Ability:\n" +
            "Zweihander: Roll an extra d4 damage on a successful hit.\n\n",
        [MercenarySubType.Grenadier] =
            "💣 GRENADIER\n\n" +
            "Gunpowder and fire call to your soul, explosions and shockwaves\n" +
            "dance in your dreams. One day, you will build a bomb that will\n" +
            "shake the world. Or turn you into a fine mist trying.\n\n" +
            "🎯 Special Ability:\n" +
            "You may reroll a bomb malfunction dice, but you must\n" +
            "keep the second roll.\n\n",
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

        var equipment = new List<EquipmentItem>();
        equipment.AddRange(CharacterEquipmentSets.CommonStartingGear);
        equipment.AddRange(CharacterEquipmentSets.GetEquipmentFor(subType));

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
            Equipment = equipment,
        };
        var newCharacterTemplate = GetNewCharacter(newCharacterDto);

        return GetCharacterResponseString(newCharacterTemplate);
    }

    private static string GetMemory() =>
        $"You saw something that will haunt you forever: {Memories[Random.Shared.Next(1, Memories.Count + 1)]}";
}
