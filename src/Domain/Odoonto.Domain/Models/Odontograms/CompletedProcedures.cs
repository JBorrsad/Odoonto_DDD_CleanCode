using System;
using System.Collections.Generic;
using System.Linq;
using Odoonto.Domain.Core.Abstractions;
using Odoonto.Domain.Core.Models.Exceptions;

namespace Odoonto.Domain.Models.Odontograms
{
    /// <summary>
    /// Value object que representa un conjunto inmutable de procedimientos completados en un odontograma
    /// </summary>
    public class CompletedProcedures : ValueObject
    {
        /// <summary>
        /// Lista inmutable de procedimientos completados
        /// </summary>
        public IReadOnlyCollection<PerformedProcedure> Procedures { get; }

        /// <summary>
        /// Fecha de la última actualización
        /// </summary>
        public DateTime LastUpdated { get; }

        /// <summary>
        /// Constructor para crear un registro con procedimientos realizados
        /// </summary>
        /// <param name="procedures">Procedimientos completados</param>
        public CompletedProcedures(IEnumerable<PerformedProcedure> procedures)
        {
            if (procedures == null)
                throw new InvalidValueException("La lista de procedimientos no puede ser nula.");

            var proceduresList = procedures.ToList();

            // Validar que no hay procedimientos nulos
            if (proceduresList.Any(p => p == null))
                throw new InvalidValueException("La lista contiene procedimientos nulos.");

            Procedures = proceduresList.AsReadOnly();
            LastUpdated = DateTime.UtcNow;
        }

        /// <summary>
        /// Constructor privado para crear una instancia con una fecha específica
        /// </summary>
        private CompletedProcedures(IEnumerable<PerformedProcedure> procedures, DateTime lastUpdated)
        {
            Procedures = procedures.ToList().AsReadOnly();
            LastUpdated = lastUpdated;
        }

        /// <summary>
        /// Crea una nueva instancia con un procedimiento adicional
        /// </summary>
        /// <param name="newProcedure">Nuevo procedimiento a añadir</param>
        /// <returns>Nueva instancia de CompletedProcedures con el procedimiento adicional</returns>
        public CompletedProcedures AddProcedure(PerformedProcedure newProcedure)
        {
            if (newProcedure == null)
                throw new InvalidValueException("El procedimiento no puede ser nulo.");

            var updatedProcedures = new List<PerformedProcedure>(Procedures) { newProcedure };
            return new CompletedProcedures(updatedProcedures, DateTime.UtcNow);
        }

        /// <summary>
        /// Filtra los procedimientos por ID de tratamiento
        /// </summary>
        /// <param name="treatmentId">ID del tratamiento a filtrar</param>
        /// <returns>Lista de procedimientos que coinciden con el tratamiento</returns>
        public IEnumerable<PerformedProcedure> GetByTreatmentId(Guid treatmentId)
        {
            return Procedures.Where(p => p.TreatmentId == treatmentId);
        }

        /// <summary>
        /// Obtiene los procedimientos realizados en un período específico
        /// </summary>
        /// <param name="startDate">Fecha de inicio</param>
        /// <param name="endDate">Fecha de fin</param>
        /// <returns>Lista de procedimientos realizados en el período</returns>
        public IEnumerable<PerformedProcedure> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            return Procedures.Where(p => 
                p.CompletionDate.Date >= startDate.Date && 
                p.CompletionDate.Date <= endDate.Date);
        }

        /// <summary>
        /// Verifica si alguno de los procedimientos se realizó en un diente específico
        /// </summary>
        /// <param name="toothNumber">Número de diente a verificar</param>
        /// <returns>True si se encontraron procedimientos en ese diente</returns>
        public bool HasProceduresForTooth(int toothNumber)
        {
            return Procedures.Any(p => p.TreatedSurfaces.Any(s => s.ToothNumber == toothNumber));
        }

        /// <summary>
        /// Obtiene todos los procedimientos realizados en un diente específico
        /// </summary>
        /// <param name="toothNumber">Número de diente</param>
        /// <returns>Lista de procedimientos realizados en ese diente</returns>
        public IEnumerable<PerformedProcedure> GetProceduresForTooth(int toothNumber)
        {
            return Procedures.Where(p => p.TreatedSurfaces.Any(s => s.ToothNumber == toothNumber));
        }

        // Implementación de ValueObject
        protected override IEnumerable<object> GetEqualityComponents()
        {
            foreach (var procedure in Procedures.OrderBy(p => p.CompletionDate))
            {
                yield return procedure;
            }
        }

        // Representación en string
        public override string ToString() =>
            $"Procedimientos completados: {Procedures.Count}, Última actualización: {LastUpdated:yyyy-MM-dd}";
    }
} 