using System;
using System.Collections.Generic;

namespace Odoonto.Domain.Core.Models.Exceptions
{
    /// <summary>
    /// Excepción que se lanza cuando falla la validación de datos
    /// </summary>
    public class ValidationException : Exception
    {
        /// <summary>
        /// Errores de validación
        /// </summary>
        public IDictionary<string, string[]> Errors { get; }

        /// <summary>
        /// Constructor con mensaje
        /// </summary>
        /// <param name="message">Mensaje de error</param>
        public ValidationException(string message) : base(message)
        {
            Errors = new Dictionary<string, string[]>();
        }

        /// <summary>
        /// Constructor con mensaje y errores
        /// </summary>
        /// <param name="message">Mensaje de error</param>
        /// <param name="errors">Diccionario de errores</param>
        public ValidationException(string message, IDictionary<string, string[]> errors) : base(message)
        {
            Errors = errors ?? new Dictionary<string, string[]>();
        }

        /// <summary>
        /// Constructor con mensaje y excepción interna
        /// </summary>
        /// <param name="message">Mensaje de error</param>
        /// <param name="innerException">Excepción interna</param>
        public ValidationException(string message, Exception innerException) : base(message, innerException)
        {
            Errors = new Dictionary<string, string[]>();
        }
    }
} 