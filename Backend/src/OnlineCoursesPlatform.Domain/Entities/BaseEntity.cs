namespace OnlineCoursesPlatform.Domain.Entities;

/// <summary>
/// Clase base abstracta para todas las entidades del dominio.
/// Proporciona campos comunes de auditoría y soft delete.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Identificador único de la entidad.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Indica si el registro ha sido eliminado (soft delete).
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Fecha y hora de creación del registro.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Fecha y hora de la última actualización del registro.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
