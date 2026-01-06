using CoursePlatform.Core.Entities;
using CoursePlatform.Core.Interfaces;
using CoursePlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Infrastructure.Repositories;

public class LessonRepository : ILessonRepository
{
    private readonly ApplicationDbContext _context;

    public LessonRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Lesson?> GetByIdAsync(Guid id)
    {
        return await _context.Lessons.FindAsync(id);
    }

    public async Task<IEnumerable<Lesson>> GetByCourseIdAsync(Guid courseId)
    {
        return await _context.Lessons
            .Where(l => l.CourseId == courseId)
            .OrderBy(l => l.Order)
            .ToListAsync();
    }

    public async Task<Lesson> AddAsync(Lesson lesson)
    {
        await _context.Lessons.AddAsync(lesson);
        await _context.SaveChangesAsync();
        return lesson;
    }

    public async Task UpdateAsync(Lesson lesson)
    {
        _context.Lessons.Update(lesson);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var lesson = await _context.Lessons.FindAsync(id);
        if (lesson != null)
        {
            lesson.IsDeleted = true;
            lesson.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task HardDeleteAsync(Guid id)
    {
        var lesson = await _context.Lessons.IgnoreQueryFilters().FirstOrDefaultAsync(l => l.Id == id);
        if (lesson != null)
        {
            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();
        }
    }


    public async Task<bool> OrderExistsInCourseAsync(Guid courseId, int order, Guid? excludeLessonId = null)
    {
        var query = _context.Lessons.Where(l => l.CourseId == courseId && l.Order == order);
        
        if (excludeLessonId.HasValue)
        {
            query = query.Where(l => l.Id != excludeLessonId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<Lesson?> GetByOrderInCourseAsync(Guid courseId, int order)
    {
        return await _context.Lessons
            .FirstOrDefaultAsync(l => l.CourseId == courseId && l.Order == order);
    }

    public async Task<int> GetMaxOrderAsync(Guid courseId)
    {
        return await _context.Lessons
            .Where(l => l.CourseId == courseId)
            .MaxAsync(l => (int?)l.Order) ?? 0;
    }
}