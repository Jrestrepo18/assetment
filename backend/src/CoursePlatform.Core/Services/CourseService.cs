using CoursePlatform.Core.Entities;
using CoursePlatform.Core.Interfaces;

namespace CoursePlatform.Core.Services;

public class CourseService
{
    private readonly ICourseRepository _courseRepository;

    public CourseService(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<Course?> GetByIdAsync(Guid id)
    {
        return await _courseRepository.GetByIdAsync(id);
    }

    public async Task<(IEnumerable<Course> Items, int TotalCount)> SearchAsync(string? query, CourseStatus? status, int page, int pageSize)
    {
        return await _courseRepository.SearchAsync(query, status, page, pageSize);
    }

    public async Task<Course> CreateAsync(string title, string? description = null)
    {
        var course = new Course
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description,
            Status = CourseStatus.Draft,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return await _courseRepository.AddAsync(course);
    }

    public async Task UpdateAsync(Guid id, string title, string? description)
    {
        var course = await _courseRepository.GetByIdAsync(id);
        if (course == null)
            throw new InvalidOperationException("Course not found");

        course.Title = title;
        course.Description = description;
        course.UpdatedAt = DateTime.UtcNow;

        await _courseRepository.UpdateAsync(course);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _courseRepository.DeleteAsync(id);
    }

    public async Task HardDeleteAsync(Guid id)
    {
        await _courseRepository.HardDeleteAsync(id);
    }

    public async Task PublishAsync(Guid id)

    {
        var course = await _courseRepository.GetByIdAsync(id);
        if (course == null)
            throw new InvalidOperationException("Course not found");

        var activeLessonsCount = await _courseRepository.GetActiveLessonsCountAsync(id);
        if (activeLessonsCount == 0)
            throw new InvalidOperationException("Cannot publish a course without active lessons");

        course.Status = CourseStatus.Published;
        course.UpdatedAt = DateTime.UtcNow;

        await _courseRepository.UpdateAsync(course);
    }

    public async Task UnpublishAsync(Guid id)
    {
        var course = await _courseRepository.GetByIdAsync(id);
        if (course == null)
            throw new InvalidOperationException("Course not found");

        course.Status = CourseStatus.Draft;
        course.UpdatedAt = DateTime.UtcNow;

        await _courseRepository.UpdateAsync(course);
    }

    public async Task<object> GetSummaryAsync(Guid id)
    {
        var course = await _courseRepository.GetByIdAsync(id);
        if (course == null)
            throw new InvalidOperationException("Course not found");

        var lessonsCount = await _courseRepository.GetActiveLessonsCountAsync(id);

        return new
        {
            course.Id,
            course.Title,
            course.Status,
            TotalLessons = lessonsCount,
            LastModified = course.UpdatedAt
        };
    }

    public async Task<(int Total, int Drafts, int Published)> GetStatsAsync()
    {
        return await _courseRepository.GetStatsAsync();
    }
}