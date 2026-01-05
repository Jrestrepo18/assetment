using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineCoursesPlatform.Domain.Entities;

namespace OnlineCoursesPlatform.Infrastructure.Data.Configurations;

/// <summary>
/// Configuración de Entity Framework para la entidad Lesson.
/// Nota: La configuración principal está en ApplicationDbContext.OnModelCreating
/// Este archivo se mantiene para configuraciones adicionales si se necesitan.
/// </summary>
public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        // Las configuraciones principales están en ApplicationDbContext
        // Este archivo puede usarse para configuraciones adicionales
    }
}
