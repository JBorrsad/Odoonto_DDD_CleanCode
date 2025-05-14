using System;
using System.Collections.Generic;
using System.Linq;
using Google.Cloud.Firestore;
using Odoonto.Domain.Models.Appointments;
using Odoonto.Domain.Models.ValueObjects;

namespace Odoonto.Data.Mappings
{
    /// <summary>
    /// Clase para mapear entre documentos Firestore y entidades Appointment
    /// </summary>
    public static class AppointmentMapper
    {
        /// <summary>
        /// Convierte un documento Firestore a una entidad Appointment
        /// </summary>
        public static Appointment ToEntity(DocumentSnapshot document)
        {
            if (document == null || !document.Exists)
                return null;

            var data = document.ToDictionary();
            var id = Guid.Parse(document.Id);

            // Crear la cita base
            var appointment = Appointment.Create(id);

            // Extraer IDs de paciente y doctor
            var patientId = GetGuidValue(data, "PatientId");
            var doctorId = GetGuidValue(data, "DoctorId");

            // Extraer fecha y horario
            var date = data.GetValueOrDefault("Date") is Timestamp timestamp
                ? timestamp.ToDateTime()
                : DateTime.Now;

            // Extraer TimeSlot
            var startTimeStr = data.GetValueOrDefault("StartTime")?.ToString();
            var endTimeStr = data.GetValueOrDefault("EndTime")?.ToString();

            TimeSlot timeSlot = null;
            if (!string.IsNullOrEmpty(startTimeStr) && !string.IsNullOrEmpty(endTimeStr) &&
                TimeSpan.TryParse(startTimeStr, out var startTime) &&
                TimeSpan.TryParse(endTimeStr, out var endTime))
            {
                timeSlot = new TimeSlot(startTime, endTime);
            }
            else
            {
                timeSlot = new TimeSlot(TimeSpan.FromHours(9), TimeSpan.FromHours(10)); // Valor por defecto
            }

            // Establecer información básica
            appointment.SetBasicInfo(patientId, doctorId, date, timeSlot);

            // Extraer notas
            var notes = data.GetValueOrDefault("Notes")?.ToString() ?? "";
            appointment.SetNotes(notes);

            // Extraer estado
            var statusStr = data.GetValueOrDefault("Status")?.ToString() ?? "";
            if (!string.IsNullOrEmpty(statusStr) &&
                Enum.TryParse<AppointmentStatus>(statusStr, true, out var status))
            {
                // Establecer el estado según el valor guardado
                switch (status)
                {
                    case AppointmentStatus.WaitingRoom:
                        appointment.MarkAsWaitingRoom();
                        break;
                    case AppointmentStatus.InProgress:
                        appointment.MarkAsWaitingRoom(); // Primero en sala de espera
                        appointment.MarkAsInProgress();  // Luego en progreso
                        break;
                    case AppointmentStatus.Completed:
                        appointment.MarkAsWaitingRoom(); // Primero en sala de espera
                        appointment.MarkAsInProgress();  // Luego en progreso
                        appointment.MarkAsCompleted();   // Finalmente completada
                        break;
                    case AppointmentStatus.Cancelled:
                        appointment.Cancel();
                        break;
                        // Si es "Scheduled", no necesitamos hacer nada ya que es el valor por defecto
                }
            }

            // Extraer plan de tratamiento si existe
            if (data.TryGetValue("TreatmentPlan", out var treatmentPlanObj) &&
                treatmentPlanObj is Dictionary<string, object> treatmentPlanDict)
            {
                var planId = GetGuidValue(treatmentPlanDict, "Id");
                var description = treatmentPlanDict.GetValueOrDefault("Description")?.ToString() ?? "";

                var treatmentPlan = TreatmentPlan.Create(planId);
                treatmentPlan.SetDescription(description);

                // Extraer procedimientos planificados
                if (treatmentPlanDict.TryGetValue("PlannedProcedures", out var proceduresObj) &&
                    proceduresObj is List<object> proceduresList)
                {
                    foreach (var procObj in proceduresList)
                    {
                        if (procObj is Dictionary<string, object> procDict)
                        {
                            var procedureId = GetGuidValue(procDict, "Id");
                            var treatmentId = GetGuidValue(procDict, "TreatmentId");
                            var toothNumber = procDict.GetValueOrDefault("ToothNumber")?.ToString();

                            // Extraer superficies dentales si existen
                            List<string> surfaces = new List<string>();
                            if (procDict.TryGetValue("Surfaces", out var surfacesObj) &&
                                surfacesObj is List<object> surfacesList)
                            {
                                surfaces = surfacesList
                                    .Select(s => s?.ToString() ?? "")
                                    .Where(s => !string.IsNullOrEmpty(s))
                                    .ToList();
                            }

                            // Crear y añadir el procedimiento al plan
                            var procedure = PlannedProcedure.Create(procedureId);
                            procedure.SetTreatmentId(treatmentId);

                            if (!string.IsNullOrEmpty(toothNumber))
                            {
                                procedure.SetToothNumber(toothNumber);

                                if (surfaces.Any())
                                {
                                    var toothSurfaces = new ToothSurfaces();
                                    foreach (var surface in surfaces)
                                    {
                                        toothSurfaces.AddSurface(surface);
                                    }
                                    procedure.SetSurfaces(toothSurfaces);
                                }
                            }

                            treatmentPlan.AddProcedure(procedure);
                        }
                    }
                }

                appointment.SetTreatmentPlan(treatmentPlan);
            }

            return appointment;
        }

