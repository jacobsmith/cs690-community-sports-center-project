using System.Numerics;

namespace SportsTracker;

public class EquipmentDamage : BaseEntity
{
    public required Equipment equipment { get; set; }
    public required Borrower borrower { get; set; }
    public required int damageAmount { get; set; }

    override public string SelectionDisplay()
    {
        return this.equipment.Name + " - " + this.borrower.SelectionDisplay() + " (" + this.damageAmount + ")";
    }
}