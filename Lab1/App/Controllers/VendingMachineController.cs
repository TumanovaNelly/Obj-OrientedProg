using Obj_OrientedProg.Lab1.Core.Models;
using Obj_OrientedProg.Lab1.App.UI;

namespace Obj_OrientedProg.Lab1.App.Controllers;

public enum UserRole { Customer, Administrator }

public class VendingMachineController(string adminPassword, Human currentUser)
{
    private readonly VendingMachine _vendingMachine = new();
    public Human CurrentUser { private get; set; } = currentUser;

    private UserRole _currentRole;
    private bool _isRunning;

    private readonly Dictionary<int, Action> _customerCommands = new();
    private readonly Dictionary<int, Action> _adminCommands = new();

    public void Run()
    {
        InitializeCommands();
        _isRunning = true;
        _currentRole = UserRole.Customer;
        ConsoleView.ShowCustomerMenu();

        while (_isRunning)
        {
            switch (_currentRole)
            {
                case UserRole.Customer:
                    ProcessCommand(_customerCommands);
                    break;
                case UserRole.Administrator:
                    ProcessCommand(_adminCommands);
                    break;
                default:
                    ConsoleView.DisplayError("Invalid user role");
                    return;
            }
        }
    }

    private void InitializeCommands()
    {
        _customerCommands[1] = HandlePrintProductsInfo;
        _customerCommands[2] = HandlePrintHumanInfo;
        _customerCommands[3] = HandleDepositCoins;
        _customerCommands[4] = HandleBuyProduct;
        _customerCommands[5] = HandleReturnMoney;
        _customerCommands[6] = HandleSwitchToAdmin;
        _customerCommands[7] = HandleExit;
        
        _adminCommands[1] = HandlePrintProductsInfo;
        _adminCommands[2] = HandlePrintHumanInfo;
        _adminCommands[3] = HandlePrintMachineCashRegisterInfo;
        _adminCommands[4] = HandleTakeAllMoney;
        _adminCommands[5] = HandleDepositCoins;
        _adminCommands[6] = HandlePutProducts;
        _adminCommands[7] = HandleChangePrice;
        _adminCommands[8] = HandleSwitchToCustomer;
        _adminCommands[9] = HandleExit;
    }
    
    private static void ProcessCommand(Dictionary<int, Action> commands)
    {
        int commandNumber;
        do
        {
            ConsoleView.DisplayRequestMessage("Введите номер команды: ");
        } 
        while (!ConsoleInput.TryReadNumber(out commandNumber));
        
        if (commands.TryGetValue(commandNumber, out var action))
            action.Invoke();
        else ConsoleView.DisplayError("Unknown command");
    }

    private void HandlePrintProductsInfo() =>
        ConsoleView.DisplayProducts(_vendingMachine.GetProductStorageInfo(), _vendingMachine.DepositedAmount);

    private void HandlePrintHumanInfo() => 
        ConsoleView.DisplayWalletInfo("Ваши деньги", CurrentUser.GetWalletInfo());

    private void HandlePrintMachineCashRegisterInfo() =>
        ConsoleView.DisplayWalletInfo("Деньги в аппарате", _vendingMachine.GetRevenueMoneyInfo());

    private void HandleDepositCoins()
    {
        List<int> nominals;
        List<string> ignored;
        do
        {
            ConsoleView.DisplayRequestMessage("Введите номиналы монет: ");
        } 
        while (!ConsoleInput.TryReadNominals(out nominals, out ignored));

        foreach (var nominal in nominals)
        {
            try
            {
                Coin coinIn = CurrentUser.SpendSalary((NominalValue)nominal);
                if (_currentRole == UserRole.Administrator)
                    _vendingMachine.PutCoinInCashRegister(coinIn);
                else _vendingMachine.AcceptCoin(coinIn);
            }
            catch (KeyNotFoundException ex)
            {
                ConsoleView.DisplayError(ex.Message);
            }
        }

        foreach (var input in ignored)
            ConsoleView.DisplayError($"Invalid nominal value {input} was ignored");
    }
    
    private void HandleBuyProduct()
    {
        string productName;
        do
        {
            ConsoleView.DisplayRequestMessage("Введите название товара: ");
        } 
        while (!ConsoleInput.TryReadWord(out productName));
        
        try
        {
            CurrentUser.GetProduct(_vendingMachine.BuyProduct(productName)); 
        }
        catch (ApplicationException ex)
        {
            ConsoleView.DisplayError(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            ConsoleView.DisplayError(ex.Message);
        }
    }

    private void HandleReturnMoney()
    {
        if (_vendingMachine.DepositedAmount == 0)
        {
            ConsoleView.DisplayError("There are no deposited money");
            return;
        }
        
        try
        {
            CurrentUser.GetSalary(_vendingMachine.ReturnDepositedAmount());
        }
        catch (ApplicationException ex)
        {
            ConsoleView.DisplayError(ex.Message);
        }
    }
    private void HandleTakeAllMoney() => CurrentUser.GetSalary(_vendingMachine.StealRevenue());

    private void HandlePutProducts()
    {
        List<string> productsNames;

        do
        {
            ConsoleView.DisplayRequestMessage("Введите названия товаров: ");
        } 
        while (!ConsoleInput.TryReadWords(out productsNames));

        foreach (var productName in productsNames)
        {
            var product = new Product(productName);
            _vendingMachine.AddProductToStorage(product);
            
            if (_vendingMachine.GetProductInfo(productName).Count > 1) continue;
            ChangePriceByName(productName);
        }
    }

    private void HandleChangePrice()
    {
        string productName;
        do
        {
            ConsoleView.DisplayRequestMessage("Введите название товара: ");
        } 
        while (!ConsoleInput.TryReadWord(out productName));
        
        ChangePriceByName(productName);
    }
    
    private void ChangePriceByName(string productName)
    {
        int price;
        do
        {
            ConsoleView.DisplayRequestMessage($"Установите цену на товар \"{productName}\": ");
        } 
        while (!ConsoleInput.TryReadNumber(out price));
                
        try
        {
            _vendingMachine.ChangeProductsPrice(productName, price);
        }
        catch (ArgumentException ex)
        {
            ConsoleView.DisplayError(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            ConsoleView.DisplayError(ex.Message);
        }
    }

    
    private void HandleSwitchToAdmin()
    {
        HandleReturnMoney();
        string password;
        do
        {
            ConsoleView.DisplayRequestMessage("Введите пароль: ");
        } 
        while (!ConsoleInput.TryReadWord(out password));
        
        if (string.Compare(password, adminPassword, StringComparison.Ordinal) != 0)
        {
            ConsoleView.DisplayError("Invalid password");
            return;
        }
        
        _currentRole = UserRole.Administrator;
        ConsoleView.ShowAdminMenu();
    }
    private void HandleSwitchToCustomer()
    {
        _currentRole = UserRole.Customer;
        ConsoleView.ShowCustomerMenu();
    }
    private void HandleExit()
    {
        _isRunning = false;
        Console.Clear();
    }
}