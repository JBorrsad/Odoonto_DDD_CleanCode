using System;
using System.Collections.Generic;
using Odoonto.Domain.Core.Abstractions;
using Odoonto.Domain.Core.Models.Exceptions;

namespace Odoonto.Domain.Models.ValueObjects
{
    /// <summary>
    /// Value object que representa un valor monetario con monto y divisa
    /// </summary>
    public class Money : ValueObject, IComparable<Money>
    {
        public decimal Amount { get; }
        public string Currency { get; }

        public Money(decimal amount, string currency)
        {
            if (amount < 0)
            {
                throw new InvalidValueException("El monto no puede ser negativo.");
            }

            if (string.IsNullOrWhiteSpace(currency))
            {
                throw new InvalidValueException("La divisa no puede estar vacía.");
            }

            Amount = amount;
            Currency = currency.Trim().ToUpperInvariant();
        }

        // Constructor simplificado con Euro como moneda predeterminada
        public Money(decimal amount) : this(amount, "EUR")
        {
        }

        // Métodos aritméticos (devuelven nuevas instancias)
        public Money Add(Money other)
        {
            EnsureSameCurrency(other);
            return new Money(Amount + other.Amount, Currency);
        }

        public Money Subtract(Money other)
        {
            EnsureSameCurrency(other);
            decimal result = Amount - other.Amount;
            if (result < 0)
            {
                throw new InvalidValueException("El resultado no puede ser negativo.");
            }
            return new Money(result, Currency);
        }

        public Money Multiply(decimal multiplier)
        {
            if (multiplier < 0)
            {
                throw new InvalidValueException("El multiplicador no puede ser negativo.");
            }
            return new Money(Amount * multiplier, Currency);
        }

        // Validación de misma moneda
        private void EnsureSameCurrency(Money other)
        {
            if (other == null)
            {
                throw new InvalidValueException("El valor monetario no puede ser nulo.");
            }

            if (Currency != other.Currency)
            {
                throw new InvalidValueException($"No se pueden operar montos con diferentes divisas: {Currency} y {other.Currency}.");
            }
        }

        // Implementación para ValueObject base
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }

        // Implementación de IComparable para ordenamiento
        public int CompareTo(Money other)
        {
            if (other is null) return 1;
            
            EnsureSameCurrency(other);
            return Amount.CompareTo(other.Amount);
        }
        
        // Operadores adicionales de comparación
        public static bool operator <(Money left, Money right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            return left.CompareTo(right) < 0;
        }
        
        public static bool operator >(Money left, Money right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            return left.CompareTo(right) > 0;
        }
        
        public static bool operator <=(Money left, Money right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            return left.CompareTo(right) <= 0;
        }
        
        public static bool operator >=(Money left, Money right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            return left.CompareTo(right) >= 0;
        }

        public override string ToString() => $"{Amount:N2} {Currency}";
    }
} 