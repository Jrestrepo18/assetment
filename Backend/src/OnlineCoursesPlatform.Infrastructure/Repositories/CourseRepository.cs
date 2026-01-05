using Microsoft.EntityFrameworkCore;
using OnlineCoursesPlatform.Domain.Entities;
using OnlineCoursesPlatform.Domain.Enums;
using OnlineCoursesPlatform.Domain.Interfaces;
using OnlineCoursesPlatform.Infrastructure.Data;

namespace OnlineCoursesPlatform.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de cursos.
/// </summary>
public class CourseRepository : Repository<Course>, ICourseRepository
{
    public CourseRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Course>> GetCoursesByInstructorAsync(Guid instructorId)
    {
        return await _dbSet
            .Include(c => c.Instructor)
            .Include(c => c.Lessons)
            .Where(c => c.InstructorId == instructorId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Course>> GetCoursesByStatusAsync(CourseStatus status)
    {
        return await _dbSet
            .Include(c => c.Instructor)
            .Include(c => c.Lessons)
            .Where(c => c.Status == status)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<Course?> GetCourseWithLessonsAsync(Guid courseId)
    {
        return await _dbSet
            .Include(c => c.Instructor)
            .Include(c => c.Lessons.OrderBy(l => l.Order))
            .FirstOrDefaultAsync(c => c.Id == courseId);
    }

    public async Task<IEnumerable<Course>> GetPublishedCoursesAsync()
    {
        return await _dbSet
            .Include(c => c.Instructor)
            .Include(c => c.Lessons)
            .Where(c => c.Status == CourseStatus.Published)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Course>> SearchCoursesAsync(string searchTerm)
    {
        var lowerSearchTerm = searchTerm.ToLower();
        
        return await _dbSet
            .Include(c => c.Instructor)
            .Include(c => c.Lessons)
            .Where(c => c.Status == CourseStatus.Published &&
                       (c.Title.ToLower().Contains(lowerSearchTerm) ||
                        c.Description.ToLower().Contains(lowerSearchTerm)))
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }
}
