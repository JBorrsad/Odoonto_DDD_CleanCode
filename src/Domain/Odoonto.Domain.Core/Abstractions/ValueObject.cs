using System;
using System.Collections.Generic;
using System.Linq;

namespace Odoonto.Domain.Core.Abstractions
{
    /// <summary>
    /// Clase base para todos los value objects del dominio.
    /// Los value objects son inmutables y se comparan por valor, no por referencia.
    /// </summary>
    public abstract class ValueObject
    {
        /// <summary>
        /// Cuando se sobrescribe, devuelve los componentes que forman parte de la igualdad del value object
        /// </summary>
        /// <returns>Colección de componentes para comparación de igualdad</returns>
        protected abstract IEnumerable<object> GetEqualityComponents();

        /// <summary>
        /// Implementación genérica de Equals que compara todos los componentes
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (ValueObject)obj;
            
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        /// <summary>
        /// Implementación genérica de GetHashCode que combina todos los componentes
        /// </summary>
        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Aggregate(1, (current, obj) =>
                {
                    unchecked
                    {
                        return (current * 23) + (obj?.GetHashCode() ?? 0);
                    }
                });
        }

        /// <summary>
        /// Operador de igualdad para comparar value objects
        /// </summary>
        public static bool operator ==(ValueObject a, ValueObject b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        /// <summary>
        /// Operador de desigualdad para comparar value objects
        /// </summary>
        public static bool operator !=(ValueObject a, ValueObject b)
        {
            return !(a == b);
        }
    }
} 