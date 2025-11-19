using Obj_OrientedProg.Lab1.Sources.Core.Interfaces;

namespace Obj_OrientedProg.Lab1.Sources.Core.Models;

public class OnlineFormat(Address address, ITime time) : ICourseFormat
{
    public IPlace Place { get; } = address;
    public ITime Time { get; } = time;
}