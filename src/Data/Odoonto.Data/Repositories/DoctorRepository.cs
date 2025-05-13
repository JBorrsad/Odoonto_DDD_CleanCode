using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Odoonto.Data.Contexts.Configurations;
using Odoonto.Data.Core.Contexts;
using Odoonto.Data.Core.Repositories;
using Odoonto.Domain.Models.Doctors;
using Odoonto.Domain.Models.ValueObjects;
using Odoonto.Domain.Repositories;

namespace Odoonto.Data.Repositories
{
    /// <summary>
    /// Implementación del repositorio de doctores utilizando Firestore
    /// </summary>
    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {
        private const string CollectionName = "doctors";

        public DoctorRepository(FirestoreContext context) 
            : base(context, CollectionName)
        {
        }

        public async Task<IEnumerable<Doctor>> FindByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Enumerable.Empty<Doctor>();

            var allDoctors = await GetAllAsync();
            
            // Normalizar para la búsqueda (quitar acentos, minúsculas)
            string normalizedSearch = name.ToLowerInvariant().Trim();
            
            return allDoctors.Where(d => 
                (d.FullName?.FirstName?.ToLowerInvariant()?.Contains(normalizedSearch) == true) || 
                (d.FullName?.LastName?.ToLowerInvariant()?.Contains(normalizedSearch) == true));
        }

        public async Task<IEnumerable<Doctor>> FindBySpecialtyAsync(string specialty)
        {
            if (string.IsNullOrWhiteSpace(specialty))
                return Enumerable.Empty<Doctor>();

            var allDoctors = await GetAllAsync();
            
            // Normalizar para la búsqueda (quitar acentos, minúsculas)
            string normalizedSearch = specialty.ToLowerInvariant().Trim();
            
            return allDoctors.Where(d => 
                d.Specialty?.ToLowerInvariant()?.Contains(normalizedSearch) == true);
        }

        public async Task<Doctor> FindByLicenseNumberAsync(string licenseNumber)
        {
            if (string.IsNullOrWhiteSpace(licenseNumber))
                return null;

            var allDoctors = await GetAllAsync();
            
            // En este ejemplo no tenemos licenseNumber en el modelo, se agregaría en una implementación real
            // Por ahora, simplemente devolvemos null
            return null;
        }

        public async Task<Doctor> FindByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var allDoctors = await GetAllAsync();
            
            return allDoctors.FirstOrDefault(d => 
                string.Equals(d.ContactInfo?.Email, email.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public async Task<int> GetTotalDoctorsCountAsync()
        {
            var snapshot = await _context.GetAllDocumentsAsync(_collectionName);
            return snapshot.Count;
        }

        public async Task<IEnumerable<Doctor>> GetPaginatedAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            // Firestore no tiene paginación nativa como SQL, debemos simularla
            var allDoctors = (await GetAllAsync()).ToList();
            
            int skipCount = (pageNumber - 1) * pageSize;
            return allDoctors.Skip(skipCount).Take(pageSize);
        }

        public async Task<IEnumerable<Doctor>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            var normalizedSearch = searchTerm.ToLowerInvariant().Trim();
            var allDoctors = await GetAllAsync();
            
            return allDoctors.Where(d => 
                (d.FullName?.FirstName?.ToLowerInvariant()?.Contains(normalizedSearch) == true) ||
                (d.FullName?.LastName?.ToLowerInvariant()?.Contains(normalizedSearch) == true) ||
                (d.ContactInfo?.Email?.ToLowerInvariant()?.Contains(normalizedSearch) == true) ||
                (d.ContactInfo?.PhoneNumber?.Contains(normalizedSearch) == true) ||
                (d.ContactInfo?.Address?.ToLowerInvariant()?.Contains(normalizedSearch) == true) ||
                (d.Specialty?.ToLowerInvariant()?.Contains(normalizedSearch) == true));
        }

        public async Task<bool> IsAvailableAsync(Guid doctorId, DateTime date, TimeSlot timeSlot)
        {
            if (doctorId == Guid.Empty || timeSlot == null)
                return false;

            var doctor = await GetByIdAsync(doctorId);
            if (doctor == null)
                return false;

            return doctor.IsAvailable(date, timeSlot);
        }

        public async Task<IEnumerable<Doctor>> SearchByNameAsync(string searchTerm)
        {
            // Esta es esencialmente la misma implementación que FindByNameAsync
            return await FindByNameAsync(searchTerm);
        }

        protected override Doctor MapToEntity(DocumentSnapshot document)
        {
            return DoctorConfiguration.MapToEntity(document);
        }

        protected override object MapToDocument(Doctor entity)
        {
            return DoctorConfiguration.MapToDocument(entity);
        }
    }
} 