namespace SportsTracker;

using System.Collections;
using System.Numerics;
using Spectre.Console;

class Program
{
    static void Main(string[] args)
    {
        var db = new AppDbContext();
        var running = true;


        while (running) {
        // ask employee what to do (add borrower, add equipment, check in equipment, check out equipment)
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>().Title("What would you like to do?").AddChoices("Check Out Equipment", "Check In Equipment", "Add Borrower", "Add Equipment", "Quit")
        );

        switch (choice)
        {
            case ("Check Out Equipment"):
                {
                    Console.WriteLine("Check Out");

                    List<Equipment> equipment = db.Equipment.ToList();
                    var equipmentChoices = new List<string>();
                    foreach (var item in equipment)
                    {
                        if (item.inInventory && item.Status == EquipmentStatus.Undamaged) {
                            equipmentChoices.Add(item.Id + ":" + item.Name);
                        }
                    }

                    var selected = AnsiConsole.Prompt(new MultiSelectionPrompt<string>()
                    .Title("Select equipment to check out")
                    .AddChoices(equipmentChoices.ToArray()));


                    var selectedEquipmentIds = new List<Int32>();
                    foreach (var item in selected)
                    {
                        selectedEquipmentIds.Add(Int32.Parse(item.Split(":")[0]));
                    }

                    var selectedEquipment = equipment.FindAll(equipment => selectedEquipmentIds.Contains(equipment.Id));

                    var borrowers = db.Borrower.ToList();
                    var borrowerChoices = new List<string>();
                    foreach (var borrower in borrowers)
                    {
                        borrowerChoices.Add(borrower.Id.ToString() + ":" + borrower.Name + " (" + borrower.PhoneNumber.ToString() + ")");
                    }
                    borrowerChoices.Add("Add New Borrower");

                    var borrowerString = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("Select borrower to assign to")
                    .AddChoices(borrowerChoices));

                    Borrower? selectedBorrower = null;
                    if (borrowerString == "Add New Borrower")
                    {
                        Borrower? maybeBorrower = BorrowerUI.AddBorrower(db);

                        if (maybeBorrower != null)
                        {
                            selectedBorrower = maybeBorrower;
                        }
                    } else
                    {
                        int borrowerId = Int32.Parse(borrowerString.Split(":")[0]);
                        Borrower? possibleBorrower = db.Borrower.Find(borrowerId);

                        if (possibleBorrower != null)
                        {
                            selectedBorrower = possibleBorrower;
                        }
                    }
                    if (selectedBorrower == null)
                    {
                        break;
                    }

                    var today = DateTime.Now;

                    var dateOptions = new List<string>();
                    for (int i = 0; i < 7; i++)
                    {
                        var date = today.AddDays(i);
                        dateOptions.Add(date.ToLongDateString());
                    }


                    var fromDate = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("Select checkout date")
                    .AddChoices(dateOptions));

                    var hours = new List<string>([
                        "8:00 am",
                        "9:00 am",
                        "10:00 am",
                        "11:00 am",
                        "12:00 pm",
                        "1:00 pm",
                        "2:00 pm",
                        "3:00 pm",
                        "4:00 pm",
                        "5:00 pm",
                        "6:00 pm",
                        "7:00 pm",
                        "8:00 pm",
                    ]);

                    var fromTime = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Checkout Time").AddChoices(hours));

                    // Only show hours after the checkout time
                    var toHours = new List<string>();
                    var haveSeenFromTime = false;
                    for (var i = 0; i < hours.Count; i++)
                    {
                        if (fromTime == hours[i])
                        {
                            haveSeenFromTime = true;
                            continue;
                        }

                        if (haveSeenFromTime)
                        {
                            toHours.Add(hours[i]);
                        }
                    }

                    var toTime = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Return Time").AddChoices(toHours));

                    var beginDateTime = DateTime.Parse(fromDate + " " + fromTime);
                    var endDateTime = DateTime.Parse(fromDate + " " + toTime);

                    foreach (var item in selectedEquipment)
                    {
                        var reservation = new Reservation { beginDateTime=beginDateTime, endDateTime=endDateTime, equipment=item, borrower=selectedBorrower };
                        db.Reservation.Add(reservation);
                        item.inInventory = false;

                        db.SaveChanges();
                    }

                    break;
                }
            case ("Check In Equipment"):
                {
                    Console.WriteLine("Check In Equipment");

                    List<Equipment> equipment = db.Equipment.ToList();
                    var equipmentChoices = new List<string>();
                    foreach (var item in equipment)
                    {
                        if (!item.inInventory) {
                            equipmentChoices.Add(item.Id + ":" + item.Name);
                        }
                    }

                    var selected = AnsiConsole.Prompt(new MultiSelectionPrompt<string>()
                    .Title("Select equipment to check in")
                    .AddChoices(equipmentChoices.ToArray()));


                    var selectedEquipmentIds = new List<Int32>();
                    foreach (var item in selected)
                    {
                        selectedEquipmentIds.Add(Int32.Parse(item.Split(":")[0]));
                    }

                    var selectedEquipment = equipment.FindAll(equipment => selectedEquipmentIds.Contains(equipment.Id));

                    foreach (var item in selectedEquipment)
                    {
                        item.inInventory = true;
                    }
                    db.SaveChanges();
                    break;
                }
            case ("Add Borrower"):
                {
                    BorrowerUI.AddBorrower(db);
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
                    running = false;
                    break;
                }
        }
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
