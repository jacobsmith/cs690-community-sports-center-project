namespace SportsTracker;

class Program
{
    static void Main(string[] args)
    {
        var db = new AppDbContext();

        db.Equipment.Add(new Equipment { Name="Test Baseball", Status=EquipmentStatus.Undamaged, ValueInCents=4, inInventory = true });
        db.SaveChanges();

        List<Equipment> equipmentList = db.Equipment.ToList();
        for (int i = 0; i < equipmentList.Count; i++)
        {
            Console.WriteLine(equipmentList[i].Name + " " + equipmentList[i].Id);
        }

        Console.WriteLine("hello world");
    }
}
