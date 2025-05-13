namespace Odoonto.Domain.Models;

using System;
using Odoonto.Domain.Core.Models;
using Odoonto.Domain.Core.Exceptions;

public class Patient : Entity
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public Gender Gender { get; private set; }
    public string PhoneNumber { get; private set; }
    public string Email { get; private set; }
    public string Address { get; private set; }
    public Odontogram Odontogram { get; private set; }

    private Patient(Guid id) : base(id)
    {
        Odontogram = new Odontogram();
    }

    public static Patient Create(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new InvalidValueException("El ID del paciente no puede estar vacío.");
        }

        Patient patient = new Patient(id);
        patient.UpdateEditDate();
        return patient;
    }

    public void SetPersonalInfo(string firstName, string lastName, DateTime dateOfBirth, Gender gender)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new InvalidValueException("El nombre del paciente no puede estar vacío.");
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new InvalidValueException("El apellido del paciente no puede estar vacío.");
        }

        if (dateOfBirth > DateTime.Today)
        {
            throw new InvalidValueException("La fecha de nacimiento no puede ser en el futuro.");
        }

        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Gender = gender;

        UpdateEditDate();
    }

    public void SetContactInfo(string phoneNumber, string email, string address)
    {
        // Validación básica de email (se podría mejorar con regex)
        if (!string.IsNullOrWhiteSpace(email) && !email.Contains("@"))
        {
            throw new InvalidValueException("El formato del email no es válido.");
        }

        PhoneNumber = phoneNumber;
        Email = email;
        Address = address;

        UpdateEditDate();
    }

    public string GetFullName()
    {
        return $"{FirstName} {LastName}";
    }

    public int GetAge()
    {
        int age = DateTime.Today.Year - DateOfBirth.Year;
        if (DateOfBirth.Date > DateTime.Today.AddYears(-age))
        {
            age--;
        }
        return age;
    }

    public void AddToothRecord(ToothRecord toothRecord)
    {
        if (toothRecord == null)
        {
            throw new InvalidValueException("El registro dental no puede ser nulo.");
        }

        Odontogram.AddToothRecord(toothRecord);
        UpdateEditDate();
    }
}

public enum Gender
{
    Male,
    Female,
    Other
}