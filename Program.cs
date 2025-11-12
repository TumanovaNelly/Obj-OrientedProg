namespace Obj_OrientedProg;

public static class Program
{
    public static void Main()
    {
        Console.Write("Введите номер лабораторной работы: ");

        string? num = Console.ReadLine();

        switch (num)
        {
            case "0":
                Lab0.Program.Run();
                break;
            default:
                Console.WriteLine("Неверный ввод. Попробуйте еще раз.");
                break;
        }
    }
}