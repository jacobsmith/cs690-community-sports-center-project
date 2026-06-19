namespace SportsTracker;

public class EquipmentReservation : BaseEntity
{
    public required int reservationId { get; set; }
    public Reservation Reservation { get; set; }

    public required int equipmentId { get; set; }

    public EquipmentReservation Equipment { get; set; }

    public required Borrower borrower { get; set; }

    public DateTime? returnedAt { get; set; }

    // This acts as a join table so won't be printed directly
    override public string SelectionDisplay()
    {
        return "";
    }
}