using System;
using System.Linq.Expressions;
using Odoonto.Domain.Core.Specifications;
using Odoonto.Domain.Models.Appointments;
using Odoonto.Domain.Models.ValueObjects;

namespace Odoonto.Domain.Specifications.Appointments
{
    /// <summary>
    /// Especificación para buscar citas que se superpongan con un horario específico
    /// </summary>
    public class AppointmentOverlapSpecification : BaseSpecification<Appointment>
    {
        /// <summary>
        /// Constructor para crear una especificación de superposición de citas
        /// </summary>
        /// <param name="doctorId">ID del doctor</param>
        /// <param name="date">Fecha de la cita</param>
        /// <param name="timeSlot">Franja horaria</param>
        /// <param name="excludeAppointmentId">ID de cita a excluir (opcional, para actualizaciones)</param>
        public AppointmentOverlapSpecification(
            Guid doctorId,
            DateTime date,
            TimeSlot timeSlot,
            Guid? excludeAppointmentId = null)
        {
            if (doctorId == Guid.Empty)
                throw new ArgumentException("El ID del doctor no puede estar vacío", nameof(doctorId));

            if (timeSlot == null)
                throw new ArgumentException("La franja horaria no puede ser nula", nameof(timeSlot));

            // Normalizar fecha
            date = date.Date;

            // Criterio de doctor y fecha
            Expression<Func<Appointment, bool>> doctorAndDateCriteria = a =>
                a.DoctorId == doctorId &&
                a.Date.Date == date;

            // Criterio de superposición de horarios usando el método Overlaps de TimeSlot
            Expression<Func<Appointment, bool>> overlapCriteria = a =>
                a.TimeSlot.Overlaps(timeSlot);

            // Criterio para excluir una cita (para actualizaciones)
            Expression<Func<Appointment, bool>> excludeCriteria = a => true;
            if (excludeAppointmentId.HasValue)
            {
                excludeCriteria = a => a.Id != excludeAppointmentId.Value;
            }

            // Combinar todos los criterios
            Criteria = a =>
                doctorAndDateCriteria.Compile()(a) &&
                overlapCriteria.Compile()(a) &&
                excludeCriteria.Compile()(a);
        }
    }
}