using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Odoonto.Application.DTOs.Doctors
{
    /// <summary>
    /// DTO para crear un nuevo doctor
    /// </summary>
    public class CreateDoctorDto
    {
        /// <summary>
        /// Nombre del doctor (requerido)
        /// </summary>
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres")]
        public string FirstName { get; set; }

        /// <summary>
        /// Apellidos del doctor (requerido)
        /// </summary>
        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Los apellidos deben tener entre 2 y 50 caracteres")]
        public string LastName { get; set; }

        /// <summary>
        /// Número de licencia profesional (requerido)
        /// </summary>
        [Required(ErrorMessage = "El número de licencia es obligatorio")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "El número de licencia debe tener entre 3 y 20 caracteres")]
        public string LicenseNumber { get; set; }

        /// <summary>
        /// Especialidad del doctor (requerido)
        /// </summary>
        [Required(ErrorMessage = "La especialidad es obligatoria")]
        [StringLength(50, ErrorMessage = "La especialidad no puede exceder los 50 caracteres")]
        public string Specialty { get; set; }

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
        /// Horarios disponibles para citas
        /// </summary>
        public List<CreateScheduleDto> Schedule { get; set; } = new List<CreateScheduleDto>();

        /// <summary>
        /// Notas adicionales (opcional)
        /// </summary>
        [StringLength(1000, ErrorMessage = "Las notas no pueden exceder los 1000 caracteres")]
        public string Notes { get; set; }
    }
} 