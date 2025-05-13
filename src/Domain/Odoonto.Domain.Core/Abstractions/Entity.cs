using System;

namespace Odoonto.Domain.Core.Abstractions
{
    /// <summary>
    /// Clase base para todas las entidades del dominio.
    /// Proporciona comportamiento común como identidad y propiedades de auditoría.
    /// </summary>
    public abstract class Entity
    {
        // Identificador único de la entidad
        public Guid Id { get; protected set; }

        // Propiedades de auditoría
        public DateTime CreationDate { get; protected set; }
        public DateTime EditDate { get; protected set; }

        // Constructor protegido, solo accesible desde clases derivadas
        protected Entity(Guid id)
        {
            Id = id;
            var now = DateTime.UtcNow;
            CreationDate = now;
            EditDate = now;
        }

        // Constructor protegido sin parámetros para EF Core
        protected Entity()
        {
            Id = Guid.Empty;
            CreationDate = DateTime.UtcNow;
            EditDate = DateTime.UtcNow;
        }

        // Método para actualizar la fecha de edición
        public void UpdateEditDate()
        {
            EditDate = DateTime.UtcNow;
        }

        // Sobrescritura de Equals para comparación de entidades por Id
        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj.GetType() != this.GetType())
                return false;

            Entity other = (Entity)obj;
            return Id.Equals(other.Id);
        }

        // Sobrescritura de GetHashCode basada en Id
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        // Sobrecarga del operador de igualdad
        public static bool operator ==(Entity left, Entity right)
        {
            if (left is null && right is null)
                return true;

            if (left is null || right is null)
                return false;

            return left.Equals(right);
        }

        // Sobrecarga del operador de desigualdad
        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }
    }
} 