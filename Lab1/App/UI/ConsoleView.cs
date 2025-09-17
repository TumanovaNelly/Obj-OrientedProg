using Obj_OrientedProg.Lab1.App.Controllers;
using Obj_OrientedProg.Lab1.Contracts.DTOs;

namespace Obj_OrientedProg.Lab1.App.UI;

public static class ConsoleView
{
    public static void Clear()
    {
        for (int i = 0; i < 100; i++)
            Console.WriteLine();
        Console.Clear();
    }

    public static void ShowMenu(string titleInfo, List<Command> commands)
    {
        string title = $"~~~~~ {titleInfo} ~~~~~";
        DisplayMessage(title, ConsoleColor.Cyan);

        foreach (var command in commands)
        {
            DisplayMessage($"[{command}] ", ConsoleColor.Cyan, false);
            Console.WriteLine(command.GetDescription());
        }
        
        DisplayMessage(new string('~', title.Length), ConsoleColor.Cyan);
        Console.WriteLine();
    }

    public static void DisplayProducts(IEnumerable<ProductInfo> products, int depositedAmount)
    {
        Console.WriteLine("~~~~~ Ассортимент ~~~~~");
        foreach (var productInfo in products)
            Console.WriteLine($"{productInfo.Name} ({productInfo.Count} шт.) : {productInfo.Price}$");
        
        Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~");
        Console.WriteLine($"Внесено средств: {depositedAmount}$");
        Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~");
        Console.WriteLine();
    }

    public static void DisplayWalletInfo(string titleInfo, WalletInfo walletInfo)
    {
        string title = $"~~~~~ {titleInfo} ~~~~~";
        Console.WriteLine(title);
        Console.WriteLine($"Всего денег: {walletInfo.TotalAmount}$");
        foreach (var kvp in walletInfo.CoinCounts)
            Console.WriteLine($"{(int)kvp.Key}$ ({kvp.Value} шт.)");
        Console.WriteLine(new string('~', title.Length));
        Console.WriteLine();
    }

    public static void DisplayUserInfo(HumanInfo humanInfo)
    {
        DisplayWalletInfo("Ваши деньги", humanInfo.WalletInfo);
        Console.Write("Купленные продукты: ");
        foreach (var productName in humanInfo.ProductsInfo)
        {
            Console.Write(productName);
            Console.Write(' ');
        }
        Console.WriteLine();
        Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~");
    }

    public static void DisplayError(string message) => DisplayMessage(message, ConsoleColor.Magenta);

    public static void DisplayRequestMessage(string message) => DisplayMessage(message, ConsoleColor.Green, false);
    
    private static void DisplayMessage(string message, ConsoleColor color, bool toNewLine = true)
    {
        Console.ForegroundColor = color;
        Console.Write(message);
        if (toNewLine) Console.WriteLine();
        Console.ResetColor();
    }
}