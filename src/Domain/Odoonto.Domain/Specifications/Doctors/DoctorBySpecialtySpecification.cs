using System;
using System.Linq.Expressions;
using Odoonto.Domain.Core.Specifications;
using Odoonto.Domain.Models.Doctors;

namespace Odoonto.Domain.Specifications.Doctors
{
    /// <summary>
    /// Especificación para buscar doctores por especialidad
    /// </summary>
    public class DoctorBySpecialtySpecification : BaseSpecification<Doctor>
    {
        /// <summary>
        /// Constructor para crear una especificación de búsqueda por especialidad
        /// </summary>
        /// <param name="specialty">Especialidad a buscar</param>
        public DoctorBySpecialtySpecification(string specialty)
        {
            if (string.IsNullOrWhiteSpace(specialty))
                throw new ArgumentException("La especialidad no puede estar vacía", nameof(specialty));

            string normalizedSpecialty = specialty.ToLower().Trim();

            Criteria = d =>
                !string.IsNullOrEmpty(d.Specialty) &&
                d.Specialty.ToLower() == normalizedSpecialty;
        }
    }
}