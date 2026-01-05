namespace OnlineCoursesPlatform.Application.DTOs;

/// <summary>
/// DTO para transferir datos de lección.
/// </summary>
public class LessonDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public string? VideoUrl { get; set; }
    public int Order { get; set; }
    public int DurationInMinutes { get; set; }
    public Guid CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
}

/// <summary>
/// DTO para crear una nueva lección.
/// </summary>
public class CreateLessonDto
{
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public string? VideoUrl { get; set; }
    public int DurationInMinutes { get; set; }
    public Guid CourseId { get; set; }
}

/// <summary>
/// DTO para actualizar una lección existente.
/// </summary>
public class UpdateLessonDto
{
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public string? VideoUrl { get; set; }
    public int DurationInMinutes { get; set; }
}
