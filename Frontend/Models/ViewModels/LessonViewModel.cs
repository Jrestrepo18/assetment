namespace OnlineCoursesPlatform.Web.Models.ViewModels;

/// <summary>
/// ViewModel para mostrar información de una lección.
/// </summary>
public class LessonViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public string? VideoUrl { get; set; }
    public int Order { get; set; }
    public int DurationInMinutes { get; set; }
    public Guid CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;

    public string FormattedDuration => $"{DurationInMinutes} min";
}

/// <summary>
/// ViewModel para crear una nueva lección.
/// </summary>
public class CreateLessonViewModel
{
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public string? VideoUrl { get; set; }
    public int DurationInMinutes { get; set; }
    public Guid CourseId { get; set; }
}
