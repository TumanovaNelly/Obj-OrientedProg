using Obj_OrientedProg.Lab1.Sources.Core.Interfaces;

namespace Obj_OrientedProg.Lab1.Sources.Core.Models;

public class Address(string street, int houseNumber, int classNumber) : IPlace
{
    public string Info => $"ул. {street}, д. {houseNumber}, ауд. {classNumber}";
}