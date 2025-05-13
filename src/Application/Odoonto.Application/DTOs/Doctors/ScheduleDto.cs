using System;

namespace Odoonto.Application.DTOs.Doctors
{
    /// <summary>
    /// DTO para representar un horario disponible de un doctor
    /// </summary>
    public class ScheduleDto
    {
        /// <summary>
        /// Día de la semana (0: Domingo, 1: Lunes, ..., 6: Sábado)
        /// </summary>
        public int DayOfWeek { get; set; }

        /// <summary>
        /// Nombre del día de la semana
        /// </summary>
        public string DayName => ((DayOfWeek)DayOfWeek).ToString();

        /// <summary>
        /// Hora de inicio en formato HH:mm
        /// </summary>
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// Hora de fin en formato HH:mm
        /// </summary>
        public TimeSpan EndTime { get; set; }

        /// <summary>
        /// Indica si está activo este horario
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
} 