using System;
using System.Collections.Generic;
using Odoonto.Domain.Core.Models;
using Odoonto.Domain.Core.Models.Exceptions;
using Odoonto.Domain.Models.ValueObjects;
using Odoonto.Domain.Core.Abstractions;


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
        }

        // Método factory para crear instancias válidas
        public static Doctor Create(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new InvalidValueException("El identificador del doctor no puede estar vacío.");
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
                throw new InvalidValueException("El nombre del doctor no puede estar vacío.");
            }

            if (string.IsNullOrWhiteSpace(lastNames))
            {
                throw new InvalidValueException("Los apellidos del doctor no pueden estar vacíos.");
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
            Availability = availability ?? throw new InvalidValueException("La disponibilidad no puede ser nula.");
            UpdateEditDate();
        }

        // Método para verificar disponibilidad en una fecha y hora específicas
        public bool IsAvailable(DateTime date, TimeSlot timeSlot)
        {
            if (Availability == null)
            {
                return false;
            }

            return Availability.IsAvailable(date.DayOfWeek, timeSlot);
        }
    }
} 