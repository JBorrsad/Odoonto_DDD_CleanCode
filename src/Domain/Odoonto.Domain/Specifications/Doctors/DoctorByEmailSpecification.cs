using System;
using System.Linq.Expressions;
using Odoonto.Domain.Core.Specifications;
using Odoonto.Domain.Models.Doctors;

namespace Odoonto.Domain.Specifications.Doctors
{
    /// <summary>
    /// Especificación para buscar doctores por email
    /// </summary>
    public class DoctorByEmailSpecification : BaseSpecification<Doctor>
    {
        /// <summary>
        /// Constructor para crear una especificación de búsqueda por email
        /// </summary>
        /// <param name="email">Email a buscar</param>
        public DoctorByEmailSpecification(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email no puede estar vacío", nameof(email));

            string normalizedEmail = email.ToLower().Trim();

            Criteria = d =>
                d.ContactInfo != null &&
                !string.IsNullOrEmpty(d.ContactInfo.Email) &&
                d.ContactInfo.Email.ToLower() == normalizedEmail;
        }
    }
}