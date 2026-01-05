using OnlineCoursesPlatform.Web.Models.ViewModels;

namespace OnlineCoursesPlatform.Web.Services;

/// <summary>
/// Cliente API para operaciones con cursos.
/// </summary>
public class CourseApiClient
{
    private readonly ApiClient _apiClient;

    public CourseApiClient(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IEnumerable<CourseViewModel>> GetCoursesAsync()
    {
        return await _apiClient.GetAsync<IEnumerable<CourseViewModel>>("api/courses") 
               ?? Enumerable.Empty<CourseViewModel>();
    }

    public async Task<CourseViewModel?> GetCourseAsync(Guid id)
    {
        return await _apiClient.GetAsync<CourseViewModel>($"api/courses/{id}");
    }

    public async Task<IEnumerable<CourseViewModel>> SearchCoursesAsync(string term)
    {
        return await _apiClient.GetAsync<IEnumerable<CourseViewModel>>($"api/courses/search?term={Uri.EscapeDataString(term)}") 
               ?? Enumerable.Empty<CourseViewModel>();
    }

    public async Task<CourseViewModel?> CreateCourseAsync(CreateCourseViewModel model)
    {
        return await _apiClient.PostAsync<CreateCourseViewModel, CourseViewModel>("api/courses", model);
    }

    public async Task<CourseViewModel?> UpdateCourseAsync(Guid id, CreateCourseViewModel model)
    {
        return await _apiClient.PutAsync<CreateCourseViewModel, CourseViewModel>($"api/courses/{id}", model);
    }

    public async Task<bool> DeleteCourseAsync(Guid id)
    {
        return await _apiClient.DeleteAsync($"api/courses/{id}");
    }

    public async Task<bool> PublishCourseAsync(Guid id)
    {
        var result = await _apiClient.PostAsync<object, object>($"api/courses/{id}/publish", new { });
        return result != null;
    }
}
