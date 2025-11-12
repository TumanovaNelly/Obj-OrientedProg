using Obj_OrientedProg.Lab0.App.UI;
using Obj_OrientedProg.Lab0.Core.Models;

namespace Obj_OrientedProg.Lab0.App.Controllers;

public class ProgramController
{
    private readonly Dictionary<Command, Action> _commands = new();
    private readonly VendingMachineController _vendingMachineController = new("admin123");

    private Human _currentUser = new();
    private bool _isRunning;

    public ProgramController()
    {
        _commands[Command.S] = HandleStart;
        _commands[Command.UI] = HandlePrintHumanInfo;
        _commands[Command.GS] = HandlePutMoney;
        _commands[Command.CU] = HandleChangeUser;
        _commands[Command.E] = HandleExit;
    }

    public void Run()
    {
        ConsoleView.Clear();
        _isRunning = true;
        ConsoleView.ShowMenu("Главное меню", _commands.Keys.ToList());

        while (_isRunning)
            ProcessCommand(_commands);
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

    private void HandleStart()
    {
        ConsoleView.Clear();
        _vendingMachineController.Run(_currentUser);
        ConsoleView.ShowMenu("Главное меню", _commands.Keys.ToList());
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
            coins.Add(new Coin((NominalValue)nominal));
        
        _currentUser.GetSalary(coins);
    }

    private void HandleChangeUser() => _currentUser = new Human();
}