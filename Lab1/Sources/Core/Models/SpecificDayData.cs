using Obj_OrientedProg.Lab1.Sources.Core.Interfaces;

namespace Obj_OrientedProg.Lab1.Sources.Core.Models;

public class SpecificDayData(DateOnly date) : IData
{
    public string Info { get; } = date.ToShortDateString();
}