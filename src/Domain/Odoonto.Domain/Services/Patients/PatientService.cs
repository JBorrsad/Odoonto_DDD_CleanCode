using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Odoonto.Domain.Core.Models.Exceptions;
using Odoonto.Domain.Models.Patients;
using Odoonto.Domain.Models.ValueObjects;
using Odoonto.Domain.Repositories;

namespace Odoonto.Domain.Services.Patients
{
    /// <summary>
    /// Implementación del servicio de dominio para operaciones básicas con pacientes
    /// </summary>
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        }

        public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
        {
            return await _patientRepository.GetAllAsync();
        }

        public async Task<Patient> GetPatientByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("El ID del paciente no puede estar vacío", nameof(id));

            return await _patientRepository.GetByIdOrThrowAsync(id);
        }

        public async Task<Guid> CreatePatientAsync(string firstName, string lastName, DateTime dateOfBirth,
            GenderValue gender, string email, string phoneNumber, string address)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("El nombre no puede estar vacío", nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Los apellidos no pueden estar vacíos", nameof(lastName));

            if (dateOfBirth > DateTime.Today)
                throw new ArgumentException("La fecha de nacimiento no puede ser futura", nameof(dateOfBirth));

            // Crear objetos de valor
            var fullName = new FullName(firstName, lastName);
            var birthDate = new Date(dateOfBirth);
            var contactInfo = new ContactInfo(address, phoneNumber, email);

            // Crear la entidad
            var patient = new Patient(fullName, birthDate, gender, contactInfo);

            // Guardar en el repositorio
            return await _patientRepository.AddAsync(patient);
        }

        public async Task UpdatePatientInfoAsync(Guid id, string firstName, string lastName, DateTime dateOfBirth,
            GenderValue gender, string email, string phoneNumber, string address)
        {
            // Validaciones
            if (id == Guid.Empty)
                throw new ArgumentException("El ID del paciente no puede estar vacío", nameof(id));

            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("El nombre no puede estar vacío", nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Los apellidos no pueden estar vacíos", nameof(lastName));

            if (dateOfBirth > DateTime.Today)
                throw new ArgumentException("La fecha de nacimiento no puede ser futura", nameof(dateOfBirth));

            // Obtener el paciente existente
            var patient = await _patientRepository.GetByIdOrThrowAsync(id);

            // Crear objetos de valor
            var fullName = new FullName(firstName, lastName);
            var birthDate = new Date(dateOfBirth);
            var contactInfo = new ContactInfo(address, phoneNumber, email);

            // Actualizar la entidad
            patient.UpdateBasicInfo(fullName, birthDate, gender, contactInfo);

            // Guardar en el repositorio
            await _patientRepository.UpdateAsync(patient);
        }

        public async Task UpdateMedicalHistoryAsync(Guid id, string medicalHistory)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("El ID del paciente no puede estar vacío", nameof(id));

            var patient = await _patientRepository.GetByIdOrThrowAsync(id);
            patient.UpdateMedicalHistory(medicalHistory);
            await _patientRepository.UpdateAsync(patient);
        }

        public async Task AddAllergyAsync(Guid id, string allergy)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("El ID del paciente no puede estar vacío", nameof(id));

            if (string.IsNullOrWhiteSpace(allergy))
                throw new ArgumentException("La alergia no puede estar vacía", nameof(allergy));

            var patient = await _patientRepository.GetByIdOrThrowAsync(id);
            patient.AddAllergy(allergy);
            await _patientRepository.UpdateAsync(patient);
        }

        public async Task RemoveAllergyAsync(Guid id, string allergy)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("El ID del paciente no puede estar vacío", nameof(id));

            if (string.IsNullOrWhiteSpace(allergy))
                throw new ArgumentException("La alergia no puede estar vacía", nameof(allergy));

            var patient = await _patientRepository.GetByIdOrThrowAsync(id);
            patient.RemoveAllergy(allergy);
            await _patientRepository.UpdateAsync(patient);
        }

        public async Task UpdateNotesAsync(Guid id, string notes)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("El ID del paciente no puede estar vacío", nameof(id));

            var patient = await _patientRepository.GetByIdOrThrowAsync(id);
            patient.UpdateNotes(notes);
            await _patientRepository.UpdateAsync(patient);
        }

        public async Task<bool> DeletePatientAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("El ID del paciente no puede estar vacío", nameof(id));

            return await _patientRepository.DeleteAsync(id);
        }
    }
}