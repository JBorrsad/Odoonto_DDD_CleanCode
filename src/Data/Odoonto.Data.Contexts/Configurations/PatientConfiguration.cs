using System;
using System.Collections.Generic;
using System.Linq;
using Google.Cloud.Firestore;
using Odoonto.Domain.Models.Patients;
using Odoonto.Domain.Models.ValueObjects;

namespace Odoonto.Data.Contexts.Configurations
{
    /// <summary>
    /// Configuración para el mapeo entre documentos Firestore y la entidad Patient
    /// </summary>
    public static class PatientConfiguration
    {
        /// <summary>
        /// Mapea un documento Firestore a una entidad Patient
        /// </summary>
        public static Patient MapToEntity(DocumentSnapshot document)
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

        /// <summary>
        /// Mapea una entidad Patient a un documento Firestore
        /// </summary>
        public static Dictionary<string, object> MapToDocument(Patient entity)
        {
            if (entity == null)
                return null;
                
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