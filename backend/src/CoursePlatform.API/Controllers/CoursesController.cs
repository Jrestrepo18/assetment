using CoursePlatform.Core.Entities;
using CoursePlatform.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoursePlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CoursesController : ControllerBase
{
    private readonly CourseService _courseService;

    public CoursesController(CourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var (total, drafts, published) = await _courseService.GetStatsAsync();
        return Ok(new { total, drafts, published });
    }


    [HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] string? q,
        [FromQuery] string? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        CourseStatus? courseStatus = null;
        if (!string.IsNullOrEmpty(status) && Enum.TryParse<CourseStatus>(status, true, out var parsedStatus))
        {
            courseStatus = parsedStatus;
        }

        var (items, totalCount) = await _courseService.SearchAsync(q, courseStatus, page, pageSize);

        return Ok(new
        {
            items,
            totalCount,
            page,
            pageSize,
            totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var course = await _courseService.GetByIdAsync(id);
        if (course == null)
            return NotFound();

        return Ok(course);
    }

    [HttpGet("{id}/summary")]
    public async Task<IActionResult> GetSummary(Guid id)
    {
        try
        {
            var summary = await _courseService.GetSummaryAsync(id);
            return Ok(summary);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCourseRequest request)
    {
        var course = await _courseService.CreateAsync(request.Title, request.Description);
        return CreatedAtAction(nameof(GetById), new { id = course.Id }, course);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCourseRequest request)
    {
        try
        {
            await _courseService.UpdateAsync(id, request.Title, request.Description);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _courseService.HardDeleteAsync(id);
        return NoContent();
    }


    [HttpPatch("{id}/publish")]
    public async Task<IActionResult> Publish(Guid id)
    {
        try
        {
            await _courseService.PublishAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("{id}/unpublish")]
    public async Task<IActionResult> Unpublish(Guid id)
    {
        try
        {
            await _courseService.UnpublishAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}

public record CreateCourseRequest(string Title, string? Description);
public record UpdateCourseRequest(string Title, string? Description);