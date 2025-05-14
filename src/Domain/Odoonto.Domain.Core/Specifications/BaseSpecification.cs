using System;
using System.Linq.Expressions;

namespace Odoonto.Domain.Core.Specifications
{
    /// <summary>
    /// Clase base para implementar especificaciones
    /// </summary>
    /// <typeparam name="T">Tipo de entidad</typeparam>
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        /// <summary>
        /// Obtiene la expresión que define la especificación
        /// </summary>
        public Expression<Func<T, bool>> Criteria { get; }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        protected BaseSpecification()
        {
            Criteria = x => true;
        }

        /// <summary>
        /// Constructor con criterio
        /// </summary>
        /// <param name="criteria">Expresión de criterio</param>
        protected BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria ?? throw new ArgumentNullException(nameof(criteria));
        }

        /// <summary>
        /// Aplica la especificación a una entidad
        /// </summary>
        /// <param name="entity">Entidad a evaluar</param>
        /// <returns>True si la entidad cumple la especificación, False en caso contrario</returns>
        public bool IsSatisfiedBy(T entity)
        {
            var predicate = Criteria.Compile();
            return predicate(entity);
        }

        /// <summary>
        /// Combina esta especificación con otra usando operador AND
        /// </summary>
        /// <param name="other">Otra especificación</param>
        /// <returns>Nueva especificación combinada</returns>
        public BaseSpecification<T> And(ISpecification<T> other)
        {
            return new AndSpecification<T>(this, other);
        }

        /// <summary>
        /// Combina esta especificación con otra usando operador OR
        /// </summary>
        /// <param name="other">Otra especificación</param>
        /// <returns>Nueva especificación combinada</returns>
        public BaseSpecification<T> Or(ISpecification<T> other)
        {
            return new OrSpecification<T>(this, other);
        }

        /// <summary>
        /// Niega esta especificación
        /// </summary>
        /// <returns>Nueva especificación negada</returns>
        public BaseSpecification<T> Not()
        {
            return new NotSpecification<T>(this);
        }
    }

    /// <summary>
    /// Especificación que combina dos especificaciones con operador AND
    /// </summary>
    /// <typeparam name="T">Tipo de entidad</typeparam>
    public class AndSpecification<T> : BaseSpecification<T>
    {
        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            var paramExpr = Expression.Parameter(typeof(T), "x");
            var exprBody = Expression.AndAlso(
                Expression.Invoke(left.Criteria, paramExpr),
                Expression.Invoke(right.Criteria, paramExpr)
            );

            var expr = Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);
            Criteria = expr;
        }
    }

    /// <summary>
    /// Especificación que combina dos especificaciones con operador OR
    /// </summary>
    /// <typeparam name="T">Tipo de entidad</typeparam>
    public class OrSpecification<T> : BaseSpecification<T>
    {
        public OrSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            var paramExpr = Expression.Parameter(typeof(T), "x");
            var exprBody = Expression.OrElse(
                Expression.Invoke(left.Criteria, paramExpr),
                Expression.Invoke(right.Criteria, paramExpr)
            );

            var expr = Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);
            Criteria = expr;
        }
    }

    /// <summary>
    /// Especificación que niega otra especificación
    /// </summary>
    /// <typeparam name="T">Tipo de entidad</typeparam>
    public class NotSpecification<T> : BaseSpecification<T>
    {
        public NotSpecification(ISpecification<T> specification)
        {
            var paramExpr = Expression.Parameter(typeof(T), "x");
            var exprBody = Expression.Not(
                Expression.Invoke(specification.Criteria, paramExpr)
            );

            var expr = Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);
            Criteria = expr;
        }
    }
}