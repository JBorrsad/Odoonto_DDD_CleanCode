using System;
using System.Threading.Tasks;
using Odoonto.Domain.Models.ValueObjects;

namespace Odoonto.Domain.Services.Doctors
{
    /// <summary>
    /// Servicio de dominio para verificar la disponibilidad de doctores
    /// </summary>
    public interface IDoctorAvailabilityService
    {
        /// <summary>
        /// Verifica si un doctor está disponible en una fecha y franja horaria específicas
        /// </summary>
        /// <param name="doctorId">ID del doctor</param>
        /// <param name="date">Fecha</param>
        /// <param name="timeSlot">Franja horaria</param>
        /// <returns>True si está disponible, False si no está disponible</returns>
        Task<bool> IsAvailableAsync(Guid doctorId, DateTime date, TimeSlot timeSlot);
    }
}