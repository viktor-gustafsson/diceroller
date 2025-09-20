using DiscordBot.Rollers.Models;

namespace DiscordBot.Rollers.Characters;

public abstract class NewCharacterRollerBase
{
    private static readonly Dictionary<int, string> CharacterTypes = new()
    {
        [1] = "Shifty",
        [2] = "Cheery",
        [3] = "Sullen",
        [4] = "Brash",
        [5] = "Smarmy",
        [6] = "Generous",
        [7] = "Pious",
        [8] = "Blasphemous",
        [9] = "Foul Mouthed",
        [10] = "Smouldering",
    };

    private static readonly Dictionary<int, string> CharacterWants = new()
    {
        [1] = "Riches",
        [2] = "Anarchy",
        [3] = "Knowledge",
        [4] = "Fame",
        [5] = "Peace",
        [6] = "Revenge",
        [7] = "Revelry",
        [8] = "Adventure",
        [9] = "To write a book",
        [10] = "Justice",
    };

    private static readonly Dictionary<int, string> CharacterSetbacks = new()
    {
        [1] = "Greedy",
        [2] = "Overconfident",
        [3] = "Paranoid",
        [4] = "Cowardly",
        [5] = "Vengeful",
        [6] = "Vain",
        [7] = "Lazy",
        [8] = "Haunted",
        [9] = "Impatient",
        [10] = "Rude",
    };

    private static readonly Dictionary<int, string> AdditionalSkills = new()
    {
        [1] = "Carpentry",
        [2] = "Good with animals",
        [3] = "Metalwork",
        [4] = "Literate",
        [5] = "Cookery",
        [6] = "Brewing",
        [7] = "Sewing",
        [8] = "Sailing",
        [9] = "Singing",
        [10] = "Painting",
        [11] = "Dancing",
        [12] = "Music",
        [13] = "Poetry",
        [14] = "Storytelling",
        [15] = "Languages",
        [16] = "Cooking",
        [17] = "Fishing",
        [18] = "Diplomacy",
        [19] = "Whittling",
        [20] = "Arithmetic",
    };

    private static readonly Dictionary<int, string> Passions = new()
    {
        [1] = "History",
        [2] = "Politics",
        [3] = "Gossip",
        [4] = "Technology",
        [5] = "Nature",
        [6] = "Books",
        [7] = "Sex",
        [8] = "Food and Drink",
        [9] = "Fashion",
        [10] = "Money",
        [11] = "Creativity",
        [12] = "Sport",
        [13] = "Hunting",
        [14] = "Debate",
        [15] = "Theatre",
        [16] = "Weapons",
        [17] = "Anarchy",
        [18] = "Law",
        [19] = "Gambling",
        [20] = "Astrology",
    };

    private static readonly Dictionary<int, string> PhysicalAttributes = new()
    {
        [1] = "Well-groomed",
        [2] = "Distinctive smell",
        [3] = "Thousand yard stare",
        [4] = "Rough hands",
        [5] = "Tall",
        [6] = "Short",
        [7] = "Scruffy",
        [8] = "Scarred",
        [9] = "Ruddy complexion",
        [10] = "Wild hair",
        [11] = "Piercing eyes",
        [12] = "Notable jewellery",
        [13] = "Broken nose",
        [14] = "Gold teeth",
        [15] = "Torture scars",
        [16] = "Flamboyant",
        [17] = "Demure",
        [18] = "Corpulent",
        [19] = "Gaunt",
        [20] = "Battle worn",
    };

    private static readonly Dictionary<int, string> PartyConnections = new()
    {
        [1] = "Both survived an attack on a convoy.",
        [2] = "Were cellmates in prison.",
        [3] = "Are the last survivors of another band.",
        [4] = "Awoke together in bed with no memory of the night before.",
        [5] = "Emerged from the woods swearing never to speak of what happened.",
        [6] = "Both seek bloody revenge on the same person.",
        [7] = "Were formerly Servant and Master.",
        [8] = "Grew up in the same town/street.",
        [9] = "Were former rivals for a treasure more trouble than it's worth.",
        [10] = "Both swore to put an evil to rest.",
        [11] = "Are bound by a blood oath.",
        [12] = "Promised to help the other with their goal.",
        [13] = "Are partners in a fledgling business.",
        [14] = "Are fighting for the affections of the same person.",
        [15] = "Escaped a raid on your town.",
        [16] = "Washed ashore after a storm wrecked your boat.",
        [17] = "Are on the run from your former leader.",
        [18] = "Escaped the Inquisition (for now).",
        [19] = "Dreamt of the other's death.",
        [20] = "Were cursed to keep the other alive.",
    };
    
