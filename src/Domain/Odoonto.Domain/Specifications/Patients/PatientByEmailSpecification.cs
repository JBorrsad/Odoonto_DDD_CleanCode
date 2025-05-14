using System;
using System.Linq.Expressions;
using Odoonto.Domain.Core.Specifications;
using Odoonto.Domain.Models.Patients;

namespace Odoonto.Domain.Specifications.Patients
{
    /// <summary>
    /// Especificación para buscar pacientes por correo electrónico
    /// </summary>
    public class PatientByEmailSpecification : BaseSpecification<Patient>
    {
        /// <summary>
        /// Constructor para crear una especificación por correo electrónico
        /// </summary>
        /// <param name="email">Correo electrónico a buscar</param>
        public PatientByEmailSpecification(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El correo electrónico no puede estar vacío", nameof(email));

            string normalizedEmail = email.ToLower().Trim();

            Criteria = p =>
                !string.IsNullOrEmpty(p.Contact?.Email) &&
                p.Contact.Email.ToLower() == normalizedEmail;
        }
    }
}