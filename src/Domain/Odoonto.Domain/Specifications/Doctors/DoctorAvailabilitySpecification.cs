using System;
using System.Linq.Expressions;
using Odoonto.Domain.Core.Specifications;
using Odoonto.Domain.Models.Doctors;
using Odoonto.Domain.Models.ValueObjects;

namespace Odoonto.Domain.Specifications.Doctors
{
    /// <summary>
    /// Especificación para verificar disponibilidad de doctores
    /// </summary>
    public class DoctorAvailabilitySpecification : BaseSpecification<Doctor>
    {
        /// <summary>
        /// Constructor para crear una especificación de disponibilidad
        /// </summary>
        /// <param name="date">Fecha para verificar disponibilidad</param>
        /// <param name="timeSlot">Franja horaria para verificar disponibilidad</param>
        public DoctorAvailabilitySpecification(DateTime date, TimeSlot timeSlot)
        {
            if (timeSlot == null)
                throw new ArgumentException("La franja horaria no puede ser nula", nameof(timeSlot));

            Criteria = d =>
                d.Availability != null &&
                d.IsAvailable(date, timeSlot);
        }
    }
}