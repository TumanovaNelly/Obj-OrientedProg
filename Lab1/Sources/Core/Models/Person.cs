using Obj_OrientedProg.Lab1.Sources.Core.Interfaces;

namespace Obj_OrientedProg.Lab1.Sources.Core.Models;

public class Person(string firstName, string lastName) : IEntity
{
    public Guid Id { get; init; } = Guid.NewGuid();
    
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;

    public StudentProfile? StudentInfo { get; set; }
    public TeacherProfile? TeacherInfo { get; set; }
}