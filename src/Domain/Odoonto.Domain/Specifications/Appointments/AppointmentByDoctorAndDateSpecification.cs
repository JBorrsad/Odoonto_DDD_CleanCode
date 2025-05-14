using System;
using System.Linq.Expressions;
using Odoonto.Domain.Core.Specifications;
using Odoonto.Domain.Models.Appointments;

namespace Odoonto.Domain.Specifications.Appointments
{
    /// <summary>
    /// Especificación para buscar citas de un doctor en una fecha específica
    /// </summary>
    public class AppointmentByDoctorAndDateSpecification : BaseSpecification<Appointment>
    {
        /// <summary>
        /// Constructor para crear una especificación de citas por doctor y fecha específica
        /// </summary>
        /// <param name="doctorId">ID del doctor</param>
        /// <param name="date">Fecha específica</param>
        public AppointmentByDoctorAndDateSpecification(Guid doctorId, DateTime date)
        {
            if (doctorId == Guid.Empty)
                throw new ArgumentException("El ID del doctor no puede estar vacío", nameof(doctorId));

            // Normalizar fecha
            date = date.Date;
            var endDate = date.AddDays(1).AddTicks(-1); // Hasta el final del día

            Criteria = a =>
                a.DoctorId == doctorId &&
                a.Date >= date &&
                a.Date <= endDate;
        }
    }
}