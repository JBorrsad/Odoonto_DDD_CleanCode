using System;
using Odoonto.Domain.Core.Models.Exceptions;

namespace Odoonto.Domain.Models.ValueObjects
{
    /// <summary>
    /// Factory para crear slots de tiempo con intervalos de media hora
    /// </summary>
    public static class TimeSlotFactory
    {
        // Intervalo mínimo para citas (30 minutos)
        private static readonly TimeSpan HalfHourInterval = TimeSpan.FromMinutes(30);

        /// <summary>
        /// Crea un slot de tiempo normalizado a intervalos de media hora
        /// </summary>
        /// <param name="startHour">Hora de inicio (0-23)</param>
        /// <param name="startMinute">Minuto de inicio (debe ser 0 o 30)</param>
        /// <param name="durationInHalfHours">Duración en bloques de media hora (1 = 30min, 2 = 1h, etc.)</param>
        /// <returns>TimeSlot normalizado</returns>
        /// <exception cref="InvalidValueException">Si los parámetros no son válidos</exception>
        public static TimeSlot CreateNormalizedSlot(int startHour, int startMinute, int durationInHalfHours)
        {
            // Validar hora
            if (startHour < 0 || startHour > 23)
                throw new InvalidValueException("La hora debe estar entre 0 y 23");

            // Validar minuto (solo permitir 0 o 30)
            if (startMinute != 0 && startMinute != 30)
                throw new InvalidValueException("Los minutos deben ser 0 o 30 para formar bloques de media hora");

            // Validar duración
            if (durationInHalfHours < 1)
                throw new InvalidValueException("La duración debe ser al menos de media hora (1 bloque)");

            // Crear hora de inicio
            var startTime = new TimeSpan(startHour, startMinute, 0);

            // Calcular hora de fin sumando la duración en bloques de media hora
            var endTime = startTime.Add(TimeSpan.FromMinutes(durationInHalfHours * 30));

            return new TimeSlot(startTime, endTime);
        }

        /// <summary>
        /// Crea un slot de tiempo a partir de horas y minutos de inicio y fin
        /// </summary>
        /// <param name="startHour">Hora de inicio (0-23)</param>
        /// <param name="startMinute">Minuto de inicio (debe ser 0 o 30)</param>
        /// <param name="endHour">Hora de fin (0-23)</param>
        /// <param name="endMinute">Minuto de fin (debe ser 0 o 30)</param>
        /// <returns>TimeSlot normalizado</returns>
        public static TimeSlot CreateSlot(int startHour, int startMinute, int endHour, int endMinute)
        {
            // Validar horas
            if (startHour < 0 || startHour > 23 || endHour < 0 || endHour > 23)
                throw new InvalidValueException("Las horas deben estar entre 0 y 23");

            // Validar minutos (solo permitir 0 o 30)
            if ((startMinute != 0 && startMinute != 30) || (endMinute != 0 && endMinute != 30))
                throw new InvalidValueException("Los minutos deben ser 0 o 30 para formar bloques de media hora");

            // Crear tiempos
            var startTime = new TimeSpan(startHour, startMinute, 0);
            var endTime = new TimeSpan(endHour, endMinute, 0);

            // Validar que la duración sea en bloques de media hora
            var duration = endTime - startTime;
            if (duration.TotalMinutes % 30 != 0)
                throw new InvalidValueException("La duración debe ser en bloques exactos de media hora");

            return new TimeSlot(startTime, endTime);
        }

        /// <summary>
        /// Obtiene todos los slots de tiempo disponibles para un día, en el horario de la clínica
        /// </summary>
        /// <param name="clinicOpenHour">Hora de apertura de la clínica (por defecto 9:00)</param>
        /// <param name="clinicCloseHour">Hora de cierre de la clínica (por defecto 19:00)</param>
        /// <returns>Lista de slots disponibles</returns>
        public static TimeSlot[] GetAllDailySlots(int clinicOpenHour = 9, int clinicCloseHour = 19)
        {
            // Validar horas de clínica
            if (clinicOpenHour < 0 || clinicOpenHour > 23 || clinicCloseHour < 0 || clinicCloseHour > 23)
                throw new InvalidValueException("Las horas deben estar entre 0 y 23");

            if (clinicOpenHour >= clinicCloseHour)
                throw new InvalidValueException("La hora de apertura debe ser anterior a la de cierre");

            // Calcular cuántos slots de media hora hay entre la apertura y el cierre
            int totalSlots = (clinicCloseHour - clinicOpenHour) * 2;
            var slots = new TimeSlot[totalSlots];

            for (int i = 0; i < totalSlots; i++)
            {
                int offsetMinutes = i * 30;
                TimeSpan startTime = new TimeSpan(clinicOpenHour, 0, 0).Add(TimeSpan.FromMinutes(offsetMinutes));
                TimeSpan endTime = startTime.Add(HalfHourInterval);

                slots[i] = new TimeSlot(startTime, endTime);
            }

            return slots;
        }
    }
}