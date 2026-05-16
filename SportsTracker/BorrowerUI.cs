using SportsTracker;
using Spectre.Console;
using System.Data.Common;

class BorrowerUI
{
    public static Borrower? AddBorrower(AppDbContext db)
    {
                    Console.WriteLine("Add Borrower");
                    var getName = new TextPrompt<string>("Borrower Name: ").Validate(input => input.Length > 2, "[red]Must be at least 2 characters long.[/]");
                    var name = AnsiConsole.Prompt(getName);
                    
                    var getPhoneNumber = new TextPrompt<string>("Phone Number").Validate(input => input.Count(char.IsDigit) == 10, "Must enter a 10 digit phone number");
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
}