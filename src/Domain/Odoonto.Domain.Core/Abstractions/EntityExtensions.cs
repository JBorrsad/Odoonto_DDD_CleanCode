using System;

namespace Odoonto.Domain.Core.Abstractions
{
    /// <summary>
    /// Métodos de extensión para las entidades de dominio
    /// </summary>
    public static class EntityExtensions
    {
        /// <summary>
        /// Actualiza la fecha de edición de una entidad al momento actual
        /// </summary>
        /// <param name="entity">Entidad a actualizar</param>
        public static void UpdateEditDate(this Entity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
                
            entity.UpdatedAt = DateTime.UtcNow;
        }
    }
} 