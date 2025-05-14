using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Odoonto.Application.DTOs.Lesions;

namespace Odoonto.Application.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de gestión de lesiones
    /// </summary>
    public interface ILesionService
    {
        /// <summary>
        /// Obtiene todas las lesiones registradas
        /// </summary>
        Task<IEnumerable<LesionDto>> GetAllLesionsAsync();

        /// <summary>
        /// Obtiene las lesiones activas
        /// </summary>
        Task<IEnumerable<LesionDto>> GetActiveLesionsAsync();

        /// <summary>
        /// Obtiene las lesiones por categoría
        /// </summary>
        Task<IEnumerable<LesionDto>> GetLesionsByCategoryAsync(string category);

        /// <summary>
        /// Obtiene una lesión por su ID
        /// </summary>
        Task<LesionDto> GetLesionByIdAsync(Guid id);

        /// <summary>
        /// Crea una nueva lesión
        /// </summary>
        Task<LesionDto> CreateLesionAsync(CreateLesionDto createLesionDto);

        /// <summary>
        /// Actualiza una lesión existente
        /// </summary>
        Task<LesionDto> UpdateLesionAsync(UpdateLesionDto updateLesionDto);

        /// <summary>
        /// Activa una lesión
        /// </summary>
        Task<LesionDto> ActivateLesionAsync(Guid id);

        /// <summary>
        /// Desactiva una lesión
        /// </summary>
        Task<LesionDto> DeactivateLesionAsync(Guid id);

        /// <summary>
        /// Verifica si una lesión existe por su ID
        /// </summary>
        Task<bool> LesionExistsAsync(Guid id);
    }
}