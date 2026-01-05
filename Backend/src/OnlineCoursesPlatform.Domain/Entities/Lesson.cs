namespace OnlineCoursesPlatform.Domain.Entities;

/// <summary>
/// Representa una lección dentro de un curso.
/// Hereda de BaseEntity para campos comunes.
/// </summary>
public class Lesson : BaseEntity
{
    /// <summary>
    /// Título de la lección.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Orden de la lección dentro del curso.
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// ID del curso al que pertenece la lección.
    /// </summary>
    public Guid CourseId { get; set; }

    /// <summary>
    /// Referencia de navegación al curso.
    /// </summary>
    public Course? Course { get; set; }
}
