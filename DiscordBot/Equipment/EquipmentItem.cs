using DiscordBot.Equipment.Enums;

namespace DiscordBot.Equipment;

public class EquipmentItem
{
    public EquipmentName Name { get; }
    public string Weight { get; }
    public EquipmentCategory Category { get; }
    public int Quantity { get; }

    public EquipmentItem(EquipmentName name, int quantity = 1)
    {
        Name = name;
        Quantity = quantity;
        (Weight, Category) = EquipmentData.GetItemData(name);
    }

    public string DisplayName => EquipmentData.GetDisplayName(Name);
    public string CategoryDisplayName => EquipmentData.GetCategoryDisplayName(Category);
}
