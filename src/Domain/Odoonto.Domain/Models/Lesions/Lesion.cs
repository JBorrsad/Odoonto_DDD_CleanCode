using System;
using Odoonto.Domain.Core.Models;
using Odoonto.Domain.Core.Models.Exceptions;

namespace Odoonto.Domain.Models.Lesions
{
    /// <summary>
    /// Catálogo de patologías/lesiones bucales disponibles para registrar en pacientes.
    /// </summary>
    public class Lesion : Entity
    {
        // Propiedades con getters públicos y setters privados
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Category { get; private set; }
        public bool IsActive { get; private set; }

        // Constructor privado - solo accesible desde método factory
        private Lesion(Guid id) : base(id)
        {
            Name = string.Empty;
            Description = string.Empty;
            Category = string.Empty;
            IsActive = true;
        }

        // Método factory para crear instancias válidas
        public static Lesion Create(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new InvalidValueException("El identificador de la lesión no puede estar vacío.");
            }

            var lesion = new Lesion(id);
            lesion.UpdateEditDate();
            return lesion;
        }

        // Método para establecer el nombre
        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidValueException("El nombre de la lesión no puede estar vacío.");
            }

            Name = name.Trim();
            UpdateEditDate();
        }

        // Método para establecer la descripción
        public void SetDescription(string description)
        {
            Description = description?.Trim() ?? string.Empty;
            UpdateEditDate();
        }

        // Método para establecer la categoría
        public void SetCategory(string category)
        {
            Category = category?.Trim() ?? string.Empty;
            UpdateEditDate();
        }

        // Método para activar la lesión en el catálogo
        public void Activate()
        {
            IsActive = true;
            UpdateEditDate();
        }

        // Método para desactivar la lesión en el catálogo
        public void Deactivate()
        {
            IsActive = false;
            UpdateEditDate();
        }
    }
} 