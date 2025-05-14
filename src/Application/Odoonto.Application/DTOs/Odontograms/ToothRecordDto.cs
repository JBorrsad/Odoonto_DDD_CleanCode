using System.Collections.Generic;

namespace Odoonto.Application.DTOs.Odontograms
{
    /// <summary>
    /// DTO para representar un registro dental dentro del odontograma
    /// </summary>
    public class ToothRecordDto
    {
        /// <summary>
        /// Número del diente
        /// </summary>
        public int ToothNumber { get; set; }

        /// <summary>
        /// Lista de lesiones registradas
        /// </summary>
        public List<LesionRecordDto> Lesions { get; set; } = new List<LesionRecordDto>();

        /// <summary>
        /// Lista de procedimientos realizados
        /// </summary>
        public List<PerformedProcedureDto> CompletedProcedures { get; set; } = new List<PerformedProcedureDto>();

        /// <summary>
        /// Indica si el diente tiene lesiones registradas
        /// </summary>
        public bool HasLesions => Lesions.Count > 0;

        /// <summary>
        /// Indica si el diente tiene procedimientos completados
        /// </summary>
        public bool HasCompletedProcedures => CompletedProcedures.Count > 0;
    }
}