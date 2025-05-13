using AutoMapper;
using Odoonto.Application.DTOs.Doctors;
using Odoonto.Application.Interfaces;
using Odoonto.Domain.Models.Doctors;
using Odoonto.Domain.Models.ValueObjects;
using Odoonto.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Odoonto.Application.Services
{
    /// <summary>
    /// Implementación del servicio de doctores
    /// </summary>
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public DoctorService(IDoctorRepository doctorRepository, IMapper mapper)
        {
            _doctorRepository = doctorRepository ?? throw new ArgumentNullException(nameof(doctorRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto createDoctorDto)
        {
            // Validar el DTO
            if (createDoctorDto == null)
                throw new ArgumentNullException(nameof(createDoctorDto));

            // Crear la entidad Doctor
            var doctor = Doctor.Create(Guid.NewGuid());
            
            // Establecer propiedades
            doctor.SetFullName(createDoctorDto.FirstName, createDoctorDto.LastName);
            doctor.SetSpecialty(createDoctorDto.Specialty);
            doctor.SetContactInfo(
                string.Empty, // No hay campo de dirección en el DTO
                createDoctorDto.PhoneNumber,
                createDoctorDto.Email
            );

            // Crear la disponibilidad del doctor
            var availability = WeeklyAvailability.Create();
            
            // Añadir los horarios disponibles
            if (createDoctorDto.Schedule != null && createDoctorDto.Schedule.Any())
            {
                foreach (var scheduleDto in createDoctorDto.Schedule)
                {
                    // Solo agregar horarios válidos y activos
                    if (scheduleDto.IsActive)
                    {
                        var dayOfWeek = (DayOfWeek)scheduleDto.DayOfWeek;
                        var timeRange = new TimeRange(scheduleDto.StartTime, scheduleDto.EndTime);
                        availability = availability.AddTimeRange(dayOfWeek, timeRange);
                    }
                }
            }
            
            // Establecer la disponibilidad
            doctor.SetAvailability(availability);

            // Guardar en el repositorio
            await _doctorRepository.AddAsync(doctor);

            // Mapear a DTO
            return _mapper.Map<DoctorDto>(doctor);
        }

        public async Task<bool> DeleteDoctorAsync(Guid id)
        {
            return await _doctorRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<DoctorDto>> FindDoctorsBySpecialtyAsync(string specialty)
        {
            var doctors = await _doctorRepository.FindBySpecialtyAsync(specialty);
            return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
        }

        public async Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync()
        {
            var doctors = await _doctorRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
        }

        public async Task<DoctorDto> GetDoctorByIdAsync(Guid id)
        {
            var doctor = await _doctorRepository.GetByIdOrThrowAsync(id);
            return _mapper.Map<DoctorDto>(doctor);
        }

        public async Task<IEnumerable<DoctorDto>> GetPaginatedDoctorsAsync(int pageNumber, int pageSize)
        {
            var doctors = await _doctorRepository.GetPaginatedAsync(pageNumber, pageSize);
            return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
        }

        public async Task<int> GetTotalDoctorsCountAsync()
        {
            return await _doctorRepository.GetTotalDoctorsCountAsync();
        }

        public async Task<IEnumerable<DoctorDto>> SearchDoctorsAsync(string searchTerm)
        {
            var doctors = await _doctorRepository.SearchAsync(searchTerm);
            return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
        }

        public async Task<IEnumerable<DoctorDto>> SearchDoctorsByNameAsync(string name)
        {
            var doctors = await _doctorRepository.FindByNameAsync(name);
            return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
        }

        public async Task<DoctorDto> UpdateDoctorAsync(Guid id, CreateDoctorDto updateDoctorDto)
        {
            // Validar el DTO
            if (updateDoctorDto == null)
                throw new ArgumentNullException(nameof(updateDoctorDto));

            // Obtener el doctor existente
            var doctor = await _doctorRepository.GetByIdOrThrowAsync(id);

            // Actualizar propiedades básicas
            doctor.SetFullName(updateDoctorDto.FirstName, updateDoctorDto.LastName);
            doctor.SetSpecialty(updateDoctorDto.Specialty);
            doctor.SetContactInfo(
                string.Empty, // No hay campo de dirección en el DTO
                updateDoctorDto.PhoneNumber,
                updateDoctorDto.Email
            );

            // Crear la nueva disponibilidad del doctor
            var availability = WeeklyAvailability.Create();
            
            // Añadir los horarios disponibles
            if (updateDoctorDto.Schedule != null && updateDoctorDto.Schedule.Any())
            {
                foreach (var scheduleDto in updateDoctorDto.Schedule)
                {
                    // Solo agregar horarios válidos y activos
                    if (scheduleDto.IsActive)
                    {
                        var dayOfWeek = (DayOfWeek)scheduleDto.DayOfWeek;
                        var timeRange = new TimeRange(scheduleDto.StartTime, scheduleDto.EndTime);
                        availability = availability.AddTimeRange(dayOfWeek, timeRange);
                    }
                }
            }
            
            // Establecer la nueva disponibilidad
            doctor.SetAvailability(availability);

            // Guardar cambios
            await _doctorRepository.UpdateAsync(doctor);

            // Mapear a DTO
            return _mapper.Map<DoctorDto>(doctor);
        }
    }
} 