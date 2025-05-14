using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Odoonto.Domain.Core.Repositories;
using Odoonto.Domain.Models.Appointments;

namespace Odoonto.Domain.Repositories
{
    /// <summary>
    /// Interfaz que define las operaciones de repositorio para la entidad Appointment
    /// </summary>
    public interface IAppointmentRepository : IRepository<Appointment>
    {
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
    }
}