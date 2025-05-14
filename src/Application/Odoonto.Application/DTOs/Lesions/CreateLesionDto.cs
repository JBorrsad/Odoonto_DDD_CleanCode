using System.ComponentModel.DataAnnotations;

namespace Odoonto.Application.DTOs.Lesions
{
    /// <summary>
    /// DTO para la creación de una nueva lesión
    /// </summary>
    public class CreateLesionDto
    {
        /// <summary>
        /// Nombre de la lesión
        /// </summary>
        [Required(ErrorMessage = "El nombre de la lesión es obligatorio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
        public string Name { get; set; }

        /// <summary>
        /// Descripción detallada de la lesión
        /// </summary>
        [StringLength(1000, ErrorMessage = "La descripción no puede exceder los 1000 caracteres")]
        public string Description { get; set; }

        /// <summary>
        /// Categoría a la que pertenece la lesión
        /// </summary>
        [StringLength(50, ErrorMessage = "La categoría no puede exceder los 50 caracteres")]
        public string Category { get; set; }
    }
}