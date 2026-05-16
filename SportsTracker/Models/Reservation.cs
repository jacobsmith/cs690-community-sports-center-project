namespace SportsTracker;

public class Reservation
{
    public int Id { get; set; }
    public required Equipment equipment { get; set; }
    public required DateTime beginDateTime { get; set; }
    public required DateTime endDateTime { get; set; }
}