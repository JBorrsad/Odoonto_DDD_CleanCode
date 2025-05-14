using System;
using System.Linq.Expressions;
using Odoonto.Domain.Core.Specifications;
using Odoonto.Domain.Models.Appointments;

namespace Odoonto.Domain.Specifications.Appointments
{
    /// <summary>
    /// Especificación para buscar citas de un paciente en un rango de fechas
    /// </summary>
    public class AppointmentByPatientAndDateRangeSpecification : BaseSpecification<Appointment>
    {
        /// <summary>
        /// Constructor para crear una especificación de citas por paciente y rango de fechas
        /// </summary>
        /// <param name="patientId">ID del paciente</param>
        /// <param name="startDate">Fecha de inicio (inclusive)</param>
        /// <param name="endDate">Fecha de fin (inclusive)</param>
        public AppointmentByPatientAndDateRangeSpecification(Guid patientId, DateTime startDate, DateTime endDate)
        {
            if (patientId == Guid.Empty)
                throw new ArgumentException("El ID del paciente no puede estar vacío", nameof(patientId));

            // Normalizar fechas
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1).AddTicks(-1); // Hasta el final del día

            Criteria = a =>
                a.PatientId == patientId &&
                a.Date >= startDate &&
                a.Date <= endDate;
        }
    }
}