namespace SportsTracker;
using SportsTracker;
using Spectre.Console;
using Microsoft.EntityFrameworkCore;

class EquipmentUI
{
    public static void AddEquipment(AppDbContext db)
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
        newEquipment.AddColumn("Value");

        newEquipment.AddRow(name, "$" + value.ToString());
        AnsiConsole.Write(newEquipment);

        if (AnsiConsole.Confirm("Create this piece of equipment?"))
        {
            db.Equipment.Add(new Equipment { Name = name, Status = EquipmentStatus.Undamaged, ValueInDecimal = value, inInventory = true });
            db.SaveChanges();
        }
    }

    public static void CheckInEquipment(AppDbContext db)
    {

        Console.WriteLine("Check In Equipment");

        List<Equipment> equipment = db.Equipment.Where(item => item.currentlyActiveReservationId != null).ToList();

        var item = new Selector<Equipment>(equipment).GetSelectionSingular();
        if (item != null) {

            var reservation = db.Reservation.Find(item.currentlyActiveReservationId);

            if (reservation == null) {
                Console.WriteLine("No reservation found for this item. Press any key to continue.");
                Console.ReadKey();
                return;
            }

            var selected = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Item has damage?")
                .AddChoices(["yes", "no"])
            );

            if (selected == "yes")
            {
                var getValue = AnsiConsole.Prompt(new TextPrompt<string>("Charge for damages: ").Validate(input => input.Contains("."), "Must enter dollar amount"));
                var value = decimal.Parse(getValue);

                var getDescription = AnsiConsole.Prompt(new TextPrompt<string>("Description of damages: "));
                var description = getDescription;

                item.Status = EquipmentStatus.Damaged;
                db.SaveChanges();
            
                var damagePaid = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Damage paid?")
                    .AddChoices(["yes", "no"])
                );

                var equipmentDamage = new EquipmentDamage { equipment = item, borrower = reservation.EquipmentReservations.First().borrower, damageAmount = value, description = description, paid = damagePaid == "yes" };
                db.EquipmentDamage.Add(equipmentDamage);
                db.SaveChanges();
            }

            item.inInventory = true;
            item.currentlyActiveReservationId = null;
            db.SaveChanges();
        }
    }

    public static void ViewDamages(AppDbContext db)
    {
        Console.WriteLine("View Damages");

        List<EquipmentDamage> damages = db.EquipmentDamage.ToList();
        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Equipment");
        table.AddColumn("Borrower");
        table.AddColumn("Damage Amount");
        table.AddColumn("Description");

        foreach (var damage in damages)
        {
            table.AddRow(damage.Id.ToString(), damage.equipment.Name, damage.borrower.Name, damage.damageAmount.ToString(), damage.description);
        }

        AnsiConsole.Write(table);


        AnsiConsole.Markup("Press any key to continue...");
        Console.ReadKey();
    }

    public static void ViewAllEquipment(AppDbContext db)
    {

        List<Equipment> equipment = db.Equipment.
            Include(e => e.CurrentlyActiveReservation).
            ThenInclude(r => r.EquipmentReservations).
            ThenInclude(er => er.borrower).
            ToList();
        
        var table = new Table();
        table.AddColumn("Name");
        table.AddColumn("In Stock");
        table.AddColumn("Status");
        table.AddColumn("Checked Out To");

        foreach (var item in equipment)
        {
            table.AddRow(item.Name, item.inInventory.ToString(), item.Status.ToString(), item.CheckedOutTo()?.Name ?? "Not Checked Out");
        }

        AnsiConsole.Write(table);
        AnsiConsole.Markup("Press any key to continue...");
        Console.ReadKey();
    }

    public static void CheckOutEquipment(AppDbContext db)
    {
        Console.WriteLine("Check Out");

        List<Equipment> equipment = db.Equipment.Where(item => item.inInventory).Where(item => item.Status == EquipmentStatus.Undamaged).ToList();
        var selectedEquipment = new Selector<Equipment>(equipment).GetSelectionMultiple();

        if (selectedEquipment.Count == 0)
        {
            return;
        }


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
        }
        else
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
            return;
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


        var borrowerHasItem = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Borrower is taking item now?")
            .AddChoices(["yes", "no"])
        ) == "yes";

        var reservation = new Reservation { beginDateTime = beginDateTime, endDateTime = endDateTime };
        db.Reservation.Add(reservation);
        db.SaveChanges();

        foreach (var item in selectedEquipment)
        {
            var equipmentReservation = new EquipmentReservation { equipmentId = item.Id, borrower = selectedBorrower, reservationId = reservation.Id };
            db.EquipmentReservation.Add(equipmentReservation);

            // set the item to be currently active reservation
            if (borrowerHasItem) {
                item.currentlyActiveReservationId = reservation.Id;
            }
            item.inInventory = !borrowerHasItem;
        }
        db.SaveChanges();
    }
}