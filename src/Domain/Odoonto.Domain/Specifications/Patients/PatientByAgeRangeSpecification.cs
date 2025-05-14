using System;
using System.Linq.Expressions;
using Odoonto.Domain.Core.Specifications;
using Odoonto.Domain.Models.Patients;

namespace Odoonto.Domain.Specifications.Patients
{
    /// <summary>
    /// Especificación para buscar pacientes por rango de edad
    /// </summary>
    public class PatientByAgeRangeSpecification : BaseSpecification<Patient>
    {
        /// <summary>
        /// Constructor para crear una especificación por rango de edad
        /// </summary>
        /// <param name="minAge">Edad mínima (inclusive)</param>
        /// <param name="maxAge">Edad máxima (inclusive)</param>
        public PatientByAgeRangeSpecification(int minAge, int maxAge)
        {
            if (minAge < 0)
                throw new ArgumentException("La edad mínima no puede ser negativa", nameof(minAge));

            if (maxAge < minAge)
                throw new ArgumentException("La edad máxima debe ser mayor o igual que la edad mínima", nameof(maxAge));

            var today = DateTime.Today;

            // Calcular las fechas de nacimiento que corresponden al rango de edad
            var minBirthDate = today.AddYears(-maxAge - 1).AddDays(1); // +1 día para incluir a los que cumplen años hoy
            var maxBirthDate = today.AddYears(-minAge);

            Criteria = p =>
                p.DateOfBirth.Value >= minBirthDate &&
                p.DateOfBirth.Value <= maxBirthDate;
        }
    }
}