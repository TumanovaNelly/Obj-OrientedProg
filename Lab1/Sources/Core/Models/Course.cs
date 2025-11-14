using Obj_OrientedProg.Lab1.Sources.Core.Interfaces;

namespace Obj_OrientedProg.Lab1.Sources.Core.Models;

public class Course(string title) : IEntity
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Title { get; } = title;

    public Person? AssignedTeacher { get; private set; }
    private static List<Person> EnrolledStudents { get; } = [];
    private static List<ICourseFormat> formats = [];
}