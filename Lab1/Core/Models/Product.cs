namespace Obj_OrientedProg.Lab1.Core.Models;

public class Product(string name)
{
    public string Name { get; } = name;
    public bool IsUnharmed { get; private set; } = true;
    public void ToHarmed() => IsUnharmed = true;
}