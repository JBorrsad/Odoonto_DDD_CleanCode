using System;
using System.Collections.Generic;
using Odoonto.Domain.Core.Abstractions;
using Odoonto.Domain.Core.Models.Exceptions;

namespace Odoonto.Domain.Models.ValueObjects
{
    /// <summary>
    /// Value object que representa el estado de una cita odontológica
    /// </summary>
    public class AppointmentStatus : ValueObject
    {
        private readonly string _value;

        // Valores predefinidos
        public static readonly AppointmentStatus Scheduled = new AppointmentStatus("Scheduled");
        public static readonly AppointmentStatus WaitingRoom = new AppointmentStatus("WaitingRoom");
        public static readonly AppointmentStatus InProgress = new AppointmentStatus("InProgress");
        public static readonly AppointmentStatus Completed = new AppointmentStatus("Completed");
        public static readonly AppointmentStatus Cancelled = new AppointmentStatus("Cancelled");

        // Constructor privado para controlar la creación
        private AppointmentStatus(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidValueException("El valor del estado no puede estar vacío.");

            _value = value;
        }

        // Método factory para crear instancias válidas
        public static AppointmentStatus FromString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidValueException("El valor del estado no puede estar vacío.");

            // Normalizar para comparación
            string normalizedValue = value.Trim();

            return normalizedValue switch
            {
                "Scheduled" => Scheduled,
                "WaitingRoom" => WaitingRoom,
                "InProgress" => InProgress,
                "Completed" => Completed,
                "Cancelled" => Cancelled,
                _ => throw new InvalidValueException($"'{value}' no es un estado válido.")
            };
        }

        // Implementación para ValueObject base
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _value;
        }

        public override string ToString() => _value;
    }
} 