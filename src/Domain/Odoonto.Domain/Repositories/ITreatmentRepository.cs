using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Odoonto.Domain.Core.Repositories;
using Odoonto.Domain.Models.Treatments;
using Odoonto.Domain.Models.ValueObjects;

namespace Odoonto.Domain.Repositories
{
    /// <summary>
    /// Interfaz que define las operaciones de repositorio para la entidad Treatment
    /// </summary>
    public interface ITreatmentRepository : IRepository<Treatment>
    {
        /// <summary>
        /// Busca tratamientos por categoría
        /// </summary>
        /// <param name="category">Categoría a buscar</param>
        /// <returns>Lista de tratamientos en la categoría especificada</returns>
        Task<IEnumerable<Treatment>> GetByCategoryAsync(string category);
        
        /// <summary>
        /// Busca tratamientos con precio menor o igual al especificado
        /// </summary>
        /// <param name="maxPrice">Precio máximo a buscar</param>
        /// <param name="currency">Moneda (por defecto EUR)</param>
        /// <returns>Lista de tratamientos con precio menor o igual al especificado</returns>
        Task<IEnumerable<Treatment>> GetByMaxPriceAsync(decimal maxPrice, string currency = "EUR");
        
        /// <summary>
        /// Busca tratamientos por duración máxima en minutos
        /// </summary>
        /// <param name="maxDurationMinutes">Duración máxima en minutos</param>
        /// <returns>Lista de tratamientos con duración menor o igual a la especificada</returns>
        Task<IEnumerable<Treatment>> GetByMaxDurationAsync(int maxDurationMinutes);
        
        /// <summary>
        /// Busca tratamientos por nombre o descripción
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda</param>
        /// <returns>Lista de tratamientos que coinciden con el término de búsqueda</returns>
        Task<IEnumerable<Treatment>> SearchByNameOrDescriptionAsync(string searchTerm);
    }
} 