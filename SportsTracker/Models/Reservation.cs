namespace SportsTracker;

public class Reservation : BaseEntity
{
    public required DateTime beginDateTime { get; set; }
    public required DateTime endDateTime { get; set; }

    public ICollection<EquipmentReservation> EquipmentReservations { get; set; } = new List<EquipmentReservation>();

    override public string SelectionDisplay()
    {
        string start = this.beginDateTime.ToString("yyyy-MM-dd HH:mm");
        string end = this.endDateTime.ToString("HH:mm");

        string equipmentCount = this.EquipmentReservations.Count().ToString();
        return start + " - " + end + "     " + equipmentCount + " items"; 
    }
}