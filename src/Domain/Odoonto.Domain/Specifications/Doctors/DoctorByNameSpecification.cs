using System;
using System.Linq.Expressions;
using Odoonto.Domain.Core.Specifications;
using Odoonto.Domain.Models.Doctors;

namespace Odoonto.Domain.Specifications.Doctors
{
    /// <summary>
    /// Especificación para buscar doctores por nombre
    /// </summary>
    public class DoctorByNameSpecification : BaseSpecification<Doctor>
    {
        /// <summary>
        /// Constructor para crear una especificación de búsqueda por nombre
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda para el nombre</param>
        public DoctorByNameSpecification(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                throw new ArgumentException("El término de búsqueda no puede estar vacío", nameof(searchTerm));

            string normalizedSearch = searchTerm.ToLower().Trim();

            Criteria = d =>
                (d.FullName.FirstName != null && d.FullName.FirstName.ToLower().Contains(normalizedSearch)) ||
                (d.FullName.LastName != null && d.FullName.LastName.ToLower().Contains(normalizedSearch));
        }
    }
}