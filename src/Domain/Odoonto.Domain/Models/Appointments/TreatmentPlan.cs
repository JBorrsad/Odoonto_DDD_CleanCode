using System;
using System.Collections.Generic;
using System.Linq;
using Odoonto.Domain.Core.Abstractions;
using Odoonto.Domain.Core.Models.Exceptions;
using Odoonto.Domain.Models.ValueObjects;

namespace Odoonto.Domain.Models.Appointments
{
    /// <summary>
    /// Value object que representa un plan de tratamiento completo para un paciente
    /// </summary>
    public class TreatmentPlan : ValueObject
    {
        public IReadOnlyCollection<PlannedProcedure> Procedures { get; }
        public Money TotalCost { get; }

        private TreatmentPlan(IEnumerable<PlannedProcedure> procedures, Money totalCost)
        {
            Procedures = procedures.ToList().AsReadOnly();
            TotalCost = totalCost;
        }

        public static TreatmentPlan Create(IEnumerable<PlannedProcedure> procedures, Money totalCost)
        {
            if (procedures == null || !procedures.Any())
                throw new InvalidValueException("El plan de tratamiento debe incluir al menos un procedimiento.");
            
            if (totalCost == null)
                throw new InvalidValueException("El coste total no puede ser nulo.");

            return new TreatmentPlan(procedures, totalCost);
        }

        // Método para añadir un nuevo procedimiento (devuelve un nuevo TreatmentPlan)
        public TreatmentPlan AddProcedure(PlannedProcedure procedure, Money additionalCost)
        {
            if (procedure == null)
                throw new InvalidValueException("El procedimiento no puede ser nulo.");
            
            if (additionalCost == null)
                throw new InvalidValueException("El coste adicional no puede ser nulo.");

            var newProcedures = new List<PlannedProcedure>(Procedures) { procedure };
            var newTotalCost = TotalCost.Add(additionalCost);
            
            return new TreatmentPlan(newProcedures, newTotalCost);
        }

        // Implementación de ValueObject base
        protected override IEnumerable<object> GetEqualityComponents()
        {
            foreach (var procedure in Procedures)
            {
                yield return procedure;
            }
            
            yield return TotalCost;
        }

        public override string ToString() => 
            $"Plan con {Procedures.Count} procedimiento(s), Coste total: {TotalCost}";
    }
} 