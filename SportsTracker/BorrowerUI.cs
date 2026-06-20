using SportsTracker;
using Spectre.Console;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;

class BorrowerUI
{
    public static Borrower? AddBorrower(AppDbContext db)
    {
                    Console.WriteLine("Add Borrower");
                    var getName = new TextPrompt<string>("Borrower Name: ").Validate(input => input.Length > 2, "[red]Must be at least 2 characters long.[/]");
                    var name = AnsiConsole.Prompt(getName);
                    
                    var getPhoneNumber = new TextPrompt<string>("Phone Number: ").Validate(input => input.Count(char.IsDigit) == 10, "Must enter a 10 digit phone number");
                    var phoneNumber = AnsiConsole.Prompt(getPhoneNumber);

                    Console.WriteLine("About to create:");
                    var newBorrower = new Table();
                    newBorrower.AddColumn("Name");
                    newBorrower.AddColumn("Phone Number");
                    newBorrower.AddRow(name, phoneNumber);
                    AnsiConsole.Write(newBorrower);

                    if (AnsiConsole.Confirm("Create this Borrower?"))
                    {
                        Borrower borrower = new Borrower { Name=name, PhoneNumber=phoneNumber };
                        db.Borrower.Add(borrower);
                        db.SaveChanges();

                        return borrower;
                    }

                    return null;

    }

    public static void ViewBorrowers(AppDbContext db)
    {
        Console.WriteLine("View Borrowers");

        List<Borrower> borrowers = db.Borrower.Include(b => b.EquipmentReservations).ThenInclude(er => er.Equipment).Include(b => b.EquipmentDamages).ToList();
        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Name");
        table.AddColumn("Phone Number");
        table.AddColumn("Currently Checked Out");
        table.AddColumn("Total Damage");

        foreach (var borrowerToPrint in borrowers)
        {
            var totalDamage = borrowerToPrint.EquipmentDamages.Where(ed => ed.paid == false).Sum(ed => ed.damageAmount);
            var activeReservations = borrowerToPrint.EquipmentReservations.Where(er => er.Equipment.currentlyActiveReservationId == er.reservationId);

            table.AddRow(borrowerToPrint.Id.ToString(), borrowerToPrint.Name, borrowerToPrint.PhoneNumber, borrowerToPrint.EquipmentReservations.Count.ToString() + " items", totalDamage.ToString());

            foreach (var activeReservation in activeReservations)
            {
                table.AddRow("", "", "", activeReservation.Equipment.Name);
            }
            table.AddEmptyRow(); // blank row between borrowers
        }
        AnsiConsole.Write(table);
        
        var getId = AnsiConsole.Prompt(new TextPrompt<string>("ID of borrower to mark as paid or Q to quit: "));

        if (getId.ToUpper() == "Q")
        {
            return;
        }

        var id = int.Parse(getId);
        var borrower = db.Borrower.Find(id);
        if (borrower != null)
        {
            borrower.EquipmentDamages.Where(ed => ed.paid == false).ToList().ForEach(ed => ed.paid = true);
            db.SaveChanges();
        }

    }
}