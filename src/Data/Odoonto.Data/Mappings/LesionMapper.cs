using System;
using System.Collections.Generic;
using Google.Cloud.Firestore;
using Odoonto.Domain.Models.Lesions;

namespace Odoonto.Data.Mappings
{
    /// <summary>
    /// Clase para mapear entre documentos Firestore y entidades Lesion
    /// </summary>
    public static class LesionMapper
    {
        /// <summary>
        /// Convierte un documento Firestore a una entidad Lesion
        /// </summary>
        public static Lesion ToEntity(DocumentSnapshot document)
        {
            if (document == null || !document.Exists)
                return null;

            var data = document.ToDictionary();
            var id = Guid.Parse(document.Id);

            // Crear la lesión
            var lesion = Lesion.Create(id);

            // Establecer propiedades
            var name = data.GetValueOrDefault("Name")?.ToString() ?? "";
            if (!string.IsNullOrEmpty(name))
            {
                lesion.SetName(name);
            }

            var description = data.GetValueOrDefault("Description")?.ToString() ?? "";
            lesion.SetDescription(description);

            var category = data.GetValueOrDefault("Category")?.ToString() ?? "";
            lesion.SetCategory(category);

            // Establecer estado activo/inactivo
            bool isActive = true;
            if (data.TryGetValue("IsActive", out var isActiveObj) &&
                isActiveObj is bool isActiveBool)
            {
                isActive = isActiveBool;
            }

            if (isActive)
            {
                lesion.Activate();
            }
            else
            {
                lesion.Deactivate();
            }

            return lesion;
        }

        /// <summary>
        /// Convierte una entidad Lesion a un diccionario para Firestore
        /// </summary>
        public static Dictionary<string, object> ToFirestore(Lesion lesion)
        {
            if (lesion == null)
                return null;

            var data = new Dictionary<string, object>
            {
                { "Name", lesion.Name },
                { "Description", lesion.Description },
                { "Category", lesion.Category },
                { "IsActive", lesion.IsActive },
                { "CreationDate", Timestamp.FromDateTime(lesion.CreationDate.ToUniversalTime()) },
                { "EditDate", Timestamp.FromDateTime(lesion.EditDate.ToUniversalTime()) }
            };

            return data;
        }
    }
}