using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Odoonto.Domain.Models.Appointments;

namespace Odoonto.Domain.Repositories
{
    /// <summary>
    /// Interfaz que define las operaciones de repositorio para la entidad Appointment
    /// </summary>
    public interface IAppointmentRepository
    {
        /// <summary>
        /// Obtiene una cita por su identificador
        /// </summary>
        /// <param name="id">Identificador de la cita</param>
        /// <returns>La cita si existe, null si no existe</returns>
        Task<Appointment> GetByIdAsync(Guid id);
        
        /// <summary>
        /// Obtiene una cita por su identificador o lanza una excepción si no existe
        /// </summary>
        /// <param name="id">Identificador de la cita</param>
        /// <returns>La cita</returns>
        /// <exception cref="Odoonto.Domain.Core.Models.Exceptions.EntityNotFoundException">Si la cita no existe</exception>
        Task<Appointment> GetByIdOrThrowAsync(Guid id);
        
        /// <summary>
        /// Guarda una cita nueva o actualiza una existente
        /// </summary>
        /// <param name="appointment">La cita a guardar</param>
        /// <returns>Tarea asíncrona</returns>
        Task SaveAsync(Appointment appointment);
        
        /// <summary>
        /// Obtiene las citas de un paciente en un rango de fechas
        /// </summary>
        /// <param name="patientId">Identificador del paciente</param>
        /// <param name="startDate">Fecha de inicio (inclusiva)</param>
        /// <param name="endDate">Fecha de fin (inclusiva)</param>
        /// <returns>Lista de citas</returns>
        Task<IEnumerable<Appointment>> GetByPatientIdAndDateRangeAsync(Guid patientId, DateTime startDate, DateTime endDate);
        
        /// <summary>
        /// Obtiene las citas de un doctor en un rango de fechas
        /// </summary>
        /// <param name="doctorId">Identificador del doctor</param>
        /// <param name="startDate">Fecha de inicio (inclusiva)</param>
        /// <param name="endDate">Fecha de fin (inclusiva)</param>
        /// <returns>Lista de citas</returns>
        Task<IEnumerable<Appointment>> GetByDoctorIdAndDateRangeAsync(Guid doctorId, DateTime startDate, DateTime endDate);
        
        /// <summary>
        /// Obtiene las citas de un doctor en una fecha específica
        /// </summary>
        /// <param name="doctorId">Identificador del doctor</param>
        /// <param name="date">Fecha</param>
        /// <returns>Lista de citas</returns>
        Task<IEnumerable<Appointment>> GetByDoctorIdAndDateAsync(Guid doctorId, DateTime date);
        
        /// <summary>
        /// Verifica si un doctor tiene citas superpuestas con un horario dado
        /// </summary>
        /// <param name="doctorId">Identificador del doctor</param>
        /// <param name="date">Fecha a verificar</param>
        /// <param name="timeSlot">Franja horaria a verificar</param>
        /// <param name="excludeAppointmentId">Identificador de cita a excluir (para actualizaciones)</param>
        /// <returns>True si hay superposición, False si está disponible</returns>
        Task<bool> HasOverlappingAppointmentsAsync(Guid doctorId, DateTime date, TimeSlot timeSlot, Guid? excludeAppointmentId = null);
    }
} 