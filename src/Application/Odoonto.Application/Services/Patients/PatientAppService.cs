using AutoMapper;
using Odoonto.Application.DTOs.Patients;
using Odoonto.Domain.Core.Models.Exceptions;
using Odoonto.Domain.Models.Patients;
using Odoonto.Domain.Models.ValueObjects;
using Odoonto.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Odoonto.Application.Services.Patients
{
    /// <summary>
    /// Implementación del servicio de aplicación para pacientes
    /// </summary>
    public class PatientAppService : IPatientAppService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor con inyección de dependencias
        /// </summary>
        public PatientAppService(IPatientRepository patientRepository, IMapper mapper)
        {
            _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Obtiene todos los pacientes
        /// </summary>
        public async Task<IEnumerable<PatientDto>> GetAllAsync()
        {
            var patients = await _patientRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<Patient>, IEnumerable<PatientDto>>(patients);
        }

        /// <summary>
        /// Obtiene un paciente por su ID
        /// </summary>
        public async Task<PatientDto> GetByIdAsync(Guid id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null)
            {
                throw new EntityNotFoundException($"No se encontró el paciente con ID {id}");
            }

            return _mapper.Map<Patient, PatientDto>(patient);
        }

        /// <summary>
        /// Crea un nuevo paciente
        /// </summary>
        public async Task<Guid> CreateAsync(CreatePatientDto patientDto)
        {
            if (patientDto == null)
            {
                throw new ArgumentNullException(nameof(patientDto));
            }

            // Crear entidad
            var fullName = new FullName(patientDto.FirstName, patientDto.LastName);
            var birthDate = new Date(patientDto.DateOfBirth);
            var gender = Enum.Parse<Gender>(patientDto.Gender.ToString(), true);
            var contactInfo = new ContactInfo(patientDto.Address, patientDto.PhoneNumber, patientDto.Email);
            
            var patientEntity = new Patient(fullName, birthDate.Value, gender.ToString(), contactInfo);
            
            // Añadir alergias si existen
            if (patientDto.Allergies != null)
            {
                foreach (var allergy in patientDto.Allergies)
                {
                    if (!string.IsNullOrWhiteSpace(allergy))
                    {
                        patientEntity.AddAllergy(allergy);
                    }
                }
            }

            // Guardar entidad
            await _patientRepository.CreateAsync(patientEntity);
            
            return patientEntity.Id;
        }

        /// <summary>
        /// Actualiza un paciente existente
        /// </summary>
        public async Task UpdateAsync(Guid id, CreatePatientDto patientDto)
        {
            if (patientDto == null)
            {
                throw new ArgumentNullException(nameof(patientDto));
            }

            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null)
            {
                throw new EntityNotFoundException($"No se encontró el paciente con ID {id}");
            }

            // Actualizar entidad
            var fullName = new FullName(patientDto.FirstName, patientDto.LastName);
            var birthDate = new Date(patientDto.DateOfBirth);
            var gender = Enum.Parse<Gender>(patientDto.Gender.ToString(), true);
            var contactInfo = new ContactInfo(patientDto.Address, patientDto.PhoneNumber, patientDto.Email);
            
            patient.UpdateBasicInfo(fullName, birthDate.Value, gender.ToString(), contactInfo);
            
            await _patientRepository.UpdateAsync(patient);
        }

        /// <summary>
        /// Elimina un paciente
        /// </summary>
        public async Task DeleteAsync(Guid id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null)
            {
                throw new EntityNotFoundException($"No se encontró el paciente con ID {id}");
            }

            await _patientRepository.DeleteAsync(patient);
        }

        /// <summary>
        /// Busca pacientes por término de búsqueda genérico
        /// </summary>
        public async Task<IEnumerable<PatientDto>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                throw new ArgumentException("El término de búsqueda no puede estar vacío", nameof(searchTerm));
            }

            var patients = await _patientRepository.SearchAsync(searchTerm);
            return _mapper.Map<IEnumerable<Patient>, IEnumerable<PatientDto>>(patients);
        }

        /// <summary>
        /// Actualiza el historial médico de un paciente
        /// </summary>
        public async Task<PatientDto> UpdateMedicalHistoryAsync(Guid id, string medicalHistory)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null)
            {
                throw new EntityNotFoundException($"No se encontró el paciente con ID {id}");
            }

            patient.UpdateMedicalHistory(medicalHistory);
            await _patientRepository.UpdateAsync(patient);

            return _mapper.Map<Patient, PatientDto>(patient);
        }

        /// <summary>
        /// Añade una alergia al paciente
        /// </summary>
        public async Task<PatientDto> AddAllergyAsync(Guid id, string allergy)
        {
            if (string.IsNullOrWhiteSpace(allergy))
            {
                throw new ArgumentException("La alergia no puede estar vacía", nameof(allergy));
            }

            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null)
            {
                throw new EntityNotFoundException($"No se encontró el paciente con ID {id}");
            }

            patient.AddAllergy(allergy);
            await _patientRepository.UpdateAsync(patient);

            return _mapper.Map<Patient, PatientDto>(patient);
        }

        /// <summary>
        /// Elimina una alergia del paciente
        /// </summary>
        public async Task<PatientDto> RemoveAllergyAsync(Guid id, string allergy)
        {
            if (string.IsNullOrWhiteSpace(allergy))
            {
                throw new ArgumentException("La alergia no puede estar vacía", nameof(allergy));
            }

            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null)
            {
                throw new EntityNotFoundException($"No se encontró el paciente con ID {id}");
            }

            patient.RemoveAllergy(allergy);
            await _patientRepository.UpdateAsync(patient);

            return _mapper.Map<Patient, PatientDto>(patient);
        }
    }
} 