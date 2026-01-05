using OnlineCoursesPlatform.Application.DTOs;
using OnlineCoursesPlatform.Application.Interfaces;
using OnlineCoursesPlatform.Domain.Entities;
using OnlineCoursesPlatform.Domain.Interfaces;

namespace OnlineCoursesPlatform.Application.Services;

/// <summary>
/// Implementación del servicio de autenticación.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IRepository<User> _userRepository;
    // TODO: Inyectar ITokenService y IPasswordHasher desde Infrastructure

    public AuthService(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null) return null;

        // TODO: Verificar contraseña con IPasswordHasher
        // TODO: Generar token con ITokenService

        return new AuthResponseDto
        {
            Token = "generated-jwt-token",
            RefreshToken = Guid.NewGuid().ToString(),
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            User = MapToUserDto(user)
        };
    }

    public async Task<AuthResponseDto?> RegisterAsync(RegisterDto dto)
    {
        // Verificar si el email ya existe
        var existingUser = await _userRepository.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (existingUser != null) return null;

        // TODO: Hash de contraseña con IPasswordHasher
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            PasswordHash = "hashed-password", // TODO: Usar IPasswordHasher
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Role = "Student",
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return new AuthResponseDto
        {
            Token = "generated-jwt-token",
            RefreshToken = Guid.NewGuid().ToString(),
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            User = MapToUserDto(user)
        };
    }

    public async Task<AuthResponseDto?> RefreshTokenAsync(string refreshToken)
    {
        // TODO: Implementar lógica de refresh token
        await Task.CompletedTask;
        return null;
    }

    public async Task<bool> RevokeTokenAsync(string token)
    {
        // TODO: Implementar revocación de token
        await Task.CompletedTask;
        return true;
    }

    public async Task<UserDto?> GetCurrentUserAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        return user == null ? null : MapToUserDto(user);
    }

    public async Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return false;

        // TODO: Verificar contraseña actual y actualizar con nueva
        user.PasswordHash = "new-hashed-password"; // TODO: Usar IPasswordHasher
        user.UpdatedAt = DateTime.UtcNow;

        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();

        return true;
    }

    private static UserDto MapToUserDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            FullName = user.FullName,
            Role = user.Role,
            ProfileImageUrl = user.ProfileImageUrl
        };
    }
}
