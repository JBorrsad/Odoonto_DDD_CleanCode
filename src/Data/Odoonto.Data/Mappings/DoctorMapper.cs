using System;
using System.Collections.Generic;
using System.Linq;
using Google.Cloud.Firestore;
using Odoonto.Domain.Models.Doctors;
using Odoonto.Domain.Models.ValueObjects;

namespace Odoonto.Data.Mappings
{
    /// <summary>
    /// Clase para mapear entre documentos Firestore y entidades Doctor
    /// </summary>
    public static class DoctorMapper
    {
        /// <summary>
        /// Convierte un documento Firestore a una entidad Doctor
        /// </summary>
        public static Doctor ToEntity(DocumentSnapshot document)
        {
            if (document == null || !document.Exists)
                return null;

            var data = document.ToDictionary();
            var id = Guid.Parse(document.Id);

            // Crear la entidad doctor
            var doctor = Doctor.Create(id);

            // Establecer el nombre completo
            var firstName = data.GetValueOrDefault("FirstName")?.ToString() ?? "";
            var lastName = data.GetValueOrDefault("LastName")?.ToString() ?? "";
            doctor.SetFullName(firstName, lastName);

            // Establecer la especialidad
            var specialty = data.GetValueOrDefault("Specialty")?.ToString() ?? "";
            doctor.SetSpecialty(specialty);

            // Establecer la información de contacto
            var address = data.GetValueOrDefault("Address")?.ToString() ?? "";
            var phone = data.GetValueOrDefault("Phone")?.ToString() ?? "";
            var email = data.GetValueOrDefault("Email")?.ToString() ?? "";
            doctor.SetContactInfo(address, phone, email);

            // Establecer la disponibilidad
            if (data.TryGetValue("Availability", out var availabilityObj) &&
                availabilityObj is Dictionary<string, object> availabilityDict)
            {
                var weeklyAvailability = new WeeklyAvailability();

                // Procesar cada día de la semana
                foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
                {
                    var dayName = day.ToString();

                    if (availabilityDict.TryGetValue(dayName, out var timeSlotsList) &&
                        timeSlotsList is List<object> slots)
                    {
                        // Convertir cada slot de tiempo
                        foreach (var slot in slots)
                        {
                            if (slot is Dictionary<string, object> slotDict)
                            {
                                var startTime = slotDict.GetValueOrDefault("StartTime")?.ToString();
                                var endTime = slotDict.GetValueOrDefault("EndTime")?.ToString();

                                if (!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime) &&
                                    TimeSpan.TryParse(startTime, out var start) &&
                                    TimeSpan.TryParse(endTime, out var end))
                                {
                                    var timeSlot = new TimeSlot(start, end);
                                    weeklyAvailability.AddTimeSlot(day, timeSlot);
                                }
                            }
                        }
                    }
                }

                doctor.SetAvailability(weeklyAvailability);
            }

            return doctor;
        }

        /// <summary>
        /// Convierte una entidad Doctor a un diccionario para Firestore
        /// </summary>
        public static Dictionary<string, object> ToFirestore(Doctor doctor)
        {
            if (doctor == null)
                return null;

            var data = new Dictionary<string, object>
            {
                { "FirstName", doctor.FullName.FirstName },
                { "LastName", doctor.FullName.LastName },
                { "Specialty", doctor.Specialty },
                { "Address", doctor.ContactInfo?.Address ?? "" },
                { "Phone", doctor.ContactInfo?.Phone ?? "" },
                { "Email", doctor.ContactInfo?.Email ?? "" }
            };

            // Mapear la disponibilidad si existe
            if (doctor.Availability != null)
            {
                var availabilityDict = new Dictionary<string, object>();

                foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
                {
                    var slots = doctor.Availability.GetTimeSlotsForDay(day);
                    if (slots != null && slots.Any())
                    {
                        var slotsList = slots.Select(slot => new Dictionary<string, object>
                        {
                            { "StartTime", slot.StartTime.ToString() },
                            { "EndTime", slot.EndTime.ToString() }
                        }).ToList<object>();

                        availabilityDict[day.ToString()] = slotsList;
                    }
                }

                data["Availability"] = availabilityDict;
            }

            return data;
        }
    }
}