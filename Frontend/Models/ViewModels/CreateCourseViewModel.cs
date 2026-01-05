using System.ComponentModel.DataAnnotations;

namespace OnlineCoursesPlatform.Web.Models.ViewModels;

/// <summary>
/// ViewModel para crear un nuevo curso.
/// </summary>
public class CreateCourseViewModel
{
    [Required(ErrorMessage = "El título es requerido")]
    [StringLength(200, ErrorMessage = "El título no puede exceder 200 caracteres")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "La descripción es requerida")]
    [StringLength(2000, ErrorMessage = "La descripción no puede exceder 2000 caracteres")]
    public string Description { get; set; } = string.Empty;

    [Url(ErrorMessage = "Debe ser una URL válida")]
    public string? ImageUrl { get; set; }

    [Required(ErrorMessage = "El precio es requerido")]
    [Range(0, 9999.99, ErrorMessage = "El precio debe estar entre 0 y 9999.99")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "La duración es requerida")]
    [Range(1, 1000, ErrorMessage = "La duración debe estar entre 1 y 1000 horas")]
    public int DurationInHours { get; set; }
}
