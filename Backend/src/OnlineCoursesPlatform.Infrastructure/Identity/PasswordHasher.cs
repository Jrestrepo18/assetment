namespace OnlineCoursesPlatform.Infrastructure.Identity;

/// <summary>
/// Servicio para hash y verificación de contraseñas usando BCrypt.
/// </summary>
public class PasswordHasher
{
    private const int WorkFactor = 12;

    /// <summary>
    /// Genera un hash de la contraseña proporcionada.
    /// </summary>
    /// <param name="password">Contraseña en texto plano</param>
    /// <returns>Hash de la contraseña</returns>
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
    }

    /// <summary>
    /// Verifica si una contraseña coincide con su hash.
    /// </summary>
    /// <param name="password">Contraseña en texto plano</param>
    /// <param name="passwordHash">Hash almacenado</param>
    /// <returns>True si coinciden, false en caso contrario</returns>
    public bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}
