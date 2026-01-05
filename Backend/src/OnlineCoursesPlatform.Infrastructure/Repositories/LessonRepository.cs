using Microsoft.EntityFrameworkCore;
using OnlineCoursesPlatform.Domain.Entities;
using OnlineCoursesPlatform.Domain.Interfaces;
using OnlineCoursesPlatform.Infrastructure.Data;

namespace OnlineCoursesPlatform.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio de lecciones.
/// </summary>
public class LessonRepository : Repository<Lesson>, ILessonRepository
{
    public LessonRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Lesson>> GetLessonsByCourseAsync(Guid courseId)
    {
        return await _dbSet
            .Include(l => l.Course)
            .Where(l => l.CourseId == courseId)
            .OrderBy(l => l.Order)
            .ToListAsync();
    }

    public async Task<Lesson?> GetLessonWithCourseAsync(Guid lessonId)
    {
        return await _dbSet
            .Include(l => l.Course)
            .FirstOrDefaultAsync(l => l.Id == lessonId);
    }

    public async Task<int> GetMaxOrderByCourseAsync(Guid courseId)
    {
        var maxOrder = await _dbSet
            .Where(l => l.CourseId == courseId)
            .MaxAsync(l => (int?)l.Order);
        
        return maxOrder ?? 0;
    }

    public async Task ReorderLessonsAsync(Guid courseId, List<Guid> lessonIds)
    {
        var lessons = await _dbSet
            .Where(l => l.CourseId == courseId && lessonIds.Contains(l.Id))
            .ToListAsync();

        for (int i = 0; i < lessonIds.Count; i++)
        {
            var lesson = lessons.FirstOrDefault(l => l.Id == lessonIds[i]);
            if (lesson != null)
            {
                lesson.Order = i + 1;
                lesson.UpdatedAt = DateTime.UtcNow;
            }
        }

        await _context.SaveChangesAsync();
    }
}
