using System;
using System.Collections.Generic;
using Google.Cloud.Firestore;
using Odoonto.Domain.Models.Patients;
using Odoonto.Domain.Models.ValueObjects;

namespace Odoonto.Data.Mappings
{
    /// <summary>
    /// Clase para mapear entre documentos Firestore y entidades Patient
    /// </summary>
    public static class PatientMapper
    {
        /// <summary>
        /// Convierte un documento Firestore a una entidad Patient
        /// </summary>
        public static Patient ToEntity(DocumentSnapshot document)
        {
            if (document == null || !document.Exists)
                return null;

            var data = document.ToDictionary();
            var id = Guid.Parse(document.Id);

            // Extraer los value objects del documento
            var firstName = data.GetValueOrDefault("FirstName")?.ToString() ?? "";
            var lastName = data.GetValueOrDefault("LastName")?.ToString() ?? "";
            var fullName = new FullName(firstName, lastName);

            var dateOfBirthValue = data.GetValueOrDefault("DateOfBirth") is Timestamp timestamp
                ? timestamp.ToDateTime()
                : DateTime.MinValue;
            var dateOfBirth = new Date(dateOfBirthValue);

            var genderValue = data.GetValueOrDefault("Gender")?.ToString() ?? "NotSpecified";
            var gender = Gender.FromName(genderValue);

            // Extraer información de contacto
            var email = data.GetValueOrDefault("Email")?.ToString() ?? "";
            var phone = data.GetValueOrDefault("Phone")?.ToString() ?? "";
            var address = data.GetValueOrDefault("Address")?.ToString() ?? "";
            var contactInfo = new ContactInfo(email, phone, address);

            // Crear el paciente con la información básica
            var patient = new Patient(fullName, dateOfBirth, gender, contactInfo);

            // Asignar propiedades adicionales
            if (data.TryGetValue("Allergies", out var allergiesObj) && allergiesObj is List<object> allergies)
            {
                foreach (var allergy in allergies)
                {
                    patient.AddAllergy(allergy?.ToString() ?? "");
                }
            }

            var medicalHistory = data.GetValueOrDefault("MedicalHistory")?.ToString() ?? "";
            patient.UpdateMedicalHistory(medicalHistory);

            var notes = data.GetValueOrDefault("Notes")?.ToString() ?? "";
            patient.UpdateNotes(notes);

            // Método de reflexión para establecer el ID (ya que puede ser de solo lectura)
            typeof(Patient).GetProperty("Id").SetValue(patient, id);

            return patient;
        }

        /// <summary>
        /// Convierte una entidad Patient a un diccionario para Firestore
        /// </summary>
        public static Dictionary<string, object> ToFirestore(Patient patient)
        {
            if (patient == null)
                return null;

            var data = new Dictionary<string, object>
            {
                { "FirstName", patient.Name.FirstName },
                { "LastName", patient.Name.LastName },
                { "DateOfBirth", Timestamp.FromDateTime(patient.DateOfBirth.Value.ToUniversalTime()) },
                { "Gender", patient.Gender.Name },
                { "Email", patient.Contact.Email },
                { "Phone", patient.Contact.Phone },
                { "Address", patient.Contact.Address },
                { "Allergies", patient.Allergies },
                { "MedicalHistory", patient.MedicalHistory },
                { "Notes", patient.Notes }
            };

            return data;
        }
    }
}