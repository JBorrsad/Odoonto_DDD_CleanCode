using System;
using System.Collections.Generic;
using Odoonto.Application.DTOs.Common;

namespace Odoonto.Application.DTOs.Doctors
{
    /// <summary>
    /// DTO para mostrar información completa de un doctor
    /// </summary>
    public class DoctorDto
    {
        /// <summary>
        /// Identificador único
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Nombre completo del doctor
        /// </summary>
        public string FullName { get; set; }
        
        /// <summary>
        /// Especialidad médica
        /// </summary>
        public string Specialty { get; set; }
        
        /// <summary>
        /// Información de contacto
        /// </summary>
        public ContactInfoDto ContactInfo { get; set; }
        
        /// <summary>
        /// Disponibilidad semanal
        /// </summary>
        public IEnumerable<AvailabilityDto> Availability { get; set; }
        
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