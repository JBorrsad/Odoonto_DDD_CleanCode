using Odoonto.Domain.Core.Abstractions;
using System;
using System.Collections.Generic;
using Odoonto.Domain.Core.Models.Exceptions;

namespace Odoonto.Domain.Models.ValueObjects
{
    /// <summary>
    /// Representa el nombre completo de una persona
    /// </summary>
    public class FullName : ValueObject
    {
        /// <summary>
        /// Nombres de la persona
        /// </summary>
        public string FirstName { get; }

        /// <summary>
        /// Apellidos de la persona
        /// </summary>
        public string LastName { get; }

        /// <summary>
        /// Constructor para crear un nombre completo
        /// </summary>
        public FullName(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("El nombre no puede estar vacío", nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Los apellidos no pueden estar vacíos", nameof(lastName));

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
        }

        /// <summary>
        /// Devuelve el nombre completo formateado
        /// </summary>
        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }

        /// <summary>
        /// Componentes para comparación de igualdad
        /// </summary>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FirstName;
            yield return LastName;
        }
    }
} 