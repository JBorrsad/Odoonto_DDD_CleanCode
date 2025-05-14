using System;
using System.Collections.Generic;
using System.Linq;
using Google.Cloud.Firestore;
using Odoonto.Domain.Models.Odontograms;
using Odoonto.Domain.Models.ValueObjects;

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
            var odontogram = new Odontogram();

            // Extraer registros de dientes
            if (data.TryGetValue("TeethRecords", out var teethObj) &&
                teethObj is Dictionary<string, object> teethDict)
            {
                foreach (var entry in teethDict)
                {
                    if (int.TryParse(entry.Key, out var toothNumber) &&
                        entry.Value is Dictionary<string, object> toothData)
                    {
                        // Crear registro dental
                        var toothRecord = new ToothRecord(toothNumber);

                        // Extraer lesiones
                        if (toothData.TryGetValue("Lesions", out var lesionsObj) &&
                            lesionsObj is List<object> lesionsList)
                        {
                            foreach (var lesionObj in lesionsList)
                            {
                                if (lesionObj is Dictionary<string, object> lesionDict)
                                {
                                    var lesionId = GetGuidValue(lesionDict, "LesionId");
                                    var detectionDate = lesionDict.GetValueOrDefault("DetectionDate") is Timestamp timestamp
                                        ? timestamp.ToDateTime()
                                        : DateTime.Now;
                                    var notes = lesionDict.GetValueOrDefault("Notes")?.ToString() ?? "";

                                    // Extraer superficies afectadas
                                    var surfaces = new List<string>();
                                    if (lesionDict.TryGetValue("Surfaces", out var surfacesObj) &&
                                        surfacesObj is List<object> surfacesList)
                                    {
                                        surfaces = surfacesList
                                            .Select(s => s?.ToString() ?? "")
                                            .Where(s => !string.IsNullOrEmpty(s))
                                            .ToList();
                                    }

                                    // Crear y añadir registro de lesión
                                    var lesionRecord = new LesionRecord(lesionId);
                                    lesionRecord.SetDetectionDate(detectionDate);
                                    lesionRecord.SetNotes(notes);

                                    foreach (var surface in surfaces)
                                    {
                                        lesionRecord.AddAffectedSurface(surface);
                                    }

                                    toothRecord.AddLesion(lesionRecord);
                                }
                            }
                        }

                        // Extraer procedimientos completados
                        if (toothData.TryGetValue("CompletedProcedures", out var proceduresObj) &&
                            proceduresObj is List<object> proceduresList)
                        {
                            var completedProcedures = new CompletedProcedures();

                            foreach (var procObj in proceduresList)
                            {
                                if (procObj is Dictionary<string, object> procDict)
                                {
                                    var procedureId = GetGuidValue(procDict, "ProcedureId");
                                    var treatmentId = GetGuidValue(procDict, "TreatmentId");
                                    var completionDate = procDict.GetValueOrDefault("CompletionDate") is Timestamp procTimestamp
                                        ? procTimestamp.ToDateTime()
                                        : DateTime.Now;
                                    var notes = procDict.GetValueOrDefault("Notes")?.ToString() ?? "";

                                    // Extraer superficies tratadas
                                    var surfaces = new List<string>();
                                    if (procDict.TryGetValue("Surfaces", out var surfacesObj) &&
                                        surfacesObj is List<object> surfacesList)
                                    {
                                        surfaces = surfacesList
                                            .Select(s => s?.ToString() ?? "")
                                            .Where(s => !string.IsNullOrEmpty(s))
                                            .ToList();
                                    }

                                    // Crear y añadir procedimiento realizado
                                    var performedProcedure = new PerformedProcedure(procedureId);
                                    performedProcedure.SetTreatmentId(treatmentId);
                                    performedProcedure.SetCompletionDate(completionDate);
                                    performedProcedure.SetNotes(notes);

                                    if (surfaces.Any())
                                    {
                                        var toothSurfaces = new ToothSurfaces();
                                        foreach (var surface in surfaces)
                                        {
                                            toothSurfaces.AddSurface(surface);
                                        }
                                        performedProcedure.SetSurfaces(toothSurfaces);
                                    }

                                    completedProcedures.AddProcedure(performedProcedure);
                                }
                            }

                            toothRecord.SetCompletedProcedures(completedProcedures);
                        }

                        // Añadir registro dental al odontograma
                        odontogram.AddToothRecord(toothRecord);
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

            var data = new Dictionary<string, object>();
            var teethRecords = new Dictionary<string, object>();

            // Mapear cada registro dental
            foreach (var toothRecord in odontogram.ToothRecords)
            {
                var toothData = new Dictionary<string, object>();

                // Mapear lesiones
                if (toothRecord.HasLesions)
                {
                    var lesionsList = toothRecord.Lesions.Select(lesion =>
                    {
                        var lesionDict = new Dictionary<string, object>
                        {
                            { "LesionId", lesion.LesionId.ToString() },
                            { "DetectionDate", Timestamp.FromDateTime(lesion.DetectionDate.ToUniversalTime()) },
                            { "Notes", lesion.Notes ?? "" }
                        };

                        if (lesion.AffectedSurfaces.Any())
                        {
                            lesionDict["Surfaces"] = lesion.AffectedSurfaces.ToList<object>();
                        }

                        return lesionDict;
                    }).ToList<object>();

                    toothData["Lesions"] = lesionsList;
                }

                // Mapear procedimientos completados
                if (toothRecord.HasCompletedProcedures)
                {
                    var proceduresList = toothRecord.CompletedProcedures.Procedures.Select(proc =>
                    {
                        var procDict = new Dictionary<string, object>
                        {
                            { "ProcedureId", proc.Id.ToString() },
                            { "TreatmentId", proc.TreatmentId.ToString() },
                            { "CompletionDate", Timestamp.FromDateTime(proc.CompletionDate.ToUniversalTime()) },
                            { "Notes", proc.Notes ?? "" }
                        };

                        if (proc.Surfaces?.GetSurfaces()?.Any() == true)
                        {
                            procDict["Surfaces"] = proc.Surfaces.GetSurfaces().ToList<object>();
                        }

                        return procDict;
                    }).ToList<object>();

                    toothData["CompletedProcedures"] = proceduresList;
                }

                // Añadir datos del diente si hay algún dato registrado
                if (toothData.Count > 0)
                {
                    teethRecords[toothRecord.ToothNumber.ToString()] = toothData;
                }
            }

            if (teethRecords.Count > 0)
            {
                data["TeethRecords"] = teethRecords;
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