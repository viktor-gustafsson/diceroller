using DiscordBot.Rollers.Characters.Models;

namespace DiscordBot.Rollers.Characters;

public abstract class NewCharacterRollerBase
{
    protected static int GetGold(int numberOfd6)
    {
        var random = new Random();
        var sum = 0;
        for (var i = 0; i < numberOfd6; i++)
        {
            sum += random.Next(1, 7) * 10;
        }

        return sum;
    }
    protected static int GetAbilityModifier(int abilityScore) => Lists.AbilityModifiers[abilityScore];
    protected static int GetHp(int toughness, int modifier, int dice)
    {
        var random = new Random();
        var diceResult = random.Next(1,dice + 1);
        var hp = diceResult+toughness+modifier;
        
        return hp < 1 ? 1 : hp;
    }
    
    protected static int GetStat(int modifier = 0)
    {
        var random = new Random();
        var sum = 0;
        for (var i = 0; i < 3; i++)
        {
            sum += random.Next(1,7);
        }

        return sum + modifier;
    }

    protected static Character GetNewCharacter(int strength, int agility, int presence, int toughness, int hp, int gold, string classSpecificEvent, string specificInfo)
    {
        var random = new Random();

        return new Character
        {
            Type = Lists.CharacterTypes[random.Next(1, 11)],
            Wants = Lists.CharacterWants[random.Next(1, 11)],
            SetBack = Lists.CharacterSetbacks[random.Next(1, 11)],
            AdditionalSkill = Lists.AdditionalSkills[random.Next(1, 21)],
            Passion = Lists.Passions[random.Next(1, 21)],
            PhysicalAttribute = Lists.PhysicalAttributes[random.Next(1, 21)],
            PartyConnection = Lists.PartyConnections[random.Next(1, 21)],
            Agility = agility,
            Presence = presence,
            Strength = strength,
            Toughness = toughness,
            Gold = gold,
            Hp = hp,
            ClassSpecificEvent = classSpecificEvent,
            ArcheTypeSpecificInfo = specificInfo,
        };
    }

    protected static string GetCharacterResponseString(Character character)
    {
        var characterDetails = $"🎲 CHARACTER DETAILS\n\n" +
                               $"🔧 Additional Skill: {character.AdditionalSkill} (+2 to any roll connected to {character.AdditionalSkill}\n" +
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
               $"💪 Strength:  [{GetAbilityModifier(character.Strength),3}]  (Rolled: {character.Strength,2})\n" +
               $"🏃 Agility:   [{GetAbilityModifier(character.Agility),3}]  (Rolled: {character.Agility,2})\n" +
               $"👑 Presence:  [{GetAbilityModifier(character.Presence),3}]  (Rolled: {character.Presence,2})\n" +
               $"🛡️ Toughness: [{GetAbilityModifier(character.Toughness),3}]  (Rolled: {character.Toughness,2})\n\n" +
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