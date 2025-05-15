using System;
using Odoonto.Domain.Core.Models.Exceptions;

namespace Odoonto.Domain.Models.ValueObjects
{
    /// <summary>
    /// Representa la información de contacto de una persona
    /// </summary>
    public class ContactInfo : IEquatable<ContactInfo>
    {
        public string Address { get; }
        public string PhoneNumber { get; }
        public string Email { get; }
        
        public ContactInfo(string address, string phoneNumber, string email)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(phoneNumber) && string.IsNullOrWhiteSpace(email))
                throw new DomainException("Se debe proporcionar al menos un teléfono o email");
                
            if (!string.IsNullOrWhiteSpace(email) && !IsValidEmail(email))
                throw new DomainException("El formato del email es inválido");
                
            if (!string.IsNullOrWhiteSpace(phoneNumber) && !IsValidPhoneNumber(phoneNumber))
                throw new DomainException("El formato del teléfono es inválido");
                
            // Asignaciones
            Address = address?.Trim();
            PhoneNumber = phoneNumber?.Trim();
            Email = email?.Trim().ToLowerInvariant();
        }
        
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            // Implementar validación según requisitos
            return System.Text.RegularExpressions.Regex.IsMatch(
                phoneNumber, @"^\+?[0-9]{8,15}$");
        }
        
        public bool HasPhone => !string.IsNullOrWhiteSpace(PhoneNumber);
        public bool HasEmail => !string.IsNullOrWhiteSpace(Email);
        public bool HasAddress => !string.IsNullOrWhiteSpace(Address);
        
        // Implementación de IEquatable<ContactInfo>
        public bool Equals(ContactInfo other)
        {
            if (other is null) return false;
            
            return string.Equals(PhoneNumber, other.PhoneNumber, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(Email, other.Email, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(Address, other.Address, StringComparison.OrdinalIgnoreCase);
        }
        
        public override bool Equals(object obj)
        {
            return obj is ContactInfo other && Equals(other);
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(
                PhoneNumber?.ToLowerInvariant(), 
                Email?.ToLowerInvariant(),
                Address?.ToLowerInvariant());
        }
        
        public static bool operator ==(ContactInfo left, ContactInfo right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }
        
        public static bool operator !=(ContactInfo left, ContactInfo right) => !(left == right);
        
        public override string ToString()
        {
            return $"Email: {Email}, Teléfono: {PhoneNumber}, Dirección: {Address}";
        }
    }
} 