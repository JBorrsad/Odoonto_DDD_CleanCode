using System;
using System.Collections.Generic;
using Odoonto.Domain.Core.Models.Exceptions;
using Odoonto.Domain.Core.Abstractions;

namespace Odoonto.Domain.Models.ValueObjects
{
    /// <summary>
    /// Value object que representa el género de una persona
    /// </summary>
    public class GenderValue : ValueObject, IEquatable<GenderValue>
    {
        private readonly string _value;

        // Valores predefinidos
        public static readonly GenderValue Male = new GenderValue("Male");
        public static readonly GenderValue Female = new GenderValue("Female");
        public static readonly GenderValue Other = new GenderValue("Other");

        // Constructor privado para controlar la creación
        private GenderValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidValueException("El valor del género no puede estar vacío.");

            _value = value;
        }

        // Método factory para crear instancias válidas
        public static GenderValue FromString(string value)
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

        // Método para convertir desde el enum
        public static GenderValue FromEnum(GenderType genderType)
        {
            return genderType switch
            {
                GenderType.Male => Male,
                GenderType.Female => Female,
                GenderType.Other => Other,
                _ => throw new InvalidValueException($"'{genderType}' no es un género válido.")
            };
        }

        // Value objects deben ser inmutables y comparables por valor
        public bool Equals(GenderValue other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            
            return string.Equals(_value, other._value, StringComparison.OrdinalIgnoreCase);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _value?.ToLowerInvariant();
        }

        public override string ToString() => _value;
    }
} 