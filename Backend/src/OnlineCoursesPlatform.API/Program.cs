using Microsoft.EntityFrameworkCore;
using OnlineCoursesPlatform.API.Extensions;
using OnlineCoursesPlatform.API.Middleware;
using OnlineCoursesPlatform.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    { 
        Title = "OnlineCoursesPlatform API", 
        Version = "v1",
        Description = "API REST para la plataforma de cursos en línea. Permite gestionar cursos, lecciones y autenticación de usuarios.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Soporte OnlineCoursesPlatform",
            Email = "soporte@onlinecourses.com",
            Url = new Uri("https://onlinecourses.com")
        },
        License = new Microsoft.OpenApi.Models.OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });
    
    // Incluir comentarios XML en la documentación de Swagger
    var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
    
    // Configurar autenticación JWT en Swagger
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Ingrese el token JWT. Ejemplo: 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...'"
    });
    
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Agregar servicios personalizados
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(builder.Configuration["AllowedOrigins"] ?? "http://localhost:5002")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configurar el pipeline de solicitudes HTTP
// Swagger habilitado en todos los ambientes
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "OnlineCoursesPlatform API v1");
    options.RoutePrefix = "swagger"; // Acceder en /swagger
    options.DocumentTitle = "OnlineCoursesPlatform API - Documentación";
    options.DefaultModelsExpandDepth(-1); // Ocultar modelos por defecto
    options.DisplayRequestDuration(); // Mostrar duración de las peticiones
});

if (app.Environment.IsDevelopment())
{
    // Aplicar migraciones y sembrar datos en desarrollo
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();
    await DataSeeder.SeedAsync(context);
}

// Middleware de manejo de excepciones
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
