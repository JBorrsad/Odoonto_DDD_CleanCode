using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Odoonto.Domain.Core.Models.Exceptions;
using Odoonto.Domain.Models.Patients;
using Odoonto.Domain.Models.ValueObjects;
using Odoonto.Domain.Repositories;

namespace Odoonto.Data.Repositories
{
    /// <summary>
    /// Implementación del repositorio de pacientes utilizando Firebase Firestore.
    /// </summary>
    public class PatientRepository : IPatientRepository
    {
        private readonly FirestoreDb _firestoreDb;
        private readonly string _collectionName = "patients";

        public PatientRepository(FirestoreDb firestoreDb)
        {
            _firestoreDb = firestoreDb ?? throw new ArgumentNullException(nameof(firestoreDb));
        }

        /// <summary>
        /// Obtiene un paciente por su identificador.
        /// </summary>
        public async Task<Patient> GetByIdAsync(Guid id)
        {
            var documentSnapshot = await _firestoreDb
                .Collection(_collectionName)
                .Document(id.ToString())
                .GetSnapshotAsync();

            if (!documentSnapshot.Exists)
            {
                return null;
            }

            return ConvertToEntity(documentSnapshot);
        }

        /// <summary>
        /// Obtiene todos los pacientes.
        /// </summary>
        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            var querySnapshot = await _firestoreDb
                .Collection(_collectionName)
                .GetSnapshotAsync();

            return querySnapshot.Documents
                .Select(ConvertToEntity)
                .Where(p => p != null);
        }

        /// <summary>
        /// Agrega un nuevo paciente.
        /// </summary>
        public async Task AddAsync(Patient patient)
        {
            if (patient == null)
            {
                throw new ArgumentNullException(nameof(patient));
            }

            var documentReference = _firestoreDb
                .Collection(_collectionName)
                .Document(patient.Id.ToString());

            var documentSnapshot = await documentReference.GetSnapshotAsync();
            if (documentSnapshot.Exists)
            {
                throw new DomainException($"Ya existe un paciente con el ID {patient.Id}");
            }

            var patientData = ConvertToDocument(patient);
            await documentReference.SetAsync(patientData);
        }

        /// <summary>
        /// Actualiza un paciente existente.
        /// </summary>
        public async Task UpdateAsync(Patient patient)
        {
            if (patient == null)
            {
                throw new ArgumentNullException(nameof(patient));
            }

            var documentReference = _firestoreDb
                .Collection(_collectionName)
                .Document(patient.Id.ToString());

            var documentSnapshot = await documentReference.GetSnapshotAsync();
            if (!documentSnapshot.Exists)
            {
                throw new EntityNotFoundException($"No se encontró un paciente con el ID {patient.Id}");
            }

            var patientData = ConvertToDocument(patient);
            await documentReference.UpdateAsync(patientData);
        }

        /// <summary>
        /// Elimina un paciente por su identificador.
        /// </summary>
        public async Task DeleteAsync(Guid id)
        {
            var documentReference = _firestoreDb
                .Collection(_collectionName)
                .Document(id.ToString());

            var documentSnapshot = await documentReference.GetSnapshotAsync();
            if (!documentSnapshot.Exists)
            {
                throw new EntityNotFoundException($"No se encontró un paciente con el ID {id}");
            }

            await documentReference.DeleteAsync();
        }

        /// <summary>
        /// Busca pacientes por nombre.
        /// </summary>
        public async Task<IEnumerable<Patient>> SearchByNameAsync(string nameQuery)
        {
            if (string.IsNullOrWhiteSpace(nameQuery))
            {
                return await GetAllAsync();
            }

            var querySnapshot = await _firestoreDb
                .Collection(_collectionName)
                .WhereGreaterThanOrEqualTo("fullName.firstName", nameQuery)
                .WhereLessThanOrEqualTo("fullName.firstName", nameQuery + "~")
                .GetSnapshotAsync();

            return querySnapshot.Documents
                .Select(ConvertToEntity)
                .Where(p => p != null);
        }

        #region Helpers

