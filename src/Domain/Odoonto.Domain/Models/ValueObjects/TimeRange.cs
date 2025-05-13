using System;
using System.Collections.Generic;
using Odoonto.Domain.Core.Abstractions;
using Odoonto.Domain.Core.Models.Exceptions;

namespace Odoonto.Domain.Models.ValueObjects
{
    /// <summary>
    /// Value object que representa un rango de tiempo con hora de inicio y fin
    /// </summary>
    public class TimeRange : ValueObject
    {
        public TimeSpan StartTime { get; }
        public TimeSpan EndTime { get; }

        public TimeRange(TimeSpan startTime, TimeSpan endTime)
        {
            if (endTime <= startTime)
            {
                throw new InvalidValueException("La hora de fin debe ser posterior a la hora de inicio.");
            }

            StartTime = startTime;
            EndTime = endTime;
        }

        // Verifica si este rango se superpone con otro
        public bool Overlaps(TimeRange other)
        {
            return StartTime < other.EndTime && EndTime > other.StartTime;
        }

        // Verifica si un TimeSlot está contenido en este rango
        public bool Contains(TimeSlot timeSlot)
        {
            return StartTime <= timeSlot.StartTime && EndTime >= timeSlot.EndTime;
        }

        // Implementación para ValueObject base
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return StartTime;
            yield return EndTime;
        }

        public override string ToString() => $"{StartTime:hh\\:mm} - {EndTime:hh\\:mm}";
    }
} 