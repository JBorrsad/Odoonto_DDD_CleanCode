using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Odoonto.Domain.Models.ValueObjects;

namespace Odoonto.Domain.Services.Appointments
{
    /// <summary>
    /// Interfaz para el servicio de programación de citas
    /// </summary>
    public interface IAppointmentSchedulingService
    {
        /// <summary>
        /// Obtiene los horarios disponibles para un doctor en una fecha específica
        /// </summary>
        /// <param name="doctorId">ID del doctor</param>
        /// <param name="date">Fecha para verificar disponibilidad</param>
        /// <param name="appointmentDurationInHalfHours">Duración de la cita en bloques de media hora</param>
        /// <returns>Lista de slots de tiempo disponibles</returns>
        Task<IEnumerable<TimeSlot>> GetAvailableSlotsAsync(
            Guid doctorId, 
            DateTime date, 
            int appointmentDurationInHalfHours = 1);

        /// <summary>
        /// Verifica si un slot de tiempo específico está disponible
        /// </summary>
        /// <param name="doctorId">ID del doctor</param>
        /// <param name="date">Fecha para verificar disponibilidad</param>
        /// <param name="timeSlot">Slot de tiempo a verificar</param>
        /// <param name="excludeAppointmentId">ID de cita a excluir de la verificación (opcional)</param>
        /// <returns>True si el slot está disponible, false en caso contrario</returns>
        Task<bool> IsSlotAvailableAsync(
            Guid doctorId, 
            DateTime date, 
            TimeSlot timeSlot, 
            Guid? excludeAppointmentId = null);
    }
} 