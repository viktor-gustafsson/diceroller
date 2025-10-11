namespace DiscordBot.Rollers.CharacterRollers;

public interface ICharacterRoller<in T> where T : Enum
{
    public static abstract string Roll(T subType);
}