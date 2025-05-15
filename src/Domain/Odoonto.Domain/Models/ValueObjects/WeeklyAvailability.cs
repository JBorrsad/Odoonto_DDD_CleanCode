using System;
using System.Collections.Generic;
using System.Linq;
using Odoonto.Domain.Core.Abstractions;
using Odoonto.Domain.Core.Models.Exceptions;

namespace Odoonto.Domain.Models.ValueObjects
{
    /// <summary>
    /// Value object que representa la disponibilidad semanal de un doctor
    /// </summary>
    public class WeeklyAvailability : ValueObject
    {
        // Mapa de disponibilidad por día de la semana
        private readonly IReadOnlyDictionary<DayOfWeek, IReadOnlyCollection<TimeRange>> _availability;

        // Propiedad de solo lectura para acceder a la disponibilidad
        public IReadOnlyDictionary<DayOfWeek, IReadOnlyCollection<TimeRange>> Availability => _availability;

        // Constructor simple
        public WeeklyAvailability(Dictionary<DayOfWeek, ICollection<TimeRange>> availability)
        {
            if (availability == null)
            {
                throw new DomainException("La disponibilidad no puede ser nula.");
            }

            // Crear copia inmutable de la disponibilidad
            var availabilityDict = new Dictionary<DayOfWeek, IReadOnlyCollection<TimeRange>>();
            
            // Asegurar que todos los días de la semana están incluidos
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                if (availability.TryGetValue(day, out var ranges) && ranges != null)
                {
                    // Validar que no haya superposición de horarios
                    ValidateNoOverlaps(ranges, day);
                    
                    // Ordenar y crear colección inmutable 
                    availabilityDict[day] = ranges
                        .OrderBy(r => r.StartTime)
                        .ToList()
                        .AsReadOnly();
                }
                else
                {
                    // Si el día no está incluido, agregar una lista vacía
                    availabilityDict[day] = new List<TimeRange>().AsReadOnly();
                }
            }
            
            _availability = availabilityDict;
        }

        // Constructor vacío para crear disponibilidad sin rangos
        public WeeklyAvailability() : this(new Dictionary<DayOfWeek, ICollection<TimeRange>>())
        {
        }

        // Factory para crear con un método fluido
        public static WeeklyAvailability Create()
        {
            return new WeeklyAvailability();
        }

        // Método para crear una nueva disponibilidad con un rango adicional
        public WeeklyAvailability AddTimeRange(DayOfWeek day, TimeRange timeRange)
        {
            if (timeRange == null)
            {
                throw new DomainException("El rango de tiempo no puede ser nulo.");
            }

            // Crear copia de la disponibilidad actual
            var newAvailability = new Dictionary<DayOfWeek, ICollection<TimeRange>>();
            
            foreach (var kvp in _availability)
            {
                newAvailability[kvp.Key] = new List<TimeRange>(kvp.Value);
            }
            
            // Agregar el nuevo rango para el día especificado
            if (!newAvailability.ContainsKey(day))
            {
                newAvailability[day] = new List<TimeRange>();
            }
            
            newAvailability[day].Add(timeRange);
            
            // Crear y devolver una nueva instancia con el rango agregado
            return new WeeklyAvailability(newAvailability);
        }

        // Agregar TimeSlot a la disponibilidad
        public WeeklyAvailability AddTimeSlot(DayOfWeek day, TimeSlot timeSlot)
        {
            if (timeSlot == null)
            {
                throw new DomainException("El slot de tiempo no puede ser nulo.");
            }

            // Crear TimeRange a partir del TimeSlot
            var timeRange = new TimeRange(
                TimeSpan.FromHours(timeSlot.StartTime.Hour) + TimeSpan.FromMinutes(timeSlot.StartTime.Minute),
                TimeSpan.FromHours(timeSlot.EndTime.Hour) + TimeSpan.FromMinutes(timeSlot.EndTime.Minute)
            );

            return AddTimeRange(day, timeRange);
        }

        // Método para verificar disponibilidad en un día y rango específico
        public bool IsAvailable(DayOfWeek day, TimeSlot timeSlot)
        {
            if (timeSlot == null)
            {
                throw new DomainException("El período de tiempo no puede ser nulo.");
            }

            // Verificar si el período está dentro de algún rango disponible
            return _availability[day].Any(tr => tr.Contains(timeSlot));
        }

        // Método para verificar si un TimeSlot está dentro de la disponibilidad de un día específico
        public bool IsWithinAvailability(DayOfWeek day, TimeSlot timeSlot)
        {
            if (timeSlot == null)
            {
                throw new DomainException("El período de tiempo no puede ser nulo.");
            }

            if (!_availability.ContainsKey(day))
            {
                return false;
            }

            return _availability[day].Any(tr => 
            {
                // Convertir TimeOnly a TimeSpan para comparar
                var slotStartSpan = TimeSpan.FromHours(timeSlot.StartTime.Hour) + TimeSpan.FromMinutes(timeSlot.StartTime.Minute);
                var slotEndSpan = TimeSpan.FromHours(timeSlot.EndTime.Hour) + TimeSpan.FromMinutes(timeSlot.EndTime.Minute);
                
                // El TimeSlot está dentro del rango si su inicio es >= al inicio del rango
                // y su fin es <= al fin del rango
                return slotStartSpan >= tr.StartTime && slotEndSpan <= tr.EndTime;
            });
        }
        
        // Obtiene los días que tienen al menos un rango de disponibilidad
        public IEnumerable<DayOfWeek> GetDaysWithAvailability()
        {
            return _availability
                .Where(kvp => kvp.Value.Count > 0)
                .Select(kvp => kvp.Key);
        }

        // Obtiene los slots de tiempo disponibles para un día específico
        public IEnumerable<TimeSlot> GetTimeSlots(DayOfWeek day)
        {
            if (!_availability.ContainsKey(day))
            {
                return Enumerable.Empty<TimeSlot>();
            }

            return _availability[day].Select(tr => 
            {
                // Convertir TimeSpan a TimeOnly para crear TimeSlot
                var startTime = new TimeOnly((int)tr.StartTime.TotalHours, tr.StartTime.Minutes);
                var endTime = new TimeOnly((int)tr.EndTime.TotalHours, tr.EndTime.Minutes);
                return new TimeSlot(startTime, endTime);
            });
        }
        
        // Valida que no haya superposición entre los rangos
        private void ValidateNoOverlaps(ICollection<TimeRange> ranges, DayOfWeek day)
        {
            var rangesList = ranges.ToList();
            
            for (int i = 0; i < rangesList.Count; i++)
            {
                for (int j = i + 1; j < rangesList.Count; j++)
                {
                    if (rangesList[i].Overlaps(rangesList[j]))
                    {
                        throw new DomainException($"El rango de tiempo se superpone con otro existente en {day}.");
                    }
                }
            }
        }

        // Implementación para ValueObject base
        protected override IEnumerable<object> GetEqualityComponents()
        {
            // Para cada día de la semana, agregar sus rangos al conjunto de comparación
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                // Primero el día
                yield return day;
                
                // Luego cada rango ordenado
                foreach (var range in _availability[day])
                {
                    yield return range;
                }
            }
        }
    }
} 