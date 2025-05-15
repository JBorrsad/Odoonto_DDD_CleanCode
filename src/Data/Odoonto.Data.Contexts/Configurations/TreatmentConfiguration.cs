using System;
using System.Collections.Generic;
using Google.Cloud.Firestore;
using Odoonto.Domain.Models.Treatments;
using Odoonto.Domain.ValueObjects;

namespace Odoonto.Data.Contexts.Configurations
{
    /// <summary>
    /// Configuración para mapeo entre entidad Treatment y documento Firestore
    /// </summary>
    public class TreatmentConfiguration
    {
        private const string CollectionName = "treatments";

        /// <summary>
        /// Obtiene el nombre de la colección
        /// </summary>
        public static string Collection => CollectionName;

        /// <summary>
        /// Convierte un Treatment a un Dictionary para Firestore
        /// </summary>
        public static Dictionary<string, object> ToFirestore(Treatment treatment)
        {
            if (treatment == null)
                throw new ArgumentNullException(nameof(treatment));

            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "id", treatment.Id.ToString() },
                { "name", treatment.Name },
                { "description", treatment.Description },
                { "price", new Dictionary<string, object>
                    {
                        { "amount", treatment.Price.Amount },
                        { "currency", treatment.Price.Currency }
                    }
                },
                { "duration", treatment.Duration },
                { "category", treatment.Category },
                { "createdAt", treatment.CreatedAt.ToDateTime() },
                { "updatedAt", treatment.UpdatedAt.ToDateTime() }
            };

            // Convertir lista de dientes requeridos si no es nula
            if (treatment.RequiredTeeth != null && treatment.RequiredTeeth.Count > 0)
            {
                var teethList = new List<string>();
                foreach (var tooth in treatment.RequiredTeeth)
                {
                    teethList.Add(tooth.Number.ToString());
                }
                data.Add("requiredTeeth", teethList);
            }

            return data;
        }

        /// <summary>
        /// Convierte un documento Firestore a un Treatment
        /// </summary>
        public static Treatment FromFirestore(DocumentSnapshot snapshot)
        {
            if (snapshot == null || !snapshot.Exists)
                return null;

            var id = Guid.Parse(snapshot.Id);
            var name = snapshot.GetValue<string>("name");
            var description = snapshot.GetValue<string>("description");
            
            // Obtener precio
            var priceData = snapshot.GetValue<Dictionary<string, object>>("price");
            decimal amount = Convert.ToDecimal(priceData["amount"]);
            string currency = priceData["currency"].ToString();
            var price = new Money(amount, currency);

            // Obtener duración
            int duration = snapshot.GetValue<int>("duration");

            // Obtener categoría
            string category = snapshot.GetValue<string>("category");

            // Crear instancia de Treatment
            var treatment = new Treatment(id, name, description, price, duration, category);

            // Agregar dientes requeridos si existen
            if (snapshot.TryGetValue<List<string>>("requiredTeeth", out var teethStringList) && teethStringList != null)
            {
                foreach (var toothString in teethStringList)
                {
                    int toothNumber = int.Parse(toothString);
                    treatment.AddRequiredTooth(new ToothNumber(toothNumber));
                }
            }

            return treatment;
        }
    }
} 