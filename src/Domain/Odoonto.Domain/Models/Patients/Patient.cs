using System;
using System.Collections.Generic;
using Odoonto.Domain.Core.Models;
using Odoonto.Domain.Core.Models.Exceptions;
using Odoonto.Domain.Models.ValueObjects;
using Odoonto.Domain.Models.Odontograms;
using Odoonto.Domain.Core.Abstractions;
using Odoonto.Domain.Models.Appointments;

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
        public FullName FullName { get; private set; }

        /// <summary>
        /// Fecha de nacimiento
        /// </summary>
        public DateTime BirthDate { get; private set; }

        /// <summary>
        /// Género del paciente
        /// </summary>
        public string Gender { get; private set; }

        /// <summary>
        /// Información de contacto
        /// </summary>
        public ContactInfo ContactInfo { get; private set; }

        /// <summary>
        /// Dirección del paciente
        /// </summary>
        public Address Address { get; private set; }

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
        /// Colección de citas asociadas al paciente
        /// </summary>
        private readonly List<Appointment> _appointments;

        /// <summary>
        /// Colección de citas asociadas al paciente como solo lectura
        /// </summary>
        public IReadOnlyCollection<Appointment> Appointments => _appointments.AsReadOnly();

        /// <summary>
        /// Constructor privado para serialización
        /// </summary>
        private Patient(Guid id) : base(id)
        {
            Allergies = new List<string>();
            _appointments = new List<Appointment>();
            MedicalHistory = string.Empty;
            Gender = string.Empty;
        }

        /// <summary>
        /// Constructor para crear un nuevo paciente
        /// </summary>
        public Patient(FullName name, DateTime birthDate, string gender, ContactInfo contact)
        {
            ValidatePatient(name, birthDate);

            FullName = name;
            BirthDate = birthDate;
            Gender = gender?.Trim() ?? string.Empty;
            ContactInfo = contact;
            Allergies = new List<string>();
            MedicalHistory = string.Empty;
            Notes = string.Empty;
            _appointments = new List<Appointment>();
        }

        /// <summary>
        /// Actualiza la información básica del paciente
        /// </summary>
        public void UpdateBasicInfo(FullName name, DateTime birthDate, string gender, ContactInfo contact)
        {
            ValidatePatient(name, birthDate);

            FullName = name;
            BirthDate = birthDate;
            Gender = gender?.Trim() ?? string.Empty;
            ContactInfo = contact;
        }

        /// <summary>
        /// Actualiza el historial médico del paciente
        /// </summary>
        public void UpdateMedicalHistory(string medicalHistory)
        {
            MedicalHistory = medicalHistory?.Trim() ?? string.Empty;
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
            var age = today.Year - BirthDate.Year;
            
            // Ajustar si el cumpleaños no ha ocurrido todavía este año
            if (BirthDate.Date > today.AddYears(-age))
            {
                age--;
            }
            
            return age;
        }

        /// <summary>
        /// Establece la dirección del paciente
        /// </summary>
        public void SetAddress(Address address)
        {
            Address = address;
        }

        /// <summary>
        /// Agrega una cita al historial del paciente
        /// </summary>
        public void AddAppointment(Appointment appointment)
        {
            if (appointment == null)
            {
                throw new DomainException("La cita no puede ser nula.");
            }

            if (_appointments.Contains(appointment))
            {
                throw new DomainException("Esta cita ya está asociada al paciente.");
            }

            _appointments.Add(appointment);
        }

        private void ValidatePatient(FullName name, DateTime birthDate)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name), "El nombre no puede ser nulo.");

            if (birthDate > DateTime.Today)
                throw new ArgumentException("La fecha de nacimiento no puede ser una fecha futura.", nameof(birthDate));
        }
    }
} 