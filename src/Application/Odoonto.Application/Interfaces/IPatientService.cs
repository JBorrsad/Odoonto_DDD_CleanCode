using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Odoonto.Application.DTOs.Patients;

namespace Odoonto.Application.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de pacientes
    /// </summary>
    public interface IPatientService
    {
        /// <summary>
        /// Obtiene todos los pacientes
        /// </summary>
        Task<IEnumerable<PatientDto>> GetAllPatientsAsync();

        /// <summary>
        /// Obtiene un paciente por su ID
        /// </summary>
        Task<PatientDto> GetPatientByIdAsync(Guid id);

        /// <summary>
        /// Crea un nuevo paciente
        /// </summary>
        Task<PatientDto> CreatePatientAsync(CreatePatientDto createPatientDto);

        /// <summary>
        /// Actualiza un paciente existente
        /// </summary>
        Task<PatientDto> UpdatePatientAsync(Guid id, CreatePatientDto updatePatientDto);

        /// <summary>
        /// Elimina un paciente
        /// </summary>
        Task<bool> DeletePatientAsync(Guid id);

        /// <summary>
        /// Busca pacientes por nombre
        /// </summary>
        Task<IEnumerable<PatientDto>> SearchPatientsByNameAsync(string name);

        /// <summary>
        /// Busca pacientes por término de búsqueda genérico
        /// </summary>
        Task<IEnumerable<PatientDto>> SearchPatientsAsync(string searchTerm);

        /// <summary>
        /// Obtiene el conteo total de pacientes
        /// </summary>
        Task<int> GetTotalPatientsCountAsync();

        /// <summary>
        /// Obtiene pacientes con paginación
        /// </summary>
        Task<IEnumerable<PatientDto>> GetPaginatedPatientsAsync(int pageNumber, int pageSize);
    }
} 