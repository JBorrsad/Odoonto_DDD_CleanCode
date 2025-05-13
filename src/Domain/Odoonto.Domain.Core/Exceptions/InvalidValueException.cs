using System;

namespace Odoonto.Domain.Core.Models.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando un valor no cumple con las reglas de validación del dominio
    /// </summary>
    public class InvalidValueException : Exception
    {
        /// <summary>
        /// Crea una nueva instancia de la excepción con un mensaje
        /// </summary>
        /// <param name="message">Mensaje que describe la razón de la excepción</param>
        public InvalidValueException(string message) : base(message)
        {
        }

        /// <summary>
        /// Crea una nueva instancia de la excepción con un mensaje y una excepción interna
        /// </summary>
        /// <param name="message">Mensaje que describe la razón de la excepción</param>
        /// <param name="innerException">Excepción que causó esta excepción</param>
        public InvalidValueException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
} 