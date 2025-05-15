using System;
using Odoonto.Domain.Core.Abstractions;
using Odoonto.Domain.Core.Models.Exceptions;
using Odoonto.Domain.Models.ValueObjects;

namespace Odoonto.Domain.Models.Doctors
{
    /// <summary>
    /// Representa al profesional odontológico y controla su identidad y disponibilidad de agenda.
    /// </summary>
    public class Doctor : Entity
    {
        // Propiedades con getters públicos y setters privados
        public FullName FullName { get; private set; }
        public string Specialty { get; private set; }
        public WeeklyAvailability Availability { get; private set; }
        public ContactInfo ContactInfo { get; private set; }

        // Constructor privado - solo accesible desde método factory
        private Doctor(Guid id) : base(id)
        {
            // Inicialización de propiedades por defecto
            Specialty = string.Empty;
            Availability = new WeeklyAvailability();
        }

        // Método factory para crear instancias válidas
        public static Doctor Create(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new DomainException("El identificador del doctor no puede estar vacío.");
            }

            var doctor = new Doctor(id);
            doctor.UpdateEditDate();
            return doctor;
        }

        // Método para establecer el nombre completo
        public void SetFullName(string firstNames, string lastNames)
        {
            if (string.IsNullOrWhiteSpace(firstNames))
            {
                throw new DomainException("El nombre del doctor no puede estar vacío.");
            }

            if (string.IsNullOrWhiteSpace(lastNames))
            {
                throw new DomainException("Los apellidos del doctor no pueden estar vacíos.");
            }

            FullName = new FullName(firstNames, lastNames);
            UpdateEditDate();
        }

        // Método para establecer la especialidad
        public void SetSpecialty(string specialty)
        {
            Specialty = specialty?.Trim() ?? string.Empty;
            UpdateEditDate();
        }

        // Método para establecer la información de contacto
        public void SetContactInfo(string postalAddress, string phoneNumber, string email)
        {
            ContactInfo = new ContactInfo(postalAddress, phoneNumber, email);
            UpdateEditDate();
        }

        // Método para establecer la disponibilidad semanal
        public void SetAvailability(WeeklyAvailability availability)
        {
            Availability = availability ?? throw new DomainException("La disponibilidad no puede ser nula.");
            UpdateEditDate();
        }

        // Método para verificar disponibilidad en una fecha y hora específicas
        public bool IsAvailable(DateTime date, TimeSlot timeSlot)
        {
            if (Availability == null)
            {
                return false;
            }

            return Availability.IsWithinAvailability(date.DayOfWeek, timeSlot);
        }

        // Método para establecer la fecha de creación (usado para mapeo desde Firebase)
        internal void SetCreatedAt(DateTime createdAt)
        {
            CreatedAt = createdAt;
        }
        
        // Método para establecer la fecha de actualización (usado para mapeo desde Firebase)
        internal void SetUpdatedAt(DateTime updatedAt)
        {
            UpdatedAt = updatedAt;
        }
    }
} 