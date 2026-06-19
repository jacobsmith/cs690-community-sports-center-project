namespace SportsTracker.Tests;

public class UnitTest1
{

    [Fact]
    public async Task CanAddAndRetrieveEquipment()
    {
        var _context = new TestDbContextFactory();
        var equipment = new Equipment { Name = "Soccer Ball" };
        _context.Equipment.Add(equipment);
        await _context.SaveChangesAsync();

        var retrieved = await _context.Equipment.FirstOrDefaultAsync(e => e.Name == "Soccer Ball");

        Assert.NotNull(retrieved);
        Assert.Equal("Soccer Ball", retrieved.Name);
    }
}
