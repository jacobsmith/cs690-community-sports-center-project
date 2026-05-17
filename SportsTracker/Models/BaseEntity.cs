namespace SportsTracker;
public abstract class BaseEntity
{
    public int Id { get; set; }

    abstract public string SelectionDisplay();
}