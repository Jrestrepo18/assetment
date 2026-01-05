using OnlineCoursesPlatform.Application.DTOs;

namespace OnlineCoursesPlatform.Application.Interfaces;

/// <summary>
/// Interfaz para el servicio de autenticación.
/// </summary>
public interface IAuthService
{
    Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    
    Task<AuthResponseDto?> RegisterAsync(RegisterDto dto);
    
    Task<AuthResponseDto?> RefreshTokenAsync(string refreshToken);
    
    Task<bool> RevokeTokenAsync(string token);
    
    Task<UserDto?> GetCurrentUserAsync(Guid userId);
    
    Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
}
