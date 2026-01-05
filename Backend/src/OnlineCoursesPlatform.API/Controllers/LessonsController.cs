using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCoursesPlatform.Application.DTOs;
using OnlineCoursesPlatform.Application.Interfaces;

namespace OnlineCoursesPlatform.API.Controllers;

/// <summary>
/// Controlador para gestión de lecciones.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class LessonsController : ControllerBase
{
    private readonly ILessonService _lessonService;
    private readonly ILogger<LessonsController> _logger;

    public LessonsController(ILessonService lessonService, ILogger<LessonsController> logger)
    {
        _lessonService = lessonService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todas las lecciones de un curso.
    /// </summary>
    [HttpGet("course/{courseId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<LessonDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLessonsByCourse(Guid courseId)
    {
        var lessons = await _lessonService.GetLessonsByCourseAsync(courseId);
        return Ok(lessons);
    }

    /// <summary>
    /// Obtiene una lección por su ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(LessonDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLesson(Guid id)
    {
        var lesson = await _lessonService.GetLessonByIdAsync(id);
        
        if (lesson == null)
        {
            return NotFound(new { message = "Lección no encontrada." });
        }

        return Ok(lesson);
    }

    /// <summary>
    /// Crea una nueva lección (solo instructores).
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "InstructorOrAdmin")]
    [ProducesResponseType(typeof(LessonDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateLesson([FromBody] CreateLessonDto dto)
    {
        var lesson = await _lessonService.CreateLessonAsync(dto);
        
        _logger.LogInformation("Lesson created: {Title} for course {CourseId}", dto.Title, dto.CourseId);
        return CreatedAtAction(nameof(GetLesson), new { id = lesson.Id }, lesson);
    }

    /// <summary>
    /// Actualiza una lección existente.
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Policy = "InstructorOrAdmin")]
    [ProducesResponseType(typeof(LessonDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateLesson(Guid id, [FromBody] UpdateLessonDto dto)
    {
        var lesson = await _lessonService.UpdateLessonAsync(id, dto);
        
        if (lesson == null)
        {
            return NotFound(new { message = "Lección no encontrada." });
        }

        _logger.LogInformation("Lesson updated: {Id}", id);
        return Ok(lesson);
    }

    /// <summary>
    /// Elimina una lección (soft delete).
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "InstructorOrAdmin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteLesson(Guid id)
    {
        var result = await _lessonService.DeleteLessonAsync(id);
        
        if (!result)
        {
            return NotFound(new { message = "Lección no encontrada." });
        }

        _logger.LogInformation("Lesson deleted: {Id}", id);
        return NoContent();
    }

    /// <summary>
    /// Reordena las lecciones de un curso.
    /// </summary>
    [HttpPost("course/{courseId:guid}/reorder")]
    [Authorize(Policy = "InstructorOrAdmin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ReorderLessons(Guid courseId, [FromBody] List<Guid> lessonIds)
    {
        await _lessonService.ReorderLessonsAsync(courseId, lessonIds);
        
        _logger.LogInformation("Lessons reordered for course: {CourseId}", courseId);
        return Ok(new { message = "Lecciones reordenadas exitosamente." });
    }
}
