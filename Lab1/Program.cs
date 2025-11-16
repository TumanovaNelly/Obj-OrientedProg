using Obj_OrientedProg.Lab1.Sources.App.Services;
using Obj_OrientedProg.Lab1.Sources.Core.Models;

namespace Obj_OrientedProg.Lab1;

public static class Program
{
    public static void Run()
    {
        UniversityService service = new UniversityService(new CoursesRepository(), new PersonRepository());
        var courseInfo = service.CreateCourse("ООП");
        var teacherInfo = service.CreatePerson("Бенедикт", "Камбербэтч");
        service.PromotePersonToTeacher(teacherInfo.Id, "ПИН");
        var studentInfo = service.CreatePerson("Пит", "Мошник");
        service.PromotePersonToStudent(studentInfo.Id, "K3141");
        
        service.AssignTeacherToCourse(courseInfo.Id, teacherInfo.Id);
        service.AssignStudentToCourse(courseInfo.Id, studentInfo.Id);
        
        foreach (var person in service.GetPersons())
        {
            Console.WriteLine($"{person.Id} {person.FirstName} {person.LastName} {person.IsTeacher} {person.IsStudent}");
        }
        
        foreach (var course in service.GetCourses())
        {
            Console.WriteLine($"{course.Id} {course.Title} {course.ResponsiblePersonId}");
        }
    }
}