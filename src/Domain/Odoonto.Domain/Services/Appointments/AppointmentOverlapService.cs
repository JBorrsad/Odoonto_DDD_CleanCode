using System;
using System.Linq;
using System.Threading.Tasks;
using Odoonto.Domain.Core.Repositories;
using Odoonto.Domain.Models.Appointments;
using Odoonto.Domain.Models.ValueObjects;
using Odoonto.Domain.Specifications.Appointments;

namespace Odoonto.Domain.Services.Appointments
{
    /// <summary>
    /// Implementación del servicio de dominio para verificar superposición de citas
    /// </summary>
    public class AppointmentOverlapService : IAppointmentOverlapService
    {
        private readonly IRepository<Appointment> _appointmentRepository;

        public AppointmentOverlapService(IRepository<Appointment> appointmentRepository)
        {
            _appointmentRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
        }

        public async Task<bool> HasOverlappingAppointmentsAsync(Guid doctorId, DateTime date, TimeSlot timeSlot, Guid? excludeAppointmentId = null)
        {
            if (doctorId == Guid.Empty || timeSlot == null)
                return false;

            // Crear la especificación para buscar citas superpuestas
            var overlapSpec = new AppointmentOverlapSpecification(doctorId, date, timeSlot, excludeAppointmentId);

            // Verificar si existe al menos una cita que cumpla la especificación
            return await _appointmentRepository.AnyAsync(overlapSpec);
        }
    }
}