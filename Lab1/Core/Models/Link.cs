using Obj_OrientedProg.Lab1.Sources.Core.Interfaces;

namespace Obj_OrientedProg.Lab1.Sources.Core.Models;

public class Link(string link) : IPlace
{
    public string Info => $"Онлайн занятие: {link}";
}