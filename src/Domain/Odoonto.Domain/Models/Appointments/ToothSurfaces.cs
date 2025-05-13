using System;
using System.Collections.Generic;
using System.Linq;
using Odoonto.Domain.Core.Models.Exceptions;
using Odoonto.Domain.Models.ValueObjects;

namespace Odoonto.Domain.Models.Appointments
{
    /// <summary>
    /// Conjunto de superficies para un diente específico
    /// </summary>
    public class ToothSurfaces : IEquatable<ToothSurfaces>
    {
        public int ToothNumber { get; }
        public IReadOnlyCollection<ToothSurface> Surfaces { get; }

        public ToothSurfaces(int toothNumber, IEnumerable<ToothSurface> surfaces)
        {
            // Validar número de diente
            bool isValidAdult = toothNumber >= 1 && toothNumber <= 32;
            bool isValidChild = toothNumber >= 51 && toothNumber <= 85;

            if (!isValidAdult && !isValidChild)
                throw new InvalidValueException($"Número de diente inválido: {toothNumber}. Debe estar entre 1-32 (adultos) o 51-85 (niños).");
            
            if (surfaces == null || !surfaces.Any())
                throw new InvalidValueException("Debe especificar al menos una superficie dental.");

            // Validar que dientes anteriores no tengan superficie oclusal
            bool isAnterior = (toothNumber >= 6 && toothNumber <= 11) || 
                             (toothNumber >= 22 && toothNumber <= 27) ||
                             (toothNumber >= 53 && toothNumber <= 63) || 
                             (toothNumber >= 73 && toothNumber <= 83);

            if (isAnterior && surfaces.Contains(ToothSurface.Occlusal))
                throw new InvalidValueException($"Los dientes anteriores (como el {toothNumber}) no pueden tener superficie oclusal.");

            ToothNumber = toothNumber;
            Surfaces = surfaces.ToList().AsReadOnly();
        }

        // Value objects deben ser inmutables y comparables por valor
        public bool Equals(ToothSurfaces other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            
            return ToothNumber == other.ToothNumber &&
                   Surfaces.Count == other.Surfaces.Count &&
                   Surfaces.All(s => other.Surfaces.Contains(s));
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            
            return Equals((ToothSurfaces)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                ToothNumber,
                Surfaces.Count);
        }

        public override string ToString() => 
            $"Diente {ToothNumber}: {string.Join(", ", Surfaces)}";

        // Operadores de comparación
        public static bool operator ==(ToothSurfaces left, ToothSurfaces right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(ToothSurfaces left, ToothSurfaces right) => !(left == right);
    }
} 