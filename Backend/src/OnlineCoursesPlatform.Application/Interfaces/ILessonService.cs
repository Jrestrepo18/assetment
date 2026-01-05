using OnlineCoursesPlatform.Application.DTOs;

namespace OnlineCoursesPlatform.Application.Interfaces;

/// <summary>
/// Interfaz para el servicio de lecciones.
/// </summary>
public interface ILessonService
{
    Task<IEnumerable<LessonDto>> GetLessonsByCourseAsync(Guid courseId);
    
    Task<LessonDto?> GetLessonByIdAsync(Guid id);
    
    Task<LessonDto> CreateLessonAsync(CreateLessonDto dto);
    
    Task<LessonDto?> UpdateLessonAsync(Guid id, UpdateLessonDto dto);
    
    Task<bool> DeleteLessonAsync(Guid id);
    
    Task<bool> ReorderLessonsAsync(Guid courseId, List<Guid> lessonIds);
}
