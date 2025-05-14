using System;

namespace Odoonto.Domain.Core.Models.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando se intenta agregar un valor duplicado
    /// </summary>
    public class DuplicatedValueException : Exception
    {
        /// <summary>
        /// Crea una nueva instancia de la excepción con un mensaje predeterminado
        /// </summary>
        public DuplicatedValueException() : base("El valor ya existe en la colección.")
        {
        }

        /// <summary>
        /// Crea una nueva instancia de la excepción con un mensaje
        /// </summary>
        /// <param name="message">Mensaje que describe la razón de la excepción</param>
        public DuplicatedValueException(string message) : base(message)
        {
        }

        /// <summary>
        /// Crea una nueva instancia de la excepción con un mensaje y una excepción interna
        /// </summary>
        /// <param name="message">Mensaje que describe la razón de la excepción</param>
        /// <param name="innerException">Excepción que causó esta excepción</param>
        public DuplicatedValueException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}