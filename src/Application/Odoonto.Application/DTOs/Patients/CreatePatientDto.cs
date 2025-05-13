using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Odoonto.Application.DTOs.Patients
{
    /// <summary>
    /// DTO para crear un nuevo paciente
    /// </summary>
    public class CreatePatientDto
    {
        /// <summary>
        /// Nombre del paciente (requerido)
        /// </summary>
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres")]
        public string FirstName { get; set; }

        /// <summary>
        /// Apellidos del paciente (requerido)
        /// </summary>
        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Los apellidos deben tener entre 2 y 50 caracteres")]
        public string LastName { get; set; }

        /// <summary>
        /// Fecha de nacimiento (requerida)
        /// </summary>
        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Género del paciente (0=No especificado, 1=Masculino, 2=Femenino, 3=Otro)
        /// </summary>
        [Range(0, 3, ErrorMessage = "El género debe ser un valor entre 0 y 3")]
        public int Gender { get; set; }

        /// <summary>
        /// Dirección postal (opcional)
        /// </summary>
        [StringLength(200, ErrorMessage = "La dirección no puede exceder los 200 caracteres")]
        public string Address { get; set; }

        /// <summary>
        /// Número de teléfono (opcional)
        /// </summary>
        [Phone(ErrorMessage = "El formato del número de teléfono no es válido")]
        [StringLength(20, ErrorMessage = "El teléfono no puede exceder los 20 caracteres")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Correo electrónico (opcional)
        /// </summary>
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
        [StringLength(100, ErrorMessage = "El email no puede exceder los 100 caracteres")]
        public string Email { get; set; }

        /// <summary>
        /// Historial médico (opcional)
        /// </summary>
        [StringLength(2000, ErrorMessage = "El historial médico no puede exceder los 2000 caracteres")]
        public string MedicalHistory { get; set; }

        /// <summary>
        /// Lista de alergias (opcional)
        /// </summary>
        public List<string> Allergies { get; set; } = new List<string>();

        /// <summary>
        /// Notas adicionales (opcional)
        /// </summary>
        [StringLength(1000, ErrorMessage = "Las notas no pueden exceder los 1000 caracteres")]
        public string Notes { get; set; }
    }
} 