namespace SportsTracker;

using System.Collections;
using System.Numerics;
using Spectre.Console;
using Microsoft.EntityFrameworkCore;


class Program
{
    static void Main(string[] args)
    {
        var db = new AppDbContext();
        db.Database.Migrate();

        var running = true;
        Console.Clear();


        while (running) {
        // ask employee what to do (add borrower, add equipment, check in equipment, check out equipment)
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>().Title("What would you like to do?").AddChoices("Check Out Equipment", "Check In Equipment", "View All Equipment", "Add Borrower", "Add Equipment", "Quit")
        );

        switch (choice)
        {
            case ("Check Out Equipment"):
                {
                    EquipmentUI.CheckOutEquipment(db);
                    break;
                }
            case ("Check In Equipment"):
                {
                    EquipmentUI.CheckInEquipment(db);
                    break;
                }
            case ("View All Equipment"):
                {
                    EquipmentUI.ViewAllEquipment(db);
                    break;
                }
            case ("Add Borrower"):
                {
                    BorrowerUI.AddBorrower(db);
                    break;
                }
            case ("Add Equipment"):
                {
                    EquipmentUI.AddEquipment(db);
                    break;
                }
            case ("Quit"):
                {
                    Console.WriteLine("Quit");
                    running = false;
                    break;
                }
        }

        Console.Clear();
        }



        // db.Equipment.Add(new Equipment { Name="Test Baseball", Status=EquipmentStatus.Undamaged, ValueInDecimal=4, inInventory = true });
        // db.SaveChanges();

        // List<Equipment> equipmentList = db.Equipment.ToList();

        // var table = new Table();
        // table.AddColumn("ID");
        // table.AddColumn("Name");
        // table.AddColumn("Status");
        // table.AddColumn("In Inventory");
        // table.AddColumn("Value");
        // for (int i = 0; i < equipmentList.Count; i++)
        // {
        //     var e = equipmentList[i];
        //     table.AddRow(e.Id.ToString(), e.Name, e.Status.ToString(), e.inInventory.ToString(), e.ValueInDecimal.ToString());
        // }

        // AnsiConsole.Write(table);
    }
}
