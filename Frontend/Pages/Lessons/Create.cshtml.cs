using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineCoursesPlatform.Web.Models.ViewModels;
using OnlineCoursesPlatform.Web.Services;

namespace OnlineCoursesPlatform.Web.Pages.Lessons;

public class CreateModel : PageModel
{
    private readonly LessonApiClient _lessonApiClient;
    private readonly AuthApiClient _authApiClient;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(
        LessonApiClient lessonApiClient,
        AuthApiClient authApiClient,
        ILogger<CreateModel> logger)
    {
        _lessonApiClient = lessonApiClient;
        _authApiClient = authApiClient;
        _logger = logger;
    }

    [BindProperty]
    public CreateLessonViewModel Lesson { get; set; } = new();
    
    [BindProperty(SupportsGet = true)]
    public Guid CourseId { get; set; }

    public IActionResult OnGet(Guid courseId)
    {
        if (!_authApiClient.IsAuthenticated())
        {
            return RedirectToPage("/Login");
        }

        CourseId = courseId;
        Lesson.CourseId = courseId;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!_authApiClient.IsAuthenticated())
        {
            return RedirectToPage("/Login");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var result = await _lessonApiClient.CreateLessonAsync(Lesson);

        if (result != null)
        {
            _logger.LogInformation("Lesson created: {Title}", Lesson.Title);
            TempData["SuccessMessage"] = "Lección creada exitosamente.";
            return RedirectToPage("/Courses/Details", new { id = Lesson.CourseId });
        }

        ModelState.AddModelError(string.Empty, "Error al crear la lección.");
        return Page();
    }
}
