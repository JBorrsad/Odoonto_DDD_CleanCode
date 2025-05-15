using System;

namespace Odoonto.Domain.Core.Models.Exceptions
{
    /// <summary>
    /// Excepción base para todas las excepciones de dominio
    /// </summary>
    public class DomainException : Exception
    {
        /// <summary>
        /// Constructor con mensaje
        /// </summary>
        /// <param name="message">Mensaje de error</param>
        public DomainException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor con mensaje y excepción interna
        /// </summary>
        /// <param name="message">Mensaje de error</param>
        /// <param name="innerException">Excepción interna</param>
        public DomainException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
} 