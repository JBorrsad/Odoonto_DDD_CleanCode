using System;
using Odoonto.Domain.Core.Models.Exceptions;

namespace Odoonto.Domain.Models.ValueObjects
{
    /// <summary>
    /// Objeto de valor que representa una dirección
    /// </summary>
    public class Address : IEquatable<Address>
    {
        public string Street { get; }
        public string City { get; }
        public string State { get; }
        public string ZipCode { get; }
        public string Country { get; }

        public Address(string street, string city, string state, string zipCode, string country)
        {
            if (string.IsNullOrWhiteSpace(street))
                throw new InvalidValueException("La calle no puede estar vacía");
            
            if (string.IsNullOrWhiteSpace(city))
                throw new InvalidValueException("La ciudad no puede estar vacía");
            
            if (string.IsNullOrWhiteSpace(state))
                throw new InvalidValueException("El estado/provincia no puede estar vacío");
            
            if (string.IsNullOrWhiteSpace(zipCode))
                throw new InvalidValueException("El código postal no puede estar vacío");
            
            if (string.IsNullOrWhiteSpace(country))
                throw new InvalidValueException("El país no puede estar vacío");

            Street = street.Trim();
            City = city.Trim();
            State = state.Trim();
            ZipCode = zipCode.Trim();
            Country = country.Trim();
        }

        public string GetFullAddress()
        {
            return $"{Street}, {City}, {State} {ZipCode}, {Country}";
        }

        public bool Equals(Address other)
        {
            if (other is null) return false;
            
            return string.Equals(Street, other.Street, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(City, other.City, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(State, other.State, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(ZipCode, other.ZipCode, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(Country, other.Country, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return obj is Address address && Equals(address);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                Street.ToLowerInvariant(),
                City.ToLowerInvariant(),
                State.ToLowerInvariant(),
                ZipCode.ToLowerInvariant(),
                Country.ToLowerInvariant());
        }

        public static bool operator ==(Address left, Address right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(Address left, Address right) => !(left == right);

        public override string ToString() => GetFullAddress();
    }
} 