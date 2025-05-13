using System;
using System.Collections.Generic;
using Odoonto.Domain.Core.Abstractions;
using Odoonto.Domain.Core.Models.Exceptions;

namespace Odoonto.Domain.Models.ValueObjects
{
    /// <summary>
    /// Representa una fecha con zona horaria Europe/Madrid
    /// </summary>
    public class Date : ValueObject
    {
        /// <summary>
        /// Valor de la fecha
        /// </summary>
        public DateTime Value { get; }

        /// <summary>
        /// Constructor para crear una fecha
        /// </summary>
        public Date(DateTime value)
        {
            // Asegurar que la fecha no incluye componente de tiempo
            Value = value.Date;
        }

        /// <summary>
        /// Crea una fecha a partir de un string en formato dd/MM/yyyy
        /// </summary>
        public static Date FromString(string dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString))
                throw new ArgumentException("La fecha no puede estar vacía", nameof(dateString));

            if (!DateTime.TryParse(dateString, out DateTime date))
                throw new ArgumentException("Formato de fecha inválido. Use dd/MM/yyyy", nameof(dateString));

            return new Date(date);
        }

        /// <summary>
        /// Retorna la representación en formato corto de la fecha
        /// </summary>
        public override string ToString()
        {
            return Value.ToShortDateString();
        }

        /// <summary>
        /// Componentes para comparación de igualdad
        /// </summary>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value.Date;
        }

        /// <summary>
        /// Conversión implícita desde DateTime
        /// </summary>
        public static implicit operator DateTime(Date date)
        {
            return date.Value;
        }

        /// <summary>
        /// Conversión explícita desde Date a DateTime
        /// </summary>
        public static explicit operator Date(DateTime dateTime)
        {
            return new Date(dateTime);
        }
    }
} 