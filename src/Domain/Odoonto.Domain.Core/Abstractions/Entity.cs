using System;

namespace Odoonto.Domain.Core.Abstractions
{
    /// <summary>
    /// Clase base para todas las entidades de dominio
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// Identificador único de la entidad
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Fecha y hora de creación
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Fecha y hora de última actualización
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Constructor sin parámetros para serialización
        /// </summary>
        protected Entity()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Constructor con ID específico
        /// </summary>
        /// <param name="id">Identificador único de la entidad</param>
        protected Entity(Guid id) : this()
        {
            Id = id;
        }

        /// <summary>
        /// Verifica si dos entidades son iguales comparando sus identificadores
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            if (obj is not Entity other)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (GetType() != other.GetType())
                return false;

            if (Id == Guid.Empty || other.Id == Guid.Empty)
                return false;

            return Id == other.Id;
        }

        /// <summary>
        /// Obtiene el código hash basado en el identificador
        /// </summary>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// Operador de igualdad
        /// </summary>
        public static bool operator ==(Entity left, Entity right)
        {
            if (left is null && right is null)
                return true;

            if (left is null || right is null)
                return false;

            return left.Equals(right);
        }

        /// <summary>
        /// Operador de desigualdad
        /// </summary>
        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }
    }
} 