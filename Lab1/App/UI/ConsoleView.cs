namespace Obj_OrientedProg.Lab1.App.UI;

using Obj_OrientedProg.Lab1.Contracts.DTOs;

public static class ConsoleView
{
    public static void ShowCustomerMenu()
    {
        Console.WriteLine("~~~~~ Меню Клиента ~~~~~");
        Console.WriteLine("1. Посмотреть товары");
        Console.WriteLine("2. Посмотреть, сколько денег в кошельке");
        Console.WriteLine("3. Внести монету");
        Console.WriteLine("4. Купить продукт");
        Console.WriteLine("5. Вернуть внесенные средства");
        Console.WriteLine("6. Перейти в режим админа");
        Console.WriteLine("7. Уйти");
        Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~");
        Console.WriteLine();
    }

    public static void ShowAdminMenu()
    {
        Console.WriteLine("~~~~~ Меню Админа ~~~~~");
        Console.WriteLine("1. Посмотреть товары");
        Console.WriteLine("2. Посмотреть, сколько денег в кошельке");
        Console.WriteLine("3. Посмотреть, сколько денег в автомате");
        Console.WriteLine("4. Забрать все деньги");
        Console.WriteLine("5. Положить деньги");
        Console.WriteLine("6. Положить товар");
        Console.WriteLine("7. Изменить цену на товар");
        Console.WriteLine("8. Перейти в режим пользователя");
        Console.WriteLine("9. Уйти");
        Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~");
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
            Console.WriteLine($"{(int)kvp.Key}$ ({kvp.Value})");
        Console.WriteLine(new string('~', title.Length));
        Console.WriteLine();
    }

    public static void DisplayMessage(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void DisplayError(string message)
    {
        DisplayMessage(message, ConsoleColor.Magenta);
    }
}