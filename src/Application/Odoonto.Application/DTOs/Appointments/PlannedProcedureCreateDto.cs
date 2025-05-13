using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Odoonto.Application.DTOs.Appointments
{
    /// <summary>
    /// DTO para la creación o actualización de un procedimiento planificado
    /// </summary>
    public class PlannedProcedureCreateDto
    {
        [Required(ErrorMessage = "El identificador del tratamiento es obligatorio")]
        public Guid TreatmentId { get; set; }

        [Required(ErrorMessage = "Las superficies dentales son obligatorias")]
        public List<ToothSurfaceCreateDto> ToothSurfaces { get; set; } = new List<ToothSurfaceCreateDto>();
    }

    /// <summary>
    /// DTO para la creación o actualización de una superficie dental afectada
    /// </summary>
    public class ToothSurfaceCreateDto
    {
        [Required(ErrorMessage = "El número de diente es obligatorio")]
        [Range(1, 85, ErrorMessage = "El número de diente debe estar entre 1 y 85")]
        public int ToothNumber { get; set; }

        [Required(ErrorMessage = "Las superficies son obligatorias")]
        public List<string> Surfaces { get; set; } = new List<string>();
    }
} 