namespace Obj_OrientedProg.Lab1.Sources.Core.Interfaces;

public interface ICourseFormat
{
    public IPlace Place { get; }
    public ITime Time { get; }
    public string Info => $"""
                                  Место: {Place.Info} 
                                  Время: {Time.Info}
                                  """;
}