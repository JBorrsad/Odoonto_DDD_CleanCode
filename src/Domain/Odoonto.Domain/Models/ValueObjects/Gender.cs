using System;
using Odoonto.Domain.Core.Models.Exceptions;

namespace Odoonto.Domain.Models.ValueObjects
{
    /// <summary>
    /// Value object que representa el género de una persona
    /// </summary>
    public class Gender : IEquatable<Gender>
    {
        private readonly string _value;

        // Valores predefinidos
        public static readonly Gender Male = new Gender("Male");
        public static readonly Gender Female = new Gender("Female");
        public static readonly Gender Other = new Gender("Other");

        // Constructor privado para controlar la creación
        private Gender(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidValueException("El valor del género no puede estar vacío.");

            _value = value;
        }

        // Método factory para crear instancias válidas
        public static Gender FromString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidValueException("El valor del género no puede estar vacío.");

            // Normalizar a minúsculas para comparación
            string normalizedValue = value.Trim().ToLowerInvariant();

            return normalizedValue switch
            {
                "male" => Male,
                "female" => Female,
                "other" => Other,
                _ => throw new InvalidValueException($"'{value}' no es un género válido. Use 'Male', 'Female' u 'Other'.")
            };
        }

        // Value objects deben ser inmutables y comparables por valor
        public bool Equals(Gender other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            
            return string.Equals(_value, other._value, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            
            return Equals((Gender)obj);
        }

        public override int GetHashCode()
        {
            return _value?.ToLowerInvariant().GetHashCode() ?? 0;
        }

        public override string ToString() => _value;

        // Operadores de comparación
        public static bool operator ==(Gender left, Gender right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(Gender left, Gender right) => !(left == right);
    }
} 