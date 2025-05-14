using System;
using System.Collections.Generic;
using System.Linq;
using Google.Cloud.Firestore;
using Odoonto.Domain.Models.Odontograms;
using Odoonto.Domain.Models.Treatments;

namespace Odoonto.Data.Mappings
{
    /// <summary>
    /// Clase para mapear entre documentos Firestore y entidades Odontogram
    /// </summary>
    public static class OdontogramMapper
    {
        /// <summary>
        /// Convierte un documento Firestore a una entidad Odontogram
        /// </summary>
        public static Odontogram ToEntity(DocumentSnapshot document)
        {
            if (document == null || !document.Exists)
                return null;

            var data = document.ToDictionary();
            var id = Guid.Parse(document.Id);

            // Obtener el PatientId
            Guid patientId = Guid.Empty;
            if (data.TryGetValue("PatientId", out var patientIdObj) && patientIdObj is string patientIdStr)
            {
                patientId = Guid.Parse(patientIdStr);
            }

            // Crear el odontograma
            var odontogram = new Odontogram(id, patientId);

            // Cargar los registros dentales si existen
            if (data.TryGetValue("ToothRecords", out var toothRecordsObj) &&
                toothRecordsObj is Dictionary<string, object> toothRecordsDict)
            {
                foreach (var kvp in toothRecordsDict)
                {
                    if (int.TryParse(kvp.Key, out int toothNumber) &&
                        kvp.Value is Dictionary<string, object> toothRecordData)
                    {
                        var toothRecord = ToothRecordMapper.FromDictionary(toothNumber, toothRecordData);
                        if (toothRecord != null)
                        {
                            odontogram.AddToothRecord(toothRecord);
                        }
                    }
                }
            }

            return odontogram;
        }

        /// <summary>
        /// Convierte una entidad Odontogram a un diccionario para Firestore
        /// </summary>
        public static Dictionary<string, object> ToFirestore(Odontogram odontogram)
        {
            if (odontogram == null)
                return null;

            var data = new Dictionary<string, object>
            {
                { "PatientId", odontogram.PatientId.ToString() },
                { "CreationDate", Timestamp.FromDateTime(odontogram.CreatedAt.ToUniversalTime()) },
                { "EditDate", Timestamp.FromDateTime(odontogram.UpdatedAt.ToUniversalTime()) }
            };

            // Convertir los registros dentales
            var toothRecordsDict = new Dictionary<string, object>();
            foreach (var toothRecord in odontogram.ToothRecords)
            {
                toothRecordsDict.Add(
                    toothRecord.ToothNumber.ToString(),
                    ToothRecordMapper.ToDictionary(toothRecord)
                );
            }

            data.Add("ToothRecords", toothRecordsDict);

            return data;
        }
    }

    /// <summary>
    /// Clase auxiliar para mapear ToothRecord
    /// </summary>
    public static class ToothRecordMapper
    {
        /// <summary>
        /// Convierte un diccionario a una entidad ToothRecord
        /// </summary>
        public static ToothRecord FromDictionary(int toothNumber, Dictionary<string, object> data)
        {
            if (data == null)
                return null;

            var toothRecord = new ToothRecord(toothNumber);

            // Cargar lesiones
            if (data.TryGetValue("Lesions", out var lesionsObj) &&
                lesionsObj is List<object> lesionsList)
            {
                foreach (var lesionObj in lesionsList)
                {
                    if (lesionObj is Dictionary<string, object> lesionDict)
                    {
                        var lesionRecord = LesionRecordMapper.FromDictionary(lesionDict);
                        if (lesionRecord != null)
                        {
                            toothRecord.AddLesionRecord(lesionRecord);
                        }
                    }
                }
            }

            // Cargar procedimientos realizados
            if (data.TryGetValue("CompletedProcedures", out var proceduresObj) &&
                proceduresObj is List<object> proceduresList)
            {
                foreach (var procedureObj in proceduresList)
                {
                    if (procedureObj is Dictionary<string, object> procedureDict)
                    {
                        var performedProcedure = PerformedProcedureMapper.FromDictionary(procedureDict);
                        if (performedProcedure != null)
                        {
                            toothRecord.AddPerformedProcedure(performedProcedure);
                        }
                    }
                }
            }

            return toothRecord;
        }

