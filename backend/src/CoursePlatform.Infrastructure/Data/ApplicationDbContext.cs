using CoursePlatform.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Course> Courses { get; set; }
    public DbSet<Lesson> Lessons { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Global query filter for soft delete
        builder.Entity<Course>().HasQueryFilter(c => !c.IsDeleted);
        builder.Entity<Lesson>().HasQueryFilter(l => !l.IsDeleted);

        // Course configuration
        builder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Status).HasConversion<string>();
            entity.HasMany(e => e.Lessons)
                .WithOne(e => e.Course)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Lesson configuration
        builder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            // Removed unique index on Order - conflicts with soft delete
        });

        // Seed data
        SeedData(builder);
    }

    private void SeedData(ModelBuilder builder)
    {
        var adminRoleId = "5a0a36e9-3870-466d-9781-a960965e6837";
        var teacherRoleId = "9a7f3478-f73c-4c66-b333-649033480838";
        var adminUserId = "6e7c7a11-1c5c-4f7d-8178-6548a3c8e9d9";

        // Seed Roles
        builder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole { Id = teacherRoleId, Name = "Teacher", NormalizedName = "TEACHER" }
        );

        var hasher = new PasswordHasher<IdentityUser>();

        // Seed Admin User
        var adminUser = new IdentityUser
        {
            Id = adminUserId,
            UserName = "jerorrpo@gmail.com",
            NormalizedUserName = "JERORRPO@GMAIL.COM",
            Email = "jerorrpo@gmail.com",
            NormalizedEmail = "JERORRPO@GMAIL.COM",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        };
        adminUser.PasswordHash = hasher.HashPassword(adminUser, "Jero0312*");

        builder.Entity<IdentityUser>().HasData(adminUser);

        // Assign Admin Role to Admin User
        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = adminRoleId,
            UserId = adminUserId
        });

        // Seed Test User (optional but keeping for backward compatibility)
        var testUserId = "f74551c1-17f8-443b-bff0-324a36b25926";
        var testUser = new IdentityUser
        {
            Id = testUserId,
            UserName = "test@test.com",
            NormalizedUserName = "TEST@TEST.COM",
            Email = "test@test.com",
            NormalizedEmail = "TEST@TEST.COM",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        };
        testUser.PasswordHash = hasher.HashPassword(testUser, "Test12345*");

        builder.Entity<IdentityUser>().HasData(testUser);
        
        // Assign Admin role to test user
        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = adminRoleId,
            UserId = testUserId
        });
    }

}