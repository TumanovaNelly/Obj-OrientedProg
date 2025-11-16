using Obj_OrientedProg.Lab1.Sources.Core.Interfaces;

namespace Obj_OrientedProg.Lab1.Sources.Core.Models;

public class Course(string title) : IEntity
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Title { get; } = title;
    public Person? ResponsiblePerson { get; private set; }
    public IReadOnlyList<Person> EnrolledPersons => _enrolledPersons.AsReadOnly();
    public IReadOnlyList<ICourseFormat> Formats => _formats.AsReadOnly();
    
    
    private readonly List<Person> _enrolledPersons = [];
    private readonly List<ICourseFormat> _formats = [];

    public void AssignResponsiblePerson(Person person)
    {
        ResponsiblePerson = person;
    }
    
    public void AddEnrolledPerson(Person person)
    {
        _enrolledPersons.Add(person);
    }
    
    public void AddFormat(ICourseFormat format) => _formats.Add(format);
}