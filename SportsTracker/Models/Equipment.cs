namespace SportsTracker;

public enum EquipmentStatus
{
    Undamaged,
    Damaged
}

public class Equipment : BaseEntity
{
    public required string Name { get; set; }
    public required decimal ValueInDecimal { get; set; }
    public required EquipmentStatus Status { get; set; }
    public required Boolean inInventory { get; set; }

    // allow for a reservation to end early, not be returned on time, etc.
    public int? currentlyActiveReservationId { get; set; }
    public Reservation CurrentlyActiveReservation { get; set; }

    public ICollection<EquipmentReservation> EquipmentReservations { get; set; } = new List<EquipmentReservation>();
    override public string SelectionDisplay()
    {
        return this.Name;
    }
}