using System;
using System.Collections.Generic;
using System.Linq;
using Odoonto.Domain.Core.Models.Exceptions;
using Odoonto.Domain.Models.ValueObjects;

namespace Odoonto.Domain.Models.Odontograms
{
    /// <summary>
    /// Registro histórico de un tratamiento que ya se completó sobre determinadas superficies dentales
    /// </summary>
    public class PerformedProcedure : IEquatable<PerformedProcedure>
    {
        public Guid TreatmentId { get; }
        public IReadOnlyCollection<ToothSurface> TreatedSurfaces { get; }
        public DateTime CompletionDate { get; }
        public string Notes { get; }

        public PerformedProcedure(Guid treatmentId, IEnumerable<ToothSurface> treatedSurfaces, DateTime completionDate, string notes = null)
        {
            if (treatmentId == Guid.Empty)
                throw new InvalidValueException("El identificador de tratamiento no puede estar vacío.");
            
            if (treatedSurfaces == null || !treatedSurfaces.Any())
                throw new InvalidValueException("Debe especificar al menos una superficie dental tratada.");
            
            if (completionDate > DateTime.Now)
                throw new InvalidValueException("La fecha de realización no puede ser futura.");

            TreatmentId = treatmentId;
            TreatedSurfaces = treatedSurfaces.ToList().AsReadOnly();
            CompletionDate = completionDate;
            Notes = notes?.Trim() ?? string.Empty;
        }

        // Value objects deben ser inmutables y comparables por valor
        public bool Equals(PerformedProcedure other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            
            return TreatmentId.Equals(other.TreatmentId) &&
                   TreatedSurfaces.Count == other.TreatedSurfaces.Count &&
                   TreatedSurfaces.All(s => other.TreatedSurfaces.Contains(s)) &&
                   CompletionDate.Date.Equals(other.CompletionDate.Date);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            
            return Equals((PerformedProcedure)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                TreatmentId,
                TreatedSurfaces.Count,
                CompletionDate.Date);
        }

        public override string ToString() => 
            $"Tratamiento: {TreatmentId}, Superficies: {string.Join(", ", TreatedSurfaces)}, Fecha: {CompletionDate.ToShortDateString()}";

        // Operadores de comparación
        public static bool operator ==(PerformedProcedure left, PerformedProcedure right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(PerformedProcedure left, PerformedProcedure right) => !(left == right);
    }
} 