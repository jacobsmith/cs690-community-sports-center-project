namespace SportsTracker;

class Program
{
    static void Main(string[] args)
    {
        var db = new AppDbContext();
        db.Database.Migrate();

        var running = true;
        Console.Clear();

        string CheckOut = "Check Out Equipment";
        string CheckIn = "Check In Equipment";
        string ViewAll = "View All Equipment";
        string AddBorrower = "Add Borrower";
        string AddEquipment = "Add Equipment";
        string Quit = Quit;


        while (running) {
        // ask employee what to do (add borrower, add equipment, check in equipment, check out equipment)
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>().Title("What would you like to do?").AddChoices(CheckOut, CheckIn, ViewAll, AddBorrower, AddEquipment, Quit)
        );

        switch (choice)
        {
            case (CheckOut):
                {
                    EquipmentUI.CheckOutEquipment(db);
                    break;
                }
            case (CheckIn):
                {
                    EquipmentUI.CheckInEquipment(db);
                    break;
                }
            case (ViewAll):
                {
                    EquipmentUI.ViewAllEquipment(db);
                    break;
                }
            case (AddBorrower):
                {
                    BorrowerUI.AddBorrower(db);
                    break;
                }
            case (AddEquipment):
                {
                    EquipmentUI.AddEquipment(db);
                    break;
                }
            case (Quit):
                {
                    Console.WriteLine("Quit");
                    running = false;
                    break;
                }
        }

        Console.Clear();
        }
    }
}
