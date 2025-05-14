using System;
using System.Linq.Expressions;
using Odoonto.Domain.Core.Specifications;
using Odoonto.Domain.Models.Doctors;

namespace Odoonto.Domain.Specifications.Doctors
{
    /// <summary>
    /// Especificación para buscar doctores por número de licencia
    /// </summary>
    public class DoctorByLicenseNumberSpecification : BaseSpecification<Doctor>
    {
        /// <summary>
        /// Constructor para crear una especificación de búsqueda por número de licencia
        /// </summary>
        /// <param name="licenseNumber">Número de licencia a buscar</param>
        public DoctorByLicenseNumberSpecification(string licenseNumber)
        {
            if (string.IsNullOrWhiteSpace(licenseNumber))
                throw new ArgumentException("El número de licencia no puede estar vacío", nameof(licenseNumber));

            string normalizedLicenseNumber = licenseNumber.Trim();

            Criteria = d => d.LicenseNumber == normalizedLicenseNumber;
        }
    }
}