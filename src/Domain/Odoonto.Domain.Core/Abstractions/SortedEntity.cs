using System;

namespace Odoonto.Domain.Core.Abstractions
{
    /// <summary>
    /// Clase base para entidades que requieren ordenamiento.
    /// Extiende la clase Entity añadiendo funcionalidad de comparación.
    /// </summary>
    public abstract class SortedEntity : Entity, IComparable<SortedEntity>
    {
        /// <summary>
        /// Orden de la entidad para determinar su posición en colecciones ordenadas
        /// </summary>
        public int Order { get; protected set; }

        /// <summary>
        /// Constructor para entidades ordenables
        /// </summary>
        /// <param name="id">Identificador único de la entidad</param>
        /// <param name="order">Orden inicial de la entidad (por defecto 0)</param>
        protected SortedEntity(Guid id, int order = 0) : base(id)
        {
            Order = order;
        }

        /// <summary>
        /// Constructor protegido sin parámetros para EF Core
        /// </summary>
        protected SortedEntity() : base()
        {
            Order = 0;
        }

        /// <summary>
        /// Cambia el orden de la entidad
        /// </summary>
        /// <param name="order">Nuevo orden</param>
        public void SetOrder(int order)
        {
            Order = order;
            UpdateEditDate();
        }

        /// <summary>
        /// Compara esta entidad con otra para ordenamiento
        /// </summary>
        /// <param name="other">Otra entidad a comparar</param>
        /// <returns>
        /// -1 si esta entidad va antes,
        /// 0 si son equivalentes en orden,
        /// 1 si esta entidad va después
        /// </returns>
        public int CompareTo(SortedEntity other)
        {
            if (other is null) return 1;
            
            // Comparar primero por Order
            int orderComparison = Order.CompareTo(other.Order);
            if (orderComparison != 0)
                return orderComparison;
            
            // Si el Order es igual, compara por fecha de creación
            return CreationDate.CompareTo(other.CreationDate);
        }

        /// <summary>
        /// Permite comparar dos entidades ordenables
        /// </summary>
        public static bool operator <(SortedEntity left, SortedEntity right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Permite comparar dos entidades ordenables
        /// </summary>
        public static bool operator >(SortedEntity left, SortedEntity right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// Permite comparar dos entidades ordenables
        /// </summary>
        public static bool operator <=(SortedEntity left, SortedEntity right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            return left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// Permite comparar dos entidades ordenables
        /// </summary>
        public static bool operator >=(SortedEntity left, SortedEntity right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            return left.CompareTo(right) >= 0;
        }
    }
} 