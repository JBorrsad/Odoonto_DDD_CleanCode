using System;
using System.Collections.Generic;
using Odoonto.Domain.Core.Abstractions;
using Odoonto.Domain.Core.Models.Exceptions;
using Odoonto.Domain.Models.ValueObjects;

namespace Odoonto.Domain.Models.Treatments
{
    /// <summary>
    /// Define los procedimientos clínicos que la clínica ofrece y factura
    /// </summary>
    public class Treatment : Entity
    {
        // Propiedades de dominio con getters públicos y setters privados
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Money Price { get; private set; }
        public int DurationMinutes { get; private set; }
        public string Category { get; private set; }
        
        // Colecciones privadas
        private readonly List<int> _requiredTeeth;
        
        // Propiedades de acceso público a colecciones como solo lectura
        public IReadOnlyCollection<int> RequiredTeeth => _requiredTeeth.AsReadOnly();

        // Constructor privado - solo accesible desde método factory
        private Treatment(Guid id) : base(id)
        {
            _requiredTeeth = new List<int>();
            Name = string.Empty;
            Description = string.Empty;
            Category = string.Empty;
        }

        // Método factory para crear instancias válidas
        public static Treatment Create(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new DomainException("El identificador del tratamiento no puede estar vacío.");
            }

            var treatment = new Treatment(id);
            treatment.UpdateEditDate();
            return treatment;
        }

        // Método para establecer la información básica del tratamiento
        public void SetBasicInfo(string name, string description, string category)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new DomainException("El nombre del tratamiento no puede estar vacío.");
            }

            Name = name.Trim();
            Description = description?.Trim() ?? string.Empty;
            Category = category?.Trim() ?? string.Empty;
            
            UpdateEditDate();
        }

        // Método para establecer el precio
        public void SetPrice(Money price)
        {
            Price = price ?? throw new DomainException("El precio no puede ser nulo.");
            UpdateEditDate();
        }

        // Método para establecer la duración
        public void SetDuration(int durationMinutes)
        {
            if (durationMinutes <= 0)
            {
                throw new DomainException("La duración debe ser mayor que cero.");
            }

            DurationMinutes = durationMinutes;
            UpdateEditDate();
        }

        // Método para agregar un diente requerido
        public void AddRequiredTooth(int toothNumber)
        {
            if (toothNumber < 1 || toothNumber > 32)
            {
                throw new DomainException("El número de diente debe estar entre 1 y 32.");
            }

            if (!_requiredTeeth.Contains(toothNumber))
            {
                _requiredTeeth.Add(toothNumber);
                UpdateEditDate();
            }
        }

        // Método para quitar un diente requerido
        public void RemoveRequiredTooth(int toothNumber)
        {
            if (_requiredTeeth.Contains(toothNumber))
            {
                _requiredTeeth.Remove(toothNumber);
                UpdateEditDate();
            }
        }

        // Método para limpiar la lista de dientes requeridos
        public void ClearRequiredTeeth()
        {
            if (_requiredTeeth.Count > 0)
            {
                _requiredTeeth.Clear();
                UpdateEditDate();
            }
        }

        // Métodos para gestionar fechas internas (para mapeo en repositorios)
        internal void SetCreatedAt(DateTime createdAt)
        {
            CreatedAt = createdAt;
        }
        
        internal void SetUpdatedAt(DateTime updatedAt)
        {
            UpdatedAt = updatedAt;
        }
    }
} 