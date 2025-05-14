using System;
using System.Linq;
using System.Linq.Expressions;
using Odoonto.Domain.Core.Specifications;
using Odoonto.Domain.Models.Patients;

namespace Odoonto.Domain.Specifications.Patients
{
    /// <summary>
    /// Especificación para búsqueda general de pacientes
    /// </summary>
    public class PatientSearchSpecification : BaseSpecification<Patient>
    {
        /// <summary>
        /// Constructor para crear una especificación de búsqueda general
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda general</param>
        public PatientSearchSpecification(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                throw new ArgumentException("El término de búsqueda no puede estar vacío", nameof(searchTerm));

            string normalizedSearch = searchTerm.ToLower().Trim();

            Criteria = p =>
                (p.Name.FirstName != null && p.Name.FirstName.ToLower().Contains(normalizedSearch)) ||
                (p.Name.LastName != null && p.Name.LastName.ToLower().Contains(normalizedSearch)) ||
                (!string.IsNullOrEmpty(p.Contact?.Email) && p.Contact.Email.ToLower().Contains(normalizedSearch)) ||
                (!string.IsNullOrEmpty(p.Contact?.PhoneNumber) && p.Contact.PhoneNumber.Contains(normalizedSearch)) ||
                (!string.IsNullOrEmpty(p.Contact?.Address) && p.Contact.Address.ToLower().Contains(normalizedSearch)) ||
                (p.Allergies != null && p.Allergies.Any(a => a.ToLower().Contains(normalizedSearch))) ||
                (!string.IsNullOrEmpty(p.MedicalHistory) && p.MedicalHistory.ToLower().Contains(normalizedSearch)) ||
                (!string.IsNullOrEmpty(p.Notes) && p.Notes.ToLower().Contains(normalizedSearch));
        }
    }
}