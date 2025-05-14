using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Odoonto.Application.DTOs.Odontograms
{
    /// <summary>
    /// DTO para crear un procedimiento realizado
    /// </summary>
    public class CreatePerformedProcedureDto
    {
        /// <summary>
        /// Identificador del tratamiento realizado
        /// </summary>
        [Required(ErrorMessage = "El identificador del tratamiento es obligatorio")]
        public Guid TreatmentId { get; set; }

        /// <summary>
        /// Superficies tratadas del diente (O, M, D, V, P/L)
        /// </summary>
        [Required(ErrorMessage = "Se debe especificar al menos una superficie tratada")]
        public List<string> TreatedSurfaces { get; set; } = new List<string>();

        /// <summary>
        /// Fecha de realización del procedimiento
        /// </summary>
        [Required(ErrorMessage = "La fecha de realización es obligatoria")]
        public DateTime CompletionDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Notas sobre el procedimiento (opcional)
        /// </summary>
        [StringLength(500, ErrorMessage = "Las notas no pueden exceder los 500 caracteres")]
        public string Notes { get; set; }
    }
}