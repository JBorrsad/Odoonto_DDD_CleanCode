using System;
using System.Linq.Expressions;
using Odoonto.Domain.Core.Specifications;
using Odoonto.Domain.Models.Patients;

namespace Odoonto.Domain.Specifications.Patients
{
    /// <summary>
    /// Especificación para buscar pacientes por número de teléfono
    /// </summary>
    public class PatientByPhoneSpecification : BaseSpecification<Patient>
    {
        /// <summary>
        /// Constructor para crear una especificación por número de teléfono
        /// </summary>
        /// <param name="phoneNumber">Número de teléfono a buscar</param>
        public PatientByPhoneSpecification(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("El número de teléfono no puede estar vacío", nameof(phoneNumber));

            string normalizedPhone = phoneNumber.Trim();

            Criteria = p =>
                !string.IsNullOrEmpty(p.Contact?.PhoneNumber) &&
                p.Contact.PhoneNumber.Contains(normalizedPhone);
        }
    }
}