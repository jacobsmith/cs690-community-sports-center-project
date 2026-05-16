namespace SportsTracker;

public enum EquipmentStatus
{
    Undamaged,
    Damaged
}

public class Equipment
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int ValueInCents { get; set; }
    public required EquipmentStatus Status { get; set; }
    public required Boolean inInventory { get; set; }
}