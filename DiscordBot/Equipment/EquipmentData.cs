using DiscordBot.Equipment.Enums;

namespace DiscordBot.Equipment;

public static class EquipmentData
{
    private static readonly Dictionary<EquipmentName, string> CustomEquipmentDescription = new()
    {
        [EquipmentName.FirearmsRepairKit] = "Firearms repair kit:\n" +
            "\tWhile at rest, you may use this handy kit to repair a\n" +
            "\tdamaged firearm. However, if the weapon is\n" +
            "\tbroken beyond repair it cannot be salvaged.",
        [EquipmentName.BaitBag] = "Bait bag:\n" +
            "\tThis stinking, musky bag will attract predators both\n" +
            "\tnatural and demonic. Be careful where you place it.",
        [EquipmentName.BulletKit] = "Bullet kit:\n" +
            "\tAt rest, you can make your own bullets from metal that\n" +
            "\tyou have about with the use of this kit.",
        [EquipmentName.BombMakersKit] = "Bomb maker's kit:\n"+
            "\tWhile at rest, you may roll d4 to see how many bombs you make with\n" +
            "\tthe materials you have. This includes the gunpowder and\n" +
            "\twick. Best do this away from an open flame.",
        [EquipmentName.FlamboyantClothes] = "Flamboyant Clothes:\n" +
            "\tThese garments are a sign of your martial skill and so will\n" +
            "\tcommand respect amongst other men and women of war.",
        [EquipmentName.BrokenCompass] = "Broken compass:\n" +
            "\tThis compass does not point north,\n" +
            "\tit does however point to the thing\n" +
            "\tyou most desire.",
        [EquipmentName.SportsmansSword] = "Sportsman's sword (d6+2):\n" +
            "\tDR10 to disarm an opponent of\n" +
            "\ttheir held weapon rather than\n" +
            "\tinflict harm.",
        [EquipmentName.GrapplingHookCrossbow] = "Grappling hook crossbow:\n" +
            "\tThis custom made crossbow will shoot\n" +
            "\ta grappling hook and rope up to\n" +
            "\t30ft. The rope is light and can only\n" +
            "\tsupport one person at a time.",
        [EquipmentName.SmokeBombs] = "Smoke bombs:\n" +
            "\tOnce thrown, these will cover a ten\n" +
            "\tfoot sphere and dissipate after a\n" +
            "\tcouple of seconds.",
        [EquipmentName.LuckyCoin] = "Lucky coin:\n" +
            "\tA large brilliantly shiny gold coin\n" +
            "\tfrom a foreign land. It will return\n" +
            "\tto your hands in a few minutes.\n" +
            "\tPerfect for bribery.",
        [EquipmentName.Perfume] = "Perfume:\n" +
            "\tThe smell of a rose is irresistible to all that smell\n" +
            "\tit. Perfect for those with no time to bathe.",
        [EquipmentName.TrustyBird] = "Trusty bird:\n" +
            "\tYour bird is loyal but only to you and despises\n" +
            "\teveryone else. You have a primeval link to its\n" +
            "\tthoughts and it will do as you command. It can scout,\n" +
            "\tkeep watch and even attack your foes.\n\n" +
            "\tHP 4\n" +
            "\tClaws/bite d4",
        [EquipmentName.Talisman] = "Talisman:\n" +
            "\tA small wood and stone item that can be used to\n" +
            "\tbless others. You can give +1 to any roll made by\n" +
            "\tyourself once per round.",
        [EquipmentName.BrewersKit] = "Brewer's kit:\n" +
            "\t- Brass pot\n" +
            "\t- Weights and scales\n" +
            "\t- Flint and steel\n" +
            "\t- Six glass bottles with stops\n" +
            "\t- One pint of pure ethanol",
        [EquipmentName.HealersMask] = "Healer's mask:\n" +
            "\tThis mask has glass lenses and a long beak stuffed\n" +
            "\twith healing herbs. This will protect you\n" +
            "\tfrom most airborne foul smells and airborne\n" +
            "\tmaladies. It also makes people uncomfortable.",
        [EquipmentName.BlackCandles] = "Black candles:\n" +
            "\tThese inky black candles once lit, can\n" +
            "\toffer protection from malevolent spirits\n" +
            "\tand entities.",
        [EquipmentName.DeckOfCards] = "Deck of cards:\n" +
            "\tThese esoteric cards can be shuffled and drawn to\n" +
            "\tdivine the future, however the future is always dark\n" +
            "\tand vague.",
        [EquipmentName.BadgeOfHonor] = "Badge of honor:\n" +
            "\tYou wear a badge of honour and\n" +
            "\trespect in combat. This will\n" +
            "\tcommand respect from the faithful.",
        [EquipmentName.DoctorsKit] = "Doctor's kit:\n" +
            "\t- Needle\n" +
            "\t- Catgut thread\n" +
            "\t- Gauze and bandages\n" +
            "\t- Surgical tongs\n" +
            "\t- Ointment\n" +
            "\t- Smelling salts\n" +
            "\t- Poppy extract\n" +
            "\t- Small sharp knife\n" +
            "\t- Cauterising iron\n" +
            "\t- Tincture",
        [EquipmentName.CookingKit] = "Cooking kit:\n" +
            "\t- 8 rations and two water skins\n" +
            "\t- Box of salt\n" +
            "\t- Cooking pot\n" +
            "\t- Skillet\n" +
            "\t- Cutting knife and spoon\n" +
            "\t- Enough bowls for the rest of the party\n" +
            "\t- A precious cache of herbs and spices",
    };

