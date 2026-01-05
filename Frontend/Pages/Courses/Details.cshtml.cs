using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineCoursesPlatform.Web.Models.ViewModels;
using OnlineCoursesPlatform.Web.Services;

namespace OnlineCoursesPlatform.Web.Pages.Courses;

public class DetailsModel : PageModel
{
    private readonly CourseApiClient _courseApiClient;
    private readonly LessonApiClient _lessonApiClient;
    private readonly AuthApiClient _authApiClient;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(
        CourseApiClient courseApiClient,
        LessonApiClient lessonApiClient,
        AuthApiClient authApiClient,
        ILogger<DetailsModel> logger)
    {
        _courseApiClient = courseApiClient;
        _lessonApiClient = lessonApiClient;
        _authApiClient = authApiClient;
        _logger = logger;
    }

    public CourseViewModel? Course { get; set; }
    public IEnumerable<LessonViewModel> Lessons { get; set; } = Enumerable.Empty<LessonViewModel>();
    public bool IsInstructor => _authApiClient.GetCurrentUserRole() is "Instructor" or "Admin";

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        Course = await _courseApiClient.GetCourseAsync(id);

        if (Course == null)
        {
            return NotFound();
        }

        Lessons = await _lessonApiClient.GetLessonsByCourseAsync(id);
        return Page();
    }

    public async Task<IActionResult> OnPostPublishAsync(Guid id)
    {
        if (!_authApiClient.IsAuthenticated())
        {
            return RedirectToPage("/Login");
        }

        var result = await _courseApiClient.PublishCourseAsync(id);

        if (result)
        {
            TempData["SuccessMessage"] = "Curso publicado exitosamente.";
        }
        else
        {
            TempData["ErrorMessage"] = "Error al publicar el curso.";
        }

        return RedirectToPage(new { id });
    }
}
