using Obj_OrientedProg.Lab1.App.UI;
using Obj_OrientedProg.Lab1.Core.Models;

namespace Obj_OrientedProg.Lab1.App.Controllers;

public class ProgramController
{
    private readonly Dictionary<int, Action> _commands = new();

    private readonly Human _currentUser = new();
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
        _commands[4] = HandleExit;
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
        Console.Clear();
        _vendingMachineController.Run();
        ConsoleView.ShowMainMenu();
    }

    private void HandleExit()
    {
        Console.Clear();
        _isRunning = false;
    }

    private void HandlePrintHumanInfo() => ConsoleView.DisplayWalletInfo("Ваши деньги", _currentUser.GetWalletInfo());

    private void HandlePutMoney()
    {
        List<int> nominals;
        List<string> ignored;
        do
        {
            ConsoleView.DisplayRequestMessage("Введите номиналы монет: ");
        } 
        while (!ConsoleInput.TryReadNominals(out nominals, out ignored));
        
        var coins = nominals.Select(nominal => new Coin((NominalValue)nominal)).ToList();
        _currentUser.GetSalary(coins);

        foreach (var input in ignored)
            ConsoleView.DisplayError($"Invalid nominal value {input} was ignored");
    }
}