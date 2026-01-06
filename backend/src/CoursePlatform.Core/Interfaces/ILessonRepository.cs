using CoursePlatform.Core.Entities;

namespace CoursePlatform.Core.Interfaces;

public interface ILessonRepository
{
    Task<Lesson?> GetByIdAsync(Guid id);
    Task<IEnumerable<Lesson>> GetByCourseIdAsync(Guid courseId);
    Task<Lesson> AddAsync(Lesson lesson);
    Task UpdateAsync(Lesson lesson);
    Task DeleteAsync(Guid id);
    Task HardDeleteAsync(Guid id);

    Task<bool> OrderExistsInCourseAsync(Guid courseId, int order, Guid? excludeLessonId = null);
    Task<Lesson?> GetByOrderInCourseAsync(Guid courseId, int order);
    Task<int> GetMaxOrderAsync(Guid courseId);
}