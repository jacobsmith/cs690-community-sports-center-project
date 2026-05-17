namespace SportsTracker;

public class Reservation : BaseEntity
{
    public required Equipment equipment { get; set; }
    public required DateTime beginDateTime { get; set; }
    public required DateTime endDateTime { get; set; }

    public required Borrower borrower { get; set; }

    override public string SelectionDisplay()
    {
        return this.equipment.SelectionDisplay() + " " + this.borrower.SelectionDisplay();
    }
}