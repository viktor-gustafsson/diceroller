namespace DiscordBot.Rollers.CharacterRollers.Models;

public class Character
{
    public required string Type { get; init; }
    public required string Wants { get; init; }
    public required string SetBack { get; init; }
    public required string AdditionalSkill { get; init; }
    public required string Passion { get; init; }
    public required string PhysicalAttribute { get; init; }
    public required string PartyConnection { get; init; }
    public required int Hp { get; init; }
    public required int Gold { get; init; }
    public required int Strength { get; init; }
    public required int Agility { get; init; }
    public required int Presence { get; init; }
    public required int Toughness { get; init; }
    public required string ClassSpecificEvent { get; init; }
    public required string ArcheTypeSpecificInfo { get; init; }
    public required string SubTypeSpecificInfo { get; set; } = "";

}