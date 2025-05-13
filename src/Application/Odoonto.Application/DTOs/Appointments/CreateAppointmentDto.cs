using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Odoonto.Application.DTOs.Appointments
{
    /// <summary>
    /// DTO para la creación o actualización de una cita
    /// </summary>
    public class CreateAppointmentDto
    {
        [Required(ErrorMessage = "El identificador del paciente es obligatorio")]
        public Guid PatientId { get; set; }

        [Required(ErrorMessage = "El identificador del doctor es obligatorio")]
        public Guid DoctorId { get; set; }

        [Required(ErrorMessage = "La fecha de la cita es obligatoria")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "La hora de inicio es obligatoria")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "La hora de fin es obligatoria")]
        public TimeSpan EndTime { get; set; }

        public string Notes { get; set; }

        public List<PlannedProcedureCreateDto> Procedures { get; set; } = new List<PlannedProcedureCreateDto>();
    }
} 