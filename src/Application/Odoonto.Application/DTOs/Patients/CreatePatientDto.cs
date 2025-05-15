using System;
using System.ComponentModel.DataAnnotations;

namespace Odoonto.Application.DTOs.Patients
{
    /// <summary>
    /// DTO para crear un nuevo paciente
    /// </summary>
    public class CreatePatientDto
    {
        /// <summary>
        /// Nombre completo
        /// </summary>
        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre completo no puede exceder los 100 caracteres")]
        public string FullName { get; set; }

        /// <summary>
        /// Fecha de nacimiento
        /// </summary>
        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Correo electrónico
        /// </summary>
        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
        [StringLength(100, ErrorMessage = "El correo electrónico no puede exceder los 100 caracteres")]
        public string Email { get; set; }

        /// <summary>
        /// Número de teléfono
        /// </summary>
        [Required(ErrorMessage = "El número de teléfono es obligatorio")]
        [Phone(ErrorMessage = "El formato del número de teléfono no es válido")]
        [StringLength(20, ErrorMessage = "El número de teléfono no puede exceder los 20 caracteres")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Dirección
        /// </summary>
        [StringLength(200, ErrorMessage = "La dirección no puede exceder los 200 caracteres")]
        public string Address { get; set; }

        /// <summary>
        /// Historia médica
        /// </summary>
        [StringLength(2000, ErrorMessage = "La historia médica no puede exceder los 2000 caracteres")]
        public string MedicalHistory { get; set; }
    }
}