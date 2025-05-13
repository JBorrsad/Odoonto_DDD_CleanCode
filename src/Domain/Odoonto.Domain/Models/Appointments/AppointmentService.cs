using System;
using System.Threading.Tasks;
using Odoonto.Domain.Core.Models.Exceptions;
using Odoonto.Domain.Models.ValueObjects;
using Odoonto.Domain.Repositories;

namespace Odoonto.Domain.Models.Appointments
{
    /// <summary>
    /// Servicio de dominio que implementa la lógica de negocio relacionada con citas
    /// </summary>
    public class AppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        
        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
        }
        
        /// <summary>
        /// Verifica si es posible programar una cita en el horario especificado
        /// </summary>
        /// <param name="doctorId">Identificador del doctor</param>
        /// <param name="date">Fecha de la cita</param>
        /// <param name="timeSlot">Horario de la cita</param>
        /// <param name="excludeAppointmentId">ID de cita a excluir (para actualizaciones)</param>
        /// <returns>True si la cita se puede programar, false si hay conflictos</returns>
        public async Task<bool> CanScheduleAppointmentAsync(Guid doctorId, DateTime date, TimeSlot timeSlot, Guid? excludeAppointmentId = null)
        {
            if (doctorId == Guid.Empty)
                throw new InvalidValueException("El identificador del doctor no puede estar vacío.");
                
            if (timeSlot == null)
                throw new InvalidValueException("El horario no puede ser nulo.");
                
            if (date.Date < DateTime.Today)
                throw new InvalidValueException("No se pueden programar citas en fechas pasadas.");
                
            // Verificar si hay superposición con otras citas del doctor
            bool hasOverlap = await _appointmentRepository.HasOverlappingAppointmentsAsync(
                doctorId, date, timeSlot, excludeAppointmentId);
                
            return !hasOverlap;
        }
        
        /// <summary>
        /// Programar una nueva cita verificando disponibilidad
        /// </summary>
        /// <param name="appointment">La cita a programar</param>
        /// <returns>True si la cita se programó correctamente, excepción en caso contrario</returns>
        public async Task ScheduleAppointmentAsync(Appointment appointment)
        {
            if (appointment == null)
                throw new ArgumentNullException(nameof(appointment));
                
            // Verificar disponibilidad
            bool canSchedule = await CanScheduleAppointmentAsync(
                appointment.DoctorId, 
                appointment.Date, 
                appointment.TimeSlot,
                appointment.Id);
                
            if (!canSchedule)
                throw new InvalidValueException("No se puede programar la cita porque hay conflictos de horario con otras citas.");
                
            // Guardar la cita
            await _appointmentRepository.SaveAsync(appointment);
        }
        
        /// <summary>
        /// Cancela una cita existente
        /// </summary>
        /// <param name="appointmentId">Identificador de la cita</param>
        /// <returns>Tarea asíncrona</returns>
        public async Task CancelAppointmentAsync(Guid appointmentId)
        {
            if (appointmentId == Guid.Empty)
                throw new InvalidValueException("El identificador de la cita no puede estar vacío.");
                
            // Obtener la cita
            var appointment = await _appointmentRepository.GetByIdOrThrowAsync(appointmentId);
            
            // Cancelar la cita
            appointment.Cancel();
            
            // Guardar la cita
            await _appointmentRepository.SaveAsync(appointment);
        }
        
        /// <summary>
        /// Cambiar el estado de una cita a "En sala de espera"
        /// </summary>
        /// <param name="appointmentId">Identificador de la cita</param>
        /// <returns>Tarea asíncrona</returns>
        public async Task MarkAppointmentAsWaitingRoomAsync(Guid appointmentId)
        {
            var appointment = await _appointmentRepository.GetByIdOrThrowAsync(appointmentId);
            appointment.MarkAsWaitingRoom();
            await _appointmentRepository.SaveAsync(appointment);
        }
        
        /// <summary>
        /// Cambiar el estado de una cita a "En progreso"
        /// </summary>
        /// <param name="appointmentId">Identificador de la cita</param>
        /// <returns>Tarea asíncrona</returns>
        public async Task MarkAppointmentAsInProgressAsync(Guid appointmentId)
        {
            var appointment = await _appointmentRepository.GetByIdOrThrowAsync(appointmentId);
            appointment.MarkAsInProgress();
            await _appointmentRepository.SaveAsync(appointment);
        }
        
        /// <summary>
        /// Cambiar el estado de una cita a "Completada"
        /// </summary>
        /// <param name="appointmentId">Identificador de la cita</param>
        /// <returns>Tarea asíncrona</returns>
        public async Task MarkAppointmentAsCompletedAsync(Guid appointmentId)
        {
            var appointment = await _appointmentRepository.GetByIdOrThrowAsync(appointmentId);
            appointment.MarkAsCompleted();
            await _appointmentRepository.SaveAsync(appointment);
        }
    }
} 