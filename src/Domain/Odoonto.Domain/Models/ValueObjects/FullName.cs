using System;
using Odoonto.Domain.Core.Models.Exceptions;

namespace Odoonto.Domain.Models.ValueObjects
{
    /// <summary>
    /// Representa el nombre completo de una persona
    /// </summary>
    public class FullName : IEquatable<FullName>
    {
        /// <summary>
        /// Nombres de la persona
        /// </summary>
        public string FirstNames { get; }

        /// <summary>
        /// Apellidos de la persona
        /// </summary>
        public string LastNames { get; }

        /// <summary>
        /// Constructor para crear un nombre completo
        /// </summary>
        public FullName(string firstNames, string lastNames)
        {
            if (string.IsNullOrWhiteSpace(firstNames))
                throw new DomainException("Los nombres no pueden estar vacíos");

            if (string.IsNullOrWhiteSpace(lastNames))
                throw new DomainException("Los apellidos no pueden estar vacíos");

            FirstNames = firstNames.Trim();
            LastNames = lastNames.Trim();
        }

        /// <summary>
        /// Devuelve el nombre completo formateado
        /// </summary>
        public override string ToString()
        {
            return $"{FirstNames} {LastNames}";
        }

        /// <summary>
        /// Implementación de IEquatable<FullName>
        /// </summary>
        public bool Equals(FullName other)
        {
            if (other is null) return false;
            
            return string.Equals(FirstNames, other.FirstNames, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(LastNames, other.LastNames, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Implementación de IEquatable<FullName>
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is FullName other && Equals(other);
        }

        /// <summary>
        /// Implementación de IEquatable<FullName>
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(
                FirstNames.ToLowerInvariant(), 
                LastNames.ToLowerInvariant());
        }

        /// <summary>
        /// Implementación de IEquatable<FullName>
        /// </summary>
        public static bool operator ==(FullName left, FullName right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        /// <summary>
        /// Implementación de IEquatable<FullName>
        /// </summary>
        public static bool operator !=(FullName left, FullName right) => !(left == right);

        /// <summary>
        /// Devuelve el nombre completo con los apellidos primero
        /// </summary>
        public string FullNameWithLastNameFirst => $"{LastNames}, {FirstNames}";

        /// <summary>
        /// Devuelve el nombre completo con los nombres primero
        /// </summary>
        public string FullNameWithFirstNameFirst => $"{FirstNames} {LastNames}";
    }
} 