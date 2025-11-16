namespace Obj_OrientedProg.Lab1.Sources.App.DTOs;

public record CourseDTO(Guid Id, string Title, Guid? ResponsiblePersonId = null) {}