        /// <summary>
        /// Convierte una entidad Appointment a un diccionario para Firestore
        /// </summary>
        public static Dictionary<string, object> ToFirestore(Appointment appointment)
        {
            if (appointment == null)
                return null;

            var data = new Dictionary<string, object>
            {
                { "PatientId", appointment.PatientId.ToString() },
                { "DoctorId", appointment.DoctorId.ToString() },
                { "Date", Timestamp.FromDateTime(appointment.Date.ToUniversalTime()) },
                { "StartTime", appointment.TimeSlot.StartTime.ToString() },
                { "EndTime", appointment.TimeSlot.EndTime.ToString() },
                { "Status", appointment.Status.ToString() },
                { "Notes", appointment.Notes }
            };

            // Mapear el plan de tratamiento si existe
            if (appointment.TreatmentPlan != null)
            {
                var treatmentPlanDict = new Dictionary<string, object>
                {
                    { "Id", appointment.TreatmentPlan.Id.ToString() },
                    { "Description", appointment.TreatmentPlan.Description ?? "" }
                };

                // Mapear los procedimientos planificados
                if (appointment.TreatmentPlan.Procedures?.Any() == true)
                {
                    var proceduresList = appointment.TreatmentPlan.Procedures.Select(proc =>
                    {
                        var procDict = new Dictionary<string, object>
                        {
                            { "Id", proc.Id.ToString() },
                            { "TreatmentId", proc.TreatmentId.ToString() }
                        };

                        if (!string.IsNullOrEmpty(proc.ToothNumber))
                        {
                            procDict["ToothNumber"] = proc.ToothNumber;

                            if (proc.Surfaces?.GetSurfaces()?.Any() == true)
                            {
                                procDict["Surfaces"] = proc.Surfaces.GetSurfaces().ToList<object>();
                            }
                        }

                        return procDict;
                    }).ToList<object>();

                    treatmentPlanDict["PlannedProcedures"] = proceduresList;
                }

                data["TreatmentPlan"] = treatmentPlanDict;
            }

            return data;
        }

        // Método de utilidad para extraer un Guid de un diccionario
        private static Guid GetGuidValue(Dictionary<string, object> dict, string key)
        {
            var guidStr = dict.GetValueOrDefault(key)?.ToString();
            return !string.IsNullOrEmpty(guidStr) && Guid.TryParse(guidStr, out var guid)
                ? guid
                : Guid.Empty;
        }
    }
}