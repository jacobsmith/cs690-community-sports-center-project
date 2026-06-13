using System.Collections;
using System.Runtime.InteropServices.Swift;
using Spectre.Console;
using SportsTracker;

class Selector<T> where T: BaseEntity
{
    List<T> choices;

    public Selector(List<T> choices)
    {
        this.choices = choices;
    }

    public List<T> GetSelection()
    {
        if (this.choices.Count == 0)
        {
            Console.WriteLine("No items to select from. Press any key to continue.");
            Console.ReadKey();
            return [];
        }

        var choices = new List<string>();
        foreach (var item in this.choices)
        {
            choices.Add(item.Id.ToString() + ":" + item.SelectionDisplay());
        }

        var selected = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
            .Title("Select")
            .AddChoices(choices.ToArray())
        );


        var selectedItems = new List<Int32>();
        foreach (var item in selected)
        {
            selectedItems.Add(Int32.Parse(item.Split(":")[0]));
        }

        var selectedEquipment = this.choices.FindAll(equipment => selectedItems.Contains(equipment.Id));

        return selectedEquipment;
    }

}
