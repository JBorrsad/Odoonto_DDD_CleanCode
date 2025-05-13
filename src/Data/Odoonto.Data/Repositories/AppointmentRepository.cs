using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Odoonto.Data.Contexts.Configurations;
using Odoonto.Data.Core.Contexts;
using Odoonto.Data.Core.Repositories;
using Odoonto.Domain.Models.Appointments;
using Odoonto.Domain.Models.ValueObjects;
using Odoonto.Domain.Repositories;

namespace Odoonto.Data.Repositories
{
    /// <summary>
    /// Implementación del repositorio de citas utilizando Firestore
    /// </summary>
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        private const string CollectionName = "appointments";

        public AppointmentRepository(FirestoreContext context) 
            : base(context, CollectionName)
        {
        }

        public async Task<IEnumerable<Appointment>> GetByPatientIdAndDateRangeAsync(Guid patientId, DateTime startDate, DateTime endDate)
        {
            if (patientId == Guid.Empty || startDate > endDate)
                return Enumerable.Empty<Appointment>();

            var allAppointments = await GetAllAsync();
            
            return allAppointments.Where(a => 
                a.PatientId == patientId && 
                a.AppointmentDate.Value.Date >= startDate.Date && 
                a.AppointmentDate.Value.Date <= endDate.Date);
        }

        public async Task<IEnumerable<Appointment>> GetByDoctorIdAndDateRangeAsync(Guid doctorId, DateTime startDate, DateTime endDate)
        {
            if (doctorId == Guid.Empty || startDate > endDate)
                return Enumerable.Empty<Appointment>();

            var allAppointments = await GetAllAsync();
            
            return allAppointments.Where(a => 
                a.DoctorId == doctorId && 
                a.AppointmentDate.Value.Date >= startDate.Date && 
                a.AppointmentDate.Value.Date <= endDate.Date);
        }

        public async Task<IEnumerable<Appointment>> GetByDoctorIdAndDateAsync(Guid doctorId, DateTime date)
        {
            if (doctorId == Guid.Empty)
                return Enumerable.Empty<Appointment>();

            var allAppointments = await GetAllAsync();
            
            return allAppointments.Where(a => 
                a.DoctorId == doctorId && 
                a.AppointmentDate.Value.Date == date.Date);
        }

        public async Task<bool> HasOverlappingAppointmentsAsync(Guid doctorId, DateTime date, TimeSlot timeSlot, Guid? excludeAppointmentId = null)
        {
            if (doctorId == Guid.Empty || timeSlot == null)
                return false;

            var appointmentsOnDate = await GetByDoctorIdAndDateAsync(doctorId, date);
            
            return appointmentsOnDate.Any(a => 
                a.Id != excludeAppointmentId && // Excluir la cita que se está actualizando
                a.TimeSlot.Overlaps(timeSlot));
        }

        protected override Appointment MapToEntity(DocumentSnapshot document)
        {
            return AppointmentConfiguration.MapToEntity(document);
        }

        protected override object MapToDocument(Appointment entity)
        {
            return AppointmentConfiguration.MapToDocument(entity);
        }
    }
} 