        /// <summary>
        /// Convierte un documento de Firestore a una entidad Patient.
        /// </summary>
        private Patient ConvertToEntity(DocumentSnapshot document)
        {
            try
            {
                var id = Guid.Parse(document.Id);
                var patient = Patient.Create(id);

                var data = document.ToDictionary();

                // Asignar datos personales
                if (data.TryGetValue("fullName", out var fullNameObj) && fullNameObj is Dictionary<string, object> fullNameDict)
                {
                    var firstName = fullNameDict.TryGetValue("firstName", out var firstNameObj) ? firstNameObj.ToString() : string.Empty;
                    var lastName = fullNameDict.TryGetValue("lastName", out var lastNameObj) ? lastNameObj.ToString() : string.Empty;
                    var fullName = new FullName(firstName, lastName);
                    
                    var birthDate = data.TryGetValue("birthDate", out var birthDateObj) && birthDateObj is Timestamp timestamp
                        ? timestamp.ToDateTime()
                        : DateTime.Today;
                    
                    var gender = data.TryGetValue("gender", out var genderObj) ? genderObj.ToString() : string.Empty;
                    
                    patient.SetPersonalInfo(fullName, birthDate, gender);
                }

                // Asignar información de contacto
                if (data.TryGetValue("contactInfo", out var contactInfoObj) && contactInfoObj is Dictionary<string, object> contactInfoDict)
                {
                    var email = contactInfoDict.TryGetValue("email", out var emailObj) ? emailObj.ToString() : string.Empty;
                    var phone = contactInfoDict.TryGetValue("phone", out var phoneObj) ? phoneObj.ToString() : string.Empty;
                    var contactInfo = new ContactInfo(email, phone);
                    
                    patient.SetContactInfo(contactInfo);
                }

                // Asignar dirección
                if (data.TryGetValue("address", out var addressObj) && addressObj is Dictionary<string, object> addressDict)
                {
                    var street = addressDict.TryGetValue("street", out var streetObj) ? streetObj.ToString() : string.Empty;
                    var city = addressDict.TryGetValue("city", out var cityObj) ? cityObj.ToString() : string.Empty;
                    var state = addressDict.TryGetValue("state", out var stateObj) ? stateObj.ToString() : string.Empty;
                    var zipCode = addressDict.TryGetValue("zipCode", out var zipCodeObj) ? zipCodeObj.ToString() : string.Empty;
                    var country = addressDict.TryGetValue("country", out var countryObj) ? countryObj.ToString() : string.Empty;
                    
                    var address = new Address(street, city, state, zipCode, country);
                    patient.SetAddress(address);
                }

                // Asignar historia médica
                if (data.TryGetValue("medicalHistory", out var medicalHistoryObj))
                {
                    patient.SetMedicalHistory(medicalHistoryObj.ToString());
                }

                // Asignar fechas de creación y actualización
                if (data.TryGetValue("createdAt", out var createdAtObj) && createdAtObj is Timestamp createdAtTimestamp)
                {
                    patient.SetCreatedAt(createdAtTimestamp.ToDateTime());
                }

                if (data.TryGetValue("updatedAt", out var updatedAtObj) && updatedAtObj is Timestamp updatedAtTimestamp)
                {
                    patient.SetUpdatedAt(updatedAtTimestamp.ToDateTime());
                }

                return patient;
            }
            catch (Exception ex)
            {
                // Registrar el error y retornar null
                Console.WriteLine($"Error al convertir documento a entidad Patient: {ex}");
                return null;
            }
        }

        /// <summary>
        /// Convierte una entidad Patient a un documento de Firestore.
        /// </summary>
        private Dictionary<string, object> ConvertToDocument(Patient patient)
        {
            var data = new Dictionary<string, object>();

            // Datos personales
            if (patient.FullName != null)
            {
                data["fullName"] = new Dictionary<string, object>
                {
                    ["firstName"] = patient.FullName.FirstName,
                    ["lastName"] = patient.FullName.LastName
                };
            }

            data["birthDate"] = Timestamp.FromDateTime(patient.BirthDate);
            data["gender"] = patient.Gender;

            // Información de contacto
            if (patient.ContactInfo != null)
            {
                data["contactInfo"] = new Dictionary<string, object>
                {
                    ["email"] = patient.ContactInfo.Email,
                    ["phone"] = patient.ContactInfo.Phone
                };
            }

            // Dirección
            if (patient.Address != null)
            {
                data["address"] = new Dictionary<string, object>
                {
                    ["street"] = patient.Address.Street,
                    ["city"] = patient.Address.City,
                    ["state"] = patient.Address.State,
                    ["zipCode"] = patient.Address.ZipCode,
                    ["country"] = patient.Address.Country
                };
            }

            // Historia médica
            data["medicalHistory"] = patient.MedicalHistory;

            // Fechas de creación y actualización
            data["createdAt"] = Timestamp.FromDateTime(patient.CreatedAt);
            data["updatedAt"] = Timestamp.FromDateTime(DateTime.UtcNow);

            return data;
        }

        #endregion
    }
}