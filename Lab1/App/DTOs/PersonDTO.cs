namespace Obj_OrientedProg.Lab1.Sources.App.DTOs;

public record PersonDTO(Guid Id, string FirstName, string LastName, bool IsTeacher = false, bool IsStudent = false);