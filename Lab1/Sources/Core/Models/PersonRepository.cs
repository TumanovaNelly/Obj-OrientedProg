using Obj_OrientedProg.Lab1.Sources.Core.Interfaces;

namespace Obj_OrientedProg.Lab1.Sources.Core.Models;

public class PersonRepository : ARepository<Person>, IPersonRepository
{
    public IEnumerable<Person> GetAllStudents() => Storage.Values.Where(p => p.IsStudent);
    public IEnumerable<Person> GetAllTeachers() => Storage.Values.Where(p => p.IsTeacher);
}