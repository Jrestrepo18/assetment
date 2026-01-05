using OnlineCoursesPlatform.Domain.Entities;
using OnlineCoursesPlatform.Domain.Enums;

namespace OnlineCoursesPlatform.Domain.Interfaces;

/// <summary>
/// Interfaz de repositorio específica para cursos.
/// </summary>
public interface ICourseRepository : IRepository<Course>
{
    Task<IEnumerable<Course>> GetCoursesByInstructorAsync(Guid instructorId);
    
    Task<IEnumerable<Course>> GetCoursesByStatusAsync(CourseStatus status);
    
    Task<Course?> GetCourseWithLessonsAsync(Guid courseId);
    
    Task<IEnumerable<Course>> GetPublishedCoursesAsync();
    
    Task<IEnumerable<Course>> SearchCoursesAsync(string searchTerm);
}
