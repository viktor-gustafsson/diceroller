namespace DiscordBot.Rollers.CharacterRollers.Models;

public class NewCharacterDto
{
    public required int Strength { get; init; }
    public required int Agility { get; init; }
    public required int Presence { get; init; }
    public required int Toughness { get; init; }
    public required int Hp { get; init; }
    public required int Gold { get; init; }
    public required string ClassSpecificEvent { get; init; }
    public required string SpecificInfo { get; init; }
    public required string SubTypeSpecificInfo { get; init; }
}