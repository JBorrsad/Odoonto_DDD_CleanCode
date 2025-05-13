using System;
using System.ComponentModel.DataAnnotations;

namespace Odoonto.Application.DTOs.Treatments
{
    /// <summary>
    /// DTO para la creación o actualización de un tratamiento
    /// </summary>
    public class CreateTreatmentDto
    {
        [Required(ErrorMessage = "El nombre del tratamiento es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres")]
        public string Description { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0, 99999.99, ErrorMessage = "El precio debe estar entre 0 y 99999.99")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "La moneda es obligatoria")]
        [StringLength(3, ErrorMessage = "La moneda debe ser un código de 3 caracteres")]
        public string Currency { get; set; } = "EUR";

        [Required(ErrorMessage = "La duración estimada es obligatoria")]
        [Range(1, 480, ErrorMessage = "La duración debe estar entre 1 y 480 minutos")]
        public int DurationMinutes { get; set; }

        [StringLength(50, ErrorMessage = "La categoría no puede superar los 50 caracteres")]
        public string Category { get; set; }
    }
} 