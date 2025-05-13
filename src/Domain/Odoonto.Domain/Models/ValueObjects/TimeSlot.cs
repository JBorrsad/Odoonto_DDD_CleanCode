using System;
using System.Collections.Generic;
using Odoonto.Domain.Core.Abstractions;
using Odoonto.Domain.Core.Models.Exceptions;

namespace Odoonto.Domain.Models.ValueObjects
{
    /// <summary>
    /// Value object que representa un período de tiempo con hora de inicio y fin
    /// </summary>
    public class TimeSlot : ValueObject
    {
        public TimeSpan StartTime { get; }
        public TimeSpan EndTime { get; }
        
        // Duración calculada
        public TimeSpan Duration => EndTime - StartTime;

        public TimeSlot(TimeSpan startTime, TimeSpan endTime)
        {
            if (endTime <= startTime)
            {
                throw new InvalidValueException("La hora de fin debe ser posterior a la hora de inicio.");
            }

            StartTime = startTime;
            EndTime = endTime;
        }

        // Verifica si este período se superpone con otro
        public bool Overlaps(TimeSlot other)
        {
            return StartTime < other.EndTime && EndTime > other.StartTime;
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