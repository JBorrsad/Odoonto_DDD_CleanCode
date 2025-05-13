using System;
using System.Collections.Generic;
using System.Linq;
using Odoonto.Domain.Core.Models.Exceptions;

namespace Odoonto.Domain.Models.Odontograms
{
    /// <summary>
    /// Representa el mapa dental completo del paciente y agrupa el historial individual de cada diente
    /// </summary>
    public class Odontogram
    {
        private readonly Dictionary<int, ToothRecord> _toothRecords;

        public IReadOnlyCollection<ToothRecord> ToothRecords => _toothRecords.Values;

        // Constructor
        public Odontogram()
        {
            _toothRecords = new Dictionary<int, ToothRecord>();
        }

        // Método para añadir un registro dental
        public void AddToothRecord(ToothRecord toothRecord)
        {
            if (toothRecord == null)
                throw new InvalidValueException("El registro dental no puede ser nulo.");

            int toothNumber = toothRecord.ToothNumber;
            ValidateToothNumber(toothNumber);

            if (_toothRecords.ContainsKey(toothNumber))
                throw new DuplicatedValueException($"Ya existe un registro para el diente {toothNumber}.");

            _toothRecords.Add(toothNumber, toothRecord);
        }

        // Método para obtener un registro dental
        public ToothRecord GetToothRecord(int toothNumber)
        {
            ValidateToothNumber(toothNumber);

            if (_toothRecords.TryGetValue(toothNumber, out ToothRecord toothRecord))
                return toothRecord;

            return null;
        }

        // Método para actualizar un registro dental existente
        public void UpdateToothRecord(ToothRecord toothRecord)
        {
            if (toothRecord == null)
                throw new InvalidValueException("El registro dental no puede ser nulo.");

            int toothNumber = toothRecord.ToothNumber;
            ValidateToothNumber(toothNumber);

            if (!_toothRecords.ContainsKey(toothNumber))
                throw new WrongOperationException($"No existe un registro para el diente {toothNumber}.");

            _toothRecords[toothNumber] = toothRecord;
        }

        // Método para verificar si existe un registro dental
        public bool HasToothRecord(int toothNumber)
        {
            ValidateToothNumber(toothNumber);
            return _toothRecords.ContainsKey(toothNumber);
        }

        // Método para validar número de diente
        private void ValidateToothNumber(int toothNumber)
        {
            bool isValidAdult = toothNumber >= 1 && toothNumber <= 32;
            bool isValidChild = toothNumber >= 51 && toothNumber <= 85;

            if (!isValidAdult && !isValidChild)
                throw new InvalidValueException($"Número de diente inválido: {toothNumber}. Debe estar entre 1-32 (adultos) o 51-85 (niños).");
        }

        // Método para obtener registros con lesiones
        public IEnumerable<ToothRecord> GetTeethWithLesions()
        {
            return _toothRecords.Values.Where(tr => tr.HasLesions);
        }

        // Método para obtener registros con procedimientos realizados
        public IEnumerable<ToothRecord> GetTeethWithProcedures()
        {
            return _toothRecords.Values.Where(tr => tr.HasCompletedProcedures);
        }
    }
} 