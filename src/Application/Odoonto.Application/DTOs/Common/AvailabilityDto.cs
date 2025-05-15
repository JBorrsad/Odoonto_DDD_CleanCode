using System;

namespace Odoonto.Application.DTOs.Common
{
    /// <summary>
    /// DTO para disponibilidad horaria
    /// </summary>
    public class AvailabilityDto
    {
        /// <summary>
        /// Día de la semana (0=Domingo, 1=Lunes, ..., 6=Sábado)
        /// </summary>
        public DayOfWeek DayOfWeek { get; set; }
        
        /// <summary>
        /// Hora de inicio (formato 24h)
        /// </summary>
        public TimeOnly StartTime { get; set; }
        
        /// <summary>
        /// Hora de fin (formato 24h)
        /// </summary>
        public TimeOnly EndTime { get; set; }
    }
} 