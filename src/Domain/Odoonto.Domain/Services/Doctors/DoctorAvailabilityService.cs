using System;
using System.Threading.Tasks;
using Odoonto.Domain.Core.Repositories;
using Odoonto.Domain.Models.Doctors;
using Odoonto.Domain.Models.ValueObjects;
using Odoonto.Domain.Specifications.Doctors;

namespace Odoonto.Domain.Services.Doctors
{
    /// <summary>
    /// Implementación del servicio de disponibilidad de doctores
    /// </summary>
    public class DoctorAvailabilityService : IDoctorAvailabilityService
    {
        private readonly IRepository<Doctor> _doctorRepository;

        public DoctorAvailabilityService(IRepository<Doctor> doctorRepository)
        {
            _doctorRepository = doctorRepository ?? throw new ArgumentNullException(nameof(doctorRepository));
        }

        public async Task<bool> IsAvailableAsync(Guid doctorId, DateTime date, TimeSlot timeSlot)
        {
            if (doctorId == Guid.Empty || timeSlot == null)
                return false;

            // Obtener el doctor
            var doctor = await _doctorRepository.GetByIdAsync(doctorId);
            if (doctor == null)
                return false;

            // Verificar si el doctor tiene disponibilidad configurada
            if (doctor.Availability == null)
                return false;

            // Verificar la disponibilidad usando el método del dominio
            return doctor.IsAvailable(date, timeSlot);
        }
    }
}