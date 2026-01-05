using Microsoft.AspNetCore.Mvc;
using OnlineCoursesPlatform.Application.DTOs;
using OnlineCoursesPlatform.Application.Interfaces;

namespace OnlineCoursesPlatform.API.Controllers;

/// <summary>
/// Controlador para autenticación de usuarios.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Inicia sesión con email y contraseña.
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        
        if (result == null)
        {
            _logger.LogWarning("Failed login attempt for email: {Email}", dto.Email);
            return Unauthorized(new { message = "Credenciales inválidas." });
        }

        _logger.LogInformation("User logged in: {Email}", dto.Email);
        return Ok(result);
    }

    /// <summary>
    /// Registra un nuevo usuario.
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (dto.Password != dto.ConfirmPassword)
        {
            return BadRequest(new { message = "Las contraseñas no coinciden." });
        }

        var result = await _authService.RegisterAsync(dto);
        
        if (result == null)
        {
            return BadRequest(new { message = "El email ya está registrado." });
        }

        _logger.LogInformation("New user registered: {Email}", dto.Email);
        return CreatedAtAction(nameof(Login), result);
    }

    /// <summary>
    /// Renueva el token de acceso usando el refresh token.
    /// </summary>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var result = await _authService.RefreshTokenAsync(request.RefreshToken);
        
        if (result == null)
        {
            return Unauthorized(new { message = "Refresh token inválido o expirado." });
        }

        return Ok(result);
    }
}

public class RefreshTokenRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}
