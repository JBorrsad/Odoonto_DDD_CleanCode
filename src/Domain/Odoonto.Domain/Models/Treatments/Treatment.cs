using System;
using Odoonto.Domain.Core.Models.Exceptions;
using Odoonto.Domain.Models.ValueObjects;
using Odoonto.Domain.Core.Abstractions;

namespace Odoonto.Domain.Models.Treatments
{
    /// <summary>
    /// Define los procedimientos clínicos que la clínica ofrece y factura
    /// </summary>
    public class Treatment : Entity
    {
        // Propiedades con getters públicos y setters privados
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Money Price { get; private set; }
        public TimeSpan EstimatedDuration { get; private set; }
        public string Category { get; private set; }

        // Constructor privado - solo accesible desde método factory
        private Treatment(Guid id) : base(id)
        {
            Name = string.Empty;
            Description = string.Empty;
            Category = string.Empty;
            EstimatedDuration = TimeSpan.Zero;
        }

        // Método factory para crear instancias válidas
        public static Treatment Create(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new InvalidValueException("El identificador del tratamiento no puede estar vacío.");
            }

            var treatment = new Treatment(id);
            treatment.UpdateEditDate();
            return treatment;
        }

        // Método para establecer el nombre
        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidValueException("El nombre del tratamiento no puede estar vacío.");
            }

            Name = name.Trim();
            this.UpdateEditDate();
        }

        // Método para establecer la descripción
        public void SetDescription(string description)
        {
            Description = description?.Trim() ?? string.Empty;
            this.UpdateEditDate();
        }

        // Método para establecer el precio
        public void SetPrice(Money price)
        {
            Price = price ?? throw new InvalidValueException("El precio no puede ser nulo.");
            this.UpdateEditDate();
        }

        // Método para establecer el precio con valores primitivos
        public void SetPrice(decimal amount, string currency)
        {
            Price = new Money(amount, currency);
            this.UpdateEditDate();
        }

        // Método para establecer la duración estimada
        public void SetEstimatedDuration(TimeSpan duration)
        {
            if (duration <= TimeSpan.Zero)
            {
                throw new InvalidValueException("La duración estimada debe ser mayor que cero.");
            }

            EstimatedDuration = duration;
            this.UpdateEditDate();
        }

        // Método para establecer la duración estimada en minutos
        public void SetEstimatedDurationInMinutes(int minutes)
        {
            if (minutes <= 0)
            {
                throw new InvalidValueException("La duración estimada debe ser mayor que cero.");
            }

            EstimatedDuration = TimeSpan.FromMinutes(minutes);
            this.UpdateEditDate();
        }

        // Método para establecer la categoría
        public void SetCategory(string category)
        {
            Category = category?.Trim() ?? string.Empty;
            this.UpdateEditDate();
        }
    }
} 