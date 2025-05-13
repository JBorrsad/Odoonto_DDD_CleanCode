using System;
using System.Collections.Generic;
using Google.Cloud.Firestore;
using Odoonto.Domain.Models.Appointments;
using Odoonto.Domain.Models.ValueObjects;

namespace Odoonto.Data.Contexts.Configurations
{
    /// <summary>
    /// Configuración para el mapeo entre documentos Firestore y la entidad Appointment
    /// </summary>
    public static class AppointmentConfiguration
    {
        /// <summary>
        /// Mapea un documento Firestore a una entidad Appointment
        /// </summary>
        public static Appointment MapToEntity(DocumentSnapshot document)
        {
            if (!document.Exists)
                return null;

            // Extraer datos del documento
            var data = document.ToDictionary();
            var id = Guid.Parse(document.Id);

            // Mapear campos de auditoría
            var createdAt = data.ContainsKey("createdAt") && data["createdAt"] is Timestamp createdTimestamp
                ? createdTimestamp.ToDateTime()
                : DateTime.UtcNow;
            
            var updatedAt = data.ContainsKey("updatedAt") && data["updatedAt"] is Timestamp updatedTimestamp
                ? updatedTimestamp.ToDateTime()
                : DateTime.UtcNow;

            // Mapear IDs
            var patientId = data.ContainsKey("patientId") && data["patientId"] is string patientIdStr
                ? Guid.Parse(patientIdStr)
                : Guid.Empty;
            
            var doctorId = data.ContainsKey("doctorId") && data["doctorId"] is string doctorIdStr
                ? Guid.Parse(doctorIdStr)
                : Guid.Empty;

            // Mapear objetos complejos (value objects)
            Date appointmentDate = null;
            if (data.ContainsKey("appointmentDate") && data["appointmentDate"] is Timestamp dateTimestamp)
            {
                appointmentDate = new Date(dateTimestamp.ToDateTime());
            }

            TimeSlot timeSlot = null;
            if (data.ContainsKey("timeSlot") && data["timeSlot"] is Dictionary<string, object> timeSlotDict)
            {
                var startTime = timeSlotDict.ContainsKey("startTime") && timeSlotDict["startTime"] is Timestamp startTimestamp
                    ? startTimestamp.ToDateTime().TimeOfDay
                    : TimeSpan.Zero;
                
                var endTime = timeSlotDict.ContainsKey("endTime") && timeSlotDict["endTime"] is Timestamp endTimestamp
                    ? endTimestamp.ToDateTime().TimeOfDay
                    : TimeSpan.Zero;
                
                timeSlot = new TimeSlot(startTime, endTime);
            }

            AppointmentStatus status = AppointmentStatus.Scheduled;
            if (data.ContainsKey("status") && data["status"] is long statusValue)
            {
                status = (AppointmentStatus)statusValue;
            }

            string notes = data.ContainsKey("notes") ? data["notes"].ToString() : string.Empty;

            // Crear instancia de Appointment usando reflection para acceder a constructores/propiedades protegidas
            var appointment = Activator.CreateInstance(typeof(Appointment), true) as Appointment;
            
            // Establecer propiedades a través de reflection
            typeof(Appointment).GetProperty("Id").SetValue(appointment, id);
            typeof(Appointment).GetProperty("CreatedAt").SetValue(appointment, createdAt);
            typeof(Appointment).GetProperty("UpdatedAt").SetValue(appointment, updatedAt);
            typeof(Appointment).GetProperty("PatientId").SetValue(appointment, patientId);
            typeof(Appointment).GetProperty("DoctorId").SetValue(appointment, doctorId);
            typeof(Appointment).GetProperty("AppointmentDate").SetValue(appointment, appointmentDate);
            typeof(Appointment).GetProperty("TimeSlot").SetValue(appointment, timeSlot);
            typeof(Appointment).GetProperty("Status").SetValue(appointment, status);
            typeof(Appointment).GetProperty("Notes").SetValue(appointment, notes);

            return appointment;
        }

        /// <summary>
        /// Mapea una entidad Appointment a un documento Firestore
        /// </summary>
        public static Dictionary<string, object> MapToDocument(Appointment entity)
        {
            if (entity == null)
                return null;

            var data = new Dictionary<string, object>
            {
                ["patientId"] = entity.PatientId.ToString(),
                ["doctorId"] = entity.DoctorId.ToString(),
                ["appointmentDate"] = Timestamp.FromDateTime(entity.AppointmentDate.Value),
                ["timeSlot"] = new Dictionary<string, object>
                {
                    ["startTime"] = Timestamp.FromDateTime(
                        DateTime.Today.Add(entity.TimeSlot.StartTime)),
                    ["endTime"] = Timestamp.FromDateTime(
                        DateTime.Today.Add(entity.TimeSlot.EndTime))
                },
                ["status"] = (int)entity.Status,
                ["notes"] = entity.Notes ?? string.Empty,
                ["createdAt"] = Timestamp.FromDateTime(entity.CreatedAt),
                ["updatedAt"] = Timestamp.FromDateTime(entity.UpdatedAt)
            };

            return data;
        }
    }
} 