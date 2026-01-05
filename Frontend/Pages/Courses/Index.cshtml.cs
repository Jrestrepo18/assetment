using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineCoursesPlatform.Web.Models.ViewModels;
using OnlineCoursesPlatform.Web.Services;

namespace OnlineCoursesPlatform.Web.Pages.Courses;

public class IndexModel : PageModel
{
    private readonly CourseApiClient _courseApiClient;
    private readonly AuthApiClient _authApiClient;

    public IndexModel(CourseApiClient courseApiClient, AuthApiClient authApiClient)
    {
        _courseApiClient = courseApiClient;
        _authApiClient = authApiClient;
    }

    public IEnumerable<CourseViewModel> Courses { get; set; } = Enumerable.Empty<CourseViewModel>();
    
    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    public bool IsInstructor => _authApiClient.GetCurrentUserRole() is "Instructor" or "Admin";

    public async Task OnGetAsync()
    {
        if (!string.IsNullOrWhiteSpace(SearchTerm))
        {
            Courses = await _courseApiClient.SearchCoursesAsync(SearchTerm);
        }
        else
        {
            Courses = await _courseApiClient.GetCoursesAsync();
        }
    }
}
