using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Odoonto.Data.Core.Contexts;
using Odoonto.Data.Core.Repositories;
using Odoonto.Data.Mappings;
using Odoonto.Domain.Core.Specifications;
using Odoonto.Domain.Models.Appointments;
using Odoonto.Domain.Models.ValueObjects;
using Odoonto.Domain.Repositories;
using Odoonto.Domain.Specifications.Appointments;

namespace Odoonto.Data.Repositories
{
    /// <summary>
    /// Implementación del repositorio de citas utilizando Firebase
    /// </summary>
    public class AppointmentRepository : BaseRepository<Appointment>, IAppointmentRepository
    {
        private readonly IAppointmentOverlapService _overlapService;

        public AppointmentRepository(FirestoreContext context)
            : base(context, "appointments")
        {
        }

        protected override Appointment ConvertToEntity(DocumentSnapshot document)
        {
            return AppointmentMapper.ToEntity(document);
        }

        protected override Dictionary<string, object> ConvertFromEntity(Appointment entity)
        {
            return AppointmentMapper.ToFirestore(entity);
        }

        public async Task<Appointment> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                return null;

            var document = await _context.GetDocumentByIdAsync(_collectionName, id.ToString());
            return document.Exists ? ConvertToEntity(document) : null;
        }

        public async Task<Appointment> GetByIdOrThrowAsync(Guid id)
        {
            var appointment = await GetByIdAsync(id);

            if (appointment == null)
            {
                throw new EntityNotFoundException($"Cita con ID {id} no encontrada.");
            }

            return appointment;
        }

        public async Task SaveAsync(Appointment appointment)
        {
            if (appointment == null)
                throw new ArgumentNullException(nameof(appointment));

            if (appointment.Id == Guid.Empty)
                throw new ArgumentException("La cita debe tener un ID asignado.");

            var data = ConvertFromEntity(appointment);
            await _context.SetDocumentAsync(_collectionName, appointment.Id.ToString(), data);
        }

        public async Task<IEnumerable<Appointment>> GetByPatientIdAndDateRangeAsync(Guid patientId, DateTime startDate, DateTime endDate)
        {
            var spec = new AppointmentByPatientAndDateRangeSpecification(patientId, startDate, endDate);
            return await FindAsync(spec);
        }

        public async Task<IEnumerable<Appointment>> GetByDoctorIdAndDateRangeAsync(Guid doctorId, DateTime startDate, DateTime endDate)
        {
            var spec = new AppointmentByDoctorAndDateRangeSpecification(doctorId, startDate, endDate);
            return await FindAsync(spec);
        }

        public async Task<IEnumerable<Appointment>> GetByDoctorIdAndDateAsync(Guid doctorId, DateTime date)
        {
            var spec = new AppointmentByDoctorAndDateSpecification(doctorId, date);
            return await FindAsync(spec);
        }

        /// <summary>
        /// Busca citas con paginación basada en una especificación
        /// </summary>
        public async Task<(IEnumerable<Appointment> Items, string NextPageToken)> FindPaginatedAsync(
            ISpecification<Appointment> specification,
            int pageSize = 20,
            string pageToken = null)
        {
            Query query = _context.Collection(_collectionName).Limit(pageSize);

            // Aplicar filtros de la especificación si es posible
            if (specification is AppointmentByPatientAndDateRangeSpecification patientSpec)
            {
                query = query.WhereEqualTo("PatientId", patientSpec.PatientId.ToString())
                             .WhereGreaterThanOrEqualTo("Date", Timestamp.FromDateTime(patientSpec.StartDate))
                             .WhereLessThanOrEqualTo("Date", Timestamp.FromDateTime(patientSpec.EndDate));
            }
            else if (specification is AppointmentByDoctorAndDateRangeSpecification doctorRangeSpec)
            {
                query = query.WhereEqualTo("DoctorId", doctorRangeSpec.DoctorId.ToString())
                             .WhereGreaterThanOrEqualTo("Date", Timestamp.FromDateTime(doctorRangeSpec.StartDate))
                             .WhereLessThanOrEqualTo("Date", Timestamp.FromDateTime(doctorRangeSpec.EndDate));
            }
            else if (specification is AppointmentByDoctorAndDateSpecification doctorDateSpec)
            {
                // Crear timestamp para la fecha específica
                var date = Timestamp.FromDateTime(doctorDateSpec.Date.Date);
                var nextDay = Timestamp.FromDateTime(doctorDateSpec.Date.Date.AddDays(1));

                query = query.WhereEqualTo("DoctorId", doctorDateSpec.DoctorId.ToString())
                             .WhereGreaterThanOrEqualTo("Date", date)
                             .WhereLessThan("Date", nextDay);
            }

            // Implementar token de paginación
            if (!string.IsNullOrEmpty(pageToken))
            {
                var startAfterDoc = await _context.GetDocumentByIdAsync(_collectionName, pageToken);
                if (startAfterDoc.Exists)
                {
                    query = query.StartAfter(startAfterDoc);
                }
            }

            // Ejecutamos la consulta
            var querySnapshot = await query.GetSnapshotAsync();

            if (querySnapshot.Count == 0)
            {
                return (Enumerable.Empty<Appointment>(), null);
            }

            // Convertimos los documentos a entidades
            var items = querySnapshot.Documents
                .Select(doc => ConvertToEntity(doc))
                .Where(appointment => specification.IsSatisfiedBy(appointment))
                .ToList();

            // Determinamos el token para la siguiente página
            string nextPageToken = querySnapshot.Count < pageSize
                ? null
                : querySnapshot.Documents.Last().Id;

            return (items, nextPageToken);
        }
    }
}