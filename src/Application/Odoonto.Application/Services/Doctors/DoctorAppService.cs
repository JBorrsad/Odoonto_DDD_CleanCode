using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Odoonto.Application.DTOs.Common;
using Odoonto.Application.DTOs.Doctors;
using Odoonto.Domain.Core.Models.Exceptions;
using Odoonto.Domain.Models.Doctors;
using Odoonto.Domain.Models.ValueObjects;
using Odoonto.Domain.Repositories;

namespace Odoonto.Application.Services.Doctors
{
    /// <summary>
    /// Implementación del servicio de aplicación para doctores
    /// </summary>
    public class DoctorAppService : IDoctorAppService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor con inyección de dependencias
        /// </summary>
        public DoctorAppService(IDoctorRepository doctorRepository, IMapper mapper)
        {
            _doctorRepository = doctorRepository ?? throw new ArgumentNullException(nameof(doctorRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Obtiene todos los doctores
        /// </summary>
        public async Task<IEnumerable<DoctorDto>> GetAllAsync()
        {
            var doctors = await _doctorRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<Doctor>, IEnumerable<DoctorDto>>(doctors);
        }

        /// <summary>
        /// Obtiene un doctor por su ID
        /// </summary>
        public async Task<DoctorDto> GetByIdAsync(Guid id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor == null)
            {
                throw new DomainException($"No se encontró el doctor con ID {id}");
            }

            return _mapper.Map<Doctor, DoctorDto>(doctor);
        }

        /// <summary>
        /// Obtiene doctores por especialidad
        /// </summary>
        public async Task<IEnumerable<DoctorDto>> GetBySpecialtyAsync(string specialty)
        {
            if (string.IsNullOrWhiteSpace(specialty))
            {
                throw new DomainException("La especialidad no puede estar vacía");
            }

            var doctors = await _doctorRepository.GetBySpecialtyAsync(specialty);
            return _mapper.Map<IEnumerable<Doctor>, IEnumerable<DoctorDto>>(doctors);
        }

        /// <summary>
        /// Crea un nuevo doctor
        /// </summary>
        public async Task<Guid> CreateAsync(CreateDoctorDto doctorDto)
        {
            if (doctorDto == null)
            {
                throw new DomainException("Los datos del doctor no pueden ser nulos");
            }

            // Crear nueva entidad doctor
            var doctorId = Guid.NewGuid();
            var doctor = Doctor.Create(doctorId);

            // Establecer propiedades del doctor
            doctor.SetFullName(doctorDto.FirstNames, doctorDto.LastNames);
            doctor.SetSpecialty(doctorDto.Specialty);
            doctor.SetContactInfo(doctorDto.Address, doctorDto.PhoneNumber, doctorDto.Email);

            // Guardar el doctor
            await _doctorRepository.CreateAsync(doctor);

            return doctorId;
        }

        /// <summary>
        /// Actualiza un doctor existente
        /// </summary>
        public async Task UpdateAsync(Guid id, UpdateDoctorDto doctorDto)
        {
            if (doctorDto == null)
            {
                throw new DomainException("Los datos del doctor no pueden ser nulos");
            }

            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor == null)
            {
                throw new DomainException($"No se encontró el doctor con ID {id}");
            }

            // Actualizar propiedades si se proporcionan
            if (!string.IsNullOrWhiteSpace(doctorDto.FirstNames) && !string.IsNullOrWhiteSpace(doctorDto.LastNames))
            {
                doctor.SetFullName(doctorDto.FirstNames, doctorDto.LastNames);
            }

            if (!string.IsNullOrWhiteSpace(doctorDto.Specialty))
            {
                doctor.SetSpecialty(doctorDto.Specialty);
            }

            // Solo actualizar la información de contacto si al menos un campo está presente
            if (!string.IsNullOrWhiteSpace(doctorDto.PhoneNumber) || 
                !string.IsNullOrWhiteSpace(doctorDto.Email) || 
                !string.IsNullOrWhiteSpace(doctorDto.Address))
            {
                doctor.SetContactInfo(
                    doctorDto.Address ?? doctor.ContactInfo?.Address,
                    doctorDto.PhoneNumber ?? doctor.ContactInfo?.PhoneNumber,
                    doctorDto.Email ?? doctor.ContactInfo?.Email
                );
            }

            await _doctorRepository.UpdateAsync(doctor);
        }

        /// <summary>
        /// Elimina un doctor
        /// </summary>
        public async Task DeleteAsync(Guid id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor == null)
            {
                throw new DomainException($"No se encontró el doctor con ID {id}");
            }

            await _doctorRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Verifica disponibilidad de un doctor en una fecha y hora específicas
        /// </summary>
        public async Task<bool> CheckAvailabilityAsync(Guid id, DateTime date, TimeOnly startTime, TimeOnly endTime)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor == null)
            {
                throw new DomainException($"No se encontró el doctor con ID {id}");
            }

            var timeSlot = new TimeSlot(startTime, endTime);
            return doctor.IsAvailable(date, timeSlot);
        }

        /// <summary>
        /// Establece disponibilidad para un doctor en un día específico
        /// </summary>
        public async Task SetAvailabilityAsync(Guid id, DayOfWeek day, TimeOnly startTime, TimeOnly endTime)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor == null)
            {
                throw new DomainException($"No se encontró el doctor con ID {id}");
            }

            if (doctor.Availability == null)
            {
                doctor.SetAvailability(new WeeklyAvailability());
            }

            var timeSlot = new TimeSlot(startTime, endTime);
            doctor.Availability.AddTimeSlot(day, timeSlot);

            await _doctorRepository.UpdateAsync(doctor);
        }
    }
} 