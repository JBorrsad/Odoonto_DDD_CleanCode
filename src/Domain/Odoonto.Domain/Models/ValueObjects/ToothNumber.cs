using System;
using System.Collections.Generic;
using Odoonto.Domain.Core.Abstractions;
using Odoonto.Domain.Core.Models.Exceptions;

namespace Odoonto.Domain.Models.ValueObjects
{
    /// <summary>
    /// Enumeración que representa el tipo de dentición
    /// </summary>
    public enum DentitionType
    {
        Adult,  // Dentición permanente (adultos)
        Child   // Dentición primaria (niños)
    }

    /// <summary>
    /// Value object que representa un número de diente con validación según tipo de dentición
    /// </summary>
    public class ToothNumber : ValueObject, IComparable<ToothNumber>
    {
        // Rangos válidos para dientes
        private const int MinAdultToothNumber = 1;
        private const int MaxAdultToothNumber = 32;
        private const int MinChildToothNumber = 51;
        private const int MaxChildToothNumber = 85;

        /// <summary>
        /// Número de diente
        /// </summary>
        public int Number { get; }

        /// <summary>
        /// Tipo de dentición (adulto o niño)
        /// </summary>
        public DentitionType DentitionType { get; }

        /// <summary>
        /// Constructor que acepta un número de diente y valida según la dentición detectada automáticamente
        /// </summary>
        /// <param name="number">Número de diente</param>
        public ToothNumber(int number)
        {
            if (IsValidAdultTooth(number))
            {
                Number = number;
                DentitionType = DentitionType.Adult;
            }
            else if (IsValidChildTooth(number))
            {
                Number = number;
                DentitionType = DentitionType.Child;
            }
            else
            {
                throw new InvalidValueException(
                    $"Número de diente inválido: {number}. " +
                    $"Debe estar entre {MinAdultToothNumber}-{MaxAdultToothNumber} para adultos o " +
                    $"{MinChildToothNumber}-{MaxChildToothNumber} para niños.");
            }
        }

        /// <summary>
        /// Constructor que acepta un número de diente y un tipo de dentición específico
        /// </summary>
        /// <param name="number">Número de diente</param>
        /// <param name="dentitionType">Tipo de dentición</param>
        public ToothNumber(int number, DentitionType dentitionType)
        {
            if (dentitionType == DentitionType.Adult && !IsValidAdultTooth(number))
            {
                throw new InvalidValueException(
                    $"Número de diente adulto inválido: {number}. " +
                    $"Debe estar entre {MinAdultToothNumber} y {MaxAdultToothNumber}.");
            }
            else if (dentitionType == DentitionType.Child && !IsValidChildTooth(number))
            {
                throw new InvalidValueException(
                    $"Número de diente de niño inválido: {number}. " +
                    $"Debe estar entre {MinChildToothNumber} y {MaxChildToothNumber}.");
            }

            Number = number;
            DentitionType = dentitionType;
        }

        /// <summary>
        /// Verifica si el diente es anterior (incisivos o caninos)
        /// </summary>
        public bool IsAnterior()
        {
            if (DentitionType == DentitionType.Adult)
            {
                // Dientes anteriores adultos (incisivos y caninos)
                return (Number >= 6 && Number <= 11) || (Number >= 22 && Number <= 27);
            }
            else
            {
                // Dientes anteriores de niños
                return (Number >= 53 && Number <= 63) || (Number >= 73 && Number <= 83);
            }
        }

        /// <summary>
        /// Verifica si el diente es posterior (premolares y molares)
        /// </summary>
        public bool IsPosterior()
        {
            return !IsAnterior();
        }

        /// <summary>
        /// Verifica si es un diente válido para adultos
        /// </summary>
        private static bool IsValidAdultTooth(int number)
        {
            return number >= MinAdultToothNumber && number <= MaxAdultToothNumber;
        }

        /// <summary>
        /// Verifica si es un diente válido para niños
        /// </summary>
        private static bool IsValidChildTooth(int number)
        {
            return number >= MinChildToothNumber && number <= MaxChildToothNumber;
        }

        /// <summary>
        /// Obtiene el cuadrante del diente (1-4 para adultos, 5-8 para niños)
        /// </summary>
        public int GetQuadrant()
        {
            if (DentitionType == DentitionType.Adult)
            {
                if (Number >= 1 && Number <= 8) return 1;
                if (Number >= 9 && Number <= 16) return 2;
                if (Number >= 17 && Number <= 24) return 3;
                return 4; // 25-32
            }
            else // Child
            {
                if (Number >= 51 && Number <= 55) return 5;
                if (Number >= 61 && Number <= 65) return 6;
                if (Number >= 71 && Number <= 75) return 7;
                return 8; // 81-85
            }
        }

        // Implementación de ValueObject
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Number;
        }

        // Implementación de IComparable
        public int CompareTo(ToothNumber other)
        {
            if (other is null) return 1;
            return Number.CompareTo(other.Number);
        }

        // Operadores de comparación
        public static bool operator <(ToothNumber left, ToothNumber right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(ToothNumber left, ToothNumber right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(ToothNumber left, ToothNumber right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(ToothNumber left, ToothNumber right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            return left.CompareTo(right) >= 0;
        }

        // Representación en string
        public override string ToString()
        {
            return $"{Number} ({DentitionType})";
        }
    }
} 