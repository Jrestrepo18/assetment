using OnlineCoursesPlatform.Domain.Entities;

namespace OnlineCoursesPlatform.Domain.Interfaces;

/// <summary>
/// Interfaz de repositorio específica para lecciones.
/// </summary>
public interface ILessonRepository : IRepository<Lesson>
{
    Task<IEnumerable<Lesson>> GetLessonsByCourseAsync(Guid courseId);
    
    Task<Lesson?> GetLessonWithCourseAsync(Guid lessonId);
    
    Task<int> GetMaxOrderByCourseAsync(Guid courseId);
    
    Task ReorderLessonsAsync(Guid courseId, List<Guid> lessonIds);
}
