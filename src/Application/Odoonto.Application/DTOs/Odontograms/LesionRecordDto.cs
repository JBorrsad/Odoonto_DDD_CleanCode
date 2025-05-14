using System;
using System.Collections.Generic;

namespace Odoonto.Application.DTOs.Odontograms
{
    /// <summary>
    /// DTO para representar un registro de lesión
    /// </summary>
    public class LesionRecordDto
    {
        /// <summary>
        /// Identificador de la lesión
        /// </summary>
        public Guid LesionId { get; set; }

        /// <summary>
        /// Nombre de la lesión (para visualización)
        /// </summary>
        public string LesionName { get; set; }

        /// <summary>
        /// Superficies afectadas del diente
        /// </summary>
        public List<string> AffectedSurfaces { get; set; } = new List<string>();

        /// <summary>
        /// Fecha de detección
        /// </summary>
        public DateTime DetectionDate { get; set; }

        /// <summary>
        /// Notas sobre la lesión
        /// </summary>
        public string Notes { get; set; }
    }
}