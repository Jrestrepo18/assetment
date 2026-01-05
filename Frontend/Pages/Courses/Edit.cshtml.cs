using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineCoursesPlatform.Web.Models.ViewModels;
using OnlineCoursesPlatform.Web.Services;

namespace OnlineCoursesPlatform.Web.Pages.Courses;

public class EditModel : PageModel
{
    private readonly CourseApiClient _courseApiClient;
    private readonly AuthApiClient _authApiClient;
    private readonly ILogger<EditModel> _logger;

    public EditModel(
        CourseApiClient courseApiClient,
        AuthApiClient authApiClient,
        ILogger<EditModel> logger)
    {
        _courseApiClient = courseApiClient;
        _authApiClient = authApiClient;
        _logger = logger;
    }

    [BindProperty]
    public CreateCourseViewModel Course { get; set; } = new();
    
    public Guid Id { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        if (!_authApiClient.IsAuthenticated())
        {
            return RedirectToPage("/Login", new { returnUrl = $"/Courses/Edit/{id}" });
        }

        Id = id;
        var course = await _courseApiClient.GetCourseAsync(id);

        if (course == null)
        {
            return NotFound();
        }

        Course = new CreateCourseViewModel
        {
            Title = course.Title,
            Description = course.Description,
            ImageUrl = course.ImageUrl,
            Price = course.Price,
            DurationInHours = course.DurationInHours
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

        var result = await _courseApiClient.UpdateCourseAsync(id, Course);

        if (result != null)
        {
            _logger.LogInformation("Course updated: {Id}", id);
            TempData["SuccessMessage"] = "Curso actualizado exitosamente.";
            return RedirectToPage("Details", new { id });
        }

        ModelState.AddModelError(string.Empty, "Error al actualizar el curso.");
        return Page();
    }
}
