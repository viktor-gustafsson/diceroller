using DiscordBot.Equipment;
using DiscordBot.Rollers.CharacterRollers.Models;

namespace DiscordBot.Rollers.CharacterRollers;

public abstract class NewCharacterRollerBase
{
    protected static int GetGold(int numberOfd6)
    {
        var sum = 0;
        for (var i = 0; i < numberOfd6; i++)
        {
            sum += Random.Shared.Next(1, 7) * 10;
        }

        return sum;
    }
    protected static int GetAbilityModifier(int abilityScore) => Lists.AbilityModifiers[abilityScore];

    private static T PickRandom<T>(Dictionary<int, T> dict) => dict[Random.Shared.Next(1, dict.Count + 1)];
    protected static int GetHp(int toughness, int modifier, int dice)
    {
        var diceResult = Random.Shared.Next(1,dice + 1);
        var hp = diceResult+toughness+modifier;

        return hp < 1 ? 1 : hp;
    }

    protected static int GetStat(int modifier = 0)
    {
        var sum = 0;
        for (var i = 0; i < 3; i++)
        {
            sum += Random.Shared.Next(1,7);
        }

        return sum + modifier;
    }

    protected static Character GetNewCharacter(NewCharacterDto newCharacterDto)
    {
        return new Character
        {
            Type = PickRandom(Lists.CharacterTypes),
            Wants = PickRandom(Lists.CharacterWants),
            SetBack = PickRandom(Lists.CharacterSetbacks),
            Quirk = PickRandom(Lists.CharacterQuirks),
            AdditionalSkill = PickRandom(Lists.AdditionalSkills),
            Passion = PickRandom(Lists.Passions),
            PhysicalAttribute = PickRandom(Lists.PhysicalAttributes),
            PartyConnection = PickRandom(Lists.PartyConnections),
            Agility = newCharacterDto.Agility,
            Presence = newCharacterDto.Presence,
            Strength = newCharacterDto.Strength,
            Toughness = newCharacterDto.Toughness,
            Gold = newCharacterDto.Gold,
            Hp = newCharacterDto.Hp,
            ClassSpecificEvent = newCharacterDto.ClassSpecificEvent,
            ArcheTypeSpecificInfo = newCharacterDto.SpecificInfo,
            SubTypeSpecificInfo = newCharacterDto.SubTypeSpecificInfo,
            Equipment = newCharacterDto.Equipment,
        };
    }

    protected static string GetCharacterResponseString(Character character)
    {
        var characterDetails = $"🎲 CHARACTER DETAILS\n\n" +
                               $"🔧 Additional Skill: {character.AdditionalSkill} (+2 to any roll connected to {character.AdditionalSkill})\n" +
                               $"💖 Passion: {character.Passion}\n" +
                               $"👤 Physical Attribute: {character.PhysicalAttribute}\n" +
                               $"🤝 Party Connection: {character.PartyConnection}\n";

        if (!string.IsNullOrEmpty(character.ClassSpecificEvent))
        {
            characterDetails += $"🌟 {character.ClassSpecificEvent}\n";
        }

        if (!string.IsNullOrEmpty(character.ArcheTypeSpecificInfo))
        {
            characterDetails += $"ℹ️ Extra info: {character.ArcheTypeSpecificInfo}\n";
        }

        var equipmentSection = FormatEquipment(character.Equipment);

        return $"```\n" +
               $"🎲 NEW CHARACTER STATS\n\n" +
               $"{character.SubTypeSpecificInfo}"+
               $"{equipmentSection}" +
               $"💪 Strength:  [{GetAbilityModifier(character.Strength),2}]  (Rolled: {character.Strength,2})\n" +
               $"🏃 Agility:   [{GetAbilityModifier(character.Agility),2}]  (Rolled: {character.Agility,2})\n" +
               $"👑 Presence:  [{GetAbilityModifier(character.Presence),2}]  (Rolled: {character.Presence,2})\n" +
               $"🛡️ Toughness: [{GetAbilityModifier(character.Toughness),2}]  (Rolled: {character.Toughness,2})\n\n" +
               $"❤️ Hit Points: {character.Hp}\n" +
               $"🟡 Gold: {character.Gold}\n" +
               $"```\n" +
               $"```\n" +
               $"🎲 CHARACTER TRAITS\n\n" +
               $"🎭 Character Type: {character.Type}\n" +
               $"💫 Character Wants: {character.Wants}\n" +
               $"🎪 Character Quirk: {character.Quirk}\n" +
               $"⚠️ Character Setback: {character.SetBack}\n" +
               $"```\n" +
               $"```\n" +
               characterDetails +
               "```";
    }

    private static string FormatEquipment(List<EquipmentItem> equipment)
    {
        if (equipment.Count == 0)
            return "";

        var grouped = equipment
            .GroupBy(e => e.Category)
            .OrderBy(g => g.Key);

        var sb = new System.Text.StringBuilder();
        sb.AppendLine("🎒 Starting Equipment:\n");

        foreach (var group in grouped)
        {
            sb.AppendLine($"{group.First().CategoryDisplayName}");
            foreach (var item in group)
            {
                var quantityStr = item.Quantity > 1 ? $" x{item.Quantity}" : "";
                sb.AppendLine($"• {item.DisplayName}{quantityStr} [W: {item.Weight}]");
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }
}
