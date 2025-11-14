using Obj_OrientedProg.Lab1.Sources.Core.Interfaces;

namespace Obj_OrientedProg.Lab1.Sources.Core.Models;

public class OnlineFormat(Address address) : ICourseFormat
{
    public IPlace Place { get; } = address;
}