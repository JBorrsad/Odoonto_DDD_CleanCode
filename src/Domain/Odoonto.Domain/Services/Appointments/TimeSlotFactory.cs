using System;
using System.Collections.Generic;
using Odoonto.Domain.Models.ValueObjects;

namespace Odoonto.Domain.Services.Appointments
{
    /// <summary>
    /// Factory para crear slots de tiempo
    /// </summary>
    public static class TimeSlotFactory
    {
        // Horario de inicio y fin de la clínica
        private static readonly TimeOnly ClinicOpenTime = new TimeOnly(8, 0);
        private static readonly TimeOnly ClinicCloseTime = new TimeOnly(19, 0);
        
        // Duración de slot estándar: 30 minutos
        private static readonly TimeSpan SlotDuration = TimeSpan.FromMinutes(30);
        
        /// <summary>
        /// Obtiene todos los slots disponibles en un día de clínica
        /// </summary>
        /// <returns>Array de slots de tiempo</returns>
        public static TimeSlot[] GetAllDailySlots()
        {
            var slots = new List<TimeSlot>();
            var currentTime = ClinicOpenTime;
            
            while (currentTime.AddMinutes(SlotDuration.TotalMinutes) <= ClinicCloseTime)
            {
                var endTime = currentTime.Add(SlotDuration);
                slots.Add(new TimeSlot(currentTime, endTime));
                currentTime = endTime;
            }
            
            return slots.ToArray();
        }
        
        /// <summary>
        /// Crea un slot de tiempo a partir de horas y minutos de inicio y fin
        /// </summary>
        /// <param name="startHour">Hora de inicio</param>
        /// <param name="startMinute">Minuto de inicio</param>
        /// <param name="endHour">Hora de fin</param>
        /// <param name="endMinute">Minuto de fin</param>
        /// <returns>Slot de tiempo</returns>
        public static TimeSlot Create(int startHour, int startMinute, int endHour, int endMinute)
        {
            var startTime = new TimeOnly(startHour, startMinute);
            var endTime = new TimeOnly(endHour, endMinute);
            
            return new TimeSlot(startTime, endTime);
        }
        
        /// <summary>
        /// Crea un slot de tiempo a partir de una hora de inicio y una duración
        /// </summary>
        /// <param name="startHour">Hora de inicio</param>
        /// <param name="startMinute">Minuto de inicio</param>
        /// <param name="durationInMinutes">Duración en minutos</param>
        /// <returns>Slot de tiempo</returns>
        public static TimeSlot CreateWithDuration(int startHour, int startMinute, int durationInMinutes)
        {
            var startTime = new TimeOnly(startHour, startMinute);
            var endTime = startTime.AddMinutes(durationInMinutes);
            
            return new TimeSlot(startTime, endTime);
        }
    }
} 