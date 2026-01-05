using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineCoursesPlatform.Web.Models.ViewModels;
using OnlineCoursesPlatform.Web.Services;

namespace OnlineCoursesPlatform.Web.Pages.Courses;

public class CreateModel : PageModel
{
    private readonly CourseApiClient _courseApiClient;
    private readonly AuthApiClient _authApiClient;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(
        CourseApiClient courseApiClient, 
        AuthApiClient authApiClient,
        ILogger<CreateModel> logger)
    {
        _courseApiClient = courseApiClient;
        _authApiClient = authApiClient;
        _logger = logger;
    }

    [BindProperty]
    public CreateCourseViewModel Course { get; set; } = new();

    public IActionResult OnGet()
    {
        if (!_authApiClient.IsAuthenticated())
        {
            return RedirectToPage("/Login", new { returnUrl = "/Courses/Create" });
        }

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

        var result = await _courseApiClient.CreateCourseAsync(Course);

        if (result != null)
        {
            _logger.LogInformation("Course created: {Title}", Course.Title);
            TempData["SuccessMessage"] = "Curso creado exitosamente.";
            return RedirectToPage("Details", new { id = result.Id });
        }

        ModelState.AddModelError(string.Empty, "Error al crear el curso. Intente nuevamente.");
        return Page();
    }
}
