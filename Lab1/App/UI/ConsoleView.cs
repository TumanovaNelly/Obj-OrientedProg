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
    public static void ShowMainMenu()
    {
        DisplayMessage("~~~~~ Меню ~~~~~", ConsoleColor.Cyan);
        Console.WriteLine("1. Старт");
        Console.WriteLine("2. Посмотреть информацию о пользователе");
        Console.WriteLine("3. Пополнить кошелек");
        Console.WriteLine("4. Сменить пользователя");
        Console.WriteLine("5. Выход");
        DisplayMessage("~~~~~~~~~~~~~~~~", ConsoleColor.Cyan);
        Console.WriteLine();
    }
    public static void ShowCustomerMenu()
    {
        DisplayMessage("~~~~~ Меню Клиента ~~~~~", ConsoleColor.Cyan);
        Console.WriteLine("1. Посмотреть товары");
        Console.WriteLine("2. Посмотреть информацию о пользователе");
        Console.WriteLine("3. Внести монету");
        Console.WriteLine("4. Купить продукт");
        Console.WriteLine("5. Вернуть внесенные средства");
        Console.WriteLine("6. Перейти в режим админа");
        Console.WriteLine("7. Уйти");
        DisplayMessage("~~~~~~~~~~~~~~~~~~~~~~~~", ConsoleColor.Cyan);
        Console.WriteLine();
    }

    public static void ShowAdminMenu()
    {
        DisplayMessage("~~~~~ Меню Админа ~~~~~", ConsoleColor.Cyan);
        Console.WriteLine("1. Посмотреть товары");
        Console.WriteLine("2. Посмотреть информацию о пользователе");
        Console.WriteLine("3. Посмотреть, сколько денег в автомате");
        Console.WriteLine("4. Забрать все деньги");
        Console.WriteLine("5. Положить деньги");
        Console.WriteLine("6. Положить товар");
        Console.WriteLine("7. Изменить цену на товар");
        Console.WriteLine("8. Перейти в режим пользователя");
        Console.WriteLine("9. Уйти");
        DisplayMessage("~~~~~~~~~~~~~~~~~~~~~~~", ConsoleColor.Cyan);
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