using Odoonto.Domain.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Odoonto.Domain.Core.Models.Exceptions;

namespace Odoonto.Domain.Models.ValueObjects
{
    /// <summary>
    /// Value object que representa la información de contacto de una persona
    /// </summary>
    public class ContactInfo : ValueObject
    {
        /// <summary>
        /// Dirección postal
        /// </summary>
        public string Address { get; }

        /// <summary>
        /// Número de teléfono
        /// </summary>
        public string PhoneNumber { get; }

        /// <summary>
        /// Correo electrónico
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// Constructor para crear información de contacto
        /// </summary>
        public ContactInfo(string address, string phoneNumber, string email)
        {
            // La dirección puede ser opcional
            Address = address?.Trim() ?? string.Empty;

            // Validar teléfono (formato simplificado para ejemplo)
            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                if (!Regex.IsMatch(phoneNumber.Trim(), @"^\+?[0-9\s\-\(\)]+$"))
                    throw new ArgumentException("El formato del número de teléfono no es válido", nameof(phoneNumber));
                
                PhoneNumber = phoneNumber.Trim();
            }
            else
            {
                PhoneNumber = string.Empty;
            }

            // Validar email (formato simplificado para ejemplo)
            if (!string.IsNullOrWhiteSpace(email))
            {
                if (!Regex.IsMatch(email.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    throw new ArgumentException("El formato del correo electrónico no es válido", nameof(email));
                
                Email = email.Trim();
            }
            else
            {
                Email = string.Empty;
            }
        }

        /// <summary>
        /// Componentes para comparación de igualdad
        /// </summary>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Address;
            yield return PhoneNumber;
            yield return Email;
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