        /// <summary>
        /// Convierte una entidad ToothRecord a un diccionario
        /// </summary>
        public static Dictionary<string, object> ToDictionary(ToothRecord toothRecord)
        {
            if (toothRecord == null)
                return null;

            var data = new Dictionary<string, object>
            {
                { "ToothNumber", toothRecord.ToothNumber }
            };

            // Convertir lesiones
            var lesionsList = new List<object>();
            foreach (var lesion in toothRecord.RecordedLesions)
            {
                lesionsList.Add(LesionRecordMapper.ToDictionary(lesion));
            }
            data.Add("Lesions", lesionsList);

            // Convertir procedimientos realizados
            var proceduresList = new List<object>();
            foreach (var procedure in toothRecord.CompletedProcedures)
            {
                proceduresList.Add(PerformedProcedureMapper.ToDictionary(procedure));
            }
            data.Add("CompletedProcedures", proceduresList);

            return data;
        }
    }

    /// <summary>
    /// Clase auxiliar para mapear LesionRecord
    /// </summary>
    public static class LesionRecordMapper
    {
        /// <summary>
        /// Convierte un diccionario a una entidad LesionRecord
        /// </summary>
        public static LesionRecord FromDictionary(Dictionary<string, object> data)
        {
            if (data == null)
                return null;

            // Obtener LesionId
            Guid lesionId = Guid.Empty;
            if (data.TryGetValue("LesionId", out var lesionIdObj) && lesionIdObj is string lesionIdStr)
            {
                lesionId = Guid.Parse(lesionIdStr);
            }

            // Obtener DetectionDate
            DateTime detectionDate = DateTime.Now;
            if (data.TryGetValue("DetectionDate", out var dateObj) && dateObj is Timestamp timestamp)
            {
                detectionDate = timestamp.ToDateTime();
            }

            // Obtener las superficies afectadas
            var affectedSurfaces = new List<string>();
            if (data.TryGetValue("AffectedSurfaces", out var surfacesObj) && surfacesObj is List<object> surfacesList)
            {
                foreach (var surface in surfacesList)
                {
                    if (surface is string surfaceStr)
                    {
                        affectedSurfaces.Add(surfaceStr);
                    }
                }
            }

            // Crear y retornar el registro de lesión
            return new LesionRecord(lesionId, affectedSurfaces, detectionDate);
        }

        /// <summary>
        /// Convierte una entidad LesionRecord a un diccionario
        /// </summary>
        public static Dictionary<string, object> ToDictionary(LesionRecord lesionRecord)
        {
            if (lesionRecord == null)
                return null;

            return new Dictionary<string, object>
            {
                { "LesionId", lesionRecord.LesionId.ToString() },
                { "DetectionDate", Timestamp.FromDateTime(lesionRecord.DetectionDate.ToUniversalTime()) },
                { "AffectedSurfaces", lesionRecord.AffectedSurfaces.ToList() }
            };
        }
    }

    /// <summary>
    /// Clase auxiliar para mapear PerformedProcedure
    /// </summary>
    public static class PerformedProcedureMapper
    {
        /// <summary>
        /// Convierte un diccionario a una entidad PerformedProcedure
        /// </summary>
        public static PerformedProcedure FromDictionary(Dictionary<string, object> data)
        {
            if (data == null)
                return null;

            // Obtener TreatmentId
            Guid treatmentId = Guid.Empty;
            if (data.TryGetValue("TreatmentId", out var treatmentIdObj) && treatmentIdObj is string treatmentIdStr)
            {
                treatmentId = Guid.Parse(treatmentIdStr);
            }

            // Obtener CompletionDate
            DateTime completionDate = DateTime.Now;
            if (data.TryGetValue("CompletionDate", out var dateObj) && dateObj is Timestamp timestamp)
            {
                completionDate = timestamp.ToDateTime();
            }

            // Obtener las superficies tratadas
            var treatedSurfaces = new List<string>();
            if (data.TryGetValue("TreatedSurfaces", out var surfacesObj) && surfacesObj is List<object> surfacesList)
            {
                foreach (var surface in surfacesList)
                {
                    if (surface is string surfaceStr)
                    {
                        treatedSurfaces.Add(surfaceStr);
                    }
                }
            }

            // Crear y retornar el procedimiento realizado
            return new PerformedProcedure(treatmentId, treatedSurfaces, completionDate);
        }

        /// <summary>
        /// Convierte una entidad PerformedProcedure a un diccionario
        /// </summary>
        public static Dictionary<string, object> ToDictionary(PerformedProcedure performedProcedure)
        {
            if (performedProcedure == null)
                return null;

            return new Dictionary<string, object>
            {
                { "TreatmentId", performedProcedure.TreatmentId.ToString() },
                { "CompletionDate", Timestamp.FromDateTime(performedProcedure.CompletionDate.ToUniversalTime()) },
                { "TreatedSurfaces", performedProcedure.TreatedSurfaces.ToList() }
            };
        }
    }
}