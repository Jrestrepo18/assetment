using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineCoursesPlatform.Web.Services;

namespace OnlineCoursesPlatform.Web.Pages;

public class LoginModel : PageModel
{
    private readonly AuthApiClient _authApiClient;
    private readonly ILogger<LoginModel> _logger;

    public LoginModel(AuthApiClient authApiClient, ILogger<LoginModel> logger)
    {
        _authApiClient = authApiClient;
        _logger = logger;
    }

    [BindProperty]
    [Required(ErrorMessage = "El correo es requerido")]
    [EmailAddress(ErrorMessage = "Ingrese un correo válido")]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "La contraseña es requerida")]
    public string Password { get; set; } = string.Empty;

    public string? ErrorMessage { get; set; }
    public string? ReturnUrl { get; set; }

    public void OnGet(string? returnUrl = null)
    {
        ReturnUrl = returnUrl ?? Url.Content("~/");
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/Courses");

        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var result = await _authApiClient.LoginAsync(new LoginRequest
            {
                Email = Email,
                Password = Password
            });

            if (result != null)
            {
                _logger.LogInformation("User {Email} logged in successfully", Email);
                return LocalRedirect(returnUrl);
            }

            ErrorMessage = "Credenciales inválidas. Por favor, verifique su correo y contraseña.";
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for user {Email}", Email);
            ErrorMessage = "Ocurrió un error al intentar iniciar sesión. Intente nuevamente.";
            return Page();
        }
    }
}
