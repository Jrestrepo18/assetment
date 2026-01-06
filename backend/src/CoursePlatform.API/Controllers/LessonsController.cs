using CoursePlatform.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoursePlatform.API.Controllers;

[ApiController]
[Route("api")]
[Authorize]
public class LessonsController : ControllerBase
{
    private readonly LessonService _lessonService;

    public LessonsController(LessonService lessonService)
    {
        _lessonService = lessonService;
    }

    [HttpGet("courses/{courseId}/lessons")]
    public async Task<IActionResult> GetByCourseId(Guid courseId)
    {
        var lessons = await _lessonService.GetByCourseIdAsync(courseId);
        return Ok(lessons);
    }

    [HttpGet("lessons/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var lesson = await _lessonService.GetByIdAsync(id);
        if (lesson == null)
            return NotFound();

        return Ok(lesson);
    }

    [HttpPost("lessons")]
    public async Task<IActionResult> Create([FromBody] CreateLessonRequest request)
    {
        try
        {
            var lesson = await _lessonService.CreateAsync(request.CourseId, request.Title, request.Order);
            return CreatedAtAction(nameof(GetById), new { id = lesson.Id }, lesson);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("lessons/{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLessonRequest request)
    {
        try
        {
            await _lessonService.UpdateAsync(id, request.Title, request.Order);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("lessons/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (User.IsInRole("Admin"))
        {
            await _lessonService.HardDeleteAsync(id);
        }
        else
        {
            await _lessonService.DeleteAsync(id);
        }
        return NoContent();
    }


    [HttpPatch("lessons/{id}/reorder")]
    public async Task<IActionResult> Reorder(Guid id, [FromBody] ReorderLessonRequest request)
    {
        try
        {
            await _lessonService.ReorderAsync(id, request.NewOrder);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

public record CreateLessonRequest(Guid CourseId, string Title, int Order);
public record UpdateLessonRequest(string Title, int Order);
public record ReorderLessonRequest(int NewOrder);