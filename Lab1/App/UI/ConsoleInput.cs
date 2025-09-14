using Obj_OrientedProg.Lab1.Core.Models;

namespace Obj_OrientedProg.Lab1.App.UI;

// Этот класс тоже можно сделать статическим
public static class ConsoleInput
{
    public static string ReadCommand()
    {
        string? command = null;
        while (string.IsNullOrWhiteSpace(command))
        {
            Console.Write("Выберите команду: ");
            command = Console.ReadLine();
        }
        
        return command;
    }

    public static string ReadPassword()
    {
        string? password = null;
        while (password is null)
        {
            Console.Write("Введите пароль: ");
            password = Console.ReadLine();
        }
        
        return password;
    }

    public static (List<int> ValidNominals, List<string> IgnoredInputs) ReadNominals()
    {
        string? nominalsData = null;
        while (nominalsData is null || string.IsNullOrWhiteSpace(nominalsData))
        {
            Console.Write("Введите номиналы через пробел: ");
            nominalsData = Console.ReadLine();
        }

        var validNominals = new List<int>(); 
        var ignoredInputs = new List<string>();
        foreach (var nominalData in nominalsData.Trim().Split())
        {
            if (int.TryParse(nominalData, out int nominal) && Enum.IsDefined(typeof(NominalValue), nominal))
                validNominals.Add(nominal);
            else ignoredInputs.Add(nominalData);
        }
        
        return (validNominals, ignoredInputs);
        
    }

    public static string ReadProductName()
    {
        string? productName = null;
        while (string.IsNullOrWhiteSpace(productName))
        {
            Console.Write("Введите название товара: ");
            productName = Console.ReadLine();
        }
        
        return productName;
    }
    
    public static int ReadPrice(string productName)
    {
        int price;

        while (true)
        {
            Console.Write($"Установите цену для {productName}: ");
            string? input = Console.ReadLine();

            if (!int.TryParse(input, out price))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid price data");
                Console.ResetColor();
            }
            else break;
        }
        return price;
    }
    
    public static List<string> ReadProductNames()
    {
        string? productsData = null;
        while (productsData is null || string.IsNullOrWhiteSpace(productsData))
        {
            Console.Write("Введите названия продуктов через \"|\" : ");
            productsData = Console.ReadLine();
        }

        return (from productData in productsData.Split("|").ToList() 
            where !string.IsNullOrWhiteSpace(productData) 
            select productData.Trim()).ToList();
    }
}