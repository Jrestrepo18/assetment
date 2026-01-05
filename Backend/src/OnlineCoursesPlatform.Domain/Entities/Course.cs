using OnlineCoursesPlatform.Domain.Enums;

namespace OnlineCoursesPlatform.Domain.Entities;

/// <summary>
/// Representa un curso en la plataforma.
/// Hereda de BaseEntity para campos comunes.
/// </summary>
public class Course : BaseEntity
{
    /// <summary>
    /// Título del curso.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Estado del curso (Draft o Published).
    /// </summary>
    public CourseStatus CourseStatus { get; set; } = CourseStatus.Draft;

    /// <summary>
    /// Colección de lecciones del curso.
    /// Relación 1:N con Lesson.
    /// </summary>
    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}
