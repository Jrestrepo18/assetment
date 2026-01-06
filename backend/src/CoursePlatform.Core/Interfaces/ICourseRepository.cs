using CoursePlatform.Core.Entities;

namespace CoursePlatform.Core.Interfaces;

public interface ICourseRepository
{
    Task<Course?> GetByIdAsync(Guid id);
    Task<(IEnumerable<Course> Items, int TotalCount)> SearchAsync(string? query, CourseStatus? status, int page, int pageSize);
    Task<Course> AddAsync(Course course);
    Task UpdateAsync(Course course);
    Task DeleteAsync(Guid id);
    Task HardDeleteAsync(Guid id);
    Task<int> GetActiveLessonsCountAsync(Guid courseId);

    Task<(int Total, int Drafts, int Published)> GetStatsAsync();
}