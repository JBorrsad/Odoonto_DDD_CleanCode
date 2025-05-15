using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Odoonto.Application.DTOs.Patients;

namespace Odoonto.Application.Services.Patients
{
    /// <summary>
    /// Interfaz para el servicio de aplicación de pacientes
    /// </summary>
    public interface IPatientAppService
    {
        /// <summary>
        /// Obtiene todos los pacientes
        /// </summary>
        Task<IEnumerable<PatientDto>> GetAllAsync();

        /// <summary>
        /// Obtiene un paciente por su ID
        /// </summary>
        Task<PatientDto> GetByIdAsync(Guid id);

        /// <summary>
        /// Crea un nuevo paciente
        /// </summary>
        Task<Guid> CreateAsync(CreatePatientDto patientDto);

        /// <summary>
        /// Actualiza un paciente existente
        /// </summary>
        Task UpdateAsync(Guid id, CreatePatientDto patientDto);

        /// <summary>
        /// Elimina un paciente
        /// </summary>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Busca pacientes por término de búsqueda genérico
        /// </summary>
        Task<IEnumerable<PatientDto>> SearchAsync(string searchTerm);

        /// <summary>
        /// Actualiza el historial médico de un paciente
        /// </summary>
        Task<PatientDto> UpdateMedicalHistoryAsync(Guid id, string medicalHistory);

        /// <summary>
        /// Añade una alergia al paciente
        /// </summary>
        Task<PatientDto> AddAllergyAsync(Guid id, string allergy);

        /// <summary>
        /// Elimina una alergia del paciente
        /// </summary>
        Task<PatientDto> RemoveAllergyAsync(Guid id, string allergy);
    }
} 