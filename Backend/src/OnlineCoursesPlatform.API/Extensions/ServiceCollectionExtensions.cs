using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineCoursesPlatform.Application.Interfaces;
using OnlineCoursesPlatform.Application.Services;
using OnlineCoursesPlatform.Domain.Interfaces;
using OnlineCoursesPlatform.Infrastructure.Data;
using OnlineCoursesPlatform.Infrastructure.Identity;
using OnlineCoursesPlatform.Infrastructure.Repositories;

namespace OnlineCoursesPlatform.API.Extensions;

/// <summary>
/// Extensiones para configurar servicios en el contenedor de DI.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Agrega los servicios de la capa Application.
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<ILessonService, LessonService>();
        services.AddScoped<IAuthService, AuthService>();
        
        return services;
    }

    /// <summary>
    /// Agrega los servicios de la capa Infrastructure.
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configurar Entity Framework con Identity
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Configurar Identity
        services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            // Configuración de contraseñas
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            
            // Configuración de usuario
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        // Registrar repositorios
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<ILessonRepository, LessonRepository>();

        // Registrar servicios de Identity
        services.AddScoped<TokenService>();
        services.AddScoped<PasswordHasher>();

        return services;
    }

    /// <summary>
    /// Configura la autenticación JWT.
    /// </summary>
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var secretKey = configuration["Jwt:SecretKey"] ?? throw new ArgumentNullException("Jwt:SecretKey");
        var issuer = configuration["Jwt:Issuer"] ?? "OnlineCoursesPlatform";
        var audience = configuration["Jwt:Audience"] ?? "OnlineCoursesPlatformUsers";

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            options.AddPolicy("InstructorOrAdmin", policy => policy.RequireRole("Admin", "Instructor"));
        });

        return services;
    }
}
