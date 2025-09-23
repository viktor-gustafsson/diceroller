using DiscordBot.Rollers.CharacterRollers.Models;

namespace DiscordBot.Rollers.CharacterRollers;

public abstract class NewCharacterRollerBase
{
    private static readonly Random Random = Random.Shared;
    protected static int GetGold(int numberOfd6)
    {
        var sum = 0;
        for (var i = 0; i < numberOfd6; i++)
        {
            sum += Random.Next(1, 7) * 10;
        }

        return sum;
    }
    protected static int GetAbilityModifier(int abilityScore) => Lists.AbilityModifiers[abilityScore];
    protected static int GetHp(int toughness, int modifier, int dice)
    {
        var diceResult = Random.Next(1,dice + 1);
        var hp = diceResult+toughness+modifier;
        
        return hp < 1 ? 1 : hp;
    }
    
    protected static int GetStat(int modifier = 0)
    {
        var sum = 0;
        for (var i = 0; i < 3; i++)
        {
            sum += Random.Next(1,7);
        }

        return sum + modifier;
    }

    protected static Character GetNewCharacter(NewCharacterDto newCharacterDto)
    {
        return new Character
        {
            Type = Lists.CharacterTypes[Random.Next(1, Lists.CharacterTypes.Count + 1)],
            Wants = Lists.CharacterWants[Random.Next(1, Lists.CharacterWants.Count + 1)],
            SetBack = Lists.CharacterSetbacks[Random.Next(1, Lists.CharacterSetbacks.Count + 1)],
            AdditionalSkill = Lists.AdditionalSkills[Random.Next(1, Lists.AdditionalSkills.Count + 1)],
            Passion = Lists.Passions[Random.Next(1, Lists.Passions.Count + 1)],
            PhysicalAttribute = Lists.PhysicalAttributes[Random.Next(1, Lists.PhysicalAttributes.Count + 1)],
            PartyConnection = Lists.PartyConnections[Random.Next(1, Lists.PartyConnections.Count + 1)],
            Agility = newCharacterDto.Agility,
            Presence = newCharacterDto.Presence,
            Strength = newCharacterDto.Strength,
            Toughness = newCharacterDto.Toughness,
            Gold = newCharacterDto.Gold,
            Hp = newCharacterDto.Hp,
            ClassSpecificEvent = newCharacterDto.ClassSpecificEvent,
            ArcheTypeSpecificInfo = newCharacterDto.SpecificInfo,
            SubTypeSpecificInfo = newCharacterDto.SubTypeSpecificInfo,
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

        return $"```\n" +
               $"🎲 NEW CHARACTER STATS\n\n" +
               $"{character.SubTypeSpecificInfo}\n"+
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
               $"⚠️ Character Setback: {character.SetBack}\n" +
               $"```\n" +
               $"```\n" +
               characterDetails +
               "```";
    }
}