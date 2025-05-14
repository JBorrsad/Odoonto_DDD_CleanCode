using System;
using System.Collections.Generic;

namespace Odoonto.Application.DTOs.Odontograms
{
    /// <summary>
    /// DTO para representar un procedimiento realizado
    /// </summary>
    public class PerformedProcedureDto
    {
        /// <summary>
        /// Identificador del tratamiento
        /// </summary>
        public Guid TreatmentId { get; set; }

        /// <summary>
        /// Nombre del tratamiento (para visualización)
        /// </summary>
        public string TreatmentName { get; set; }

        /// <summary>
        /// Superficies tratadas del diente
        /// </summary>
        public List<string> TreatedSurfaces { get; set; } = new List<string>();

        /// <summary>
        /// Fecha de realización
        /// </summary>
        public DateTime CompletionDate { get; set; }

        /// <summary>
        /// Notas sobre el procedimiento
        /// </summary>
        public string Notes { get; set; }
    }
}