using Obj_OrientedProg.Lab1.Sources.Core.Interfaces;

namespace Obj_OrientedProg.Lab1.Sources.Core.Models;

public class Time(TimeOnly startTime, TimeOnly endTime, IData data) : ITime
{
    public TimeOnly StartTime { get; } = startTime;
    public TimeOnly EndTime { get; } = endTime;
    public IData Data { get; } = data;
}