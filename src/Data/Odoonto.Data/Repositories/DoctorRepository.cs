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
    /// Implementación del repositorio de doctores usando Firebase
    /// </summary>
    public class DoctorRepository : IDoctorRepository
    {
        private readonly FirebaseDbContext _firebaseContext;
        private readonly string _collectionName = "doctors";

        /// <summary>
        /// Constructor con inyección del contexto de Firebase
        /// </summary>
        /// <param name="firebaseContext">Contexto de Firebase</param>
        public DoctorRepository(FirebaseDbContext firebaseContext)
        {
            _firebaseContext = firebaseContext ?? throw new ArgumentNullException(nameof(firebaseContext));
        }

        /// <summary>
        /// Obtiene todos los doctores
        /// </summary>
        /// <returns>Lista de doctores</returns>
        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            var snapshot = await _firebaseContext.GetCollection(_collectionName).GetSnapshotAsync();
            var doctors = new List<Doctor>();

            foreach (var document in snapshot.Documents)
            {
                var doctor = ConvertToEntity(document);
                if (doctor != null)
                {
                    doctors.Add(doctor);
                }
            }

            return doctors;
        }

        /// <summary>
        /// Obtiene un doctor por su ID
        /// </summary>
        /// <param name="id">ID del doctor</param>
        /// <returns>Doctor encontrado o null</returns>
        public async Task<Doctor> GetByIdAsync(Guid id)
        {
            var docSnapshot = await _firebaseContext.GetDocumentAsync(_collectionName, id.ToString());
            
            if (!docSnapshot.Exists)
            {
                return null;
            }

            return ConvertToEntity(docSnapshot);
        }

        /// <summary>
        /// Crea un nuevo doctor
        /// </summary>
        /// <param name="doctor">Entidad doctor</param>
        /// <returns>Task</returns>
        public async Task CreateAsync(Doctor doctor)
        {
            if (doctor == null)
            {
                throw new ArgumentNullException(nameof(doctor));
            }

            var doctorData = ConvertToDocument(doctor);
            await _firebaseContext.SetDocumentAsync(_collectionName, doctor.Id.ToString(), doctorData);
        }

        /// <summary>
        /// Actualiza un doctor existente
        /// </summary>
        /// <param name="doctor">Entidad doctor</param>
        /// <returns>Task</returns>
        public async Task UpdateAsync(Doctor doctor)
        {
            if (doctor == null)
            {
                throw new ArgumentNullException(nameof(doctor));
            }

            var doctorData = ConvertToDocument(doctor);
            await _firebaseContext.SetDocumentAsync(_collectionName, doctor.Id.ToString(), doctorData);
        }

        /// <summary>
        /// Elimina un doctor
        /// </summary>
        /// <param name="id">ID del doctor</param>
        /// <returns>Task</returns>
        public async Task DeleteAsync(Guid id)
        {
            await _firebaseContext.DeleteDocumentAsync(_collectionName, id.ToString());
        }

        /// <summary>
        /// Busca doctores por especialidad
        /// </summary>
        /// <param name="specialty">Especialidad</param>
        /// <returns>Lista de doctores</returns>
        public async Task<IEnumerable<Doctor>> GetBySpecialtyAsync(string specialty)
        {
            var query = _firebaseContext.GetCollection(_collectionName)
                .WhereEqualTo("Specialty", specialty);

            var snapshot = await query.GetSnapshotAsync();
            var doctors = new List<Doctor>();

            foreach (var document in snapshot.Documents)
            {
                var doctor = ConvertToEntity(document);
                if (doctor != null)
                {
                    doctors.Add(doctor);
                }
            }

            return doctors;
        }

        /// <summary>
        /// Busca doctores por término de búsqueda
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda</param>
        /// <returns>Lista de doctores</returns>
        public async Task<IEnumerable<Doctor>> SearchByTermAsync(string searchTerm)
        {
            // Firestore no soporta búsquedas parciales de texto, implementamos filtrado manual
            var snapshot = await _firebaseContext.GetCollection(_collectionName).GetSnapshotAsync();
            var doctors = new List<Doctor>();
            var searchTermLower = searchTerm.ToLowerInvariant();

            foreach (var document in snapshot.Documents)
            {
                var fullName = document.GetValue<string>("FullName")?.ToLowerInvariant() ?? "";
                var specialty = document.GetValue<string>("Specialty")?.ToLowerInvariant() ?? "";
                
                if (fullName.Contains(searchTermLower) || specialty.Contains(searchTermLower))
                {
                    var doctor = ConvertToEntity(document);
                    if (doctor != null)
                    {
                        doctors.Add(doctor);
                    }
                }
            }

            return doctors;
        }

        /// <summary>
        /// Verifica si existe un doctor con el ID dado
        /// </summary>
        public async Task<bool> ExistsAsync(Guid id)
        {
            var snapshot = await _firebaseContext.GetDocumentAsync(_collectionName, id.ToString());
            return snapshot.Exists;
        }

        /// <summary>
        /// Busca doctores por correo electrónico
        /// </summary>
        public async Task<Doctor> FindByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email no puede estar vacío", nameof(email));

            var collection = _firebaseContext.GetCollection(_collectionName);
            var query = collection.WhereEqualTo("contactInfo.email", email.ToLowerInvariant());
            var snapshot = await query.GetSnapshotAsync();

            return snapshot.Documents.Select(MapToEntity).FirstOrDefault();
        }

        /// <summary>
        /// Busca doctores por número de licencia
        /// </summary>
        public async Task<Doctor> FindByLicenseNumberAsync(string licenseNumber)
        {
            if (string.IsNullOrWhiteSpace(licenseNumber))
                throw new ArgumentException("El número de licencia no puede estar vacío", nameof(licenseNumber));

            var collection = _firebaseContext.GetCollection(_collectionName);
            var query = collection.WhereEqualTo("licenseNumber", licenseNumber);
            var snapshot = await query.GetSnapshotAsync();

            return snapshot.Documents.Select(MapToEntity).FirstOrDefault();
        }

        /// <summary>
        /// Busca doctores por su nombre
        /// </summary>
        public async Task<IEnumerable<Doctor>> FindByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre no puede estar vacío", nameof(name));

            var collection = _firebaseContext.GetCollection(_collectionName);
            var query = collection.WhereGreaterThanOrEqualTo("fullName.fullNameWithFirstNameFirst", name)
                                .WhereLessThanOrEqualTo("fullName.fullNameWithFirstNameFirst", name + "\uf8ff");
            
            var snapshot = await query.GetSnapshotAsync();

            return snapshot.Documents.Select(MapToEntity);
        }

        /// <summary>
        /// Busca doctores por especialidad
        /// </summary>
        public async Task<IEnumerable<Doctor>> FindBySpecialtyAsync(string specialty)
        {
            if (string.IsNullOrWhiteSpace(specialty))
                throw new ArgumentException("La especialidad no puede estar vacía", nameof(specialty));

            var collection = _firebaseContext.GetCollection(_collectionName);
            var query = collection.WhereEqualTo("specialty", specialty);
            var snapshot = await query.GetSnapshotAsync();

            return snapshot.Documents.Select(MapToEntity);
        }

        /// <summary>
        /// Obtiene doctores disponibles en una fecha y hora específicas
        /// </summary>
        public async Task<IEnumerable<Doctor>> GetAvailableDoctorsAsync(DateTime date, TimeOnly startTime, TimeOnly endTime)
        {
            // Obtenemos todos los doctores y filtramos en memoria
            // Para una implementación más eficiente, se podría agregar un campo de índice en Firebase
            var doctors = await GetAllAsync();
            var timeSlot = new TimeSlot(startTime, endTime);

            return doctors.Where(d => d.IsAvailable(date, timeSlot));
        }

        /// <summary>
        /// Obtiene doctores con paginación
        /// </summary>
        public async Task<IEnumerable<Doctor>> GetPaginatedAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                throw new ArgumentException("El número de página debe ser mayor o igual a 1", nameof(pageNumber));

            if (pageSize < 1)
                throw new ArgumentException("El tamaño de página debe ser mayor o igual a 1", nameof(pageSize));

            var collection = _firebaseContext.GetCollection(_collectionName);
            var snapshot = await collection.Limit(pageSize).Offset((pageNumber - 1) * pageSize).GetSnapshotAsync();

            return snapshot.Documents.Select(MapToEntity);
        }

        /// <summary>
        /// Obtiene la cantidad total de doctores
        /// </summary>
        public async Task<int> GetTotalDoctorsCountAsync()
        {
            var collection = _firebaseContext.GetCollection(_collectionName);
            var snapshot = await collection.GetSnapshotAsync();
            return snapshot.Count;
        }

        /// <summary>
        /// Verifica la disponibilidad de un doctor en un día y horario específico
        /// </summary>
        public async Task<bool> IsAvailableAsync(Guid doctorId, DateTime date, TimeSlot timeSlot)
        {
            var doctor = await GetByIdAsync(doctorId);
            if (doctor == null)
                return false;

            return doctor.IsAvailable(date, timeSlot);
        }

        /// <summary>
        /// Busca doctores por cualquier coincidencia en sus datos
        /// </summary>
        public async Task<IEnumerable<Doctor>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                throw new ArgumentException("El término de búsqueda no puede estar vacío", nameof(searchTerm));

            // Búsqueda por nombre
            var byName = await SearchByNameAsync(searchTerm);
            // Búsqueda por especialidad
            var bySpecialty = await FindBySpecialtyAsync(searchTerm);

            // Combinar resultados y eliminar duplicados
            return byName.Concat(bySpecialty).DistinctBy(d => d.Id);
        }

        /// <summary>
        /// Busca doctores por nombre o apellido
        /// </summary>
        public async Task<IEnumerable<Doctor>> SearchByNameAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                throw new ArgumentException("El término de búsqueda no puede estar vacío", nameof(searchTerm));

            // Búsqueda en nombres
            var collection = _firebaseContext.GetCollection(_collectionName);
            var queryFirstName = collection.WhereGreaterThanOrEqualTo("fullName.firstNames", searchTerm)
                                          .WhereLessThanOrEqualTo("fullName.firstNames", searchTerm + "\uf8ff");
            
            var queryLastName = collection.WhereGreaterThanOrEqualTo("fullName.lastNames", searchTerm)
                                         .WhereLessThanOrEqualTo("fullName.lastNames", searchTerm + "\uf8ff");

            var snapshotFirstName = await queryFirstName.GetSnapshotAsync();
            var snapshotLastName = await queryLastName.GetSnapshotAsync();

            // Combinar resultados
            var byFirstName = snapshotFirstName.Documents.Select(MapToEntity);
            var byLastName = snapshotLastName.Documents.Select(MapToEntity);

            return byFirstName.Concat(byLastName).DistinctBy(d => d.Id);
        }

        #region Private Methods

        /// <summary>
        /// Convierte un documento Firestore a una entidad Doctor
        /// </summary>
        private Doctor ConvertToEntity(DocumentSnapshot document)
        {
            try
            {
                var id = Guid.Parse(document.Id);
                var fullName = document.GetValue<string>("FullName");
                var specialty = document.GetValue<string>("Specialty");
                var licenseNumber = document.GetValue<string>("LicenseNumber");
                
                // Obtener datos del contacto
                var contactInfoDict = document.GetValue<Dictionary<string, object>>("ContactInfo");
                var email = contactInfoDict != null && contactInfoDict.ContainsKey("Email") 
                    ? contactInfoDict["Email"].ToString() 
                    : string.Empty;
                var phone = contactInfoDict != null && contactInfoDict.ContainsKey("PhoneNumber") 
                    ? contactInfoDict["PhoneNumber"].ToString() 
                    : string.Empty;
                
                var contactInfo = ContactInfo.Create(email, phone);

                // Crear el doctor usando el factory method
                return Doctor.Create(id, fullName, specialty, licenseNumber, contactInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al convertir documento a Doctor: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Convierte una entidad Doctor a un documento Firestore
        /// </summary>
        private Dictionary<string, object> ConvertToDocument(Doctor doctor)
        {
            return new Dictionary<string, object>
            {
                { "FullName", doctor.FullName },
                { "Specialty", doctor.Specialty },
                { "LicenseNumber", doctor.LicenseNumber },
                { "ContactInfo", new Dictionary<string, object> 
                    {
                        { "Email", doctor.ContactInfo.Email },
                        { "PhoneNumber", doctor.ContactInfo.PhoneNumber }
                    }
                },
                { "CreatedAt", DateTime.UtcNow },
                { "UpdatedAt", DateTime.UtcNow }
            };
        }

        /// <summary>
        /// Convierte un doctor a un documento de Firestore
        /// </summary>
        private Dictionary<string, object> MapToDocument(Doctor doctor)
        {
            var data = new Dictionary<string, object>
            {
                ["id"] = doctor.Id.ToString(),
                ["specialty"] = doctor.Specialty,
                ["createdAt"] = doctor.CreatedAt,
                ["updatedAt"] = doctor.UpdatedAt
            };

            // Mapear FullName si existe
            if (doctor.FullName != null)
            {
                data["fullName"] = new Dictionary<string, object>
                {
                    ["firstNames"] = doctor.FullName.FirstNames,
                    ["lastNames"] = doctor.FullName.LastNames,
                    ["fullNameWithFirstNameFirst"] = doctor.FullName.FullNameWithFirstNameFirst,
                    ["fullNameWithLastNameFirst"] = doctor.FullName.FullNameWithLastNameFirst
                };
            }

            // Mapear ContactInfo si existe
            if (doctor.ContactInfo != null)
            {
                data["contactInfo"] = new Dictionary<string, object>
                {
                    ["email"] = doctor.ContactInfo.Email ?? "",
                    ["phoneNumber"] = doctor.ContactInfo.PhoneNumber ?? "",
                    ["address"] = doctor.ContactInfo.Address ?? ""
                };
            }

            // Mapear Availability si existe
            if (doctor.Availability != null)
            {
                var availabilityDict = new Dictionary<string, object>();
                
                foreach (var day in Enum.GetValues<DayOfWeek>())
                {
                    var slots = doctor.Availability.GetTimeSlots(day)
                        .Select(ts => new Dictionary<string, object>
                        {
                            ["startTime"] = $"{ts.StartTime.Hour}:{ts.StartTime.Minute}",
                            ["endTime"] = $"{ts.EndTime.Hour}:{ts.EndTime.Minute}"
                        })
                        .ToList();
                    
                    availabilityDict[day.ToString()] = slots;
                }
                
                data["availability"] = availabilityDict;
            }

            return data;
        }

        /// <summary>
        /// Convierte un documento de Firestore a un doctor
        /// </summary>
        private Doctor MapToEntity(DocumentSnapshot snapshot)
        {
            var id = Guid.Parse(snapshot.Id);
            var doctor = Doctor.Create(id);

            if (snapshot.TryGetValue("specialty", out string specialty))
            {
                doctor.SetSpecialty(specialty);
            }

            // Mapear FullName
            if (snapshot.TryGetValue("fullName", out Dictionary<string, object> fullNameDict) && 
                fullNameDict != null)
            {
                string firstNames = fullNameDict.TryGetValue("firstNames", out object fnValue) ? 
                    fnValue.ToString() : "";
                string lastNames = fullNameDict.TryGetValue("lastNames", out object lnValue) ? 
                    lnValue.ToString() : "";

                if (!string.IsNullOrEmpty(firstNames) && !string.IsNullOrEmpty(lastNames))
                {
                    doctor.SetFullName(firstNames, lastNames);
                }
            }

            // Mapear ContactInfo
            if (snapshot.TryGetValue("contactInfo", out Dictionary<string, object> contactDict) && 
                contactDict != null)
            {
                string email = contactDict.TryGetValue("email", out object emailValue) ? 
                    emailValue.ToString() : "";
                string phone = contactDict.TryGetValue("phoneNumber", out object phoneValue) ? 
                    phoneValue.ToString() : "";
                string address = contactDict.TryGetValue("address", out object addressValue) ? 
                    addressValue.ToString() : "";

                doctor.SetContactInfo(address, phone, email);
            }

            // Mapear Availability
            if (snapshot.TryGetValue("availability", out Dictionary<string, object> availabilityDict) && 
                availabilityDict != null)
            {
                var availability = new Dictionary<DayOfWeek, ICollection<TimeRange>>();
                
                foreach (var dayEntry in availabilityDict)
                {
                    if (Enum.TryParse<DayOfWeek>(dayEntry.Key, out var day) && 
                        dayEntry.Value is List<Dictionary<string, object>> slots)
                    {
                        var timeRanges = new List<TimeRange>();
                        
                        foreach (var slot in slots)
                        {
                            if (ParseTimeSlot(slot, out TimeSpan start, out TimeSpan end))
                            {
                                timeRanges.Add(new TimeRange(start, end));
                            }
                        }
                        
                        availability[day] = timeRanges;
                    }
                }
                
                doctor.SetAvailability(new WeeklyAvailability(availability));
            }

            // Si tenemos timestamp de creación y actualización, actualizar la entidad
            if (snapshot.TryGetValue("createdAt", out Timestamp createdAt))
            {
                doctor.SetCreatedAt(createdAt.ToDateTime());
            }

            if (snapshot.TryGetValue("updatedAt", out Timestamp updatedAt))
            {
                doctor.SetUpdatedAt(updatedAt.ToDateTime());
            }

            return doctor;
        }

        /// <summary>
        /// Parsea un slot de tiempo desde un diccionario
        /// </summary>
        private bool ParseTimeSlot(Dictionary<string, object> slot, out TimeSpan start, out TimeSpan end)
        {
            start = TimeSpan.Zero;
            end = TimeSpan.Zero;
            
            if (!slot.TryGetValue("startTime", out object startObj) || !slot.TryGetValue("endTime", out object endObj))
                return false;
                
            string startStr = startObj.ToString();
            string endStr = endObj.ToString();
            
            // Formateo esperado: "HH:MM"
            var startParts = startStr.Split(':');
            var endParts = endStr.Split(':');
            
            if (startParts.Length != 2 || endParts.Length != 2)
                return false;
                
            if (!int.TryParse(startParts[0], out int startHour) || !int.TryParse(startParts[1], out int startMinute))
                return false;
                
            if (!int.TryParse(endParts[0], out int endHour) || !int.TryParse(endParts[1], out int endMinute))
                return false;
                
            start = new TimeSpan(startHour, startMinute, 0);
            end = new TimeSpan(endHour, endMinute, 0);
            
            return true;
        }

        #endregion
    }
}