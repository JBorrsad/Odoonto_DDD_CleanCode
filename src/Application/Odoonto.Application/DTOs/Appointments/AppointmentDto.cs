using System;
using System.Collections.Generic;

namespace Odoonto.Application.DTOs.Appointments
{
    /// <summary>
    /// DTO para transferir datos de una cita odontológica
    /// </summary>
    public class AppointmentDto
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public string PatientName { get; set; }
        public Guid DoctorId { get; set; }
        public string DoctorName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public List<PlannedProcedureDto> Procedures { get; set; }
    }
} 