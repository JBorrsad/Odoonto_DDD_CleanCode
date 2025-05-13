using System;
using System.Collections.Generic;
using System.Linq;
using Odoonto.Domain.Core.Models;
using Odoonto.Domain.Core.Models.Exceptions;
using Odoonto.Domain.Models.ValueObjects;

namespace Odoonto.Domain.Models.Odontograms
{
    /// <summary>
    /// Mantiene el historial clínico de una pieza dental específica dentro del Odontogram
    /// </summary>
    public class ToothRecord : Entity
    {
        // Propiedades
        public int ToothNumber { get; private set; }
        
        // Colecciones privadas
        private readonly List<LesionRecord> _lesionRecords;
        private readonly List<PerformedProcedure> _completedProcedures;

        // Propiedades de solo lectura para las colecciones
        public IReadOnlyCollection<LesionRecord> LesionRecords => _lesionRecords.AsReadOnly();
        public IReadOnlyCollection<PerformedProcedure> CompletedProcedures => _completedProcedures.AsReadOnly();

        // Propiedades calculadas
        public bool HasLesions => _lesionRecords.Count > 0;
        public bool HasCompletedProcedures => _completedProcedures.Count > 0;

        // Constructor privado
        private ToothRecord(Guid id, int toothNumber) : base(id)
        {
            _lesionRecords = new List<LesionRecord>();
            _completedProcedures = new List<PerformedProcedure>();
            SetToothNumber(toothNumber);
        }

        // Método factory
        public static ToothRecord Create(Guid id, int toothNumber)
        {
            if (id == Guid.Empty)
            {
                throw new InvalidValueException("El identificador del registro dental no puede estar vacío.");
            }

            var record = new ToothRecord(id, toothNumber);
            record.UpdateEditDate();
            return record;
        }

        // Método para establecer el número de diente
        private void SetToothNumber(int toothNumber)
        {
            bool isValidAdult = toothNumber >= 1 && toothNumber <= 32;
            bool isValidChild = toothNumber >= 51 && toothNumber <= 85;

            if (!isValidAdult && !isValidChild)
                throw new InvalidValueException($"Número de diente inválido: {toothNumber}. Debe estar entre 1-32 (adultos) o 51-85 (niños).");

            ToothNumber = toothNumber;
        }

        // Método para añadir un registro de lesión
        public void AddLesionRecord(LesionRecord lesionRecord)
        {
            if (lesionRecord == null)
                throw new InvalidValueException("El registro de lesión no puede ser nulo.");

            // Verificar que las superficies dentales son válidas para este diente
            ValidateToothSurfaces(lesionRecord.AffectedSurfaces);

            _lesionRecords.Add(lesionRecord);
            UpdateEditDate();
        }

        // Método para añadir un procedimiento completado
        public void AddPerformedProcedure(PerformedProcedure procedure)
        {
            if (procedure == null)
                throw new InvalidValueException("El procedimiento no puede ser nulo.");

            // Verificar que las superficies dentales son válidas para este diente
            ValidateToothSurfaces(procedure.TreatedSurfaces);

            _completedProcedures.Add(procedure);
            UpdateEditDate();
        }

        // Método para validar superficies dentales
        private void ValidateToothSurfaces(IEnumerable<ToothSurface> surfaces)
        {
            if (surfaces == null)
                throw new InvalidValueException("Las superficies dentales no pueden ser nulas.");

            // Verificar si el diente es anterior (incisivos y caninos)
            bool isAnterior = (ToothNumber >= 6 && ToothNumber <= 11) || 
                              (ToothNumber >= 22 && ToothNumber <= 27) ||
                              (ToothNumber >= 53 && ToothNumber <= 63) || 
                              (ToothNumber >= 73 && ToothNumber <= 83);

            // Los dientes anteriores no pueden tener superficie oclusal (O)
            if (isAnterior && surfaces.Any(s => s == ToothSurface.Occlusal))
                throw new InvalidValueException($"Los dientes anteriores (como el {ToothNumber}) no pueden tener superficie oclusal.");
        }

        // Método para obtener las lesiones activas (no tratadas)
        public IEnumerable<LesionRecord> GetActiveLesions()
        {
            // Una lesión se considera activa si no hay un procedimiento completado que la trate
            return _lesionRecords.Where(lesion => 
                !_completedProcedures.Any(proc => 
                    proc.CompletionDate > lesion.DetectionDate && 
                    proc.TreatedSurfaces.Intersect(lesion.AffectedSurfaces).Any()));
        }
    }
} 