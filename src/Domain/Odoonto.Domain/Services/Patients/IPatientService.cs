using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Odoonto.Domain.Models.Patients;
using Odoonto.Domain.Models.ValueObjects;

namespace Odoonto.Domain.Services.Patients
{
    /// <summary>
    /// Servicio de dominio para operaciones básicas con pacientes
    /// </summary>
    public interface IPatientService
    {
        /// <summary>
        /// Obtiene todos los pacientes
        /// </summary>
        Task<IEnumerable<Patient>> GetAllPatientsAsync();

        /// <summary>
        /// Obtiene un paciente por su ID
        /// </summary>
        Task<Patient> GetPatientByIdAsync(Guid id);

        /// <summary>
        /// Crea un nuevo paciente
        /// </summary>
        Task<Guid> CreatePatientAsync(string firstName, string lastName, DateTime dateOfBirth,
            Gender gender, string email, string phoneNumber, string address);

        /// <summary>
        /// Actualiza la información básica de un paciente
        /// </summary>
        Task UpdatePatientInfoAsync(Guid id, string firstName, string lastName, DateTime dateOfBirth,
            Gender gender, string email, string phoneNumber, string address);

        /// <summary>
        /// Actualiza el historial médico de un paciente
        /// </summary>
        Task UpdateMedicalHistoryAsync(Guid id, string medicalHistory);

        /// <summary>
        /// Agrega una alergia al paciente
        /// </summary>
        Task AddAllergyAsync(Guid id, string allergy);

        /// <summary>
        /// Elimina una alergia del paciente
        /// </summary>
        Task RemoveAllergyAsync(Guid id, string allergy);

        /// <summary>
        /// Actualiza las notas de un paciente
        /// </summary>
        Task UpdateNotesAsync(Guid id, string notes);

        /// <summary>
        /// Elimina un paciente
        /// </summary>
        Task<bool> DeletePatientAsync(Guid id);
    }
}