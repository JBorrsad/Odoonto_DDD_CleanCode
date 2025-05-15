using System;
using System.Collections.Generic;
using Odoonto.Domain.Core.Models;
using Odoonto.Domain.Core.Models.Exceptions;
using Odoonto.Domain.Models.ValueObjects;
using Odoonto.Domain.Models.Odontograms;
using Odoonto.Domain.Core.Abstractions;

namespace Odoonto.Domain.Models.Patients
{
    /// <summary>
    /// Entidad que representa a un paciente dental
    /// </summary>
    public class Patient : Entity
    {
        /// <summary>
        /// Nombre completo del paciente
        /// </summary>
        public FullName Name { get; private set; }

        /// <summary>
        /// Fecha de nacimiento
        /// </summary>
        public Date DateOfBirth { get; private set; }

        /// <summary>
        /// Género del paciente
        /// </summary>
        public GenderValue Gender { get; private set; }

        /// <summary>
        /// Información de contacto
        /// </summary>
        public ContactInfo Contact { get; private set; }

        /// <summary>
        /// Historial de alergias
        /// </summary>
        public List<string> Allergies { get; private set; }

        /// <summary>
        /// Historial médico relevante
        /// </summary>
        public string MedicalHistory { get; private set; }

        /// <summary>
        /// Notas adicionales sobre el paciente
        /// </summary>
        public string Notes { get; private set; }

        /// <summary>
        /// Constructor privado para serialización
        /// </summary>
        private Patient() 
        {
            Allergies = new List<string>();
        }

        /// <summary>
        /// Constructor para crear un nuevo paciente
        /// </summary>
        public Patient(FullName name, Date dateOfBirth, GenderValue gender, ContactInfo contact)
        {
            ValidatePatient(name, dateOfBirth);

            Name = name;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Contact = contact;
            Allergies = new List<string>();
            MedicalHistory = string.Empty;
            Notes = string.Empty;
        }

        /// <summary>
        /// Actualiza la información básica del paciente
        /// </summary>
        public void UpdateBasicInfo(FullName name, Date dateOfBirth, GenderValue gender, ContactInfo contact)
        {
            ValidatePatient(name, dateOfBirth);

            Name = name;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Contact = contact;
        }

        /// <summary>
        /// Actualiza el historial médico del paciente
        /// </summary>
        public void UpdateMedicalHistory(string medicalHistory)
        {
            MedicalHistory = medicalHistory ?? string.Empty;
        }

        /// <summary>
        /// Añade una alergia al historial del paciente
        /// </summary>
        public void AddAllergy(string allergy)
        {
            if (string.IsNullOrWhiteSpace(allergy))
                throw new ArgumentException("La alergia no puede estar vacía.", nameof(allergy));

            if (!Allergies.Contains(allergy))
            {
                Allergies.Add(allergy);
            }
        }

        /// <summary>
        /// Elimina una alergia del historial del paciente
        /// </summary>
        public void RemoveAllergy(string allergy)
        {
            Allergies.Remove(allergy);
        }

        /// <summary>
        /// Actualiza las notas del paciente
        /// </summary>
        public void UpdateNotes(string notes)
        {
            Notes = notes ?? string.Empty;
        }

        /// <summary>
        /// Calcula la edad actual del paciente
        /// </summary>
        public int CalculateAge()
        {
            var today = DateTime.Today;
            var birthDate = DateOfBirth.Value;
            var age = today.Year - birthDate.Year;

            // Ajusta la edad si aún no ha llegado el cumpleaños este año
            if (birthDate.Date > today.AddYears(-age))
                age--;

            return age;
        }

        private void ValidatePatient(FullName name, Date dateOfBirth)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name), "El nombre no puede ser nulo.");

            if (dateOfBirth == null)
                throw new ArgumentNullException(nameof(dateOfBirth), "La fecha de nacimiento no puede ser nula.");

            if (dateOfBirth.Value > DateTime.Today)
                throw new ArgumentException("La fecha de nacimiento no puede ser una fecha futura.", nameof(dateOfBirth));
        }
    }
} 