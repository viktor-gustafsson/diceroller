using DiscordBot.Equipment;
using DiscordBot.Equipment.Enums;
using DiscordBot.Rollers.CharacterRollers.Enums;

namespace DiscordBot.Rollers.CharacterRollers;

public static class CharacterEquipmentSets
{
    public static readonly List<EquipmentItem> CommonStartingGear =
    [
        new(EquipmentName.Satchel),
        new(EquipmentName.Rations, 3),
        new(EquipmentName.Waterskin),
        new(EquipmentName.Bedroll),
        new(EquipmentName.FlintAndSteel),
        new(EquipmentName.Rope15M),
        new(EquipmentName.Torches, 2),
    ];

    public static List<EquipmentItem> GetEquipmentFor(MercenarySubType subType) => subType switch
    {
        MercenarySubType.Rifleman =>
        [
            new(EquipmentName.Musket),
            new(EquipmentName.Bullets, 10),
            new(EquipmentName.BlackPowderPouch),
            new(EquipmentName.BulletKit),
        ],
        MercenarySubType.GreatSwordsman =>
        [
            new(EquipmentName.Zweihander),
            new(EquipmentName.FlamboyantClothes),
        ],
        MercenarySubType.Grenadier =>
        [
            new(EquipmentName.Bomb, 6),
            new(EquipmentName.BlackPowderPouch, 2),
            new(EquipmentName.BombMakersKit),
        ],
        _ => [],
    };

    public static List<EquipmentItem> GetEquipmentFor(BountyHunterSubType subType) => subType switch
    {
        BountyHunterSubType.Pistolier =>
        [
            new(EquipmentName.Pistol, 2),
            new(EquipmentName.Shots, 12),
            new(EquipmentName.BlackPowderPouch, 2),
            new(EquipmentName.FirearmsRepairKit),
        ],
        BountyHunterSubType.MasterTrapper =>
        [
            new(EquipmentName.BearTrap, 2),
            new(EquipmentName.Caltrops, 10),
            new(EquipmentName.Shovel),
            new(EquipmentName.HuntersKnife),
            new(EquipmentName.LargeHeavyNet),
            new(EquipmentName.JarOfBees),
            new(EquipmentName.JarOfSleepPoison),
            new(EquipmentName.Rope30M),
        ],
        BountyHunterSubType.BeastHunter =>
        [
            new(EquipmentName.Crossbow),
            new(EquipmentName.ArrowsBolts, 10),
            new(EquipmentName.BaitBag),
            new(EquipmentName.HuntersKnife),
        ],
        _ => [],
    };

    public static List<EquipmentItem> GetEquipmentFor(OpportunistSubType subType) => subType switch
    {
        OpportunistSubType.Adventurer =>
        [
            new(EquipmentName.GrapplingHook),
            new(EquipmentName.Rope15M),
            new(EquipmentName.Crossbow),
            new(EquipmentName.ArrowsBolts, 10),
            new(EquipmentName.BrokenCompass),
            new(EquipmentName.SportsmansSword),
        ],
        OpportunistSubType.SneakThief =>
        [
            new(EquipmentName.GrapplingHookCrossbow),
            new(EquipmentName.SmokeBombs, 5),
            new(EquipmentName.LockPickKit),
        ],
        OpportunistSubType.SilverTonguedTrickster =>
        [
            new(EquipmentName.LuckyCoin),
            new(EquipmentName.Perfume),
        ],
        _ => [],
    };

    public static List<EquipmentItem> GetEquipmentFor(PractitionerSubType subType) => subType switch
    {
        PractitionerSubType.VowOfWar =>
        [
            new(EquipmentName.HeavyArmor),
            new(EquipmentName.Sword),
            new(EquipmentName.BadgeOfHonor),
        ],
        PractitionerSubType.VowOfHealing =>
        [
            new(EquipmentName.DoctorsKit),
            new(EquipmentName.SmallDagger),
        ],
        PractitionerSubType.VowOfSustenance =>
        [
            new(EquipmentName.Rations, 2),
            new(EquipmentName.Waterskin, 2),
            new(EquipmentName.HerbsAndSpices),
            new(EquipmentName.CookingKit),
            new(EquipmentName.CastIronWallop),
        ],
        _ => [],
    };

    public static List<EquipmentItem> GetEquipmentFor(WitchSubType subType) => subType switch
    {
        WitchSubType.WoodsWitch =>
        [
            new(EquipmentName.Talisman),
            new(EquipmentName.TrustyBird),
        ],
        WitchSubType.Herbalist =>
        [
            new(EquipmentName.BrewersKit),
            new(EquipmentName.HealersMask),
        ],
        WitchSubType.Hexen =>
        [
            new(EquipmentName.BlackCandles),
            new(EquipmentName.DeckOfCards),
        ],
        _ => [],
    };
}
