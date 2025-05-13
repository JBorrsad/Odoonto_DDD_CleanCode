using System;
using Odoonto.Domain.Core.Models.Exceptions;

namespace Odoonto.Domain.Models.ValueObjects
{
    /// <summary>
    /// Value object que representa el nombre completo de una persona
    /// </summary>
    public class FullName : IEquatable<FullName>
    {
        public string FirstNames { get; }
        public string LastNames { get; }

        public FullName(string firstNames, string lastNames)
        {
            if (string.IsNullOrWhiteSpace(firstNames))
                throw new InvalidValueException("El nombre no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(lastNames))
                throw new InvalidValueException("Los apellidos no pueden estar vacíos.");

            FirstNames = firstNames.Trim();
            LastNames = lastNames.Trim();
        }

        // Propiedad calculada para el nombre completo
        public string FullNameFormatted => $"{FirstNames} {LastNames}";

        // Value objects deben ser inmutables y comparables por valor
        public bool Equals(FullName other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            
            return string.Equals(FirstNames, other.FirstNames, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(LastNames, other.LastNames, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            
            return Equals((FullName)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                FirstNames?.ToLowerInvariant().GetHashCode() ?? 0,
                LastNames?.ToLowerInvariant().GetHashCode() ?? 0);
        }

        public override string ToString() => FullNameFormatted;

        // Operadores de comparación
        public static bool operator ==(FullName left, FullName right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(FullName left, FullName right) => !(left == right);
    }
} 