using Obj_OrientedProg.Lab1.App.Controllers;

namespace Obj_OrientedProg.Lab1;

public static class Program
{
    public static void Main(string[] args)
    {
        ProgramController programController = new();
        programController.Run();
    }
}