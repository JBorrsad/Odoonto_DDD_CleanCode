using System;
using System.Collections.Generic;
using Odoonto.Domain.Core.Abstractions;
using Odoonto.Domain.Core.Models.Exceptions;

namespace Odoonto.Domain.Models.ValueObjects
{
    /// <summary>
    /// Representa un intervalo de tiempo
    /// </summary>
    public class TimeSlot : IEquatable<TimeSlot>
    {
        public TimeOnly StartTime { get; }
        public TimeOnly EndTime { get; }
        
        /// <summary>
        /// Constructor con TimeOnly
        /// </summary>
        public TimeSlot(TimeOnly startTime, TimeOnly endTime)
        {
            if (endTime <= startTime)
                throw new DomainException("La hora de fin debe ser posterior a la hora de inicio");
                
            StartTime = startTime;
            EndTime = endTime;
        }
        
        /// <summary>
        /// Constructor con DateTime
        /// </summary>
        public TimeSlot(DateTime startTime, DateTime endTime)
        {
            if (endTime <= startTime)
                throw new DomainException("La hora de fin debe ser posterior a la hora de inicio");
                
            StartTime = TimeOnly.FromDateTime(startTime);
            EndTime = TimeOnly.FromDateTime(endTime);
        }
        
        public TimeSpan Duration => EndTime - StartTime;
        
        public bool Overlaps(TimeSlot other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));
                
            return (StartTime < other.EndTime && EndTime > other.StartTime);
        }
        
        public bool Contains(TimeOnly time)
        {
            return time >= StartTime && time < EndTime;
        }
        
        public bool Contains(DateTime time)
        {
            return Contains(TimeOnly.FromDateTime(time));
        }
        
        // Implementación de IEquatable<TimeSlot>
        public bool Equals(TimeSlot other)
        {
            if (other is null) return false;
            
            return StartTime == other.StartTime && EndTime == other.EndTime;
        }
        
        public override bool Equals(object obj)
        {
            return obj is TimeSlot slot && Equals(slot);
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(StartTime, EndTime);
        }
        
        public static bool operator ==(TimeSlot left, TimeSlot right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }
        
        public static bool operator !=(TimeSlot left, TimeSlot right) => !(left == right);
        
        public override string ToString() => $"{StartTime:hh\\:mm} - {EndTime:hh\\:mm}";
    }
} 