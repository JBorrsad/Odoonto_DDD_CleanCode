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
        /// Nombre (obligatorio)
        /// </summary>
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres")]
        public string FirstNames { get; set; }
        
        /// <summary>
        /// Apellidos (obligatorio)
        /// </summary>
        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [StringLength(50, ErrorMessage = "Los apellidos no pueden exceder los 50 caracteres")]
        public string LastNames { get; set; }
        
        /// <summary>
        /// Especialidad médica (obligatorio)
        /// </summary>
        [Required(ErrorMessage = "La especialidad es obligatoria")]
        [StringLength(100, ErrorMessage = "La especialidad no puede exceder los 100 caracteres")]
        public string Specialty { get; set; }
        
        /// <summary>
        /// Teléfono (obligatorio)
        /// </summary>
        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        public string PhoneNumber { get; set; }
        
        /// <summary>
        /// Correo electrónico (obligatorio)
        /// </summary>
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string Email { get; set; }
        
        /// <summary>
        /// Dirección postal
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Número de licencia profesional (requerido)
        /// </summary>
        [Required(ErrorMessage = "El número de licencia es obligatorio")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "El número de licencia debe tener entre 3 y 20 caracteres")]
        public string LicenseNumber { get; set; }

        /// <summary>
        /// Número de teléfono (opcional)
        /// </summary>
        [Phone(ErrorMessage = "El formato del número de teléfono no es válido")]
        [StringLength(20, ErrorMessage = "El teléfono no puede exceder los 20 caracteres")]
        public string AdditionalPhoneNumber { get; set; }

        /// <summary>
        /// Correo electrónico (opcional)
        /// </summary>
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
        [StringLength(100, ErrorMessage = "El email no puede exceder los 100 caracteres")]
        public string AdditionalEmail { get; set; }

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