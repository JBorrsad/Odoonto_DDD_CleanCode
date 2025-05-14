using System;
using System.Linq.Expressions;
using Odoonto.Domain.Core.Specifications;
using Odoonto.Domain.Models.Doctors;

namespace Odoonto.Domain.Specifications.Doctors
{
    /// <summary>
    /// Especificación para búsqueda general de doctores
    /// </summary>
    public class DoctorSearchSpecification : BaseSpecification<Doctor>
    {
        /// <summary>
        /// Constructor para crear una especificación de búsqueda general
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda general</param>
        public DoctorSearchSpecification(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                throw new ArgumentException("El término de búsqueda no puede estar vacío", nameof(searchTerm));

            string normalizedSearch = searchTerm.ToLower().Trim();

            Criteria = d =>
                (d.FullName.FirstName != null && d.FullName.FirstName.ToLower().Contains(normalizedSearch)) ||
                (d.FullName.LastName != null && d.FullName.LastName.ToLower().Contains(normalizedSearch)) ||
                (!string.IsNullOrEmpty(d.Specialty) && d.Specialty.ToLower().Contains(normalizedSearch)) ||
                (d.ContactInfo != null && !string.IsNullOrEmpty(d.ContactInfo.Email) && d.ContactInfo.Email.ToLower().Contains(normalizedSearch)) ||
                (d.ContactInfo != null && !string.IsNullOrEmpty(d.ContactInfo.Phone) && d.ContactInfo.Phone.Contains(normalizedSearch)) ||
                (d.ContactInfo != null && !string.IsNullOrEmpty(d.ContactInfo.Address) && d.ContactInfo.Address.ToLower().Contains(normalizedSearch));
        }
    }
}