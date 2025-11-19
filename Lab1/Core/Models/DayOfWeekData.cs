using Obj_OrientedProg.Lab1.Sources.Core.Interfaces;

namespace Obj_OrientedProg.Lab1.Sources.Core.Models;

public enum DayOfWeek
{
    Понедельник,
    Вторник,
    Среда,
    Четверг,
    Пятница,
    Суббота,
    Воскресенье
}

public class DayOfWeekData(DayOfWeek dayOfWeek) : IData
{
    public string Info { get; } = dayOfWeek.ToString();
}