using System;
using System.Collections.Generic;
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
        public Expression<Func<T, bool>> Criteria { get; protected set; }

        /// <summary>
        /// Obtiene las expresiones de include para esta especificación
        /// </summary>
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        /// <summary>
        /// Obtiene las expresiones de include como strings
        /// </summary>
        public List<string> IncludeStrings { get; } = new List<string>();

        /// <summary>
        /// Obtiene la expresión de orden para esta especificación
        /// </summary>
        public Expression<Func<T, object>> OrderBy { get; private set; }

        /// <summary>
        /// Obtiene la expresión de orden descendente para esta especificación
        /// </summary>
        public Expression<Func<T, object>> OrderByDescending { get; private set; }

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
        /// Añade una expresión de include
        /// </summary>
        /// <param name="includeExpression">Expresión de include</param>
        protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        /// <summary>
        /// Añade un include como string
        /// </summary>
        /// <param name="includeString">String de include</param>
        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }

        /// <summary>
        /// Añade un criterio de ordenación ascendente
        /// </summary>
        /// <param name="orderByExpression">Expresión de ordenación</param>
        protected virtual void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        /// <summary>
        /// Añade un criterio de ordenación descendente
        /// </summary>
        /// <param name="orderByDescendingExpression">Expresión de ordenación descendente</param>
        protected virtual void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
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

            // Combinar los includes de ambas especificaciones
            foreach (var include in left.Includes)
            {
                AddInclude(include);
            }

            foreach (var include in right.Includes)
            {
                AddInclude(include);
            }

            foreach (var includeString in left.IncludeStrings)
            {
                AddInclude(includeString);
            }

            foreach (var includeString in right.IncludeStrings)
            {
                AddInclude(includeString);
            }
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

            // Combinar los includes de ambas especificaciones
            foreach (var include in left.Includes)
            {
                AddInclude(include);
            }

            foreach (var include in right.Includes)
            {
                AddInclude(include);
            }

            foreach (var includeString in left.IncludeStrings)
            {
                AddInclude(includeString);
            }

            foreach (var includeString in right.IncludeStrings)
            {
                AddInclude(includeString);
            }
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

            // Mantener los includes de la especificación original
            foreach (var include in specification.Includes)
            {
                AddInclude(include);
            }

            foreach (var includeString in specification.IncludeStrings)
            {
                AddInclude(includeString);
            }
        }
    }
}