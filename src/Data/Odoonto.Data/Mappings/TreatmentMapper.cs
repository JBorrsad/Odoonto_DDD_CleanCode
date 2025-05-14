using System;
using System.Collections.Generic;
using Google.Cloud.Firestore;
using Odoonto.Domain.Models.Treatments;
using Odoonto.Domain.Models.ValueObjects;

namespace Odoonto.Data.Mappings
{
    /// <summary>
    /// Clase para mapear entre documentos Firestore y entidades Treatment
    /// </summary>
    public static class TreatmentMapper
    {
        /// <summary>
        /// Convierte un documento Firestore a una entidad Treatment
        /// </summary>
        public static Treatment ToEntity(DocumentSnapshot document)
        {
            if (document == null || !document.Exists)
                return null;

            var data = document.ToDictionary();
            var id = Guid.Parse(document.Id);

            // Crear el tratamiento
            var treatment = Treatment.Create(id);

            // Establecer nombre
            var name = data.GetValueOrDefault("Name")?.ToString() ?? "";
            if (!string.IsNullOrEmpty(name))
            {
                treatment.SetName(name);
            }

            // Establecer descripción
            var description = data.GetValueOrDefault("Description")?.ToString() ?? "";
            treatment.SetDescription(description);

            // Establecer categoría
            var category = data.GetValueOrDefault("Category")?.ToString() ?? "";
            treatment.SetCategory(category);

            // Establecer precio
            decimal price = 0;
            string currency = "MXN";

            if (data.TryGetValue("Price", out var priceObj))
            {
                if (priceObj is double priceDouble)
                {
                    price = (decimal)priceDouble;
                }
                else if (priceObj is Dictionary<string, object> priceDict)
                {
                    if (priceDict.TryGetValue("Amount", out var amountObj) &&
                        amountObj is double amountDouble)
                    {
                        price = (decimal)amountDouble;
                    }

                    currency = priceDict.GetValueOrDefault("Currency")?.ToString() ?? "MXN";
                }
            }

            treatment.SetPrice(price, currency);

            // Establecer duración estimada
            int durationMinutes = 0;
            if (data.TryGetValue("DurationMinutes", out var durationObj))
            {
                if (durationObj is long durationLong)
                {
                    durationMinutes = (int)durationLong;
                }
                else if (durationObj is int durationInt)
                {
                    durationMinutes = durationInt;
                }
                else if (durationObj is double durationDouble)
                {
                    durationMinutes = (int)durationDouble;
                }
            }

            if (durationMinutes > 0)
            {
                treatment.SetEstimatedDurationInMinutes(durationMinutes);
            }
            else
            {
                // Valor predeterminado (30 minutos)
                treatment.SetEstimatedDurationInMinutes(30);
            }

            return treatment;
        }

        /// <summary>
        /// Convierte una entidad Treatment a un diccionario para Firestore
        /// </summary>
        public static Dictionary<string, object> ToFirestore(Treatment treatment)
        {
            if (treatment == null)
                return null;

            var data = new Dictionary<string, object>
            {
                { "Name", treatment.Name },
                { "Description", treatment.Description },
                { "Category", treatment.Category },
                { "DurationMinutes", (int)treatment.EstimatedDuration.TotalMinutes },
                { "CreationDate", Timestamp.FromDateTime(treatment.CreationDate.ToUniversalTime()) },
                { "EditDate", Timestamp.FromDateTime(treatment.EditDate.ToUniversalTime()) }
            };

            // Mapear el precio
            if (treatment.Price != null)
            {
                data["Price"] = new Dictionary<string, object>
                {
                    { "Amount", (double)treatment.Price.Amount },
                    { "Currency", treatment.Price.Currency }
                };
            }

            return data;
        }
    }
}