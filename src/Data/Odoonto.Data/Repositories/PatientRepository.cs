using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
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
            if (!document.Exists)
                return null;

            // Extraer datos del documento
            var data = document.ToDictionary();
            var id = Guid.Parse(document.Id);

            // Mapear valores primitivos
            var medicalHistory = data.ContainsKey("medicalHistory") ? data["medicalHistory"].ToString() : string.Empty;
            var notes = data.ContainsKey("notes") ? data["notes"].ToString() : string.Empty;
            var allergies = data.ContainsKey("allergies") && data["allergies"] is List<object> allergiesList 
                ? allergiesList.Select(a => a.ToString()).ToList() 
                : new List<string>();

            // Mapear campos de auditoría
            var createdAt = data.ContainsKey("createdAt") && data["createdAt"] is Timestamp createdTimestamp
                ? createdTimestamp.ToDateTime()
                : DateTime.UtcNow;
            
            var updatedAt = data.ContainsKey("updatedAt") && data["updatedAt"] is Timestamp updatedTimestamp
                ? updatedTimestamp.ToDateTime()
                : DateTime.UtcNow;

            // Mapear objetos complejos (value objects)
            FullName name = null;
            if (data.ContainsKey("name") && data["name"] is Dictionary<string, object> nameDict)
            {
                string firstName = nameDict.ContainsKey("firstName") ? nameDict["firstName"].ToString() : string.Empty;
                string lastName = nameDict.ContainsKey("lastName") ? nameDict["lastName"].ToString() : string.Empty;
                
                if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
                {
                    name = new FullName(firstName, lastName);
                }
            }

            Date dateOfBirth = null;
            if (data.ContainsKey("dateOfBirth") && data["dateOfBirth"] is Timestamp dobTimestamp)
            {
                dateOfBirth = new Date(dobTimestamp.ToDateTime());
            }

            Gender gender = Gender.NotSpecified;
            if (data.ContainsKey("gender") && data["gender"] is long genderValue)
            {
                gender = (Gender)genderValue;
            }

            ContactInfo contact = null;
            if (data.ContainsKey("contact") && data["contact"] is Dictionary<string, object> contactDict)
            {
                string address = contactDict.ContainsKey("address") ? contactDict["address"].ToString() : string.Empty;
                string phoneNumber = contactDict.ContainsKey("phoneNumber") ? contactDict["phoneNumber"].ToString() : string.Empty;
                string email = contactDict.ContainsKey("email") ? contactDict["email"].ToString() : string.Empty;
                
                contact = new ContactInfo(address, phoneNumber, email);
            }

            // Usar reflection para crear una instancia y establecer propiedades privadas
            var patient = Activator.CreateInstance(typeof(Patient), true) as Patient;
            
            // Establecer propiedades a través de métodos públicos o reflection
            typeof(Patient).GetProperty("Id").SetValue(patient, id);
            typeof(Patient).GetProperty("CreatedAt").SetValue(patient, createdAt);
            typeof(Patient).GetProperty("UpdatedAt").SetValue(patient, updatedAt);
            typeof(Patient).GetProperty("Name").SetValue(patient, name);
            typeof(Patient).GetProperty("DateOfBirth").SetValue(patient, dateOfBirth);
            typeof(Patient).GetProperty("Gender").SetValue(patient, gender);
            typeof(Patient).GetProperty("Contact").SetValue(patient, contact);
            typeof(Patient).GetProperty("MedicalHistory").SetValue(patient, medicalHistory);
            typeof(Patient).GetProperty("Notes").SetValue(patient, notes);
            typeof(Patient).GetProperty("Allergies").SetValue(patient, allergies);

            return patient;
        }

        protected override object MapToDocument(Patient entity)
        {
            // Crear diccionario para Firestore
            var documentData = new Dictionary<string, object>
            {
                ["createdAt"] = Timestamp.FromDateTime(DateTime.SpecifyKind(entity.CreatedAt, DateTimeKind.Utc)),
                ["updatedAt"] = Timestamp.FromDateTime(DateTime.SpecifyKind(entity.UpdatedAt, DateTimeKind.Utc)),
                ["medicalHistory"] = entity.MedicalHistory ?? string.Empty,
                ["notes"] = entity.Notes ?? string.Empty,
                ["allergies"] = entity.Allergies ?? new List<string>()
            };

            // Mapear objetos complejos (value objects)
            if (entity.Name != null)
            {
                documentData["name"] = new Dictionary<string, object>
                {
                    ["firstName"] = entity.Name.FirstName,
                    ["lastName"] = entity.Name.LastName
                };
            }

            if (entity.DateOfBirth != null)
            {
                documentData["dateOfBirth"] = Timestamp.FromDateTime(
                    DateTime.SpecifyKind(entity.DateOfBirth.Value, DateTimeKind.Utc));
            }

            documentData["gender"] = (int)entity.Gender;

            if (entity.Contact != null)
            {
                documentData["contact"] = new Dictionary<string, object>
                {
                    ["address"] = entity.Contact.Address ?? string.Empty,
                    ["phoneNumber"] = entity.Contact.PhoneNumber ?? string.Empty,
                    ["email"] = entity.Contact.Email ?? string.Empty
                };
            }

            return documentData;
        }
    }
} 