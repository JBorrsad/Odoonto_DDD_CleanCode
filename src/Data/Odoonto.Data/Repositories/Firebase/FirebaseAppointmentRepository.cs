using Google.Cloud.Firestore;
using Microsoft.Extensions.Logging;
using Odoonto.Domain.Models.Appointments;
using Odoonto.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Odoonto.Data.Repositories.Firebase
{
    [FirestoreData]
    public class FirebaseAppointmentRepository : FirebaseBaseRepository<Appointment>, IAppointmentRepository
    {
        private const string COLLECTION_NAME = "appointments";

        public FirebaseAppointmentRepository(ILogger<FirebaseAppointmentRepository> logger)
            : base(logger, COLLECTION_NAME)
        {
        }

        public async Task<IReadOnlyList<Appointment>> GetByPatientIdAsync(Guid patientId)
        {
            if (patientId == Guid.Empty)
                throw new ArgumentException("El ID del paciente no puede estar vacío", nameof(patientId));

            try
            {
                var allAppointments = await GetAllAsync();

                var filteredAppointments = allAppointments
                    .Where(a => a.PatientId == patientId)
                    .OrderBy(a => a.DateTime)
                    .ToList()
                    .AsReadOnly();

                _logger.LogInformation($"Recuperadas {filteredAppointments.Count} citas para paciente con ID {patientId}");
                return filteredAppointments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al buscar citas para paciente con ID {patientId}");
                throw;
            }
        }

        public async Task<IReadOnlyList<Appointment>> GetByDoctorIdAsync(Guid doctorId)
        {
            if (doctorId == Guid.Empty)
                throw new ArgumentException("El ID del doctor no puede estar vacío", nameof(doctorId));

            try
            {
                var allAppointments = await GetAllAsync();

                var filteredAppointments = allAppointments
                    .Where(a => a.DoctorId == doctorId)
                    .OrderBy(a => a.DateTime)
                    .ToList()
                    .AsReadOnly();

                _logger.LogInformation($"Recuperadas {filteredAppointments.Count} citas para doctor con ID {doctorId}");
                return filteredAppointments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al buscar citas para doctor con ID {doctorId}");
                throw;
            }
        }

        public async Task<IReadOnlyList<Appointment>> GetByDateRangeAsync(DateTime start, DateTime end)
        {
            if (start > end)
                throw new ArgumentException("La fecha de inicio debe ser anterior a la fecha de fin", nameof(start));

            try
            {
                var allAppointments = await GetAllAsync();

                var filteredAppointments = allAppointments
                    .Where(a => a.DateTime >= start && a.DateTime <= end)
                    .OrderBy(a => a.DateTime)
                    .ToList()
                    .AsReadOnly();

                _logger.LogInformation($"Recuperadas {filteredAppointments.Count} citas entre {start:yyyy-MM-dd} y {end:yyyy-MM-dd}");
                return filteredAppointments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al buscar citas entre {start:yyyy-MM-dd} y {end:yyyy-MM-dd}");
                throw;
            }
        }

        public async Task<IReadOnlyList<Appointment>> GetByStatusAsync(AppointmentStatus status)
        {
            try
            {
                var allAppointments = await GetAllAsync();

                var filteredAppointments = allAppointments
                    .Where(a => a.Status == status)
                    .OrderBy(a => a.DateTime)
                    .ToList()
                    .AsReadOnly();

                _logger.LogInformation($"Recuperadas {filteredAppointments.Count} citas con estado {status}");
                return filteredAppointments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al buscar citas con estado {status}");
                throw;
            }
        }

        public async Task<IReadOnlyList<TimeSlot>> GetAvailableTimeSlotsAsync(Guid doctorId, DateTime date)
        {
            if (doctorId == Guid.Empty)
                throw new ArgumentException("El ID del doctor no puede estar vacío", nameof(doctorId));

            try
            {
                // Por ahora, implementación simplificada
                // Supongamos que tenemos horarios fijos de 8 AM a 5 PM con slots de 1 hora
                var timeSlots = new List<TimeSlot>();
                var startHour = 8;
                var endHour = 17;
                var slotDuration = 60; // minutos

                // Obtener las citas existentes para el doctor en la fecha indicada
                var existingAppointments = await GetByDoctorIdAsync(doctorId);
                var appointmentsOnDate = existingAppointments
                    .Where(a => a.DateTime.Date == date.Date)
                    .ToList();

                // Crear slots disponibles (simplificado)
                for (int hour = startHour; hour < endHour; hour++)
                {
                    var slotStart = new DateTime(date.Year, date.Month, date.Day, hour, 0, 0);
                    var slotEnd = slotStart.AddMinutes(slotDuration);

                    // Verificar si el slot está disponible (no hay citas en ese horario)
                    var isAvailable = !appointmentsOnDate.Any(a =>
                        (a.DateTime >= slotStart && a.DateTime < slotEnd) ||
                        (a.DateTime.AddMinutes(a.Duration) > slotStart && a.DateTime < slotEnd));

                    if (isAvailable)
                    {
                        timeSlots.Add(new TimeSlot
                        {
                            Start = slotStart,
                            End = slotEnd,
                            IsAvailable = true
                        });
                    }
                }

                _logger.LogInformation($"Recuperados {timeSlots.Count} slots disponibles para doctor con ID {doctorId} en fecha {date:yyyy-MM-dd}");
                return timeSlots;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al buscar slots disponibles para doctor con ID {doctorId} en fecha {date:yyyy-MM-dd}");
                throw;
            }
        }

        public async Task<bool> IsTimeSlotAvailableAsync(Guid doctorId, DateTime start, int durationMinutes)
        {
            if (doctorId == Guid.Empty)
                throw new ArgumentException("El ID del doctor no puede estar vacío", nameof(doctorId));

            if (durationMinutes <= 0)
                throw new ArgumentException("La duración debe ser mayor a cero", nameof(durationMinutes));

            try
            {
                var end = start.AddMinutes(durationMinutes);

                // Obtener las citas existentes para el doctor en la fecha indicada
                var existingAppointments = await GetByDoctorIdAsync(doctorId);
                var appointmentsOnDate = existingAppointments
                    .Where(a => a.DateTime.Date == start.Date)
                    .ToList();

                // Verificar si hay alguna cita que se solape con el horario solicitado
                var isAvailable = !appointmentsOnDate.Any(a =>
                    (a.DateTime >= start && a.DateTime < end) ||
                    (a.DateTime.AddMinutes(a.Duration) > start && a.DateTime < end));

                _logger.LogInformation($"Slot de tiempo {start:yyyy-MM-dd HH:mm} - {end:HH:mm} para doctor con ID {doctorId} está {(isAvailable ? "disponible" : "no disponible")}");
                return isAvailable;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al verificar disponibilidad para doctor con ID {doctorId} en hora {start:yyyy-MM-dd HH:mm}");
                throw;
            }
        }
    }
}