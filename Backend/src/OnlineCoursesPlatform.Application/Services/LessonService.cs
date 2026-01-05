using OnlineCoursesPlatform.Application.DTOs;
using OnlineCoursesPlatform.Application.Interfaces;
using OnlineCoursesPlatform.Domain.Entities;
using OnlineCoursesPlatform.Domain.Interfaces;

namespace OnlineCoursesPlatform.Application.Services;

/// <summary>
/// Implementación del servicio de lecciones.
/// </summary>
public class LessonService : ILessonService
{
    private readonly ILessonRepository _lessonRepository;
    private readonly ICourseRepository _courseRepository;

    public LessonService(ILessonRepository lessonRepository, ICourseRepository courseRepository)
    {
        _lessonRepository = lessonRepository;
        _courseRepository = courseRepository;
    }

    public async Task<IEnumerable<LessonDto>> GetLessonsByCourseAsync(Guid courseId)
    {
        var lessons = await _lessonRepository.GetLessonsByCourseAsync(courseId);
        return lessons.Select(MapToDto);
    }

    public async Task<LessonDto?> GetLessonByIdAsync(Guid id)
    {
        var lesson = await _lessonRepository.GetLessonWithCourseAsync(id);
        return lesson == null ? null : MapToDto(lesson);
    }

    public async Task<LessonDto> CreateLessonAsync(CreateLessonDto dto)
    {
        var maxOrder = await _lessonRepository.GetMaxOrderByCourseAsync(dto.CourseId);
        
        var lesson = new Lesson
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Content = dto.Content,
            VideoUrl = dto.VideoUrl,
            DurationInMinutes = dto.DurationInMinutes,
            CourseId = dto.CourseId,
            Order = maxOrder + 1,
            CreatedAt = DateTime.UtcNow
        };

        await _lessonRepository.AddAsync(lesson);
        await _lessonRepository.SaveChangesAsync();

        return MapToDto(lesson);
    }

    public async Task<LessonDto?> UpdateLessonAsync(Guid id, UpdateLessonDto dto)
    {
        var lesson = await _lessonRepository.GetByIdAsync(id);
        if (lesson == null) return null;

        lesson.Title = dto.Title;
        lesson.Content = dto.Content;
        lesson.VideoUrl = dto.VideoUrl;
        lesson.DurationInMinutes = dto.DurationInMinutes;
        lesson.UpdatedAt = DateTime.UtcNow;

        _lessonRepository.Update(lesson);
        await _lessonRepository.SaveChangesAsync();

        return MapToDto(lesson);
    }

    public async Task<bool> DeleteLessonAsync(Guid id)
    {
        var lesson = await _lessonRepository.GetByIdAsync(id);
        if (lesson == null) return false;

        lesson.IsDeleted = true;
        lesson.UpdatedAt = DateTime.UtcNow;
        
        _lessonRepository.Update(lesson);
        await _lessonRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ReorderLessonsAsync(Guid courseId, List<Guid> lessonIds)
    {
        await _lessonRepository.ReorderLessonsAsync(courseId, lessonIds);
        return true;
    }

    private static LessonDto MapToDto(Lesson lesson)
    {
        return new LessonDto
        {
            Id = lesson.Id,
            Title = lesson.Title,
            Content = lesson.Content,
            VideoUrl = lesson.VideoUrl,
            Order = lesson.Order,
            DurationInMinutes = lesson.DurationInMinutes,
            CourseId = lesson.CourseId,
            CourseName = lesson.Course?.Title ?? "Unknown"
        };
    }
}