    private static readonly Dictionary<int, int> AbilityModifiers = new()
    {
        [1] = -3, 
        [2] = -3, 
        [3] = -3, 
        [4] = -3,
        [5] = -2, 
        [6] = -2,
        [7] = -1, 
        [8] = -1,
        [9] = 0, 
        [10] = 0, 
        [11] = 0, 
        [12] = 0,
        [13] = 1, 
        [14] = 1,
        [15] = 2, 
        [16] = 2,
        [17] = 3, 
        [18] = 3,
    };

    protected static int RollForGold(int numberOfd6)
    {
        var random = new Random();
        var sum = 0;
        for (var i = 0; i < numberOfd6; i++)
        {
            sum += random.Next(1, 7) * 10;
        }

        return sum;
    }
    protected static int GetAbilityModifier(int abilityScore) => AbilityModifiers[abilityScore];
    protected static int GetHp(int toughness, int modifier, int dice)
    {
        var random = new Random();

        var d8 = random.Next(1,dice + 1);
        var hp = d8+toughness+modifier;
        
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

    protected static NewCharacter GetNewCharacterTemplate(int strength, int agility, int presence, int toughness, int hp, int gold, string classSpecificEvent, string specificInfo)
    {
        var random = new Random();

        return new NewCharacter
        {
            Type = CharacterTypes[random.Next(1, 11)],
            Wants = CharacterWants[random.Next(1, 11)],
            SetBack = CharacterSetbacks[random.Next(1, 11)],
            AdditionalSkill = AdditionalSkills[random.Next(1, 21)],
            Passion = Passions[random.Next(1, 21)],
            PhysicalAttribute = PhysicalAttributes[random.Next(1, 21)],
            PartyConnection = PartyConnections[random.Next(1, 21)],
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

    protected static string GetCharacterResponseString(NewCharacter newCharacter)
    {
        var characterDetails = $"🎲 CHARACTER DETAILS\n\n" +
                               $"🔧 Additional Skill: {newCharacter.AdditionalSkill}\n" +
                               $"💖 Passion: {newCharacter.Passion}\n" +
                               $"👤 Physical Attribute: {newCharacter.PhysicalAttribute}\n" +
                               $"🤝 Party Connection: {newCharacter.PartyConnection}\n";

        if (!string.IsNullOrEmpty(newCharacter.ClassSpecificEvent))
        {
            characterDetails += $"🌟 {newCharacter.ClassSpecificEvent}\n";
        }

        if (!string.IsNullOrEmpty(newCharacter.ArcheTypeSpecificInfo))
        {
            characterDetails += $"ℹ️ Extra info: {newCharacter.ArcheTypeSpecificInfo}\n";
        }

        return $"```\n" +
               $"🎲 NEW CHARACTER STATS\n\n" +
               $"💪 Strength:  [{GetAbilityModifier(newCharacter.Strength),3}]  (Rolled: {newCharacter.Strength,2})\n" +
               $"🏃 Agility:   [{GetAbilityModifier(newCharacter.Agility),3}]  (Rolled: {newCharacter.Agility,2})\n" +
               $"👑 Presence:  [{GetAbilityModifier(newCharacter.Presence),3}]  (Rolled: {newCharacter.Presence,2})\n" +
               $"🛡️ Toughness: [{GetAbilityModifier(newCharacter.Toughness),3}]  (Rolled: {newCharacter.Toughness,2})\n\n" +
               $"❤️ Hit Points: {newCharacter.Hp}\n" +
               $"🟡 Gold: {newCharacter.Gold}\n" +
               $"```\n" +
               $"```\n" +
               $"🎲 CHARACTER TRAITS\n\n" +
               $"🎭 Character Type: {newCharacter.Type}\n" +
               $"💫 Character Wants: {newCharacter.Wants}\n" +
               $"⚠️ Character Setback: {newCharacter.SetBack}\n" +
               $"```\n" +
               $"```\n" +
               characterDetails +
               "```";
    }
}