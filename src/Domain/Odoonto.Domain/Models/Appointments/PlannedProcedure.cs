using System;
using System.Collections.Generic;
using Odoonto.Domain.Core.Abstractions;
using Odoonto.Domain.Core.Models.Exceptions;
using Odoonto.Domain.Models.ValueObjects;

namespace Odoonto.Domain.Models.Appointments
{
    /// <summary>
    /// Value object que representa un procedimiento planificado para realizar durante una cita
    /// </summary>
    public class PlannedProcedure : ValueObject
    {
        public Guid TreatmentId { get; }
        public string Notes { get; }
        public IReadOnlyCollection<ToothSurfaces> TeethToTreat { get; }

        public PlannedProcedure(Guid treatmentId, IEnumerable<ToothSurfaces> teethToTreat, string notes = null)
        {
            if (treatmentId == Guid.Empty)
                throw new InvalidValueException("El identificador de tratamiento no puede estar vacío.");
            
            if (teethToTreat == null)
                throw new InvalidValueException("Debe especificar al menos una superficie dental a tratar.");
            
            var teethList = new List<ToothSurfaces>();
            foreach (var tooth in teethToTreat)
            {
                if (tooth == null)
                    throw new InvalidValueException("Las superficies dentales no pueden ser nulas.");
                teethList.Add(tooth);
            }
            
            if (teethList.Count == 0)
                throw new InvalidValueException("Debe especificar al menos una superficie dental a tratar.");

            TreatmentId = treatmentId;
            TeethToTreat = teethList.AsReadOnly();
            Notes = notes?.Trim() ?? string.Empty;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return TreatmentId;
            
            foreach (var tooth in TeethToTreat)
            {
                yield return tooth;
            }
            
            yield return Notes;
        }

        public override string ToString() => 
            $"Tratamiento: {TreatmentId}, Dientes a tratar: {TeethToTreat.Count}, Notas: {(string.IsNullOrEmpty(Notes) ? "N/A" : Notes)}";
    }
} 