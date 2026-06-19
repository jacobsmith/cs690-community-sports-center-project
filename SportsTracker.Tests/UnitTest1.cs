using System.Runtime.CompilerServices;

namespace SportsTracker;

public class UnitTest1
{

    [Fact]
    public async Task CanAddAndRetrieveEquipment()
    {
        var factory = new TestDbContextFactory();
        var _context = factory.Context;
        var equipment = new Equipment
        {
            Name = "Soccer Ball",
            ValueInDecimal = 10.0M,
            Status = EquipmentStatus.Undamaged,
            inInventory = true
        };
        _context.Equipment.Add(equipment);
        await _context.SaveChangesAsync();

        var retrieved = _context.Equipment.Where(item => item.Name == "Soccer Ball").ToList();

        Assert.NotNull(retrieved);
        Assert.Equal("Soccer Ball", retrieved[0].Name);
    }
}
