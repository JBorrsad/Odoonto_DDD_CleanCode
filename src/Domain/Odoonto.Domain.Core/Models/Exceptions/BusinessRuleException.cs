using System;

namespace Odoonto.Domain.Core.Models.Exceptions
{
    /// <summary>
    /// Excepción que se lanza cuando se viola una regla de negocio
    /// </summary>
    public class BusinessRuleException : Exception
    {
        /// <summary>
        /// Constructor con mensaje
        /// </summary>
        /// <param name="message">Mensaje de error</param>
        public BusinessRuleException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor con mensaje y excepción interna
        /// </summary>
        /// <param name="message">Mensaje de error</param>
        /// <param name="innerException">Excepción interna</param>
        public BusinessRuleException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
} 