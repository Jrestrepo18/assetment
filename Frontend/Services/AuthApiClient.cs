namespace OnlineCoursesPlatform.Web.Services;

/// <summary>
/// Cliente API para operaciones de autenticación.
/// </summary>
public class AuthApiClient
{
    private readonly ApiClient _apiClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthApiClient(ApiClient apiClient, IHttpContextAccessor httpContextAccessor)
    {
        _apiClient = apiClient;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var response = await _apiClient.PostAsync<LoginRequest, AuthResponse>("api/auth/login", request);
        
        if (response != null)
        {
            // Guardar token en sesión
            _httpContextAccessor.HttpContext?.Session.SetString("JwtToken", response.Token);
            _httpContextAccessor.HttpContext?.Session.SetString("UserName", response.User.FullName);
            _httpContextAccessor.HttpContext?.Session.SetString("UserRole", response.User.Role);
        }

        return response;
    }

    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
    {
        var response = await _apiClient.PostAsync<RegisterRequest, AuthResponse>("api/auth/register", request);
        
        if (response != null)
        {
            _httpContextAccessor.HttpContext?.Session.SetString("JwtToken", response.Token);
            _httpContextAccessor.HttpContext?.Session.SetString("UserName", response.User.FullName);
            _httpContextAccessor.HttpContext?.Session.SetString("UserRole", response.User.Role);
        }

        return response;
    }

    public void Logout()
    {
        _httpContextAccessor.HttpContext?.Session.Clear();
    }

    public bool IsAuthenticated()
    {
        var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
        return !string.IsNullOrEmpty(token);
    }

    public string? GetCurrentUserName()
    {
        return _httpContextAccessor.HttpContext?.Session.GetString("UserName");
    }

    public string? GetCurrentUserRole()
    {
        return _httpContextAccessor.HttpContext?.Session.GetString("UserRole");
    }
}

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public UserInfo User { get; set; } = null!;
}

public class UserInfo
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
