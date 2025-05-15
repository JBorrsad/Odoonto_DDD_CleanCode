using System;
using System.Collections.Generic;
using Odoonto.Application.DTOs.Common;

namespace Odoonto.Application.DTOs.Patients
{
    /// <summary>
    /// DTO para representar un paciente completo
    /// </summary>
    public class PatientDto
    {
        /// <summary>
        /// Identificador único
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nombre completo
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Fecha de nacimiento
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Edad calculada
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Información de contacto
        /// </summary>
        public ContactInfoDto ContactInfo { get; set; }

        /// <summary>
        /// Historia médica
        /// </summary>
        public string MedicalHistory { get; set; }

        /// <summary>
        /// Próxima cita (si existe)
        /// </summary>
        public DateTime? NextAppointment { get; set; }

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