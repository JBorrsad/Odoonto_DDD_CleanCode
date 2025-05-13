using System;
using System.Collections.Generic;
using System.Linq;

namespace Odoonto.Domain.Core.Abstractions
{
    /// <summary>
    /// Clase base para objetos de valor (Value Objects) en el dominio.
    /// Los objetos de valor se comparan por el valor, no por la identidad.
    /// </summary>
    public abstract class ValueObject
    {
        /// <summary>
        /// Cuando se implementa en una clase derivada, devuelve los componentes de este objeto que deben compararse.
        /// </summary>
        protected abstract IEnumerable<object> GetEqualityComponents();

        /// <summary>
        /// Compara si este objeto es igual a otro.
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
        /// Obtiene un código hash basado en los componentes del objeto.
        /// </summary>
        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }

        /// <summary>
        /// Operador de igualdad
        /// </summary>
        public static bool operator ==(ValueObject left, ValueObject right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
                return true;

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;

            return left.Equals(right);
        }

        /// <summary>
        /// Operador de desigualdad
        /// </summary>
        public static bool operator !=(ValueObject left, ValueObject right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Crea una copia superficial de este objeto.
        /// </summary>
        public ValueObject GetCopy()
        {
            return this.MemberwiseClone() as ValueObject;
        }
    }
} 