using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Odoonto.Data.Contexts.Configurations;
using Odoonto.Data.Core.Contexts;
using Odoonto.Data.Core.Repositories;
using Odoonto.Domain.Models.Patients;
using Odoonto.Domain.Models.ValueObjects;
using Odoonto.Domain.Repositories;

namespace Odoonto.Data.Repositories
{
    /// <summary>
    /// Implementación del repositorio de pacientes utilizando Firestore
    /// </summary>
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        private const string CollectionName = "patients";

        public PatientRepository(FirestoreContext context) 
            : base(context, CollectionName)
        {
        }

        public async Task<IEnumerable<Patient>> FindByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Enumerable.Empty<Patient>();

            // Firebase no proporciona búsqueda de texto completo, así que obtenemos todos y filtramos
            var allPatients = await GetAllAsync();
            
            // Normalizar para la búsqueda (quitar acentos, minúsculas)
            string normalizedSearch = name.ToLowerInvariant().Trim();
            
            return allPatients.Where(p => 
                (p.Name?.FirstName?.ToLowerInvariant()?.Contains(normalizedSearch) == true) || 
                (p.Name?.LastName?.ToLowerInvariant()?.Contains(normalizedSearch) == true));
        }

        public async Task<IEnumerable<Patient>> FindByAgeRangeAsync(int minAge, int maxAge)
        {
            if (minAge < 0) minAge = 0;
            if (maxAge < minAge) maxAge = minAge;

            var allPatients = await GetAllAsync();
            return allPatients.Where(p => {
                int age = p.CalculateAge();
                return age >= minAge && age <= maxAge;
            });
        }

        public async Task<Patient> FindByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var allPatients = await GetAllAsync();
            return allPatients.FirstOrDefault(p => 
                string.Equals(p.Contact?.Email, email.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public async Task<IEnumerable<Patient>> FindByPhoneNumberAsync(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return Enumerable.Empty<Patient>();

            // Normalizar número de teléfono para búsqueda
            string normalizedPhone = phoneNumber.Trim();
            
            var allPatients = await GetAllAsync();
            return allPatients.Where(p => 
                p.Contact?.PhoneNumber?.Contains(normalizedPhone) == true);
        }

        public async Task<int> GetTotalPatientsCountAsync()
        {
            var snapshot = await _context.GetAllDocumentsAsync(_collectionName);
            return snapshot.Count;
        }

        public async Task<IEnumerable<Patient>> GetPaginatedAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            // Firestore no tiene paginación nativa como SQL, debemos simularla
            var allPatients = (await GetAllAsync()).ToList();
            
            int skipCount = (pageNumber - 1) * pageSize;
            return allPatients.Skip(skipCount).Take(pageSize);
        }

        public async Task<IEnumerable<Patient>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            var normalizedSearch = searchTerm.ToLowerInvariant().Trim();
            var allPatients = await GetAllAsync();
            
            return allPatients.Where(p => 
                (p.Name?.FirstName?.ToLowerInvariant()?.Contains(normalizedSearch) == true) ||
                (p.Name?.LastName?.ToLowerInvariant()?.Contains(normalizedSearch) == true) ||
                (p.Contact?.Email?.ToLowerInvariant()?.Contains(normalizedSearch) == true) ||
                (p.Contact?.PhoneNumber?.Contains(normalizedSearch) == true) ||
                (p.Contact?.Address?.ToLowerInvariant()?.Contains(normalizedSearch) == true) ||
                (p.MedicalHistory?.ToLowerInvariant()?.Contains(normalizedSearch) == true) ||
                (p.Notes?.ToLowerInvariant()?.Contains(normalizedSearch) == true) ||
                (p.Allergies?.Any(a => a.ToLowerInvariant().Contains(normalizedSearch)) == true));
        }

        protected override Patient MapToEntity(DocumentSnapshot document)
        {
            return PatientConfiguration.MapToEntity(document);
        }

        protected override object MapToDocument(Patient entity)
        {
            return PatientConfiguration.MapToDocument(entity);
        }
    }
} 