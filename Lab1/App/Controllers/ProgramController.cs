using Obj_OrientedProg.Lab1.App.UI;
using Obj_OrientedProg.Lab1.Core.Models;

namespace Obj_OrientedProg.Lab1.App.Controllers;

public class ProgramController
{
    private readonly Dictionary<int, Action> _commands = new();

    private Human _currentUser = new();
    private readonly VendingMachineController _vendingMachineController;
    
    private bool _isRunning;

    public ProgramController()
    {
        _vendingMachineController = new VendingMachineController("admin123", _currentUser);
    }
    
    public void Run()
    {
        InitializeCommands();
        _isRunning = true;
        ConsoleView.ShowMainMenu();

        while (_isRunning)
            ProcessCommand(_commands);
    }

    private void InitializeCommands()
    {
        _commands[1] = HandleStart;
        _commands[2] = HandlePrintHumanInfo;
        _commands[3] = HandlePutMoney;
        _commands[4] = HandleChangeUser;
        _commands[5] = HandleExit;
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

    private void HandleStart()
    {
        ConsoleView.Clear();
        _vendingMachineController.Run();
        ConsoleView.ShowMainMenu();
    }

    private void HandleExit()
    {
        ConsoleView.Clear();
        _isRunning = false;
    }

    private void HandlePrintHumanInfo() => ConsoleView.DisplayUserInfo(_currentUser.GetInfo());

    private void HandlePutMoney()
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

        List<Coin> coins = [];
        for (int i = 0; i < count; i++)
        {
            coins.Add(new Coin((NominalValue)nominal));
        }
        
        _currentUser.GetSalary(coins);
    }

    private void HandleChangeUser()
    {
        _currentUser = new Human();
        _vendingMachineController.CurrentUser = _currentUser;
    }
}