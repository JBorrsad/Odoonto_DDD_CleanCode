using System.ComponentModel.DataAnnotations;

namespace Odoonto.Application.DTOs.Common
{
    /// <summary>
    /// DTO para información de contacto
    /// </summary>
    public class ContactInfoDto
    {
        /// <summary>
        /// Dirección postal
        /// </summary>
        public string Address { get; set; }
        
        /// <summary>
        /// Número de teléfono
        /// </summary>
        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        public string PhoneNumber { get; set; }
        
        /// <summary>
        /// Correo electrónico
        /// </summary>
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string Email { get; set; }
    }
} 