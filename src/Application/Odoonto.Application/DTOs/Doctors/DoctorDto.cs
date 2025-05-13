using System;
using System.Collections.Generic;

namespace Odoonto.Application.DTOs.Doctors
{
    /// <summary>
    /// DTO para mostrar información de un doctor
    /// </summary>
    public class DoctorDto
    {
        /// <summary>
        /// Identificador único del doctor
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nombre del doctor
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Apellidos del doctor
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Nombre completo (Nombre + Apellidos)
        /// </summary>
        public string FullName => $"{FirstName} {LastName}";

        /// <summary>
        /// Número de licencia profesional
        /// </summary>
        public string LicenseNumber { get; set; }

        /// <summary>
        /// Especialidad del doctor
        /// </summary>
        public string Specialty { get; set; }

        /// <summary>
        /// Número de teléfono
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Correo electrónico
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Días y horas disponibles para citas
        /// </summary>
        public List<ScheduleDto> Schedule { get; set; } = new List<ScheduleDto>();

        /// <summary>
        /// Notas adicionales
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Fecha de creación del registro
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Fecha de última actualización
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
} 