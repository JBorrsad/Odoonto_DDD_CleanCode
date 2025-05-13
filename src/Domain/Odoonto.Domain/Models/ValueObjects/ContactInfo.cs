using System;
using System.Text.RegularExpressions;
using Odoonto.Domain.Core.Models.Exceptions;

namespace Odoonto.Domain.Models.ValueObjects
{
    /// <summary>
    /// Value object que representa la información de contacto de una persona
    /// </summary>
    public class ContactInfo : IEquatable<ContactInfo>
    {
        public string PostalAddress { get; }
        public string PhoneNumber { get; }
        public string Email { get; }

        public ContactInfo(string postalAddress, string phoneNumber, string email)
        {
            // Validación de dirección postal
            if (string.IsNullOrWhiteSpace(postalAddress))
                throw new InvalidValueException("La dirección postal no puede estar vacía.");

            // Validación de número de teléfono
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new InvalidValueException("El número de teléfono no puede estar vacío.");

            if (!IsValidPhoneNumber(phoneNumber))
                throw new InvalidValueException("El formato del número de teléfono no es válido.");

            // Validación de email
            if (string.IsNullOrWhiteSpace(email))
                throw new InvalidValueException("El email no puede estar vacío.");

            if (!IsValidEmail(email))
                throw new InvalidValueException("El formato del email no es válido.");

            PostalAddress = postalAddress.Trim();
            PhoneNumber = NormalizePhoneNumber(phoneNumber);
            Email = email.Trim().ToLowerInvariant();
        }

        // Método para validar formato de número de teléfono
        private static bool IsValidPhoneNumber(string phoneNumber)
        {
            // Simplificado para el ejemplo - ajustar según requisitos específicos
            var normalizedNumber = NormalizePhoneNumber(phoneNumber);
            return Regex.IsMatch(normalizedNumber, @"^\+?[0-9]{8,15}$");
        }

        // Método para normalizar número de teléfono
        private static string NormalizePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return string.Empty;

            // Eliminar espacios, guiones y paréntesis
            return Regex.Replace(phoneNumber, @"[\s\-\(\)]", "");
        }

        // Método para validar formato de email
        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Usar regex simple pero efectiva para validar emails
                return Regex.IsMatch(email.Trim(),
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        // Value objects deben ser inmutables y comparables por valor
        public bool Equals(ContactInfo other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            
            return string.Equals(PostalAddress, other.PostalAddress, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(PhoneNumber, other.PhoneNumber, StringComparison.Ordinal) &&
                   string.Equals(Email, other.Email, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            
            return Equals((ContactInfo)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                PostalAddress?.ToLowerInvariant().GetHashCode() ?? 0,
                PhoneNumber?.GetHashCode() ?? 0,
                Email?.ToLowerInvariant().GetHashCode() ?? 0);
        }

        public override string ToString() => $"Email: {Email}, Teléfono: {PhoneNumber}";

        // Operadores de comparación
        public static bool operator ==(ContactInfo left, ContactInfo right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(ContactInfo left, ContactInfo right) => !(left == right);
    }
} 