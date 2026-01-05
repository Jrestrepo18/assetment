using OnlineCoursesPlatform.Application.DTOs;
using OnlineCoursesPlatform.Application.Interfaces;
using OnlineCoursesPlatform.Domain.Entities;
using OnlineCoursesPlatform.Domain.Enums;
using OnlineCoursesPlatform.Domain.Interfaces;

namespace OnlineCoursesPlatform.Application.Services;

/// <summary>
/// Implementación del servicio de cursos.
/// </summary>
public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;

    public CourseService(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<IEnumerable<CourseDto>> GetAllCoursesAsync()
    {
        var courses = await _courseRepository.GetAllAsync();
        return courses.Select(MapToDto);
    }

    public async Task<CourseDto?> GetCourseByIdAsync(Guid id)
    {
        var course = await _courseRepository.GetCourseWithLessonsAsync(id);
        return course == null ? null : MapToDto(course);
    }

    public async Task<IEnumerable<CourseDto>> GetPublishedCoursesAsync()
    {
        var courses = await _courseRepository.GetPublishedCoursesAsync();
        return courses.Select(MapToDto);
    }

    public async Task<IEnumerable<CourseDto>> GetCoursesByInstructorAsync(Guid instructorId)
    {
        var courses = await _courseRepository.GetCoursesByInstructorAsync(instructorId);
        return courses.Select(MapToDto);
    }

    public async Task<IEnumerable<CourseDto>> SearchCoursesAsync(string searchTerm)
    {
        var courses = await _courseRepository.SearchCoursesAsync(searchTerm);
        return courses.Select(MapToDto);
    }

    public async Task<CourseDto> CreateCourseAsync(CreateCourseDto dto, Guid instructorId)
    {
        var course = new Course
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            ImageUrl = dto.ImageUrl,
            Price = dto.Price,
            DurationInHours = dto.DurationInHours,
            Status = CourseStatus.Draft,
            InstructorId = instructorId,
            CreatedAt = DateTime.UtcNow
        };

        await _courseRepository.AddAsync(course);
        await _courseRepository.SaveChangesAsync();

        return MapToDto(course);
    }

    public async Task<CourseDto?> UpdateCourseAsync(Guid id, UpdateCourseDto dto)
    {
        var course = await _courseRepository.GetByIdAsync(id);
        if (course == null) return null;

        course.Title = dto.Title;
        course.Description = dto.Description;
        course.ImageUrl = dto.ImageUrl;
        course.Price = dto.Price;
        course.DurationInHours = dto.DurationInHours;
        course.Status = dto.Status;
        course.UpdatedAt = DateTime.UtcNow;

        _courseRepository.Update(course);
        await _courseRepository.SaveChangesAsync();

        return MapToDto(course);
    }

    public async Task<bool> DeleteCourseAsync(Guid id)
    {
        var course = await _courseRepository.GetByIdAsync(id);
        if (course == null) return false;

        course.IsDeleted = true;
        course.UpdatedAt = DateTime.UtcNow;
        
        _courseRepository.Update(course);
        await _courseRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> PublishCourseAsync(Guid id)
    {
        var course = await _courseRepository.GetByIdAsync(id);
        if (course == null) return false;

        course.Status = CourseStatus.Published;
        course.UpdatedAt = DateTime.UtcNow;
        
        _courseRepository.Update(course);
        await _courseRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ArchiveCourseAsync(Guid id)
    {
        var course = await _courseRepository.GetByIdAsync(id);
        if (course == null) return false;

        course.Status = CourseStatus.Archived;
        course.UpdatedAt = DateTime.UtcNow;
        
        _courseRepository.Update(course);
        await _courseRepository.SaveChangesAsync();

        return true;
    }

    private static CourseDto MapToDto(Course course)
    {
        return new CourseDto
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,
            ImageUrl = course.ImageUrl,
            Price = course.Price,
            DurationInHours = course.DurationInHours,
            Status = course.Status,
            InstructorName = course.Instructor?.FullName ?? "Unknown",
            LessonsCount = course.Lessons?.Count ?? 0,
            CreatedAt = course.CreatedAt
        };
    }
}
