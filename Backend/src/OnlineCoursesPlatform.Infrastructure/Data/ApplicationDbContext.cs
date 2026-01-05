using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineCoursesPlatform.Domain.Entities;

namespace OnlineCoursesPlatform.Infrastructure.Data;

/// <summary>
/// Contexto de base de datos de la aplicación.
/// Hereda de IdentityDbContext para soporte de Identity.
/// </summary>
public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Lesson> Lessons => Set<Lesson>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ================================
        // QUERY FILTERS GLOBALES (Soft Delete)
        // ================================
        // Aplicar filtro global para que se ignoren los registros donde IsDeleted == true
        modelBuilder.Entity<Course>().HasQueryFilter(c => !c.IsDeleted);
        modelBuilder.Entity<Lesson>().HasQueryFilter(l => !l.IsDeleted);

        // ================================
        // CONFIGURACIÓN DE RELACIONES
        // ================================
        // Relación 1:N entre Course y Lesson
        modelBuilder.Entity<Course>()
            .HasMany(c => c.Lessons)
            .WithOne(l => l.Course)
            .HasForeignKey(l => l.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        // ================================
        // CONFIGURACIÓN DE ENTIDADES
        // ================================
        // Course
        modelBuilder.Entity<Course>(entity =>
        {
            entity.ToTable("Courses");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Title).IsRequired().HasMaxLength(200);
            entity.Property(c => c.CourseStatus)
                .HasConversion<string>()
                .HasMaxLength(20);
            entity.HasIndex(c => c.Title);
        });

        // Lesson
        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.ToTable("Lessons");
            entity.HasKey(l => l.Id);
            entity.Property(l => l.Title).IsRequired().HasMaxLength(200);
            entity.HasIndex(l => l.CourseId);
            entity.HasIndex(l => new { l.CourseId, l.Order });
        });

        // ================================
        // SEED DATA - Usuario de Prueba
        // ================================
        // Crear usuario admin@riwi.io con contraseña Test1234!
        var hasher = new PasswordHasher<IdentityUser>();
        var adminUserId = "8e445865-a24d-4543-a6c6-9443d048cdb9";

        var adminUser = new IdentityUser
        {
            Id = adminUserId,
            UserName = "admin@riwi.io",
            NormalizedUserName = "ADMIN@RIWI.IO",
            Email = "admin@riwi.io",
            NormalizedEmail = "ADMIN@RIWI.IO",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        };

        adminUser.PasswordHash = hasher.HashPassword(adminUser, "Test1234!");

        modelBuilder.Entity<IdentityUser>().HasData(adminUser);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Actualizar automáticamente las fechas de modificación
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
