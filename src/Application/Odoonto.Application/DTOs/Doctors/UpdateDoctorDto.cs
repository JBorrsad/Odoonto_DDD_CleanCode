using System.ComponentModel.DataAnnotations;

namespace Odoonto.Application.DTOs.Doctors
{
    /// <summary>
    /// DTO para actualizar un doctor existente
    /// </summary>
    public class UpdateDoctorDto
    {
        /// <summary>
        /// Nombre
        /// </summary>
        [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres")]
        public string FirstNames { get; set; }
        
        /// <summary>
        /// Apellidos
        /// </summary>
        [StringLength(50, ErrorMessage = "Los apellidos no pueden exceder los 50 caracteres")]
        public string LastNames { get; set; }
        
        /// <summary>
        /// Especialidad médica
        /// </summary>
        [StringLength(100, ErrorMessage = "La especialidad no puede exceder los 100 caracteres")]
        public string Specialty { get; set; }
        
        /// <summary>
        /// Teléfono
        /// </summary>
        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        public string PhoneNumber { get; set; }
        
        /// <summary>
        /// Correo electrónico
        /// </summary>
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string Email { get; set; }
        
        /// <summary>
        /// Dirección postal
        /// </summary>
        public string Address { get; set; }
    }
} 