using System;
using System.Threading.Tasks;
using Odoonto.Domain.Models.ValueObjects;

namespace Odoonto.Domain.Services.Appointments
{
    /// <summary>
    /// Servicio de dominio para verificar superposición de citas
    /// </summary>
    public interface IAppointmentOverlapService
    {
        /// <summary>
        /// Verifica si hay citas superpuestas para un doctor en una fecha y horario específicos
        /// </summary>
        /// <param name="doctorId">ID del doctor</param>
        /// <param name="date">Fecha de la cita</param>
        /// <param name="timeSlot">Franja horaria</param>
        /// <param name="excludeAppointmentId">ID de cita a excluir (opcional, para actualizaciones)</param>
        /// <returns>True si hay superposición, False si está disponible</returns>
        Task<bool> HasOverlappingAppointmentsAsync(Guid doctorId, DateTime date, TimeSlot timeSlot, Guid? excludeAppointmentId = null);
    }
}