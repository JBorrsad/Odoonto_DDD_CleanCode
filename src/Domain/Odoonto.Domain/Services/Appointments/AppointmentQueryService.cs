using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Odoonto.Domain.Core.Repositories;
using Odoonto.Domain.Models.Appointments;
using Odoonto.Domain.Specifications.Appointments;

namespace Odoonto.Domain.Services.Appointments
{
    /// <summary>
    /// Implementación del servicio de consultas para citas
    /// </summary>
    public class AppointmentQueryService : IAppointmentQueryService
    {
        private readonly IRepository<Appointment> _appointmentRepository;

        public AppointmentQueryService(IRepository<Appointment> appointmentRepository)
        {
            _appointmentRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
        }

        public async Task<IEnumerable<Appointment>> GetByPatientIdAndDateRangeAsync(
            Guid patientId,
            DateTime startDate,
            DateTime endDate,
            int page = 1,
            int pageSize = 10)
        {
            var spec = new AppointmentByPatientAndDateRangeSpecification(patientId, startDate, endDate);
            return await _appointmentRepository.FindAsync(spec, page, pageSize);
        }

        public async Task<IEnumerable<Appointment>> GetByDoctorIdAndDateRangeAsync(
            Guid doctorId,
            DateTime startDate,
            DateTime endDate,
            int page = 1,
            int pageSize = 10)
        {
            var spec = new AppointmentByDoctorAndDateRangeSpecification(doctorId, startDate, endDate);
            return await _appointmentRepository.FindAsync(spec, page, pageSize);
        }

        public async Task<IEnumerable<Appointment>> GetByDoctorIdAndDateAsync(
            Guid doctorId,
            DateTime date,
            int page = 1,
            int pageSize = 10)
        {
            var spec = new AppointmentByDoctorAndDateSpecification(doctorId, date);
            return await _appointmentRepository.FindAsync(spec, page, pageSize);
        }

        public async Task<int> CountByPatientIdAndDateRangeAsync(Guid patientId, DateTime startDate, DateTime endDate)
        {
            var spec = new AppointmentByPatientAndDateRangeSpecification(patientId, startDate, endDate);
            return await _appointmentRepository.CountAsync(spec);
        }

        public async Task<int> CountByDoctorIdAndDateRangeAsync(Guid doctorId, DateTime startDate, DateTime endDate)
        {
            var spec = new AppointmentByDoctorAndDateRangeSpecification(doctorId, startDate, endDate);
            return await _appointmentRepository.CountAsync(spec);
        }
    }
}