    private static readonly Dictionary<EquipmentName, (string Weight, EquipmentCategory Category, string DisplayName)> Items = new()
    {
        // Kits
        [EquipmentName.BombMakersKit] = ("2", EquipmentCategory.Kits, CustomEquipmentDescription[EquipmentName.BombMakersKit]),
        [EquipmentName.BrewersKit] = ("2", EquipmentCategory.Kits, CustomEquipmentDescription[EquipmentName.BrewersKit]),
        [EquipmentName.BulletKit] = ("2", EquipmentCategory.Kits, CustomEquipmentDescription[EquipmentName.BulletKit]),
        [EquipmentName.CookingKit] = ("2", EquipmentCategory.Kits, CustomEquipmentDescription[EquipmentName.CookingKit]),
        [EquipmentName.DoctorsKit] = ("2", EquipmentCategory.Kits, CustomEquipmentDescription[EquipmentName.DoctorsKit]),
        [EquipmentName.FirearmsRepairKit] = ("2", EquipmentCategory.Kits, CustomEquipmentDescription[EquipmentName.FirearmsRepairKit]),
        [EquipmentName.LockPickKit] = ("1L", EquipmentCategory.Kits, "Lock pick kit"),
        [EquipmentName.Toolbox] = ("2", EquipmentCategory.Kits, "Toolbox"),

        // Adventuring Gear
        [EquipmentName.Bedroll] = ("1", EquipmentCategory.AdventuringGear, "Bedroll"),
        [EquipmentName.FlintAndSteel] = ("*", EquipmentCategory.AdventuringGear, "Flint and steel"),
        [EquipmentName.Rations] = ("1", EquipmentCategory.AdventuringGear, "Rations"),
        [EquipmentName.Rope10M] = ("1L", EquipmentCategory.AdventuringGear, "Rope 10m"),
        [EquipmentName.Rope15M] = ("1", EquipmentCategory.AdventuringGear, "Rope 15m"),
        [EquipmentName.Rope30M] = ("2", EquipmentCategory.AdventuringGear, "Rope 30m"),
        [EquipmentName.Torches] = ("1L", EquipmentCategory.AdventuringGear, "Torches"),
        [EquipmentName.Waterskin] = ("1L", EquipmentCategory.AdventuringGear, "Waterskin"),
        [EquipmentName.BadgeOfHonor] = ("*", EquipmentCategory.AdventuringGear, CustomEquipmentDescription[EquipmentName.BadgeOfHonor]),
        [EquipmentName.BearTrap] = ("1L", EquipmentCategory.AdventuringGear, "Bear trap"),
        [EquipmentName.BlackCandles] = ("*", EquipmentCategory.AdventuringGear, CustomEquipmentDescription[EquipmentName.BlackCandles]),
        [EquipmentName.BrokenCompass] = ("*", EquipmentCategory.AdventuringGear, CustomEquipmentDescription[EquipmentName.BrokenCompass]),
        [EquipmentName.Caltrops] = ("1L", EquipmentCategory.AdventuringGear, "Caltrops"),
        [EquipmentName.DeckOfCards] = ("*", EquipmentCategory.AdventuringGear, CustomEquipmentDescription[EquipmentName.DeckOfCards]),
        [EquipmentName.FieldMedicineKit] = ("1", EquipmentCategory.AdventuringGear, "Field medicine kit"),
        [EquipmentName.GrapplingHook] = ("1L", EquipmentCategory.AdventuringGear, "Grappling hook"),
        [EquipmentName.GrapplingHookCrossbow] = ("2", EquipmentCategory.AdventuringGear, CustomEquipmentDescription[EquipmentName.GrapplingHookCrossbow]),
        [EquipmentName.HealersMask] = ("1L", EquipmentCategory.AdventuringGear, CustomEquipmentDescription[EquipmentName.HealersMask]),
        [EquipmentName.HerbsAndSpices] = ("*", EquipmentCategory.AdventuringGear, "Herbs and spices"),
        [EquipmentName.JarOfBees] = ("1L", EquipmentCategory.AdventuringGear, "Jar of bees"),
        [EquipmentName.JarOfSleepPoison] = ("1L", EquipmentCategory.AdventuringGear, "Jar of sleep poison"),
        [EquipmentName.LargeHeavyNet] = ("2", EquipmentCategory.AdventuringGear, "Large heavy net"),
        [EquipmentName.LuckyCoin] = ("*", EquipmentCategory.AdventuringGear, CustomEquipmentDescription[EquipmentName.LuckyCoin]),
        [EquipmentName.Perfume] = ("*", EquipmentCategory.AdventuringGear, CustomEquipmentDescription[EquipmentName.Perfume]),
        [EquipmentName.Shovel] = ("1", EquipmentCategory.AdventuringGear, "Shovel"),
        [EquipmentName.Talisman] = ("*", EquipmentCategory.AdventuringGear, CustomEquipmentDescription[EquipmentName.Talisman]),
        [EquipmentName.FlamboyantClothes] = ("1", EquipmentCategory.AdventuringGear, CustomEquipmentDescription[EquipmentName.FlamboyantClothes]),

        // Containers
        [EquipmentName.BaitBag] = ("+1", EquipmentCategory.Containers, CustomEquipmentDescription[EquipmentName.BaitBag]),
        [EquipmentName.Satchel] = ("+4", EquipmentCategory.Containers, "Satchel"),
        [EquipmentName.AmmoPouch] = ("+1L", EquipmentCategory.Containers, "Ammo pouch"),
        [EquipmentName.Quiver] = ("+1L", EquipmentCategory.Containers, "Quiver"),
        [EquipmentName.Backpack] = ("+6", EquipmentCategory.Containers, "Backpack"),

        // Potions
        [EquipmentName.TheGoodlyCure] = ("1L", EquipmentCategory.Potions, "The goodly cure"),

        // Melee Weapons
        [EquipmentName.CastIronWallop] = ("1", EquipmentCategory.MeleeWeapons, "Cast iron wallop (d6)"),
        [EquipmentName.HuntersKnife] = ("1L", EquipmentCategory.MeleeWeapons, "Hunter's knife (d4)"),
        [EquipmentName.SmallDagger] = ("1L", EquipmentCategory.MeleeWeapons, "Small dagger (d4)"),
        [EquipmentName.SportsmansSword] = ("1", EquipmentCategory.MeleeWeapons, CustomEquipmentDescription[EquipmentName.SportsmansSword]),
        [EquipmentName.Sword] = ("1", EquipmentCategory.MeleeWeapons, "Sword (d6)"),
        [EquipmentName.Zweihander] = ("2", EquipmentCategory.MeleeWeapons, "Zweihänder (d10)"),

        // Beasts
        [EquipmentName.TrustyBird] = ("*", EquipmentCategory.Beasts, CustomEquipmentDescription[EquipmentName.TrustyBird]),

        // Ranged Weapons
        [EquipmentName.Crossbow] = ("2", EquipmentCategory.RangedWeapons, "Crossbow"),
        [EquipmentName.Longbow] = ("2", EquipmentCategory.RangedWeapons, "Longbow"),
        [EquipmentName.Musket] = ("2", EquipmentCategory.RangedWeapons, "Musket"),
        [EquipmentName.Pistol] = ("1", EquipmentCategory.RangedWeapons, "Pistol"),

        // Ammunition
        [EquipmentName.ArrowsBolts] = ("1L", EquipmentCategory.Ammunition, "Arrows/Bolts"),
        [EquipmentName.Bomb] = ("1L", EquipmentCategory.Ammunition, "Bomb"),
        [EquipmentName.SmokeBombs] = ("1", EquipmentCategory.Ammunition, CustomEquipmentDescription[EquipmentName.SmokeBombs]),
        [EquipmentName.Shots] = ("1L", EquipmentCategory.Ammunition, "Shots"),
        [EquipmentName.Bullets] = ("1L", EquipmentCategory.Ammunition, "Bullets"),
        [EquipmentName.BlackPowderPouch] = ("1", EquipmentCategory.Ammunition, "Black powder pouch"),

        // Armors
        [EquipmentName.LightArmor] = ("2/4", EquipmentCategory.Armors, "Light armor (-d2)"),
        [EquipmentName.MediumArmor] = ("3/6", EquipmentCategory.Armors, "Medium armor (-d4)"),
        [EquipmentName.HeavyArmor] = ("4/8", EquipmentCategory.Armors, "Heavy armor (-d6)"),
        [EquipmentName.Shield] = ("1", EquipmentCategory.Armors, "Shield (+1)"),
    };

    private static readonly Dictionary<EquipmentCategory, string> CategoryDisplayNames = new()
    {
        [EquipmentCategory.Kits] = "Kits",
        [EquipmentCategory.AdventuringGear] = "Adventuring Gear",
        [EquipmentCategory.Containers] = "Containers",
        [EquipmentCategory.Potions] = "Potions",
        [EquipmentCategory.MeleeWeapons] = "Melee Weapons",
        [EquipmentCategory.RangedWeapons] = "Ranged Weapons",
        [EquipmentCategory.Ammunition] = "Ammunition",
        [EquipmentCategory.Armors] = "Armors",
        [EquipmentCategory.Beasts] = "Beasts",
    };

    public static (string Weight, EquipmentCategory Category) GetItemData(EquipmentName name)
    {
        var data = Items[name];
        return (data.Weight, data.Category);
    }

    public static string GetDisplayName(EquipmentName name) => Items[name].DisplayName;
    public static string GetCategoryDisplayName(EquipmentCategory category) => CategoryDisplayNames[category];


}
