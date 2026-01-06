using CoursePlatform.Core.Entities;
using CoursePlatform.Core.Interfaces;

namespace CoursePlatform.Core.Services;

public class LessonService
{
    private readonly ILessonRepository _lessonRepository;
    private readonly ICourseRepository _courseRepository;

    public LessonService(ILessonRepository lessonRepository, ICourseRepository courseRepository)
    {
        _lessonRepository = lessonRepository;
        _courseRepository = courseRepository;
    }

    public async Task<Lesson?> GetByIdAsync(Guid id)
    {
        return await _lessonRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Lesson>> GetByCourseIdAsync(Guid courseId)
    {
        return await _lessonRepository.GetByCourseIdAsync(courseId);
    }

    public async Task<Lesson> CreateAsync(Guid courseId, string title, int order)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
            throw new InvalidOperationException("Course not found");

        // Check for duplicate order
        var orderExists = await _lessonRepository.OrderExistsInCourseAsync(courseId, order);
        if (orderExists)
            throw new InvalidOperationException($"Order {order} already exists in this course");

        var lesson = new Lesson
        {
            Id = Guid.NewGuid(),
            CourseId = courseId,
            Title = title,
            Order = order,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return await _lessonRepository.AddAsync(lesson);
    }


    public async Task UpdateAsync(Guid id, string title, int order)
    {
        var lesson = await _lessonRepository.GetByIdAsync(id);
        if (lesson == null)
            throw new InvalidOperationException("Lesson not found");

        var orderExists = await _lessonRepository.OrderExistsInCourseAsync(lesson.CourseId, order, id);
        if (orderExists)
            throw new InvalidOperationException($"Order {order} already exists in this course");

        lesson.Title = title;
        lesson.Order = order;
        lesson.UpdatedAt = DateTime.UtcNow;

        await _lessonRepository.UpdateAsync(lesson);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _lessonRepository.DeleteAsync(id);
    }

    public async Task HardDeleteAsync(Guid id)
    {
        await _lessonRepository.HardDeleteAsync(id);
    }


    public async Task ReorderAsync(Guid id, int newOrder)
    {
        var lesson = await _lessonRepository.GetByIdAsync(id);
        if (lesson == null)
            throw new InvalidOperationException("Lesson not found");

        var targetLesson = await _lessonRepository.GetByOrderInCourseAsync(lesson.CourseId, newOrder);
        
        if (targetLesson != null && targetLesson.Id != id)
        {
            var tempOrder = lesson.Order;
            lesson.Order = targetLesson.Order;
            targetLesson.Order = tempOrder;
            
            lesson.UpdatedAt = DateTime.UtcNow;
            targetLesson.UpdatedAt = DateTime.UtcNow;
            
            await _lessonRepository.UpdateAsync(lesson);
            await _lessonRepository.UpdateAsync(targetLesson);
        }
        else
        {
            lesson.Order = newOrder;
            lesson.UpdatedAt = DateTime.UtcNow;
            await _lessonRepository.UpdateAsync(lesson);
        }
    }
}