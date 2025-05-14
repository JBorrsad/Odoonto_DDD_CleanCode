using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Odoonto.Domain.Models.ValueObjects;

namespace Odoonto.Domain.Services.Appointments
{
    /// <summary>
    /// Implementación del servicio de programación de citas
    /// </summary>
    public class AppointmentSchedulingService : IAppointmentSchedulingService
    {
        private readonly IAppointmentOverlapService _overlapService;

        public AppointmentSchedulingService(IAppointmentOverlapService overlapService)
        {
            _overlapService = overlapService ?? throw new ArgumentNullException(nameof(overlapService));
        }

        public async Task<IEnumerable<TimeSlot>> GetAvailableSlotsAsync(
            Guid doctorId,
            DateTime date,
            int appointmentDurationInHalfHours = 1)
        {
            if (doctorId == Guid.Empty)
                throw new ArgumentException("El ID del doctor no puede estar vacío", nameof(doctorId));

            if (appointmentDurationInHalfHours < 1)
                throw new ArgumentException("La duración de la cita debe ser al menos de media hora", nameof(appointmentDurationInHalfHours));

            // Obtener todos los slots del día en el horario de la clínica
            TimeSlot[] allSlots = TimeSlotFactory.GetAllDailySlots();
            var availableSlots = new List<TimeSlot>();

            // Para cada slot básico, verificar si hay disponibilidad para la duración requerida
            foreach (var baseSlot in allSlots)
            {
                // Crear un slot que dure la cantidad de bloques requeridos
                var start = baseSlot.StartTime;
                var end = start.Add(TimeSpan.FromMinutes(appointmentDurationInHalfHours * 30));

                // Si el slot extendido termina después del horario de la clínica, ignorarlo
                if (end > allSlots.Last().EndTime)
                    continue;

                var slotToCheck = new TimeSlot(start, end);

                // Verificar si el slot está disponible
                if (await IsSlotAvailableAsync(doctorId, date, slotToCheck))
                {
                    availableSlots.Add(slotToCheck);
                }
            }

            return availableSlots;
        }

        public async Task<bool> IsSlotAvailableAsync(
            Guid doctorId,
            DateTime date,
            TimeSlot timeSlot,
            Guid? excludeAppointmentId = null)
        {
            // Verificar si hay citas superpuestas
            bool hasOverlap = await _overlapService.HasOverlappingAppointmentsAsync(
                doctorId, date, timeSlot, excludeAppointmentId);

            // Está disponible si NO hay superposición
            return !hasOverlap;
        }
    }
}