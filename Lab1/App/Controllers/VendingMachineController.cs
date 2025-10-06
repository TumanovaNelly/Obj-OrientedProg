using Obj_OrientedProg.Lab1.Core.Models;
using Obj_OrientedProg.Lab1.App.UI;

namespace Obj_OrientedProg.Lab1.App.Controllers;

public enum UserRole { Customer, Administrator }

public class VendingMachineController
{
    private readonly VendingMachine _vendingMachine = new();
    private readonly string _adminPassword;
    
    private UserRole _currentRole;
    private bool _isRunning;
    
    private readonly Dictionary<Command, Action> _customerCommands = new();
    private readonly Dictionary<Command, Action> _adminCommands = new();

    public VendingMachineController(string adminPassword)
    {
        _adminPassword = adminPassword;
        _customerCommands[Command.MI] = HandlePrintProductsInfo;
        
        _adminCommands[Command.MI] = HandlePrintMachineInfo;
        _adminCommands[Command.PC] = HandleDepositMoney;
        _adminCommands[Command.PP] = HandlePutProducts;
        _adminCommands[Command.CP] = HandleChangePrice;
        _adminCommands[Command.TC] = HandleSwitchToCustomer;
        _adminCommands[Command.E] = HandleExit;
    }

    public void Run(Human user)
    {
        InitializeCommands(user);
        _isRunning = true;
        _currentRole = UserRole.Customer;
        ConsoleView.ShowMenu("Меню покупателя", _customerCommands.Keys.ToList());

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

    private void InitializeCommands(Human user)
    {
        _customerCommands[Command.PC] = () => HandleDepositCoins(user);
        _customerCommands[Command.BP] = () => HandleBuyProduct(user);
        _customerCommands[Command.RM] = () => HandleReturnMoney(user);
        _customerCommands[Command.TA] = () =>
        {
            if (_vendingMachine.DepositedAmount > 0) 
                HandleReturnMoney(user);
            HandleSwitchToAdmin();
        };
        _customerCommands[Command.E] = () =>
        {
            if (_vendingMachine.DepositedAmount > 0) 
                HandleReturnMoney(user);
            HandleExit();
        };
        
        _adminCommands[Command.TM] = () => HandleTakeAllMoney(user);
        _adminCommands[Command.GP] = () => HandleGetProducts(user);
    }
    
    private static void ProcessCommand(Dictionary<Command, Action> commands)
    {
        string input;
        do
        {
            ConsoleView.DisplayRequestMessage("Введите имя команды: ");
        } 
        while (!ConsoleInput.TryReadWord(out input));
        
        
        if (Enum.TryParse<Command>(input, true, out var command) 
            && commands.TryGetValue(command, out var action))
            action.Invoke();
        else ConsoleView.DisplayError("Unknown command");
    }

    private void HandlePrintMachineInfo()
    {
        HandlePrintProductsInfo();
        ConsoleView.DisplayWalletInfo("Деньги в аппарате", _vendingMachine.GetRevenueMoneyInfo());
    }
    private void HandlePrintProductsInfo() =>
        ConsoleView.DisplayProducts(_vendingMachine.GetProductStorageInfo(), _vendingMachine.DepositedAmount);
    
    private void HandleDepositCoins(Human user)
    {
        List<int> nominals;
        List<string> ignored;
        do
        {
            ConsoleView.DisplayRequestMessage("Введите номиналы монет: ");
        } 
        while (!ConsoleInput.TryReadNumbers(out nominals, out ignored));

        foreach (var nominal in nominals)
        {
            if (!Enum.IsDefined(typeof(NominalValue), nominal))
                ConsoleView.DisplayError($"Invalid nominal value {nominal} was ignored");
                
            try
            {
                Coin coinIn = user.SpendSalary((NominalValue)nominal);
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

    private void HandleDepositMoney()
    {
        int nominal;
        do
        {
            ConsoleView.DisplayRequestMessage("Введите номинал: ");
        } 
        while (!ConsoleInput.TryReadNumber(out nominal));

        if (!Enum.IsDefined(typeof(NominalValue), nominal))
        {
            ConsoleView.DisplayError("Unknown nominal");
            return;
        }
        
        int count;
        do
        {
            ConsoleView.DisplayRequestMessage("Введите количество: ");
        } 
        while (!ConsoleInput.TryReadNumber(out count));

        if (count < 0)
        {
            ConsoleView.DisplayError("Count must be positive");
            return;
        }

        for (int i = 0; i < count; i++)
            _vendingMachine.PutCoinInCashRegister(new Coin((NominalValue)nominal));
    }
    
    private void HandleBuyProduct(Human user)
    {
        string productName;
        do
        {
            ConsoleView.DisplayRequestMessage("Введите название товара: ");
        } 
        while (!ConsoleInput.TryReadWord(out productName));
        
        try
        {
            user.GetProduct(_vendingMachine.BuyProduct(productName)); 
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

    private void HandleReturnMoney(Human user)
    {
        if (_vendingMachine.DepositedAmount == 0)
        {
            ConsoleView.DisplayError("There are no deposited money");
            return;
        }
        
        try
        {
            user.GetSalary(_vendingMachine.ReturnDepositedAmount());
        }
        catch (ApplicationException ex)
        {
            ConsoleView.DisplayError(ex.Message);
        }
    }
    private void HandleTakeAllMoney(Human user) => user.GetSalary(_vendingMachine.StealRevenue());

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
    
    private void HandleGetProducts(Human user)
    {
        List<string> productsNames;

        do
        {
            ConsoleView.DisplayRequestMessage("Введите названия товаров: ");
        } 
        while (!ConsoleInput.TryReadWords(out productsNames));

        foreach (var productName in productsNames)
        {
            try
            {
                user.GetProduct(_vendingMachine.GetProductFromStorage(productName));
            }
            catch (KeyNotFoundException ex)
            {
                ConsoleView.DisplayError(ex.Message);
            }
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
        string password;
        do
        {
            ConsoleView.DisplayRequestMessage("Введите пароль: ");
        } 
        while (!ConsoleInput.TryReadWord(out password));
        
        if (string.Compare(password, _adminPassword, StringComparison.Ordinal) != 0)
        {
            ConsoleView.DisplayError("Invalid password");
            return;
        }
        
        _currentRole = UserRole.Administrator;
        ConsoleView.Clear();
        ConsoleView.ShowMenu("Меню администратора", _adminCommands.Keys.ToList());
    }
    private void HandleSwitchToCustomer()
    {
        _currentRole = UserRole.Customer;
        ConsoleView.Clear();
        ConsoleView.ShowMenu("Меню покупателя", _customerCommands.Keys.ToList());
    }
    private void HandleExit()
    {
        _isRunning = false;
        ConsoleView.Clear();
    }
}