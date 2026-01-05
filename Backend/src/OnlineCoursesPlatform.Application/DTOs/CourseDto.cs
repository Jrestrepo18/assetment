using OnlineCoursesPlatform.Domain.Enums;

namespace OnlineCoursesPlatform.Application.DTOs;

/// <summary>
/// DTO para transferir datos de curso.
/// </summary>
public class CourseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public int DurationInHours { get; set; }
    public CourseStatus Status { get; set; }
    public string InstructorName { get; set; } = string.Empty;
    public int LessonsCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// DTO para crear un nuevo curso.
/// </summary>
public class CreateCourseDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public int DurationInHours { get; set; }
}

/// <summary>
/// DTO para actualizar un curso existente.
/// </summary>
public class UpdateCourseDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public int DurationInHours { get; set; }
    public CourseStatus Status { get; set; }
}
