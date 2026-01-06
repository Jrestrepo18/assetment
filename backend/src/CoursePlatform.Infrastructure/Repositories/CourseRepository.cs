using CoursePlatform.Core.Entities;
using CoursePlatform.Core.Interfaces;
using CoursePlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Infrastructure.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly ApplicationDbContext _context;

    public CourseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Course?> GetByIdAsync(Guid id)
    {
        return await _context.Courses
            .Include(c => c.Lessons)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<(IEnumerable<Course> Items, int TotalCount)> SearchAsync(
        string? query, 
        CourseStatus? status, 
        int page, 
        int pageSize)
    {
        var queryable = _context.Courses.AsQueryable();

        if (!string.IsNullOrEmpty(query))
        {
            queryable = queryable.Where(c => c.Title.Contains(query));
        }

        if (status.HasValue)
        {
            queryable = queryable.Where(c => c.Status == status.Value);
        }

        var totalCount = await queryable.CountAsync();

        var items = await queryable
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Course> AddAsync(Course course)
    {
        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();
        return course;
    }

    public async Task UpdateAsync(Course course)
    {
        _context.Courses.Update(course);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course != null)
        {
            course.IsDeleted = true;
            course.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task HardDeleteAsync(Guid id)
    {
        var course = await _context.Courses.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.Id == id);
        if (course != null)
        {
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }
    }


    public async Task<int> GetActiveLessonsCountAsync(Guid courseId)
    {
        return await _context.Lessons
            .Where(l => l.CourseId == courseId && !l.IsDeleted)
            .CountAsync();
    }
    
    public async Task<(int Total, int Drafts, int Published)> GetStatsAsync()
    {
        var stats = await _context.Courses
            .GroupBy(c => c.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync();

        var drafts = stats.FirstOrDefault(s => s.Status == CourseStatus.Draft)?.Count ?? 0;
        var published = stats.FirstOrDefault(s => s.Status == CourseStatus.Published)?.Count ?? 0;
        var total = await _context.Courses.CountAsync(); // Total distinct from filtered status counts if needed
        
        return (total, drafts, published);
    }

}