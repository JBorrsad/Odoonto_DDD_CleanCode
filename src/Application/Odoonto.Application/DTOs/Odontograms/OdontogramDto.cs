using System;
using System.Collections.Generic;

namespace Odoonto.Application.DTOs.Odontograms
{
    /// <summary>
    /// DTO para representar un odontograma
    /// </summary>
    public class OdontogramDto
    {
        /// <summary>
        /// Identificador único del odontograma
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Identificador del paciente al que pertenece
        /// </summary>
        public Guid PatientId { get; set; }

        /// <summary>
        /// Registros dentales del odontograma
        /// </summary>
        public List<ToothRecordDto> ToothRecords { get; set; } = new List<ToothRecordDto>();

        /// <summary>
        /// Fecha de creación
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Fecha de última actualización
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}