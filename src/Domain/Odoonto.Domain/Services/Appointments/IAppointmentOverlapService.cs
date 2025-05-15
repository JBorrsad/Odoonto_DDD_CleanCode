using System;
using System.Threading.Tasks;
using Odoonto.Domain.Models.ValueObjects;

namespace Odoonto.Domain.Services.Appointments
{
    /// <summary>
    /// Interfaz para el servicio que verifica superposición de citas
    /// </summary>
    public interface IAppointmentOverlapService
    {
        /// <summary>
        /// Verifica si hay citas superpuestas para un doctor en una fecha y slot específicos
        /// </summary>
        /// <param name="doctorId">ID del doctor</param>
        /// <param name="date">Fecha para verificar</param>
        /// <param name="timeSlot">Slot de tiempo para verificar</param>
        /// <param name="excludeAppointmentId">ID de cita a excluir (opcional)</param>
        /// <returns>True si hay superposición, false en caso contrario</returns>
        Task<bool> HasOverlappingAppointmentsAsync(
            Guid doctorId, 
            DateTime date, 
            TimeSlot timeSlot, 
            Guid? excludeAppointmentId = null);
    }
}