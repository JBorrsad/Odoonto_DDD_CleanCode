using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Odoonto.Domain.Models.Appointments;

namespace Odoonto.Domain.Services.Appointments
{
    /// <summary>
    /// Servicio de dominio para consultas específicas de citas
    /// </summary>
    public interface IAppointmentQueryService
    {
        /// <summary>
        /// Obtiene las citas de un paciente en un rango de fechas
        /// </summary>
        /// <param name="patientId">ID del paciente</param>
        /// <param name="startDate">Fecha de inicio (inclusiva)</param>
        /// <param name="endDate">Fecha de fin (inclusiva)</param>
        /// <param name="page">Número de página (comienza en 1)</param>
        /// <param name="pageSize">Tamaño de página</param>
        /// <returns>Lista paginada de citas</returns>
        Task<IEnumerable<Appointment>> GetByPatientIdAndDateRangeAsync(
            Guid patientId, 
            DateTime startDate, 
            DateTime endDate, 
            int page = 1, 
            int pageSize = 10);

        /// <summary>
        /// Obtiene las citas de un doctor en un rango de fechas
        /// </summary>
        /// <param name="doctorId">ID del doctor</param>
        /// <param name="startDate">Fecha de inicio (inclusiva)</param>
        /// <param name="endDate">Fecha de fin (inclusiva)</param>
        /// <param name="page">Número de página (comienza en 1)</param>
        /// <param name="pageSize">Tamaño de página</param>
        /// <returns>Lista paginada de citas</returns>
        Task<IEnumerable<Appointment>> GetByDoctorIdAndDateRangeAsync(
            Guid doctorId, 
            DateTime startDate, 
            DateTime endDate, 
            int page = 1, 
            int pageSize = 10);

        /// <summary>
        /// Obtiene las citas de un doctor en una fecha específica
        /// </summary>
        /// <param name="doctorId">ID del doctor</param>
        /// <param name="date">Fecha específica</param>
        /// <param name="page">Número de página (comienza en 1)</param>
        /// <param name="pageSize">Tamaño de página</param>
        /// <returns>Lista paginada de citas</returns>
        Task<IEnumerable<Appointment>> GetByDoctorIdAndDateAsync(
            Guid doctorId, 
            DateTime date, 
            int page = 1, 
            int pageSize = 10);

        /// <summary>
        /// Obtiene el total de citas para un paciente en un rango de fechas
        /// </summary>
        /// <param name="patientId">ID del paciente</param>
        /// <param name="startDate">Fecha de inicio (inclusiva)</param>
        /// <param name="endDate">Fecha de fin (inclusiva)</param>
        /// <returns>Número total de citas</returns>
        Task<int> CountByPatientIdAndDateRangeAsync(Guid patientId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Obtiene el total de citas para un doctor en un rango de fechas
        /// </summary>
        /// <param name="doctorId">ID del doctor</param>
        /// <param name="startDate">Fecha de inicio (inclusiva)</param>
        /// <param name="endDate">Fecha de fin (inclusiva)</param>
        /// <returns>Número total de citas</returns>
        Task<int> CountByDoctorIdAndDateRangeAsync(Guid doctorId, DateTime startDate, DateTime endDate);
    }
} 