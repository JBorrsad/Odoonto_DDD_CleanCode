using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Odoonto.Application.DTOs.Treatments;

namespace Odoonto.Application.Interfaces
{
    /// <summary>
    /// Interfaz que define las operaciones del servicio de aplicación para tratamientos
    /// </summary>
    public interface ITreatmentService
    {
        /// <summary>
        /// Crea un nuevo tratamiento
        /// </summary>
        /// <param name="treatmentDto">DTO con la información del tratamiento</param>
        /// <returns>DTO con la información del tratamiento creado</returns>
        Task<TreatmentDto> CreateTreatmentAsync(CreateTreatmentDto treatmentDto);

        /// <summary>
        /// Actualiza un tratamiento existente
        /// </summary>
        /// <param name="id">Identificador del tratamiento</param>
        /// <param name="treatmentDto">DTO con la información actualizada</param>
        /// <returns>DTO con la información del tratamiento actualizado</returns>
        Task<TreatmentDto> UpdateTreatmentAsync(Guid id, CreateTreatmentDto treatmentDto);

        /// <summary>
        /// Obtiene un tratamiento por su identificador
        /// </summary>
        /// <param name="id">Identificador del tratamiento</param>
        /// <returns>DTO con la información del tratamiento</returns>
        Task<TreatmentDto> GetTreatmentByIdAsync(Guid id);

        /// <summary>
        /// Obtiene todos los tratamientos
        /// </summary>
        /// <returns>Lista de DTOs con la información de los tratamientos</returns>
        Task<IEnumerable<TreatmentDto>> GetAllTreatmentsAsync();

        /// <summary>
        /// Elimina un tratamiento
        /// </summary>
        /// <param name="id">Identificador del tratamiento</param>
        /// <returns>True si la operación fue exitosa</returns>
        Task<bool> DeleteTreatmentAsync(Guid id);

        /// <summary>
        /// Busca tratamientos por categoría
        /// </summary>
        /// <param name="category">Categoría a buscar</param>
        /// <returns>Lista de DTOs con la información de los tratamientos</returns>
        Task<IEnumerable<TreatmentDto>> GetTreatmentsByCategoryAsync(string category);

        /// <summary>
        /// Busca tratamientos por precio máximo
        /// </summary>
        /// <param name="maxPrice">Precio máximo a buscar</param>
        /// <param name="currency">Moneda (por defecto EUR)</param>
        /// <returns>Lista de DTOs con la información de los tratamientos</returns>
        Task<IEnumerable<TreatmentDto>> GetTreatmentsByMaxPriceAsync(decimal maxPrice, string currency = "EUR");

        /// <summary>
        /// Busca tratamientos por duración máxima en minutos
        /// </summary>
        /// <param name="maxDurationMinutes">Duración máxima en minutos</param>
        /// <returns>Lista de DTOs con la información de los tratamientos</returns>
        Task<IEnumerable<TreatmentDto>> GetTreatmentsByMaxDurationAsync(int maxDurationMinutes);

        /// <summary>
        /// Busca tratamientos por nombre o descripción
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda</param>
        /// <returns>Lista de DTOs con la información de los tratamientos</returns>
        Task<IEnumerable<TreatmentDto>> SearchTreatmentsAsync(string searchTerm);
    }
} 