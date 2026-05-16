namespace SportsTracker;

using System.Numerics;
using Spectre.Console;

class Program
{
    static void Main(string[] args)
    {
        var db = new AppDbContext();


        // ask employee what to do (add borrower, add equipment, check in equipment, check out equipment)
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>().Title("What would you like to do?").AddChoices("Check Out Equipment", "Check In Equipment", "Add Borrower", "Add Equipment", "Quit")
        );

        switch (choice)
        {
            case ("Check Out Equipment"):
                {
                    Console.WriteLine("Check Out");
                    break;
                }
            case ("Check In Equipment"):
                {
                    Console.WriteLine("Check In Equipment");
                    break;
                }
            case ("Add Borrower"):
                {
                    Console.WriteLine("Add Borrower");
                    break;
                }
            case ("Add Equipment"):
                {
                    Console.WriteLine("Add Equipment");

                    var getName = new TextPrompt<string>("Equipment Name: ").Validate(input => input.Length > 2, "[red]Must be at least 2 characters long.[/]");
                    var name = AnsiConsole.Prompt(getName);
                    
                    // var damagedStatus = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Current Status").AddChoices("Undamaged", "Damaged"));
                    // var status = damagedStatus == "Damaged" ? EquipmentStatus.Damaged : EquipmentStatus.Undamaged;
                    
                    var getValue = AnsiConsole.Prompt(new TextPrompt<string>("Value of item: ").Validate(input => input.Contains("."), "Must enter dollar amount"));
                    var value = decimal.Parse(getValue);

                    Console.WriteLine("About to create:");
                    var newEquipment = new Table();
                    newEquipment.AddColumn("Name");
                    newEquipment.AddColumn("Status");

                    // newEquipment.AddRow(name, damagedStatus);
                    AnsiConsole.Write(newEquipment);

                    if (AnsiConsole.Confirm("Create this piece of equipment?"))
                    {
                        db.Equipment.Add(new Equipment { Name=name, Status=EquipmentStatus.Undamaged, ValueInDecimal=value, inInventory = true });
                        db.SaveChanges();
                    }


                    break;
                }
            case ("Quit"):
                {
                    Console.WriteLine("Quit");
                    break;
                }
        }



        // db.Equipment.Add(new Equipment { Name="Test Baseball", Status=EquipmentStatus.Undamaged, ValueInDecimal=4, inInventory = true });
        // db.SaveChanges();

        List<Equipment> equipmentList = db.Equipment.ToList();

        var table = new Table();
        table.AddColumn("ID");
        table.AddColumn("Name");
        table.AddColumn("Status");
        table.AddColumn("In Inventory");
        table.AddColumn("Value");
        for (int i = 0; i < equipmentList.Count; i++)
        {
            var e = equipmentList[i];
            table.AddRow(e.Id.ToString(), e.Name, e.Status.ToString(), e.inInventory.ToString(), e.ValueInDecimal.ToString());
        }

        AnsiConsole.Write(table);
    }
}
