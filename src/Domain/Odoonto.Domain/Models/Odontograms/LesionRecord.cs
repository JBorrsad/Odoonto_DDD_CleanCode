using System;
using System.Collections.Generic;
using System.Linq;
using Odoonto.Domain.Core.Models.Exceptions;
using Odoonto.Domain.Models.ValueObjects;

namespace Odoonto.Domain.Models.Odontograms
{
    /// <summary>
    /// Value object que representa un registro de lesión detectada en un diente
    /// </summary>
    public class LesionRecord : IEquatable<LesionRecord>
    {
        public Guid LesionId { get; }
        public IReadOnlyCollection<ToothSurface> AffectedSurfaces { get; }
        public DateTime DetectionDate { get; }
        public string Notes { get; }

        public LesionRecord(Guid lesionId, IEnumerable<ToothSurface> affectedSurfaces, DateTime detectionDate, string notes = null)
        {
            if (lesionId == Guid.Empty)
                throw new InvalidValueException("El identificador de lesión no puede estar vacío.");
            
            if (affectedSurfaces == null || !affectedSurfaces.Any())
                throw new InvalidValueException("Debe especificar al menos una superficie dental afectada.");
            
            if (detectionDate > DateTime.Now)
                throw new InvalidValueException("La fecha de detección no puede ser futura.");

            LesionId = lesionId;
            AffectedSurfaces = affectedSurfaces.ToList().AsReadOnly();
            DetectionDate = detectionDate;
            Notes = notes?.Trim() ?? string.Empty;
        }

        // Value objects deben ser inmutables y comparables por valor
        public bool Equals(LesionRecord other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            
            return LesionId.Equals(other.LesionId) &&
                   AffectedSurfaces.Count == other.AffectedSurfaces.Count &&
                   AffectedSurfaces.All(s => other.AffectedSurfaces.Contains(s)) &&
                   DetectionDate.Date.Equals(other.DetectionDate.Date);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            
            return Equals((LesionRecord)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                LesionId,
                (AffectedSurfaces != null) ? AffectedSurfaces.Count : 0,
                DetectionDate.Date);
        }

        public override string ToString() => 
            $"Lesión: {LesionId}, Superficies: {string.Join(", ", AffectedSurfaces)}, Fecha: {DetectionDate.ToShortDateString()}";

        // Operadores de comparación
        public static bool operator ==(LesionRecord left, LesionRecord right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(LesionRecord left, LesionRecord right) => !(left == right);
    }
} 