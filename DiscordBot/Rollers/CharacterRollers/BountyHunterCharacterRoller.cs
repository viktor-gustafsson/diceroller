using DiscordBot.Equipment;
using DiscordBot.Rollers.CharacterRollers.Enums;
using DiscordBot.Rollers.CharacterRollers.Models;

namespace DiscordBot.Rollers.CharacterRollers;

public abstract class BountyHunterCharacterRoller : NewCharacterRollerBase, ICharacterRoller<BountyHunterSubType>
{
    private static readonly Dictionary<int, string> Memories = new()
    {
        [1] = "The screams in the night as wild laughter rang out.",
        [2] = "The thrill of your first kill, your father's proud hand on your shoulder.",
        [3] = "Eyes watching you from the forest, burning and hungry.",
        [4] = "The glimmer of the coins handed to you for your first bounty.",
        [5] = "The blood as it dripped down the bars of the black iron cage.",
        [6] = "The warm touch of their hand on a cold winter's night.",
    };

    private static readonly Dictionary<BountyHunterSubType, string> SubTypeInformation = new()
    {
        [BountyHunterSubType.Pistolier] =
            "🔫 PISTOLIER\n\n" +
            "Some bounties are for soldiers and mercenaries who wear the best\n" +
            "armour they can find. A pistol shot cares nothing for such defenses.\n\n" +
            "🎯 Special Ability:\n" +
            "You may reroll a misfire dice but you must keep the\n" +
            "second roll.\n\n",
        [BountyHunterSubType.MasterTrapper] =
            "🪤 MASTER TRAPPER\n\n" +
            "Honour and glory are lost words to trick the foolish into a 'fair fight'.\n" +
            "A smart hunter lets their traps do the hard work for them.\n\n" +
            "🎯 Special Ability:\n" +
            "To find and make traps is Presence +2.\n\n" +
            "Setting up a trap takes one round.\n\n",
        [BountyHunterSubType.BeastHunter] =
            "🏹 BEAST HUNTER\n\n" +
            "The wilds hold many creatures, and many of them are dangerous.\n" +
            "But you are the true peril. You can see the trail of your prey as\n" +
            "easily as bloody footprints in snow.\n\n" +
            "🎯 Special Ability:\n" +
            "All rolls to track a creature's trail is Presence DR8, and you can figure\n" +
            "out where your prey is going. This includes the Hunting Rules (see\n" +
            "page 89)\n\n",
    };

    public static string Roll(BountyHunterSubType subType)
    {
        var strength = GetStat(modifier: 1);
        var agility = GetStat(modifier: 0);
        var presence = GetStat(modifier: 2);
        var toughness = GetStat(modifier: 0);
        var hp = GetHp(toughness: GetAbilityModifier(toughness), modifier: 0, dice: 8);
        var memory = GetMemory();
        var gold = GetGold(numberOfd6: 3);

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
            SpecificInfo = "",
            Gold = gold,
            SubTypeSpecificInfo = SubTypeInformation[subType],
            Equipment = equipment,
        };
        var newCharacterTemplate = GetNewCharacter(newCharacterDto);
        return GetCharacterResponseString(newCharacterTemplate);
    }

    private static string GetMemory() => $"You will always remember: {Memories[Random.Shared.Next(1, Memories.Count + 1)]}";
}
