using Odoonto.Application.DTOs.Appointments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Odoonto.Application.Interfaces
{
    /// <summary>
    /// Interfaz que define las operaciones del servicio de aplicación para citas
    /// </summary>
    public interface IAppointmentService
    {
        /// <summary>
        /// Crea una nueva cita
        /// </summary>
        /// <param name="appointmentDto">DTO con la información de la cita</param>
        /// <returns>DTO con la información de la cita creada</returns>
        Task<AppointmentDto> CreateAppointmentAsync(CreateAppointmentDto appointmentDto);

        /// <summary>
        /// Actualiza una cita existente
        /// </summary>
        /// <param name="id">Identificador de la cita</param>
        /// <param name="appointmentDto">DTO con la información actualizada</param>
        /// <returns>DTO con la información de la cita actualizada</returns>
        Task<AppointmentDto> UpdateAppointmentAsync(Guid id, CreateAppointmentDto appointmentDto);

        /// <summary>
        /// Obtiene una cita por su identificador
        /// </summary>
        /// <param name="id">Identificador de la cita</param>
        /// <returns>DTO con la información de la cita</returns>
        Task<AppointmentDto> GetAppointmentByIdAsync(Guid id);

        /// <summary>
        /// Obtiene las citas de un paciente en un rango de fechas
        /// </summary>
        /// <param name="patientId">Identificador del paciente</param>
        /// <param name="startDate">Fecha de inicio</param>
        /// <param name="endDate">Fecha de fin</param>
        /// <returns>Lista de DTOs con la información de las citas</returns>
        Task<IEnumerable<AppointmentDto>> GetAppointmentsByPatientAsync(Guid patientId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Obtiene las citas de un doctor en un rango de fechas
        /// </summary>
        /// <param name="doctorId">Identificador del doctor</param>
        /// <param name="startDate">Fecha de inicio</param>
        /// <param name="endDate">Fecha de fin</param>
        /// <returns>Lista de DTOs con la información de las citas</returns>
        Task<IEnumerable<AppointmentDto>> GetAppointmentsByDoctorAsync(Guid doctorId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Cancela una cita
        /// </summary>
        /// <param name="id">Identificador de la cita</param>
        /// <returns>True si la operación fue exitosa</returns>
        Task<bool> CancelAppointmentAsync(Guid id);

        /// <summary>
        /// Marca una cita como paciente en sala de espera
        /// </summary>
        /// <param name="id">Identificador de la cita</param>
        /// <returns>DTO con la información de la cita actualizada</returns>
        Task<AppointmentDto> MarkAsWaitingRoomAsync(Guid id);

        /// <summary>
        /// Marca una cita como en progreso
        /// </summary>
        /// <param name="id">Identificador de la cita</param>
        /// <returns>DTO con la información de la cita actualizada</returns>
        Task<AppointmentDto> MarkAsInProgressAsync(Guid id);

        /// <summary>
        /// Marca una cita como completada
        /// </summary>
        /// <param name="id">Identificador de la cita</param>
        /// <returns>DTO con la información de la cita actualizada</returns>
        Task<AppointmentDto> MarkAsCompletedAsync(Guid id);
    }
} 