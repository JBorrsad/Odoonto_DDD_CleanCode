using AutoMapper;
using Microsoft.Extensions.Logging;
using Odoonto.Application.DTOs.Appointments;
using Odoonto.Domain.Core.Models.Exceptions;
using Odoonto.Domain.Models.Appointments;
using Odoonto.Domain.Models.ValueObjects;
using Odoonto.Domain.Repositories;
using Odoonto.Domain.Services.Appointments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Odoonto.Application.Services.Appointments
{
    /// <summary>
    /// Implementación del servicio de aplicación para citas
    /// </summary>
    public class AppointmentAppService : IAppointmentAppService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly ITreatmentRepository _treatmentRepository;
        private readonly IAppointmentOverlapService _overlapService;
        private readonly ILogger<AppointmentAppService> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor con inyección de dependencias
        /// </summary>
        public AppointmentAppService(
            IAppointmentRepository appointmentRepository,
            IDoctorRepository doctorRepository,
            IPatientRepository patientRepository,
            ITreatmentRepository treatmentRepository,
            IAppointmentOverlapService overlapService,
            ILogger<AppointmentAppService> logger,
            IMapper mapper)
        {
            _appointmentRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
            _doctorRepository = doctorRepository ?? throw new ArgumentNullException(nameof(doctorRepository));
            _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
            _treatmentRepository = treatmentRepository ?? throw new ArgumentNullException(nameof(treatmentRepository));
            _overlapService = overlapService ?? throw new ArgumentNullException(nameof(overlapService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Obtiene todas las citas
        /// </summary>
        public async Task<IEnumerable<AppointmentDto>> GetAllAsync()
        {
            var appointments = await _appointmentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }

        /// <summary>
        /// Obtiene una cita por su ID
        /// </summary>
        public async Task<AppointmentDto> GetByIdAsync(Guid id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
                throw new EntityNotFoundException($"No se encontró la cita con ID {id}");

            return _mapper.Map<AppointmentDto>(appointment);
        }

        /// <summary>
        /// Obtiene las citas de un paciente en un rango de fechas
        /// </summary>
        public async Task<IEnumerable<AppointmentDto>> GetByPatientAsync(Guid patientId, DateTime startDate, DateTime endDate)
        {
            var appointments = await _appointmentRepository.GetByPatientIdAndDateRangeAsync(patientId, startDate, endDate);
            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }

        /// <summary>
        /// Obtiene las citas de un doctor en un rango de fechas
        /// </summary>
        public async Task<IEnumerable<AppointmentDto>> GetByDoctorAsync(Guid doctorId, DateTime startDate, DateTime endDate)
        {
            var appointments = await _appointmentRepository.GetByDoctorIdAndDateRangeAsync(doctorId, startDate, endDate);
            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }

        /// <summary>
        /// Verifica disponibilidad de un doctor en un horario específico
        /// </summary>
        public async Task<bool> CheckDoctorAvailabilityAsync(Guid doctorId, DateTime date, TimeOnly startTime, TimeOnly endTime, Guid? excludeAppointmentId = null)
        {
            var timeSlot = new TimeSlot(startTime, endTime);
            return !(await _overlapService.HasOverlappingAppointmentsAsync(doctorId, date, timeSlot, excludeAppointmentId));
        }

        /// <summary>
        /// Crea una nueva cita
        /// </summary>
        public async Task<Guid> CreateAsync(CreateAppointmentDto appointmentDto)
        {
            // Implementación pendiente
            throw new NotImplementedException("Método en desarrollo");
        }

        /// <summary>
        /// Actualiza una cita existente
        /// </summary>
        public async Task UpdateAsync(Guid id, UpdateAppointmentDto appointmentDto)
        {
            // Implementación pendiente
            throw new NotImplementedException("Método en desarrollo");
        }

        /// <summary>
        /// Elimina una cita
        /// </summary>
        public async Task DeleteAsync(Guid id)
        {
            // Implementación pendiente
            throw new NotImplementedException("Método en desarrollo");
        }

        /// <summary>
        /// Cancela una cita
        /// </summary>
        public async Task CancelAsync(Guid id, string cancellationReason)
        {
            // Implementación pendiente
            throw new NotImplementedException("Método en desarrollo");
        }

        /// <summary>
        /// Marca una cita como "en sala de espera"
        /// </summary>
        public async Task MarkAsWaitingRoomAsync(Guid id)
        {
            // Implementación pendiente
            throw new NotImplementedException("Método en desarrollo");
        }

        /// <summary>
        /// Marca una cita como "en progreso"
        /// </summary>
        public async Task MarkAsInProgressAsync(Guid id)
        {
            // Implementación pendiente
            throw new NotImplementedException("Método en desarrollo");
        }

        /// <summary>
        /// Marca una cita como "completada"
        /// </summary>
        public async Task MarkAsCompletedAsync(Guid id)
        {
            // Implementación pendiente
            throw new NotImplementedException("Método en desarrollo");
        }
    }
} 