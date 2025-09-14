using Obj_OrientedProg.Lab1.App.UI;
using Obj_OrientedProg.Lab1.Core.Models;

namespace Obj_OrientedProg.Lab1;

public class Program
{
    public static void Main(string[] args)
    {
        Human human = new Human();

        List<Coin> salary = [];

        foreach (var nominal in ((NominalValue[])Enum.GetValues(typeof(NominalValue))).ToList())
        {
            for (int index = 0; index < 100; index++)
                salary.Add(new Coin(nominal));
        }
        
        human.GetSalary(salary);

        VendingMachineController service = new VendingMachineController("admin123", human);

        bool isBegin = true;
        while (isBegin)
        {
            ConsoleView.DisplayMessage("To interact with the vending machine write \"start\"", ConsoleColor.Cyan);
            ConsoleView.DisplayMessage("If you want go out, write \"exit\"", ConsoleColor.Cyan);
            string input = ConsoleInput.ReadCommand();

            switch (input)
            {
                case "start":
                    Console.Clear();
                    service.Run();
                    break;
                case "exit":
                    isBegin = false;
                    break;
                default:
                    ConsoleView.DisplayError("Invalid input");
                    break;
            }
            
        }
        
    }
}