using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Odoonto.Domain.Core.Repositories;
using Odoonto.Domain.Models.Lesions;

namespace Odoonto.Domain.Repositories
{
    /// <summary>
    /// Interfaz que define las operaciones de repositorio para la entidad Lesion
    /// </summary>
    public interface ILesionRepository : IRepository<Lesion>
    {
        /// <summary>
        /// Busca lesiones por categoría
        /// </summary>
        /// <param name="category">Categoría a buscar</param>
        /// <returns>Lista de lesiones en la categoría especificada</returns>
        Task<IEnumerable<Lesion>> GetByCategoryAsync(string category);
        
        /// <summary>
        /// Busca lesiones activas
        /// </summary>
        /// <returns>Lista de lesiones activas</returns>
        Task<IEnumerable<Lesion>> GetActiveAsync();
        
        /// <summary>
        /// Busca lesiones inactivas
        /// </summary>
        /// <returns>Lista de lesiones inactivas</returns>
        Task<IEnumerable<Lesion>> GetInactiveAsync();
        
        /// <summary>
        /// Busca lesiones por nombre o descripción
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda</param>
        /// <returns>Lista de lesiones que coinciden con el término de búsqueda</returns>
        Task<IEnumerable<Lesion>> SearchByNameOrDescriptionAsync(string searchTerm);
        
        /// <summary>
        /// Activa una lesión
        /// </summary>
        /// <param name="id">Identificador de la lesión</param>
        /// <returns>True si se activó correctamente, False si no se encontró</returns>
        Task<bool> ActivateAsync(Guid id);
        
        /// <summary>
        /// Desactiva una lesión
        /// </summary>
        /// <param name="id">Identificador de la lesión</param>
        /// <returns>True si se desactivó correctamente, False si no se encontró</returns>
        Task<bool> DeactivateAsync(Guid id);
    }
} 