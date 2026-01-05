namespace OnlineCoursesPlatform.Domain.Enums;

/// <summary>
/// Estado del curso en la plataforma.
/// </summary>
public enum CourseStatus
{
    /// <summary>
    /// Curso en borrador, no visible para estudiantes.
    /// </summary>
    Draft = 0,

    /// <summary>
    /// Curso publicado y disponible.
    /// </summary>
    Published = 1
}
