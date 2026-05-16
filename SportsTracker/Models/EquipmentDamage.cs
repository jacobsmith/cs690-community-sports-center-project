using System.Numerics;

namespace SportsTracker;

public class EquipmentDamage
{
    public int Id { get; set; }
    public required Equipment equipment { get; set; }
    public required Borrower borrower { get; set; }
    public required int damageAmount { get; set; }

}