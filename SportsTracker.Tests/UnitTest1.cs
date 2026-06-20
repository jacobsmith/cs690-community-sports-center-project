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
            inInventory = true,
        };
        _context.Equipment.Add(equipment);
        await _context.SaveChangesAsync();

        var retrieved = _context.Equipment.Where(item => item.Name == "Soccer Ball").ToList();

        Assert.NotNull(retrieved);
        Assert.Equal("Soccer Ball", retrieved[0].Name);
    }

    [Fact]
    public async Task CanCreateAReservation()
    {
        var factory = new TestDbContextFactory();
        var _context = factory.Context;
        
        var equipment = new Equipment
        {
            Name = "Soccer Ball",
            ValueInDecimal = 10.0M,
            Status = EquipmentStatus.Undamaged,
            inInventory =  true
        };
        _context.Equipment.Add(equipment);
        await _context.SaveChangesAsync();

        var borrower = new Borrower
        {
            Name = "John Doe",
            PhoneNumber = "123456890"
        };
        _context.Borrower.Add(borrower);
        await _context.SaveChangesAsync();

        var reservation = new Reservation
        {
            beginDateTime = new DateTime(),
            endDateTime = new DateTime()
        };
        _context.Reservation.Add(reservation);
        await _context.SaveChangesAsync();

        var equipmentReservation = new EquipmentReservation
        {
            equipmentId = equipment.Id,
            borrower = borrower,
            reservationId = reservation.Id,
        };
        _context.EquipmentReservation.Add(equipmentReservation);
        await _context.SaveChangesAsync();

        var retrieved = _context.EquipmentReservation.ToList();
        Assert.Equal("Soccer Ball John Doe    123456890 0001-01-01 00:00 - 00:00     1 items", retrieved[0].SelectionDisplay());
    }

    [Fact]
    public async Task CanGetCheckedOutTo()
    {
        var factory = new TestDbContextFactory();
        var _context = factory.Context;
        
        var equipment = new Equipment
        {
            Name = "Soccer Ball 2",
            ValueInDecimal = 10.0M,
            Status = EquipmentStatus.Undamaged,
            inInventory =  true
        };
        _context.Equipment.Add(equipment);
        await _context.SaveChangesAsync();

        var borrower = new Borrower
        {
            Name = "John Doe",
            PhoneNumber = "123456890"
        };
        _context.Borrower.Add(borrower);
        await _context.SaveChangesAsync();

        var reservation = new Reservation
        {
            beginDateTime = new DateTime(),
            endDateTime = new DateTime()
        };
        _context.Reservation.Add(reservation);
        await _context.SaveChangesAsync();

        var equipmentReservation = new EquipmentReservation
        {
            equipmentId = equipment.Id,
            borrower = borrower,
            reservationId = reservation.Id,
        };
        _context.EquipmentReservation.Add(equipmentReservation);
        await _context.SaveChangesAsync();

        equipment.currentlyActiveReservationId = reservation.Id;
        await _context.SaveChangesAsync();

        var retrievedEquipment = _context.Equipment.Find(equipment.Id);
        Assert.Equal(1, retrievedEquipment?.currentlyActiveReservationId);
    }
}
