using System;

namespace Odoonto.Domain.Core.Models.Exceptions
{
    /// <summary>
    /// Excepción que se lanza cuando un valor proporcionado es inválido
    /// </summary>
    public class InvalidValueException : DomainException
    {
        /// <summary>
        /// Constructor con mensaje
        /// </summary>
        /// <param name="message">Mensaje de error</param>
        public InvalidValueException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor con mensaje y excepción interna
        /// </summary>
        /// <param name="message">Mensaje de error</param>
        /// <param name="innerException">Excepción interna</param>
        public InvalidValueException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
} 