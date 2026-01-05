using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCoursesPlatform.Application.DTOs;
using OnlineCoursesPlatform.Application.Interfaces;

namespace OnlineCoursesPlatform.API.Controllers;

/// <summary>
/// Controlador para gestión de cursos.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;
    private readonly ILogger<CoursesController> _logger;

    public CoursesController(ICourseService courseService, ILogger<CoursesController> logger)
    {
        _courseService = courseService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los cursos publicados.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CourseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCourses()
    {
        var courses = await _courseService.GetPublishedCoursesAsync();
        return Ok(courses);
    }

    /// <summary>
    /// Obtiene un curso por su ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CourseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCourse(Guid id)
    {
        var course = await _courseService.GetCourseByIdAsync(id);
        
        if (course == null)
        {
            return NotFound(new { message = "Curso no encontrado." });
        }

        return Ok(course);
    }

    /// <summary>
    /// Busca cursos por término de búsqueda.
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<CourseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchCourses([FromQuery] string term)
    {
        if (string.IsNullOrWhiteSpace(term))
        {
            return Ok(Enumerable.Empty<CourseDto>());
        }

        var courses = await _courseService.SearchCoursesAsync(term);
        return Ok(courses);
    }

    /// <summary>
    /// Crea un nuevo curso (solo instructores).
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "InstructorOrAdmin")]
    [ProducesResponseType(typeof(CourseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto dto)
    {
        var userId = GetCurrentUserId();
        var course = await _courseService.CreateCourseAsync(dto, userId);
        
        _logger.LogInformation("Course created: {Title} by user {UserId}", dto.Title, userId);
        return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
    }

    /// <summary>
    /// Actualiza un curso existente.
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Policy = "InstructorOrAdmin")]
    [ProducesResponseType(typeof(CourseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCourse(Guid id, [FromBody] UpdateCourseDto dto)
    {
        var course = await _courseService.UpdateCourseAsync(id, dto);
        
        if (course == null)
        {
            return NotFound(new { message = "Curso no encontrado." });
        }

        _logger.LogInformation("Course updated: {Id}", id);
        return Ok(course);
    }

    /// <summary>
    /// Elimina un curso (soft delete).
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "InstructorOrAdmin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCourse(Guid id)
    {
        var result = await _courseService.DeleteCourseAsync(id);
        
        if (!result)
        {
            return NotFound(new { message = "Curso no encontrado." });
        }

        _logger.LogInformation("Course deleted: {Id}", id);
        return NoContent();
    }

    /// <summary>
    /// Publica un curso.
    /// </summary>
    [HttpPost("{id:guid}/publish")]
    [Authorize(Policy = "InstructorOrAdmin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PublishCourse(Guid id)
    {
        var result = await _courseService.PublishCourseAsync(id);
        
        if (!result)
        {
            return NotFound(new { message = "Curso no encontrado." });
        }

        _logger.LogInformation("Course published: {Id}", id);
        return Ok(new { message = "Curso publicado exitosamente." });
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim ?? throw new UnauthorizedAccessException());
    }
}
