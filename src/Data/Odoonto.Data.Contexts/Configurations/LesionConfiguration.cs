using System;
using System.Collections.Generic;
using Google.Cloud.Firestore;
using Odoonto.Domain.Models.Lesions;

namespace Odoonto.Data.Contexts.Configurations
{
    /// <summary>
    /// Configuración para mapeo entre entidad Lesion y documento Firestore
    /// </summary>
    public class LesionConfiguration
    {
        private const string CollectionName = "lesions";

        /// <summary>
        /// Obtiene el nombre de la colección
        /// </summary>
        public static string Collection => CollectionName;

        /// <summary>
        /// Convierte una Lesion a un Dictionary para Firestore
        /// </summary>
        public static Dictionary<string, object> ToFirestore(Lesion lesion)
        {
            if (lesion == null)
                throw new ArgumentNullException(nameof(lesion));

            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "id", lesion.Id.ToString() },
                { "name", lesion.Name },
                { "description", lesion.Description },
                { "category", lesion.Category },
                { "severity", lesion.Severity },
                { "colorCode", lesion.ColorCode },
                { "createdAt", lesion.CreatedAt.ToDateTime() },
                { "updatedAt", lesion.UpdatedAt.ToDateTime() }
            };

            // Agregar tratamientos recomendados si existen
            if (lesion.RecommendedTreatments != null && lesion.RecommendedTreatments.Count > 0)
            {
                var treatmentIds = new List<string>();
                foreach (var treatment in lesion.RecommendedTreatments)
                {
                    treatmentIds.Add(treatment.ToString());
                }
                data.Add("recommendedTreatments", treatmentIds);
            }

            return data;
        }

        /// <summary>
        /// Convierte un documento Firestore a una Lesion
        /// </summary>
        public static Lesion FromFirestore(DocumentSnapshot snapshot)
        {
            if (snapshot == null || !snapshot.Exists)
                return null;

            var id = Guid.Parse(snapshot.Id);
            var name = snapshot.GetValue<string>("name");
            var description = snapshot.GetValue<string>("description");
            var category = snapshot.GetValue<string>("category");
            var severity = snapshot.GetValue<int>("severity");
            var colorCode = snapshot.GetValue<string>("colorCode");

            // Crear instancia de Lesion
            var lesion = new Lesion(id, name, description, category, severity, colorCode);

            // Agregar tratamientos recomendados si existen
            if (snapshot.TryGetValue<List<string>>("recommendedTreatments", out var treatmentIdsList) 
                && treatmentIdsList != null)
            {
                foreach (var treatmentId in treatmentIdsList)
                {
                    lesion.AddRecommendedTreatment(Guid.Parse(treatmentId));
                }
            }

            return lesion;
        }
    }
} 