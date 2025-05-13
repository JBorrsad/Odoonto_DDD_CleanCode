using System;
using System.Collections.Generic;

namespace Odoonto.Application.DTOs.Patients
{
    /// <summary>
    /// DTO para mostrar información de un paciente
    /// </summary>
    public class PatientDto
    {
        /// <summary>
        /// Identificador único del paciente
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nombre del paciente
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Apellidos del paciente
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Nombre completo (Nombre + Apellidos)
        /// </summary>
        public string FullName => $"{FirstName} {LastName}";

        /// <summary>
        /// Fecha de nacimiento
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Edad calculada
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Género del paciente
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Dirección postal
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Número de teléfono
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Correo electrónico
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Historial médico
        /// </summary>
        public string MedicalHistory { get; set; }

        /// <summary>
        /// Alergias del paciente
        /// </summary>
        public List<string> Allergies { get; set; } = new List<string>();

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