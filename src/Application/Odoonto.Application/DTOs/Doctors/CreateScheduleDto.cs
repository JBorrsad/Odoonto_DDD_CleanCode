using System;
using System.ComponentModel.DataAnnotations;

namespace Odoonto.Application.DTOs.Doctors
{
    /// <summary>
    /// DTO para crear un nuevo horario de doctor
    /// </summary>
    public class CreateScheduleDto
    {
        /// <summary>
        /// Día de la semana (0: Domingo, 1: Lunes, ..., 6: Sábado)
        /// </summary>
        [Range(0, 6, ErrorMessage = "El día de la semana debe ser un valor entre 0 y 6")]
        public int DayOfWeek { get; set; }

        /// <summary>
        /// Hora de inicio en formato HH:mm
        /// </summary>
        [Required(ErrorMessage = "La hora de inicio es obligatoria")]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// Hora de fin en formato HH:mm
        /// </summary>
        [Required(ErrorMessage = "La hora de fin es obligatoria")]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        /// <summary>
        /// Indica si está activo este horario
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
} 