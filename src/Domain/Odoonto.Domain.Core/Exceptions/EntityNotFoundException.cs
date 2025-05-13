using System;

namespace Odoonto.Domain.Core.Models.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando no se encuentra una entidad del dominio
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        /// <summary>
        /// Identificador de la entidad que no fue encontrada
        /// </summary>
        public Guid EntityId { get; }
        
        /// <summary>
        /// Tipo de la entidad que no fue encontrada
        /// </summary>
        public string EntityType { get; }

        /// <summary>
        /// Crea una nueva instancia de la excepción con un mensaje
        /// </summary>
        /// <param name="message">Mensaje que describe la razón de la excepción</param>
        public EntityNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Crea una nueva instancia de la excepción con un mensaje, tipo de entidad e identificador
        /// </summary>
        /// <param name="entityType">Tipo de la entidad que no fue encontrada</param>
        /// <param name="entityId">Identificador de la entidad que no fue encontrada</param>
        public EntityNotFoundException(string entityType, Guid entityId)
            : base($"No se encontró la entidad {entityType} con ID {entityId}")
        {
            EntityType = entityType;
            EntityId = entityId;
        }

        /// <summary>
        /// Crea una nueva instancia de la excepción con un mensaje y una excepción interna
        /// </summary>
        /// <param name="message">Mensaje que describe la razón de la excepción</param>
        /// <param name="innerException">Excepción que causó esta excepción</param>
        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
} 