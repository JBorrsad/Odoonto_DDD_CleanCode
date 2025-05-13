using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Odoonto.Application.DTOs.Doctors;

namespace Odoonto.Application.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de doctores
    /// </summary>
    public interface IDoctorService
    {
        /// <summary>
        /// Obtiene todos los doctores
        /// </summary>
        Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync();

        /// <summary>
        /// Obtiene un doctor por su ID
        /// </summary>
        Task<DoctorDto> GetDoctorByIdAsync(Guid id);

        /// <summary>
        /// Crea un nuevo doctor
        /// </summary>
        Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto createDoctorDto);

        /// <summary>
        /// Actualiza un doctor existente
        /// </summary>
        Task<DoctorDto> UpdateDoctorAsync(Guid id, CreateDoctorDto updateDoctorDto);

        /// <summary>
        /// Elimina un doctor
        /// </summary>
        Task<bool> DeleteDoctorAsync(Guid id);

        /// <summary>
        /// Busca doctores por nombre
        /// </summary>
        Task<IEnumerable<DoctorDto>> SearchDoctorsByNameAsync(string name);

        /// <summary>
        /// Busca doctores por especialidad
        /// </summary>
        Task<IEnumerable<DoctorDto>> FindDoctorsBySpecialtyAsync(string specialty);

        /// <summary>
        /// Busca doctores por término de búsqueda genérico
        /// </summary>
        Task<IEnumerable<DoctorDto>> SearchDoctorsAsync(string searchTerm);

        /// <summary>
        /// Obtiene el conteo total de doctores
        /// </summary>
        Task<int> GetTotalDoctorsCountAsync();

        /// <summary>
        /// Obtiene doctores con paginación
        /// </summary>
        Task<IEnumerable<DoctorDto>> GetPaginatedDoctorsAsync(int pageNumber, int pageSize);
    }
} 