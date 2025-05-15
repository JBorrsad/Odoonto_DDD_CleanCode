using System;
using System.Collections.Generic;
using Google.Cloud.Firestore;
using Odoonto.Domain.Models.Odontograms;
using Odoonto.Domain.ValueObjects;

namespace Odoonto.Data.Contexts.Configurations
{
    /// <summary>
    /// Configuración para mapeo entre entidad Odontogram y documento Firestore
    /// </summary>
    public class OdontogramConfiguration
    {
        private const string CollectionName = "odontograms";
        private const string ToothRecordsCollectionName = "toothRecords";

        /// <summary>
        /// Obtiene el nombre de la colección principal
        /// </summary>
        public static string Collection => CollectionName;

        /// <summary>
        /// Obtiene el nombre de la subcolección de registros de dientes
        /// </summary>
        public static string ToothRecordsCollection => ToothRecordsCollectionName;

        /// <summary>
        /// Convierte un Odontogram a un Dictionary para Firestore
        /// </summary>
        public static Dictionary<string, object> ToFirestore(Odontogram odontogram)
        {
            if (odontogram == null)
                throw new ArgumentNullException(nameof(odontogram));

            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "id", odontogram.Id.ToString() },
                { "patientId", odontogram.PatientId.ToString() },
                { "createdAt", odontogram.CreatedAt.ToDateTime() },
                { "updatedAt", odontogram.UpdatedAt.ToDateTime() }
            };

            return data;
        }

        /// <summary>
        /// Convierte un ToothRecord a un Dictionary para Firestore
        /// </summary>
        public static Dictionary<string, object> ToothRecordToFirestore(ToothRecord record)
        {
            if (record == null)
                throw new ArgumentNullException(nameof(record));

            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "id", record.Id.ToString() },
                { "toothNumber", record.ToothNumber.Number },
                { "status", record.Status.ToString() },
                { "timestamp", record.Timestamp.ToDateTime() }
            };

            // Agregar lesiones si existen
            if (record.Lesions != null && record.Lesions.Count > 0)
            {
                var lesionsData = new List<Dictionary<string, object>>();
                foreach (var lesion in record.Lesions)
                {
                    var lesionData = new Dictionary<string, object>
                    {
                        { "lesionId", lesion.LesionId.ToString() },
                        { "surfaces", lesion.Surfaces.ToString() },
                        { "recordedAt", lesion.RecordedAt.ToDateTime() },
                        { "recordedBy", lesion.RecordedBy.ToString() }
                    };
                    lesionsData.Add(lesionData);
                }
                data.Add("lesions", lesionsData);
            }

            // Agregar tratamientos si existen
            if (record.Treatments != null && record.Treatments.Count > 0)
            {
                var treatmentsData = new List<Dictionary<string, object>>();
                foreach (var treatment in record.Treatments)
                {
                    var treatmentData = new Dictionary<string, object>
                    {
                        { "treatmentId", treatment.TreatmentId.ToString() },
                        { "surfaces", treatment.Surfaces.ToString() },
                        { "performedAt", treatment.PerformedAt.ToDateTime() },
                        { "performedBy", treatment.PerformedBy.ToString() },
                        { "status", treatment.Status.ToString() }
                    };
                    treatmentsData.Add(treatmentData);
                }
                data.Add("treatments", treatmentsData);
            }

            return data;
        }

        /// <summary>
        /// Convierte un documento Firestore a un Odontogram
        /// </summary>
        public static Odontogram FromFirestore(DocumentSnapshot snapshot)
        {
            if (snapshot == null || !snapshot.Exists)
                return null;

            var id = Guid.Parse(snapshot.Id);
            var patientId = Guid.Parse(snapshot.GetValue<string>("patientId"));

            // Crear instancia de Odontogram
            var odontogram = new Odontogram(id, patientId);

            return odontogram;
        }

        /// <summary>
        /// Convierte un documento Firestore a un ToothRecord
        /// </summary>
        public static ToothRecord ToothRecordFromFirestore(DocumentSnapshot snapshot)
        {
            if (snapshot == null || !snapshot.Exists)
                return null;

            var id = Guid.Parse(snapshot.Id);
            var toothNumber = new ToothNumber(snapshot.GetValue<int>("toothNumber"));
            var status = Enum.Parse<ToothStatus>(snapshot.GetValue<string>("status"));

            // Crear registro de diente
            var record = new ToothRecord(id, toothNumber, status);

            // Agregar lesiones si existen
            if (snapshot.TryGetValue<List<Dictionary<string, object>>>("lesions", out var lesionsData) 
                && lesionsData != null)
            {
                foreach (var lesionData in lesionsData)
                {
                    var lesionId = Guid.Parse(lesionData["lesionId"].ToString());
                    var surfaces = Enum.Parse<ToothSurfaces>(lesionData["surfaces"].ToString());
                    var recordedAt = DateTime.Parse(lesionData["recordedAt"].ToString());
                    var recordedBy = Guid.Parse(lesionData["recordedBy"].ToString());
                    
                    record.AddLesion(lesionId, surfaces, recordedAt, recordedBy);
                }
            }

            // Agregar tratamientos si existen
            if (snapshot.TryGetValue<List<Dictionary<string, object>>>("treatments", out var treatmentsData) 
                && treatmentsData != null)
            {
                foreach (var treatmentData in treatmentsData)
                {
                    var treatmentId = Guid.Parse(treatmentData["treatmentId"].ToString());
                    var surfaces = Enum.Parse<ToothSurfaces>(treatmentData["surfaces"].ToString());
                    var performedAt = DateTime.Parse(treatmentData["performedAt"].ToString());
                    var performedBy = Guid.Parse(treatmentData["performedBy"].ToString());
                    var status = Enum.Parse<TreatmentStatus>(treatmentData["status"].ToString());
                    
                    record.AddTreatment(treatmentId, surfaces, performedAt, performedBy, status);
                }
            }

            return record;
        }
    }
} 