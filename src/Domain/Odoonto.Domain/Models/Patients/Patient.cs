using System;
using System.Collections.Generic;
using Odoonto.Domain.Core.Models;
using Odoonto.Domain.Core.Models.Exceptions;
using Odoonto.Domain.Models.ValueObjects;
using Odoonto.Domain.Models.Odontograms;

namespace Odoonto.Domain.Models.Patients
{
    /// <summary>
    /// Representa a la persona atendida e incluye su información personal y 
    /// el conjunto completo de datos clínicos básicos.
    /// </summary>
    public class Patient : Entity
    {
        // Propiedades con getters públicos y setters privados
        public FullName FullName { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public Gender Gender { get; private set; }
        public ContactInfo ContactInfo { get; private set; }
        public Odontogram Odontogram { get; private set; }

        // Constructor privado - solo accesible desde método factory
        private Patient(Guid id) : base(id)
        {
            Odontogram = new Odontogram();
        }

        // Método factory para crear instancias válidas
        public static Patient Create(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new InvalidValueException("El identificador del paciente no puede estar vacío.");
            }

            var patient = new Patient(id);
            patient.UpdateEditDate();
            return patient;
        }

        // Método para establecer el nombre completo
        public void SetFullName(string firstNames, string lastNames)
        {
            if (string.IsNullOrWhiteSpace(firstNames))
            {
                throw new InvalidValueException("El nombre del paciente no puede estar vacío.");
            }

            if (string.IsNullOrWhiteSpace(lastNames))
            {
                throw new InvalidValueException("Los apellidos del paciente no pueden estar vacíos.");
            }

            FullName = new FullName(firstNames, lastNames);
            UpdateEditDate();
        }

        // Método para establecer la fecha de nacimiento
        public void SetDateOfBirth(DateTime dateOfBirth)
        {
            if (dateOfBirth > DateTime.Now)
            {
                throw new InvalidValueException("La fecha de nacimiento no puede ser futura.");
            }

            if (DateTime.Now.Year - dateOfBirth.Year > 120)
            {
                throw new InvalidValueException("La edad del paciente parece incorrecta.");
            }

            DateOfBirth = dateOfBirth;
            UpdateEditDate();
        }

        // Método para establecer el género
        public void SetGender(Gender gender)
        {
            Gender = gender ?? throw new InvalidValueException("El género no puede ser nulo.");
            UpdateEditDate();
        }

        // Método para establecer la información de contacto
        public void SetContactInfo(string postalAddress, string phoneNumber, string email)
        {
            ContactInfo = new ContactInfo(postalAddress, phoneNumber, email);
            UpdateEditDate();
        }

        // Método para añadir un registro dental al odontograma
        public void AddToothRecord(ToothRecord toothRecord)
        {
            if (toothRecord == null)
            {
                throw new InvalidValueException("El registro dental no puede ser nulo.");
            }

            Odontogram.AddToothRecord(toothRecord);
            UpdateEditDate();
        }

        // Método para calcular la edad del paciente
        public int CalculateAge()
        {
            var today = DateTime.Today;
            var age = today.Year - DateOfBirth.Year;
            
            // Ajuste si aún no ha cumplido años este año
            if (DateOfBirth.Date > today.AddYears(-age))
            {
                age--;
            }
            
            return age;
        }

        // Método para verificar si es menor de edad
        public bool IsMinor()
        {
            return CalculateAge() < 18;
        }
    }
} 