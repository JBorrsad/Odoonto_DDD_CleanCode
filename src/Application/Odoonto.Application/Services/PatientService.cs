using AutoMapper;
using Odoonto.Application.DTOs.Patients;
using Odoonto.Application.Interfaces;
using Odoonto.Domain.Models.Patients;
using Odoonto.Domain.Models.ValueObjects;
using Odoonto.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Odoonto.Application.Services
{
    /// <summary>
    /// Implementación del servicio de pacientes
    /// </summary>
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        public PatientService(IPatientRepository patientRepository, IMapper mapper)
        {
            _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<PatientDto> CreatePatientAsync(CreatePatientDto createPatientDto)
        {
            // Validar el DTO
            if (createPatientDto == null)
                throw new ArgumentNullException(nameof(createPatientDto));

            // Mapear DTO a entidad
            var patientEntity = new Patient(
                new FullName(createPatientDto.FirstName, createPatientDto.LastName),
                new Date(createPatientDto.DateOfBirth),
                (Gender)createPatientDto.Gender,
                new ContactInfo(createPatientDto.Address, createPatientDto.PhoneNumber, createPatientDto.Email)
            );

            // Añadir alergias si existen
            if (createPatientDto.Allergies != null)
            {
                foreach (var allergy in createPatientDto.Allergies)
                {
                    if (!string.IsNullOrWhiteSpace(allergy))
                    {
                        patientEntity.AddAllergy(allergy);
                    }
                }
            }

            // Actualizar campos opcionales
            if (!string.IsNullOrWhiteSpace(createPatientDto.MedicalHistory))
            {
                patientEntity.UpdateMedicalHistory(createPatientDto.MedicalHistory);
            }

            if (!string.IsNullOrWhiteSpace(createPatientDto.Notes))
            {
                patientEntity.UpdateNotes(createPatientDto.Notes);
            }

            // Guardar en el repositorio
            await _patientRepository.AddAsync(patientEntity);

            // Mapear entidad a DTO de respuesta
            return _mapper.Map<PatientDto>(patientEntity);
        }

        public async Task<bool> DeletePatientAsync(Guid id)
        {
            return await _patientRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync()
        {
            var patients = await _patientRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PatientDto>>(patients);
        }

        public async Task<IEnumerable<PatientDto>> GetPaginatedPatientsAsync(int pageNumber, int pageSize)
        {
            var patients = await _patientRepository.GetPaginatedAsync(pageNumber, pageSize);
            return _mapper.Map<IEnumerable<PatientDto>>(patients);
        }

        public async Task<PatientDto> GetPatientByIdAsync(Guid id)
        {
            var patient = await _patientRepository.GetByIdOrThrowAsync(id);
            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<int> GetTotalPatientsCountAsync()
        {
            return await _patientRepository.GetTotalPatientsCountAsync();
        }

        public async Task<IEnumerable<PatientDto>> SearchPatientsByNameAsync(string name)
        {
            var patients = await _patientRepository.FindByNameAsync(name);
            return _mapper.Map<IEnumerable<PatientDto>>(patients);
        }

        public async Task<IEnumerable<PatientDto>> SearchPatientsAsync(string searchTerm)
        {
            var patients = await _patientRepository.SearchAsync(searchTerm);
            return _mapper.Map<IEnumerable<PatientDto>>(patients);
        }

        public async Task<PatientDto> UpdatePatientAsync(Guid id, CreatePatientDto updatePatientDto)
        {
            // Validar el DTO
            if (updatePatientDto == null)
                throw new ArgumentNullException(nameof(updatePatientDto));

            // Obtener el paciente existente
            var existingPatient = await _patientRepository.GetByIdOrThrowAsync(id);

            // Actualizar los datos básicos
            existingPatient.UpdateBasicInfo(
                new FullName(updatePatientDto.FirstName, updatePatientDto.LastName),
                new Date(updatePatientDto.DateOfBirth),
                (Gender)updatePatientDto.Gender,
                new ContactInfo(updatePatientDto.Address, updatePatientDto.PhoneNumber, updatePatientDto.Email)
            );

            // Actualizar historial médico y notas
            existingPatient.UpdateMedicalHistory(updatePatientDto.MedicalHistory);
            existingPatient.UpdateNotes(updatePatientDto.Notes);

            // Actualizar alergias (eliminar todas y añadir las nuevas)
            if (existingPatient.Allergies != null)
            {
                var currentAllergies = new List<string>(existingPatient.Allergies);
                foreach (var allergy in currentAllergies)
                {
                    existingPatient.RemoveAllergy(allergy);
                }
            }

            if (updatePatientDto.Allergies != null)
            {
                foreach (var allergy in updatePatientDto.Allergies)
                {
                    if (!string.IsNullOrWhiteSpace(allergy))
                    {
                        existingPatient.AddAllergy(allergy);
                    }
                }
            }

            // Guardar cambios
            await _patientRepository.UpdateAsync(existingPatient);

            // Mapear entidad actualizada a DTO
            return _mapper.Map<PatientDto>(existingPatient);
        }
    }
} 