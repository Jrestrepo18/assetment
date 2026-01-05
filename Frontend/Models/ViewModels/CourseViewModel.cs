namespace OnlineCoursesPlatform.Web.Models.ViewModels;

/// <summary>
/// ViewModel para mostrar información de un curso.
/// </summary>
public class CourseViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public int DurationInHours { get; set; }
    public string Status { get; set; } = string.Empty;
    public string InstructorName { get; set; } = string.Empty;
    public int LessonsCount { get; set; }
    public DateTime CreatedAt { get; set; }

    public string FormattedPrice => Price.ToString("C");
    public string FormattedDuration => $"{DurationInHours} hora{(DurationInHours != 1 ? "s" : "")}";
}
