namespace CoursePlatform.Core.Entities;

public class Course
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public CourseStatus Status { get; set; } = CourseStatus.Draft;
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}

public enum CourseStatus
{
    Draft,
    Published
}