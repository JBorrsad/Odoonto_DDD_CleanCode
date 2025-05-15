using System;
using System.Collections.Generic;

namespace Odoonto.Domain.Core.Exceptions
{
    /// <summary>
    /// Excepción base para todas las excepciones del dominio
    /// </summary>
    public abstract class DomainException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected DomainException(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// Excepción lanzada cuando no se encuentra una entidad
    /// </summary>
    public class NotFoundException : DomainException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NotFoundException(string entityName, object id) 
            : base($"La entidad {entityName} con id {id} no fue encontrada")
        {
            EntityName = entityName;
            Id = id;
        }

        /// <summary>
        /// Constructor con mensaje personalizado
        /// </summary>
        public NotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Nombre de la entidad
        /// </summary>
        public string EntityName { get; } = string.Empty;

        /// <summary>
        /// Identificador de la entidad
        /// </summary>
        public object Id { get; }
    }

    /// <summary>
    /// Excepción lanzada cuando se viola una regla de negocio
    /// </summary>
    public class BusinessRuleViolationException : DomainException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BusinessRuleViolationException(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// Excepción lanzada cuando hay errores de validación
    /// </summary>
    public class ValidationException : DomainException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ValidationException(string message) : base(message)
        {
            Errors = new Dictionary<string, string[]>();
        }

        /// <summary>
        /// Constructor con errores de validación
        /// </summary>
        public ValidationException(IDictionary<string, string[]> errors) 
            : base("Se encontraron uno o más errores de validación")
        {
            Errors = errors;
        }

        /// <summary>
        /// Errores de validación
        /// </summary>
        public IDictionary<string, string[]> Errors { get; }
    }

    /// <summary>
    /// Excepción lanzada cuando un usuario no está autenticado
    /// </summary>
    public class UnauthorizedException : DomainException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UnauthorizedException() : base("Usuario no autenticado")
        {
        }

        /// <summary>
        /// Constructor con mensaje personalizado
        /// </summary>
        public UnauthorizedException(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// Excepción lanzada cuando un usuario no tiene permisos
    /// </summary>
    public class AuthorizationException : DomainException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AuthorizationException() : base("Usuario no autorizado")
        {
        }

        /// <summary>
        /// Constructor con mensaje personalizado
        /// </summary>
        public AuthorizationException(string message) : base(message)
        {
        }
    }
} 