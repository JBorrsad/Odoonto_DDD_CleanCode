using AutoMapper;
using Odoonto.Application.DTOs.Appointments;
using Odoonto.Application.Interfaces;
using Odoonto.Domain.Core.Models.Exceptions;
using Odoonto.Domain.Models.Appointments;
using Odoonto.Domain.Models.ValueObjects;
using Odoonto.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Odoonto.Application.Services
{
    /// <summary>
    /// Implementación del servicio de citas
    /// </summary>
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly ITreatmentRepository _treatmentRepository;
        private readonly IMapper _mapper;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IDoctorRepository doctorRepository,
            IPatientRepository patientRepository,
            ITreatmentRepository treatmentRepository,
            IMapper mapper)
        {
            _appointmentRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
            _doctorRepository = doctorRepository ?? throw new ArgumentNullException(nameof(doctorRepository));
            _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
            _treatmentRepository = treatmentRepository ?? throw new ArgumentNullException(nameof(treatmentRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<AppointmentDto> CreateAppointmentAsync(CreateAppointmentDto appointmentDto)
        {
            // Validar el DTO
            if (appointmentDto == null)
                throw new ArgumentNullException(nameof(appointmentDto));

            // Validar que el paciente existe
            await _patientRepository.GetByIdOrThrowAsync(appointmentDto.PatientId);

            // Validar que el doctor existe
            await _doctorRepository.GetByIdOrThrowAsync(appointmentDto.DoctorId);

            // Verificar disponibilidad del doctor
            var timeSlot = new TimeSlot(appointmentDto.StartTime, appointmentDto.EndTime);
            bool hasOverlap = await _appointmentRepository.HasOverlappingAppointmentsAsync(
                appointmentDto.DoctorId, appointmentDto.Date, timeSlot);

            if (hasOverlap)
            {
                throw new ConflictException("El doctor ya tiene una cita programada en ese horario.");
            }

            // Crear la cita
            var appointment = Appointment.Create(Guid.NewGuid());
            appointment.SetBasicInfo(appointmentDto.PatientId, appointmentDto.DoctorId, appointmentDto.Date, timeSlot);
            
            if (!string.IsNullOrWhiteSpace(appointmentDto.Notes))
            {
                appointment.SetNotes(appointmentDto.Notes);
            }

            // Crear el plan de tratamiento si hay procedimientos
            if (appointmentDto.Procedures != null && appointmentDto.Procedures.Any())
            {
                var treatmentPlan = new TreatmentPlan();
                
                foreach (var procedureDto in appointmentDto.Procedures)
                {
                    // Verificar que el tratamiento existe
                    var treatment = await _treatmentRepository.GetByIdOrThrowAsync(procedureDto.TreatmentId);
                    
                    // Crear el procedimiento planificado
                    var procedure = new PlannedProcedure(procedureDto.TreatmentId);
                    
                    // Agregar las superficies dentales afectadas
                    foreach (var surfaceDto in procedureDto.ToothSurfaces)
                    {
                        var surfaces = surfaceDto.Surfaces.Select(s => Enum.Parse<ToothSurface>(s)).ToList();
                        procedure.AddToothSurface(surfaceDto.ToothNumber, surfaces);
                    }
                    
                    treatmentPlan.AddProcedure(procedure);
                }
                
                appointment.SetTreatmentPlan(treatmentPlan);
            }

            // Guardar la cita
            await _appointmentRepository.SaveAsync(appointment);

            // Mapear y retornar el DTO
            var result = _mapper.Map<AppointmentDto>(appointment);
            
            // Completar información adicional (nombres de paciente y doctor)
            var patient = await _patientRepository.GetByIdOrThrowAsync(appointment.PatientId);
            var doctor = await _doctorRepository.GetByIdOrThrowAsync(appointment.DoctorId);
            
            result.PatientName = patient.FullName.ToString();
            result.DoctorName = doctor.FullName.ToString();
            
            // Completar información de tratamientos
            if (result.Procedures != null)
            {
                foreach (var procedure in result.Procedures)
                {
                    var treatment = await _treatmentRepository.GetByIdOrThrowAsync(procedure.TreatmentId);
                    procedure.TreatmentName = treatment.Name;
                    procedure.Price = treatment.Price.Amount;
                    procedure.Currency = treatment.Price.Currency;
                }
            }
            
            return result;
        }

        public async Task<bool> CancelAppointmentAsync(Guid id)
        {
            var appointment = await _appointmentRepository.GetByIdOrThrowAsync(id);
            appointment.Cancel();
            await _appointmentRepository.SaveAsync(appointment);
            return true;
        }

        public async Task<AppointmentDto> GetAppointmentByIdAsync(Guid id)
        {
            var appointment = await _appointmentRepository.GetByIdOrThrowAsync(id);
            
            // Mapear a DTO
            var result = _mapper.Map<AppointmentDto>(appointment);
            
            // Completar información adicional (nombres de paciente y doctor)
            var patient = await _patientRepository.GetByIdOrThrowAsync(appointment.PatientId);
            var doctor = await _doctorRepository.GetByIdOrThrowAsync(appointment.DoctorId);
            
            result.PatientName = patient.FullName.ToString();
            result.DoctorName = doctor.FullName.ToString();
            
            // Completar información de tratamientos
            if (result.Procedures != null)
            {
                foreach (var procedure in result.Procedures)
                {
                    var treatment = await _treatmentRepository.GetByIdOrThrowAsync(procedure.TreatmentId);
                    procedure.TreatmentName = treatment.Name;
                    procedure.Price = treatment.Price.Amount;
                    procedure.Currency = treatment.Price.Currency;
                }
            }
            
            return result;
        }

        public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByDoctorAsync(Guid doctorId, DateTime startDate, DateTime endDate)
        {
            var appointments = await _appointmentRepository.GetByDoctorIdAndDateRangeAsync(doctorId, startDate, endDate);
            
            // Mapear a DTOs
            var result = _mapper.Map<IEnumerable<AppointmentDto>>(appointments).ToList();
            
            // Completar información adicional para cada cita
            foreach (var appointmentDto in result)
            {
                // Obtener nombres de paciente y doctor
                var patient = await _patientRepository.GetByIdOrThrowAsync(appointmentDto.PatientId);
                var doctor = await _doctorRepository.GetByIdOrThrowAsync(appointmentDto.DoctorId);
                
                appointmentDto.PatientName = patient.FullName.ToString();
                appointmentDto.DoctorName = doctor.FullName.ToString();
                
                // Completar información de tratamientos
                if (appointmentDto.Procedures != null)
                {
                    foreach (var procedure in appointmentDto.Procedures)
                    {
                        var treatment = await _treatmentRepository.GetByIdOrThrowAsync(procedure.TreatmentId);
                        procedure.TreatmentName = treatment.Name;
                        procedure.Price = treatment.Price.Amount;
                        procedure.Currency = treatment.Price.Currency;
                    }
                }
            }
            
            return result;
        }

        public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByPatientAsync(Guid patientId, DateTime startDate, DateTime endDate)
        {
            var appointments = await _appointmentRepository.GetByPatientIdAndDateRangeAsync(patientId, startDate, endDate);
            
            // Mapear a DTOs
            var result = _mapper.Map<IEnumerable<AppointmentDto>>(appointments).ToList();
            
            // Completar información adicional para cada cita
            foreach (var appointmentDto in result)
            {
                // Obtener nombres de paciente y doctor
                var patient = await _patientRepository.GetByIdOrThrowAsync(appointmentDto.PatientId);
                var doctor = await _doctorRepository.GetByIdOrThrowAsync(appointmentDto.DoctorId);
                
                appointmentDto.PatientName = patient.FullName.ToString();
                appointmentDto.DoctorName = doctor.FullName.ToString();
                
                // Completar información de tratamientos
                if (appointmentDto.Procedures != null)
                {
                    foreach (var procedure in appointmentDto.Procedures)
                    {
                        var treatment = await _treatmentRepository.GetByIdOrThrowAsync(procedure.TreatmentId);
                        procedure.TreatmentName = treatment.Name;
                        procedure.Price = treatment.Price.Amount;
                        procedure.Currency = treatment.Price.Currency;
                    }
                }
            }
            
            return result;
        }

        public async Task<AppointmentDto> MarkAsCompletedAsync(Guid id)
        {
            var appointment = await _appointmentRepository.GetByIdOrThrowAsync(id);
            appointment.MarkAsCompleted();
            await _appointmentRepository.SaveAsync(appointment);
            return await GetAppointmentByIdAsync(id);
        }

        public async Task<AppointmentDto> MarkAsInProgressAsync(Guid id)
        {
            var appointment = await _appointmentRepository.GetByIdOrThrowAsync(id);
            appointment.MarkAsInProgress();
            await _appointmentRepository.SaveAsync(appointment);
            return await GetAppointmentByIdAsync(id);
        }

        public async Task<AppointmentDto> MarkAsWaitingRoomAsync(Guid id)
        {
            var appointment = await _appointmentRepository.GetByIdOrThrowAsync(id);
            appointment.MarkAsWaitingRoom();
            await _appointmentRepository.SaveAsync(appointment);
            return await GetAppointmentByIdAsync(id);
        }

        public async Task<AppointmentDto> UpdateAppointmentAsync(Guid id, CreateAppointmentDto appointmentDto)
        {
            // Validar el DTO
            if (appointmentDto == null)
                throw new ArgumentNullException(nameof(appointmentDto));

            // Obtener la cita existente
            var appointment = await _appointmentRepository.GetByIdOrThrowAsync(id);

            // Validar que el paciente existe
            await _patientRepository.GetByIdOrThrowAsync(appointmentDto.PatientId);

            // Validar que el doctor existe
            await _doctorRepository.GetByIdOrThrowAsync(appointmentDto.DoctorId);

            // Verificar disponibilidad del doctor (excluyendo la cita actual)
            var timeSlot = new TimeSlot(appointmentDto.StartTime, appointmentDto.EndTime);
            bool hasOverlap = await _appointmentRepository.HasOverlappingAppointmentsAsync(
                appointmentDto.DoctorId, appointmentDto.Date, timeSlot, id);

            if (hasOverlap)
            {
                throw new ConflictException("El doctor ya tiene una cita programada en ese horario.");
            }

            // Actualizar la información básica
            appointment.SetBasicInfo(appointmentDto.PatientId, appointmentDto.DoctorId, appointmentDto.Date, timeSlot);
            
            if (!string.IsNullOrWhiteSpace(appointmentDto.Notes))
            {
                appointment.SetNotes(appointmentDto.Notes);
            }

            // Actualizar el plan de tratamiento si hay procedimientos
            if (appointmentDto.Procedures != null && appointmentDto.Procedures.Any())
            {
                var treatmentPlan = new TreatmentPlan();
                
                foreach (var procedureDto in appointmentDto.Procedures)
                {
                    // Verificar que el tratamiento existe
                    var treatment = await _treatmentRepository.GetByIdOrThrowAsync(procedureDto.TreatmentId);
                    
                    // Crear el procedimiento planificado
                    var procedure = new PlannedProcedure(procedureDto.TreatmentId);
                    
                    // Agregar las superficies dentales afectadas
                    foreach (var surfaceDto in procedureDto.ToothSurfaces)
                    {
                        var surfaces = surfaceDto.Surfaces.Select(s => Enum.Parse<ToothSurface>(s)).ToList();
                        procedure.AddToothSurface(surfaceDto.ToothNumber, surfaces);
                    }
                    
                    treatmentPlan.AddProcedure(procedure);
                }
                
                appointment.SetTreatmentPlan(treatmentPlan);
            }

            // Guardar la cita actualizada
            await _appointmentRepository.SaveAsync(appointment);

            // Retornar el DTO actualizado
            return await GetAppointmentByIdAsync(id);
        }
    }
} 