using System.Numerics;

namespace SportsTracker;

public class EquipmentDamage : BaseEntity
{
    public required Equipment equipment { get; set; }
    public required Borrower borrower { get; set; }
    public required decimal damageAmount { get; set; }

    public required string description { get; set; }
    public required bool paid { get; set; }

    override public string SelectionDisplay()
    {
        return this.equipment.Name + " - " + this.borrower.SelectionDisplay() + " (" + this.damageAmount + ") - " + this.description;
    }
}