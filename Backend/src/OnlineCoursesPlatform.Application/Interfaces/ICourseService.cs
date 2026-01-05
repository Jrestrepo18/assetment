using OnlineCoursesPlatform.Application.DTOs;

namespace OnlineCoursesPlatform.Application.Interfaces;

/// <summary>
/// Interfaz para el servicio de cursos.
/// </summary>
public interface ICourseService
{
    Task<IEnumerable<CourseDto>> GetAllCoursesAsync();
    
    Task<CourseDto?> GetCourseByIdAsync(Guid id);
    
    Task<IEnumerable<CourseDto>> GetPublishedCoursesAsync();
    
    Task<IEnumerable<CourseDto>> GetCoursesByInstructorAsync(Guid instructorId);
    
    Task<IEnumerable<CourseDto>> SearchCoursesAsync(string searchTerm);
    
    Task<CourseDto> CreateCourseAsync(CreateCourseDto dto, Guid instructorId);
    
    Task<CourseDto?> UpdateCourseAsync(Guid id, UpdateCourseDto dto);
    
    Task<bool> DeleteCourseAsync(Guid id);
    
    Task<bool> PublishCourseAsync(Guid id);
    
    Task<bool> ArchiveCourseAsync(Guid id);
}
