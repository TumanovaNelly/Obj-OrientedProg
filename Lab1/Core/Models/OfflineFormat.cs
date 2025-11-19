using Obj_OrientedProg.Lab1.Sources.Core.Interfaces;

namespace Obj_OrientedProg.Lab1.Sources.Core.Models;

public class OfflineFormat(Link link, ITime time) : ICourseFormat
{
    public IPlace Place { get; } = link;
    public ITime Time { get; } = time;
}