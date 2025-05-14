using System;

namespace Odoonto.Application.DTOs.Lesions
{
    /// <summary>
    /// DTO para transferir información de lesiones
    /// </summary>
    public class LesionDto
    {
        /// <summary>
        /// Identificador único de la lesión
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nombre de la lesión
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Descripción detallada de la lesión
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Categoría a la que pertenece la lesión
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Indica si la lesión está activa en el catálogo
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Fecha de última modificación
        /// </summary>
        public DateTime LastModified { get; set; }
    }
}