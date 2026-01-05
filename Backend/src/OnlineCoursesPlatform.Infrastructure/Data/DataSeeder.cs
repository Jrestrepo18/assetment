using OnlineCoursesPlatform.Domain.Entities;
using OnlineCoursesPlatform.Domain.Enums;

namespace OnlineCoursesPlatform.Infrastructure.Data;

/// <summary>
/// Clase para sembrar datos iniciales en la base de datos (además del seed de Identity en OnModelCreating).
/// </summary>
public static class DataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Verificar si ya hay cursos
        if (context.Courses.Any()) return;

        // Crear curso de ejemplo
        var course = new Course
        {
            Id = Guid.NewGuid(),
            Title = "Introducción a la Programación",
            CourseStatus = CourseStatus.Published,
            CreatedAt = DateTime.UtcNow
        };

        await context.Courses.AddAsync(course);

        // Crear lecciones de ejemplo
        var lessons = new List<Lesson>
        {
            new Lesson
            {
                Id = Guid.NewGuid(),
                Title = "Bienvenida al curso",
                Order = 1,
                CourseId = course.Id,
                CreatedAt = DateTime.UtcNow
            },
            new Lesson
            {
                Id = Guid.NewGuid(),
                Title = "Variables y tipos de datos",
                Order = 2,
                CourseId = course.Id,
                CreatedAt = DateTime.UtcNow
            },
            new Lesson
            {
                Id = Guid.NewGuid(),
                Title = "Estructuras de control",
                Order = 3,
                CourseId = course.Id,
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.Lessons.AddRangeAsync(lessons);
        await context.SaveChangesAsync();
    }
}
