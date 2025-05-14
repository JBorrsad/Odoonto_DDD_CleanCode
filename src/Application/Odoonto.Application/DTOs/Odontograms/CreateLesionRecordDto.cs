using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Odoonto.Application.DTOs.Odontograms
{
    /// <summary>
    /// DTO para crear un registro de lesión
    /// </summary>
    public class CreateLesionRecordDto
    {
        /// <summary>
        /// Identificador de la lesión
        /// </summary>
        [Required(ErrorMessage = "El identificador de la lesión es obligatorio")]
        public Guid LesionId { get; set; }

        /// <summary>
        /// Superficies afectadas del diente (O, M, D, V, P/L)
        /// </summary>
        [Required(ErrorMessage = "Se debe especificar al menos una superficie afectada")]
        public List<string> AffectedSurfaces { get; set; } = new List<string>();

        /// <summary>
        /// Notas sobre la lesión (opcional)
        /// </summary>
        [StringLength(500, ErrorMessage = "Las notas no pueden exceder los 500 caracteres")]
        public string Notes { get; set; }
    }
}