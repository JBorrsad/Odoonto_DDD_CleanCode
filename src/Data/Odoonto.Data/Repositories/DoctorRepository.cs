using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Odoonto.Data.Core.Contexts;
using Odoonto.Data.Core.Repositories;
using Odoonto.Data.Mappings;
using Odoonto.Domain.Models.Doctors;
using Odoonto.Domain.Models.ValueObjects;
using Odoonto.Domain.Repositories;

namespace Odoonto.Data.Repositories
{
    /// <summary>
    /// Implementación del repositorio de doctores utilizando Firebase
    /// </summary>
    public class DoctorRepository : BaseRepository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(FirestoreContext context)
            : base(context, "doctors")
        {
        }

        protected override Doctor ConvertToEntity(DocumentSnapshot document)
        {
            return DoctorMapper.ToEntity(document);
        }

        protected override Dictionary<string, object> ConvertFromEntity(Doctor entity)
        {
            return DoctorMapper.ToFirestore(entity);
        }

        public async Task<IEnumerable<Doctor>> FindByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Enumerable.Empty<Doctor>();

            name = name.ToLower().Trim();

            // En Firestore, debemos buscar en memoria ya que no soporta operaciones LIKE de SQL
            var doctors = await GetAllAsync();

            return doctors.Where(d =>
                d.FullName.FirstName.ToLower().Contains(name) ||
                d.FullName.LastName.ToLower().Contains(name));
        }

        public async Task<IEnumerable<Doctor>> FindBySpecialtyAsync(string specialty)
        {
            if (string.IsNullOrWhiteSpace(specialty))
                return Enumerable.Empty<Doctor>();

            specialty = specialty.ToLower().Trim();

            // Crear una consulta con filtro por especialidad
            var query = _context.GetCollection(_collectionName)
                .WhereEqualTo("Specialty", specialty);

            var querySnapshot = await _context.QueryDocumentsAsync(query);

            return querySnapshot
                .Select(ConvertToEntity)
                .Where(d => d != null);
        }

        public async Task<Doctor> FindByLicenseNumberAsync(string licenseNumber)
        {
            if (string.IsNullOrWhiteSpace(licenseNumber))
                return null;

            licenseNumber = licenseNumber.Trim();

            // Crear una consulta con filtro por número de licencia
            var query = _context.GetCollection(_collectionName)
                .WhereEqualTo("LicenseNumber", licenseNumber);

            var querySnapshot = await _context.QueryDocumentsAsync(query);

            var doctor = querySnapshot.FirstOrDefault();
            return doctor != null ? ConvertToEntity(doctor) : null;
        }

        public async Task<Doctor> FindByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            email = email.ToLower().Trim();

            // Crear una consulta con filtro por email
            var query = _context.GetCollection(_collectionName)
                .WhereEqualTo("Email", email);

            var querySnapshot = await _context.QueryDocumentsAsync(query);

            var doctor = querySnapshot.FirstOrDefault();
            return doctor != null ? ConvertToEntity(doctor) : null;
        }

        public async Task<int> GetTotalDoctorsCountAsync()
        {
            var snapshot = await _context.GetAllDocumentsAsync(_collectionName);
            return snapshot.Count;
        }

        public async Task<IEnumerable<Doctor>> GetPaginatedAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                pageNumber = 1;

            if (pageSize < 1)
                pageSize = 10;

            // En Firestore, para implementar paginación necesitamos usar limit() y startAfter()
            // Primero obtenemos los documentos ordenados por un campo
            var query = _context.GetCollection(_collectionName)
                .OrderBy("LastName")
                .OrderBy("FirstName");

            // Si no es la primera página, necesitamos obtener el último documento de la página anterior
            if (pageNumber > 1)
            {
                // Obtener el último documento de la página anterior
                var lastDocOfPreviousPage = await query
                    .Limit((pageNumber - 1) * pageSize)
                    .GetSnapshotAsync();

                if (lastDocOfPreviousPage.Count > 0)
                {
                    var lastDoc = lastDocOfPreviousPage.Documents.Last();
                    query = query.StartAfter(lastDoc);
                }
            }

            // Obtener los documentos de la página actual
            var querySnapshot = await query
                .Limit(pageSize)
                .GetSnapshotAsync();

            return querySnapshot.Documents
                .Select(ConvertToEntity)
                .Where(d => d != null);
        }

        public async Task<IEnumerable<Doctor>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return Enumerable.Empty<Doctor>();

            searchTerm = searchTerm.ToLower().Trim();

            // En Firestore, debemos buscar en memoria
            var doctors = await GetAllAsync();

            return doctors.Where(d =>
                d.FullName.FirstName.ToLower().Contains(searchTerm) ||
                d.FullName.LastName.ToLower().Contains(searchTerm) ||
                (d.ContactInfo?.Email ?? "").ToLower().Contains(searchTerm) ||
                (d.ContactInfo?.Phone ?? "").Contains(searchTerm) ||
                (d.ContactInfo?.Address ?? "").ToLower().Contains(searchTerm) ||
                d.Specialty.ToLower().Contains(searchTerm));
        }

        public async Task<bool> IsAvailableAsync(Guid doctorId, DateTime date, TimeSlot timeSlot)
        {
            if (doctorId == Guid.Empty || timeSlot == null)
                return false;

            // Primero verificar si el doctor existe
            var doctor = await GetByIdAsync(doctorId);
            if (doctor == null)
                return false;

            // Verificar si la fecha y hora están dentro de la disponibilidad del doctor
            return doctor.IsAvailable(date, timeSlot);
        }

        public async Task<IEnumerable<Doctor>> SearchByNameAsync(string searchTerm)
        {
            // Este método es similar a FindByNameAsync, pero se mantiene para cumplir con la interfaz
            return await FindByNameAsync(searchTerm);
        }
    }
}