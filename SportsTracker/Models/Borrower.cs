namespace SportsTracker;
public class Borrower : BaseEntity
{
    public required string Name { get; set; }
    public required string PhoneNumber { get; set; }
    
    override public string SelectionDisplay()
    {
        return this.Name + "    " + this.PhoneNumber;
    }
}