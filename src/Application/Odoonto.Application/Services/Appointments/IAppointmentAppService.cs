using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Odoonto.Application.DTOs.Appointments;

namespace Odoonto.Application.Services.Appointments
{
    /// <summary>
    /// Interfaz para el servicio de aplicación de citas
    /// </summary>
    public interface IAppointmentAppService
    {
        /// <summary>
        /// Obtiene todas las citas
        /// </summary>
        Task<IEnumerable<AppointmentDto>> GetAllAsync();

        /// <summary>
        /// Obtiene una cita por su ID
        /// </summary>
        Task<AppointmentDto> GetByIdAsync(Guid id);

        /// <summary>
        /// Obtiene las citas de un paciente en un rango de fechas
        /// </summary>
        Task<IEnumerable<AppointmentDto>> GetByPatientAsync(Guid patientId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Obtiene las citas de un doctor en un rango de fechas
        /// </summary>
        Task<IEnumerable<AppointmentDto>> GetByDoctorAsync(Guid doctorId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Crea una nueva cita
        /// </summary>
        Task<Guid> CreateAsync(CreateAppointmentDto appointmentDto);

        /// <summary>
        /// Actualiza una cita existente
        /// </summary>
        Task UpdateAsync(Guid id, UpdateAppointmentDto appointmentDto);

        /// <summary>
        /// Elimina una cita
        /// </summary>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Cancela una cita
        /// </summary>
        Task CancelAsync(Guid id, string cancellationReason);

        /// <summary>
        /// Marca una cita como "en sala de espera"
        /// </summary>
        Task MarkAsWaitingRoomAsync(Guid id);

        /// <summary>
        /// Marca una cita como "en progreso"
        /// </summary>
        Task MarkAsInProgressAsync(Guid id);

        /// <summary>
        /// Marca una cita como "completada"
        /// </summary>
        Task MarkAsCompletedAsync(Guid id);

        /// <summary>
        /// Verifica disponibilidad de un doctor en un horario específico
        /// </summary>
        Task<bool> CheckDoctorAvailabilityAsync(Guid doctorId, DateTime date, TimeOnly startTime, TimeOnly endTime);
    }
} 