namespace SportsTracker;
public class Borrower : BaseEntity
{
    public required string Name { get; set; }
    public required string PhoneNumber { get; set; }
    public ICollection<EquipmentDamage> EquipmentDamages { get; set; } = new List<EquipmentDamage>();
    public ICollection<EquipmentReservation> EquipmentReservations { get; set; } = new List<EquipmentReservation>();
    override public string SelectionDisplay()
    {
        return this.Name + "    " + this.PhoneNumber;
    }
}