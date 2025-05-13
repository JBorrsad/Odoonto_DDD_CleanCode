using System;
using System.Collections.Generic;
using System.Linq;
using Google.Cloud.Firestore;
using Odoonto.Domain.Models.Doctors;
using Odoonto.Domain.Models.ValueObjects;

namespace Odoonto.Data.Contexts.Configurations
{
    /// <summary>
    /// Configuración para el mapeo entre documentos Firestore y la entidad Doctor
    /// </summary>
    public static class DoctorConfiguration
    {
        /// <summary>
        /// Mapea un documento Firestore a una entidad Doctor
        /// </summary>
        public static Doctor MapToEntity(DocumentSnapshot document)
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

            // Crear instancia de Doctor usando el método factory
            var doctor = Doctor.Create(id);
            
            // Establecer propiedades de auditoría a través de reflection
            typeof(Doctor).GetProperty("CreatedAt").SetValue(doctor, createdAt);
            typeof(Doctor).GetProperty("UpdatedAt").SetValue(doctor, updatedAt);

            // Mapear nombre completo
            if (data.ContainsKey("fullName") && data["fullName"] is Dictionary<string, object> nameDict)
            {
                string firstName = nameDict.ContainsKey("firstName") ? nameDict["firstName"].ToString() : string.Empty;
                string lastName = nameDict.ContainsKey("lastName") ? nameDict["lastName"].ToString() : string.Empty;
                
                if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
                {
                    doctor.SetFullName(firstName, lastName);
                }
            }

            // Mapear especialidad
            if (data.ContainsKey("specialty"))
            {
                doctor.SetSpecialty(data["specialty"].ToString());
            }

            // Mapear información de contacto
            if (data.ContainsKey("contactInfo") && data["contactInfo"] is Dictionary<string, object> contactDict)
            {
                string address = contactDict.ContainsKey("address") ? contactDict["address"].ToString() : string.Empty;
                string phoneNumber = contactDict.ContainsKey("phoneNumber") ? contactDict["phoneNumber"].ToString() : string.Empty;
                string email = contactDict.ContainsKey("email") ? contactDict["email"].ToString() : string.Empty;
                
                doctor.SetContactInfo(address, phoneNumber, email);
            }

            // Mapear disponibilidad semanal
            if (data.ContainsKey("availability") && data["availability"] is Dictionary<string, object> availabilityDict)
            {
                var weeklyAvailability = new Dictionary<DayOfWeek, ICollection<TimeRange>>();
                
                foreach (var dayEntry in availabilityDict)
                {
                    if (Enum.TryParse<DayOfWeek>(dayEntry.Key, out var dayOfWeek) && dayEntry.Value is List<object> rangesList)
                    {
                        var timeRanges = new List<TimeRange>();
                        
                        foreach (var rangeObj in rangesList)
                        {
                            if (rangeObj is Dictionary<string, object> rangeDict)
                            {
                                if (rangeDict.ContainsKey("startTime") && rangeDict["startTime"] is Timestamp startTimestamp &&
                                    rangeDict.ContainsKey("endTime") && rangeDict["endTime"] is Timestamp endTimestamp)
                                {
                                    var startTime = startTimestamp.ToDateTime().TimeOfDay;
                                    var endTime = endTimestamp.ToDateTime().TimeOfDay;
                                    
                                    timeRanges.Add(new TimeRange(startTime, endTime));
                                }
                            }
                        }
                        
                        weeklyAvailability[dayOfWeek] = timeRanges;
                    }
                }
                
                if (weeklyAvailability.Count > 0)
                {
                    doctor.SetAvailability(new WeeklyAvailability(weeklyAvailability));
                }
            }

            return doctor;
        }

        /// <summary>
        /// Mapea una entidad Doctor a un documento Firestore
        /// </summary>
        public static Dictionary<string, object> MapToDocument(Doctor entity)
        {
            if (entity == null)
                return null;
                
            // Crear diccionario para Firestore
            var documentData = new Dictionary<string, object>
            {
                ["createdAt"] = Timestamp.FromDateTime(DateTime.SpecifyKind(entity.CreatedAt, DateTimeKind.Utc)),
                ["updatedAt"] = Timestamp.FromDateTime(DateTime.SpecifyKind(entity.UpdatedAt, DateTimeKind.Utc)),
                ["specialty"] = entity.Specialty ?? string.Empty
            };

            // Mapear objetos complejos (value objects)
            if (entity.FullName != null)
            {
                documentData["fullName"] = new Dictionary<string, object>
                {
                    ["firstName"] = entity.FullName.FirstName,
                    ["lastName"] = entity.FullName.LastName
                };
            }

            if (entity.ContactInfo != null)
            {
                documentData["contactInfo"] = new Dictionary<string, object>
                {
                    ["address"] = entity.ContactInfo.Address ?? string.Empty,
                    ["phoneNumber"] = entity.ContactInfo.PhoneNumber ?? string.Empty,
                    ["email"] = entity.ContactInfo.Email ?? string.Empty
                };
            }

            if (entity.Availability != null)
            {
                var availabilityDict = new Dictionary<string, object>();
                
                foreach (var dayEntry in entity.Availability.Availability)
                {
                    if (dayEntry.Value.Count > 0)
                    {
                        var rangesList = new List<Dictionary<string, object>>();
                        
                        foreach (var range in dayEntry.Value)
                        {
                            rangesList.Add(new Dictionary<string, object>
                            {
                                ["startTime"] = Timestamp.FromDateTime(DateTime.Today.Add(range.StartTime)),
                                ["endTime"] = Timestamp.FromDateTime(DateTime.Today.Add(range.EndTime))
                            });
                        }
                        
                        availabilityDict[dayEntry.Key.ToString()] = rangesList;
                    }
                }
                
                documentData["availability"] = availabilityDict;
            }

            return documentData;
        }
    }
} 