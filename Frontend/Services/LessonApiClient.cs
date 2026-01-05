using OnlineCoursesPlatform.Web.Models.ViewModels;

namespace OnlineCoursesPlatform.Web.Services;

/// <summary>
/// Cliente API para operaciones con lecciones.
/// </summary>
public class LessonApiClient
{
    private readonly ApiClient _apiClient;

    public LessonApiClient(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IEnumerable<LessonViewModel>> GetLessonsByCourseAsync(Guid courseId)
    {
        return await _apiClient.GetAsync<IEnumerable<LessonViewModel>>($"api/lessons/course/{courseId}") 
               ?? Enumerable.Empty<LessonViewModel>();
    }

    public async Task<LessonViewModel?> GetLessonAsync(Guid id)
    {
        return await _apiClient.GetAsync<LessonViewModel>($"api/lessons/{id}");
    }

    public async Task<LessonViewModel?> CreateLessonAsync(CreateLessonViewModel model)
    {
        return await _apiClient.PostAsync<CreateLessonViewModel, LessonViewModel>("api/lessons", model);
    }

    public async Task<LessonViewModel?> UpdateLessonAsync(Guid id, CreateLessonViewModel model)
    {
        return await _apiClient.PutAsync<CreateLessonViewModel, LessonViewModel>($"api/lessons/{id}", model);
    }

    public async Task<bool> DeleteLessonAsync(Guid id)
    {
        return await _apiClient.DeleteAsync($"api/lessons/{id}");
    }

    public async Task<bool> ReorderLessonsAsync(Guid courseId, List<Guid> lessonIds)
    {
        var result = await _apiClient.PostAsync<List<Guid>, object>($"api/lessons/course/{courseId}/reorder", lessonIds);
        return result != null;
    }
}
