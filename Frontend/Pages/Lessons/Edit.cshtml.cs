using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineCoursesPlatform.Web.Models.ViewModels;
using OnlineCoursesPlatform.Web.Services;

namespace OnlineCoursesPlatform.Web.Pages.Lessons;

public class EditModel : PageModel
{
    private readonly LessonApiClient _lessonApiClient;
    private readonly AuthApiClient _authApiClient;
    private readonly ILogger<EditModel> _logger;

    public EditModel(
        LessonApiClient lessonApiClient,
        AuthApiClient authApiClient,
        ILogger<EditModel> logger)
    {
        _lessonApiClient = lessonApiClient;
        _authApiClient = authApiClient;
        _logger = logger;
    }

    [BindProperty]
    public CreateLessonViewModel Lesson { get; set; } = new();
    
    public Guid Id { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        if (!_authApiClient.IsAuthenticated())
        {
            return RedirectToPage("/Login");
        }

        Id = id;
        var lesson = await _lessonApiClient.GetLessonAsync(id);

        if (lesson == null)
        {
            return NotFound();
        }

        Lesson = new CreateLessonViewModel
        {
            Title = lesson.Title,
            Content = lesson.Content,
            VideoUrl = lesson.VideoUrl,
            DurationInMinutes = lesson.DurationInMinutes,
            CourseId = lesson.CourseId
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid id)
    {
        if (!_authApiClient.IsAuthenticated())
        {
            return RedirectToPage("/Login");
        }

        Id = id;

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var result = await _lessonApiClient.UpdateLessonAsync(id, Lesson);

        if (result != null)
        {
            _logger.LogInformation("Lesson updated: {Id}", id);
            TempData["SuccessMessage"] = "Lección actualizada exitosamente.";
            return RedirectToPage("/Courses/Details", new { id = Lesson.CourseId });
        }

        ModelState.AddModelError(string.Empty, "Error al actualizar la lección.");
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        if (!_authApiClient.IsAuthenticated())
        {
            return RedirectToPage("/Login");
        }

        var lesson = await _lessonApiClient.GetLessonAsync(id);
        var courseId = lesson?.CourseId ?? Guid.Empty;

        var result = await _lessonApiClient.DeleteLessonAsync(id);

        if (result)
        {
            TempData["SuccessMessage"] = "Lección eliminada exitosamente.";
        }
        else
        {
            TempData["ErrorMessage"] = "Error al eliminar la lección.";
        }

        return RedirectToPage("/Courses/Details", new { id = courseId });
    }
}
