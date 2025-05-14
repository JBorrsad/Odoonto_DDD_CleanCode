using System;
using System.Linq.Expressions;

namespace Odoonto.Domain.Core.Specifications
{
    /// <summary>
    /// Interfaz base para todas las especificaciones
    /// </summary>
    /// <typeparam name="T">Tipo de entidad</typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// Obtiene la expresión que define la especificación
        /// </summary>
        Expression<Func<T, bool>> Criteria { get; }

        /// <summary>
        /// Aplica la especificación a una entidad
        /// </summary>
        /// <param name="entity">Entidad a evaluar</param>
        /// <returns>True si la entidad cumple la especificación, False en caso contrario</returns>
        bool IsSatisfiedBy(T entity);
    }
}