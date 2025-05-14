using System;

namespace Odoonto.Domain.Core.Models.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando se intenta realizar una operación incorrecta en el contexto actual
    /// </summary>
    public class WrongOperationException : Exception
    {
        /// <summary>
        /// Crea una nueva instancia de la excepción con un mensaje predeterminado
        /// </summary>
        public WrongOperationException() : base("La operación solicitada no puede realizarse en el contexto actual.")
        {
        }

        /// <summary>
        /// Crea una nueva instancia de la excepción con un mensaje
        /// </summary>
        /// <param name="message">Mensaje que describe la razón de la excepción</param>
        public WrongOperationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Crea una nueva instancia de la excepción con un mensaje y una excepción interna
        /// </summary>
        /// <param name="message">Mensaje que describe la razón de la excepción</param>
        /// <param name="innerException">Excepción que causó esta excepción</param>
        public WrongOperationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}