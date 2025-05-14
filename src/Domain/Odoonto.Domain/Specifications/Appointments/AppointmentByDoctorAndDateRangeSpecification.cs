using System;
using System.Linq.Expressions;
using Odoonto.Domain.Core.Specifications;
using Odoonto.Domain.Models.Appointments;

namespace Odoonto.Domain.Specifications.Appointments
{
    /// <summary>
    /// Especificación para buscar citas de un doctor en un rango de fechas
    /// </summary>
    public class AppointmentByDoctorAndDateRangeSpecification : BaseSpecification<Appointment>
    {
        /// <summary>
        /// Constructor para crear una especificación de citas por doctor y rango de fechas
        /// </summary>
        /// <param name="doctorId">ID del doctor</param>
        /// <param name="startDate">Fecha de inicio (inclusive)</param>
        /// <param name="endDate">Fecha de fin (inclusive)</param>
        public AppointmentByDoctorAndDateRangeSpecification(Guid doctorId, DateTime startDate, DateTime endDate)
        {
            if (doctorId == Guid.Empty)
                throw new ArgumentException("El ID del doctor no puede estar vacío", nameof(doctorId));

            // Normalizar fechas
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1).AddTicks(-1); // Hasta el final del día

            Criteria = a =>
                a.DoctorId == doctorId &&
                a.Date >= startDate &&
                a.Date <= endDate;
        }
    }
}