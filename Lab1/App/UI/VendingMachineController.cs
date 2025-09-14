namespace Obj_OrientedProg.Lab1.App.UI;

using Obj_OrientedProg.Lab1.Core.Models;

public enum UserRole { Customer, Administrator }

public class VendingMachineController(string adminPassword, Human currentUser)
{
    private readonly VendingMachine _vendingMachine = new VendingMachine();
    public Human CurrentUser { private get; set; } = currentUser;

    private UserRole _currentRole;
    private bool _isRunning;

    private readonly Dictionary<string, Action> _customerCommands = new();
    private readonly Dictionary<string, Action> _adminCommands = new();

    public void Run()
    {
        InitializeCommands();
        _isRunning = true;
        _currentRole = UserRole.Customer;

        while (_isRunning)
        {
            switch (_currentRole)
            {
                case UserRole.Customer:
                    ConsoleView.ShowCustomerMenu();
                    ProcessCommand(_customerCommands);
                    break;
                case UserRole.Administrator:
                    ConsoleView.ShowAdminMenu();
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
        _customerCommands["1"] = HandlePrintProductsInfo;
        _customerCommands["2"] = HandlePrintHumanInfo;
        _customerCommands["3"] = HandleDepositCoins;
        _customerCommands["4"] = HandleBuyProduct;
        _customerCommands["5"] = HandleReturnMoney;
        _customerCommands["6"] = HandleSwitchToAdmin;
        _customerCommands["7"] = HandleExit;
        
        _adminCommands["1"] = HandlePrintProductsInfo;
        _adminCommands["2"] = HandlePrintHumanInfo;
        _adminCommands["3"] = HandlePrintMachineCashRegisterInfo;
        _adminCommands["4"] = HandleTakeAllMoney;
        _adminCommands["5"] = HandleDepositCoins;
        _adminCommands["6"] = HandlePutProducts;
        _adminCommands["7"] = HandleChangePrice;
        _adminCommands["8"] = HandleSwitchToCustomer;
        _adminCommands["9"] = HandleExit;
    }
    
    private static void ProcessCommand(Dictionary<string, Action> commands)
    {
        string command = ConsoleInput.ReadCommand();
        if (commands.TryGetValue(command, out var action))
        {
            action.Invoke();
        }
        else
        {
            ConsoleView.DisplayError("Unknown command");
        }
    }

    private void HandlePrintProductsInfo() =>
        ConsoleView.DisplayProducts(_vendingMachine.GetProductStorageInfo(), _vendingMachine.DepositedAmount);

    private void HandlePrintHumanInfo() => 
        ConsoleView.DisplayWalletInfo("Ваши деньги", CurrentUser.GetWalletInfo());

    private void HandlePrintMachineCashRegisterInfo() =>
        ConsoleView.DisplayWalletInfo("Деньги в аппарате", _vendingMachine.GetRevenueMoneyInfo());

    private void HandleDepositCoins()
    {
        var (nominalsList, ignoredList) = ConsoleInput.ReadNominals();
        foreach (var nominal in nominalsList)
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

        foreach (var ignoredInput in ignoredList)
            ConsoleView.DisplayError($"Invalid nominal value {ignoredInput} was ignored");
    }
    
    private void HandleBuyProduct()
    {
        string productName = ConsoleInput.ReadProductName();

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
        var productsNamesList = ConsoleInput.ReadProductNames();

        foreach (var productName in productsNamesList)
        {
            var product = new Product(productName);
            _vendingMachine.AddProductToStorage(product);
            
            if (_vendingMachine.GetProductInfo(productName).Count > 1) continue;

            int newPrice = ConsoleInput.ReadPrice(productName);
                
            try
            {
                _vendingMachine.ChangeProductsPrice(productName, newPrice);
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
    }

    private void HandleChangePrice()
    {
        string productName = ConsoleInput.ReadProductName();
        int newPrice = ConsoleInput.ReadPrice(productName);
        
        try
        {
            _vendingMachine.ChangeProductsPrice(productName, newPrice);
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
        string password = ConsoleInput.ReadPassword();
        
        if (string.Compare(password, adminPassword, StringComparison.Ordinal) != 0)
        {
            ConsoleView.DisplayError("Invalid password");
            return;
        }
        
        _currentRole = UserRole.Administrator;
    }
    private void HandleSwitchToCustomer()
    {
        _currentRole = UserRole.Customer;
    }
    private void HandleExit()
    {
        _isRunning = false;
        Console.Clear();
    }
}