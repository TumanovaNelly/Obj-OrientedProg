using Obj_OrientedProg.Lab1.Sources.Core.Models;

namespace Obj_OrientedProg.Lab1.Sources.Core.Interfaces;

public interface IPersonRepository : IRepository<Person>
{
    public IEnumerable<Person> GetAllStudents();
    public IEnumerable<Person> GetAllTeachers();
}