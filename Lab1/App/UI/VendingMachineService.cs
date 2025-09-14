using Obj_OrientedProg.Lab1.Contracts.DTOs;
using Obj_OrientedProg.Lab1.Core.Models;

namespace Obj_OrientedProg.Lab1.App.UI;

public class VendingMachineService(string adminPassword)
{
    private readonly VendingMachine _vendingMachine = new VendingMachine();

    public void Interact(Human user)
    {
        ShowCustomerMenu();
        
        while (true)
        {
            Console.Write("Выберите команду: ");
            string? command = Console.ReadLine();
            
            switch (command)
            {
                case "1":
                    PrintProductsInfo();
                    break;
                case "2":
                    PrintHumanInfo(user);
                    break;
                case "3":
                    DepositCoinsCommand(user);
                    break;
                case "4":
                    BuyProductCommand(user);
                    break;
                case "5":
                    ReturnMoneyCommand(user);
                    break;
                case "6":
                    Console.Write("Введите пароль: ");
                    string? password = Console.ReadLine();
                    if (string.Compare(password, adminPassword, StringComparison.Ordinal) != 0)
                    {
                        PrintError("Invalid password");
                        continue;
                    }
                    InteractAsAdmin(user);
                    return;
                case "7":
                    return;
                default:
                    PrintError("Invalid command");
                    break;
            }
        }
    }

    private void InteractAsAdmin(Human user)
    {
        ShowAdminMenu();
        
        while (true)
        {
            Console.Write("Выберите команду: ");
            string? command = Console.ReadLine();

            switch (command)
            {
                case "1":
                    PrintProductsInfo();
                    break;
                case "2":
                    PrintHumanInfo(user);
                    break;
                case "3":
                    PrintMachineCashRegisterInfo();
                    break;
                case "4":
                    TakeAllMoneyCommand(user);
                    break;
                case "5":
                    DepositCoinsCommand(user);
                    break;
                case "6":
                    PutProductsCommand(user);
                    break;
                case "7":
                    ChangePriceCommand();
                    break;
                case "8":
                    Interact(user);
                    return;
                case "9":
                    return;
                default:
                    PrintError("Invalid command");
                    break;
            }
        }
    }

    private static void ShowCustomerMenu()
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

    private static void ShowAdminMenu()
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
    private void PrintProductsInfo()
    {
        Console.WriteLine("~~~~~ Ассортимент ~~~~~");
        foreach (var productInfo in _vendingMachine.GetProductStorageInfo())
            Console.WriteLine($"{productInfo.Name} ({productInfo.Count} шт.) : {productInfo.Price}$");
        
        Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~");
        Console.WriteLine($"Внесено средств: {_vendingMachine.DepositedAmount}$");
        Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~");
        Console.WriteLine();
    }

    private static void PrintHumanInfo(Human user)
    {
        Console.WriteLine("~~~~~ Ваши деньги ~~~~~");
        PrintWalletInfo(user.GetWalletInfo()); 
        Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~");
        Console.WriteLine();
    }
    
    private void PrintMachineCashRegisterInfo()
    {
        Console.WriteLine("~~~~~ Вся выручка ~~~~~");
        PrintWalletInfo(_vendingMachine.GetRevenueMoneyInfo());
        Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~");
        Console.WriteLine();
    }

    private static void PrintWalletInfo(WalletInfo walletInfo)
    {
        Console.WriteLine($"Всего денег: {walletInfo.TotalAmount}$");
        foreach (var kvp in walletInfo.CoinCounts)
            Console.WriteLine($"{(int)kvp.Key}$ ({kvp.Value})");
    }

    private void DepositCoinsCommand(Human user)
    {
        string? nominalsData = null;
        while (nominalsData is null || string.IsNullOrWhiteSpace(nominalsData))
        {
            Console.Write("Введите номиналы через пробел: ");
            nominalsData = Console.ReadLine();
        }
        
        var nominalsList = nominalsData.Split(" ").ToList();

        foreach (var nominalData in nominalsList)
        {
            if (!int.TryParse(nominalData, out int nominal) || !Enum.IsDefined(typeof(NominalValue), nominal))
            {
                PrintError($"Invalid nominal value {nominalData} was ignored");
                continue;
            }

            try
            {
                _vendingMachine.AcceptCoin(user.SpendSalary((NominalValue)nominal));
            }
            catch (KeyNotFoundException ex)
            {
                PrintError(ex.Message);
            }
        }
    }
    
    private void ReturnMoneyCommand(Human user)
    {
        try
        {
            user.GetSalary(_vendingMachine.ReturnDepositedAmount());
        }
        catch (ApplicationException ex)
        {
            PrintError(ex.Message);
        }
    }
    
    private void TakeAllMoneyCommand(Human user) => user.GetSalary(_vendingMachine.StealRevenue()); 

    private void ChangePriceCommand()
    {
        Console.Write("Введите название продукта: ");
        var productName = Console.ReadLine();
        if (productName is null || string.IsNullOrWhiteSpace(productName))
        {
            PrintError("Invalid product name");
            return;
        }
        
        Console.Write("Введите новую цену продукта: ");
        if (!int.TryParse(Console.ReadLine(), out int newPrice))
        {
            PrintError("Invalid new price data");
            return;
        }

        ChangePrice(productName, newPrice);
    }

    private void PutProductsCommand(Human user)
    {
        string? productsData = null;
        while (productsData is null || string.IsNullOrWhiteSpace(productsData))
        {
            Console.Write("Введите названия продуктов через \"|\" : ");
            productsData = Console.ReadLine();
        }
        
        var productsList = productsData.Split("|").ToList();

        foreach (var productData in productsList)
        {
            if (string.IsNullOrWhiteSpace(productData))
                continue;
                
            var productName = productData.Trim();
            var product = new Product(productName);
            _vendingMachine.AddProductToStorage(product);
            
            if (_vendingMachine.GetProductInfo(productName).Count > 1) continue;
            
            int newPrice = 0;
            while (newPrice == 0)
            {
                Console.Write($"Установите цену для {productName}: ");
                if (int.TryParse(Console.ReadLine(), out newPrice)) continue;
                PrintError("Invalid new price data");
                return;
            }
                
            ChangePrice(productName, newPrice);
        }
    }

    private void ChangePrice(string productName, int newPrice)
    {
        try
        {
            _vendingMachine.ChangeProductsPrice(productName, newPrice);
        }
        catch (ArgumentException ex)
        {
            PrintError(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            PrintError(ex.Message);
        }
    }
    
    private void BuyProductCommand(Human user)
    {
        Console.Write("Введите название продукта: ");
        var productName = Console.ReadLine();
        if (productName is null || string.IsNullOrWhiteSpace(productName))
        {
            PrintError("Invalid product name");
            return;
        }

        try
        {
            user.GetProduct(_vendingMachine.BuyProduct(productName)); 
        }
        catch (ApplicationException ex)
        {
            PrintError(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            PrintError(ex.Message);
        }
    }
    

    private static void PrintError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }

}