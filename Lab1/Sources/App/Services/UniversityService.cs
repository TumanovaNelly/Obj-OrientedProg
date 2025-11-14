using Obj_OrientedProg.Lab1.Sources.Core.Interfaces;
using Obj_OrientedProg.Lab1.Sources.Core.Models;

namespace Obj_OrientedProg.Lab1.Sources.App.Services;

public class UniversityService(ICourseRepository courseRepository, IPersonRepository personRepository)
{
    private readonly ICourseRepository _coursesRepository = courseRepository;
    private readonly IPersonRepository _personRepository = personRepository;
}