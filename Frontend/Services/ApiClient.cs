using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace OnlineCoursesPlatform.Web.Services;

/// <summary>
/// Cliente HTTP base para comunicación con la API.
/// </summary>
public class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    private void AddAuthorizationHeader()
    {
        var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.GetAsync(endpoint);
        
        if (!response.IsSuccessStatusCode)
            return default;

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, _jsonOptions);
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        AddAuthorizationHeader();
        var json = JsonSerializer.Serialize(data, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync(endpoint, content);
        
        if (!response.IsSuccessStatusCode)
            return default;

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TResponse>(responseContent, _jsonOptions);
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        AddAuthorizationHeader();
        var json = JsonSerializer.Serialize(data, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PutAsync(endpoint, content);
        
        if (!response.IsSuccessStatusCode)
            return default;

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TResponse>(responseContent, _jsonOptions);
    }

    public async Task<bool> DeleteAsync(string endpoint)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.DeleteAsync(endpoint);
        return response.IsSuccessStatusCode;
    }
}
