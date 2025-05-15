using System;

namespace Odoonto.Domain.Core.Models.Exceptions
{
    /// <summary>
    /// Excepción que se lanza cuando se intenta crear una entidad que ya existe
    /// </summary>
    public class DuplicateEntityException : Exception
    {
        /// <summary>
        /// Constructor con mensaje
        /// </summary>
        /// <param name="message">Mensaje de error</param>
        public DuplicateEntityException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor con mensaje y excepción interna
        /// </summary>
        /// <param name="message">Mensaje de error</param>
        /// <param name="innerException">Excepción interna</param>
        public DuplicateEntityException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructor que genera un mensaje estándar para entidad duplicada
        /// </summary>
        /// <param name="entityName">Nombre de la entidad</param>
        /// <param name="propertyName">Nombre de la propiedad</param>
        /// <param name="value">Valor duplicado</param>
        public DuplicateEntityException(string entityName, string propertyName, string value)
            : base($"Ya existe un/a {entityName} con {propertyName} '{value}'")
        {
        }
    }
} 