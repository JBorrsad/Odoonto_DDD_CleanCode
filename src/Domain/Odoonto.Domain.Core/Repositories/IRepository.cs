using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Odoonto.Domain.Core.Abstractions;

namespace Odoonto.Domain.Core.Repositories
{
    /// <summary>
    /// Interfaz base para todos los repositorios que proporcionan operaciones CRUD
    /// </summary>
    /// <typeparam name="T">Tipo de entidad que maneja el repositorio</typeparam>
    public interface IRepository<T> where T : Entity
    {
        /// <summary>
        /// Obtiene todas las entidades
        /// </summary>
        /// <returns>Colección de entidades</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Obtiene una entidad por su identificador
        /// </summary>
        /// <param name="id">Identificador de la entidad</param>
        /// <returns>La entidad si existe, null si no existe</returns>
        Task<T> GetByIdAsync(Guid id);

        /// <summary>
        /// Obtiene una entidad por su identificador o lanza una excepción si no existe
        /// </summary>
        /// <param name="id">Identificador de la entidad</param>
        /// <param name="errorMessage">Mensaje de error personalizado (opcional)</param>
        /// <returns>La entidad</returns>
        /// <exception cref="Odoonto.Domain.Core.Models.Exceptions.EntityNotFoundException">Si la entidad no existe</exception>
        Task<T> GetByIdOrThrowAsync(Guid id, string errorMessage = null);

        /// <summary>
        /// Verifica si existe una entidad con el identificador especificado
        /// </summary>
        /// <param name="id">Identificador de la entidad</param>
        /// <returns>True si existe, False si no existe</returns>
        Task<bool> ExistsAsync(Guid id);

        /// <summary>
        /// Busca entidades que cumplan un predicado
        /// </summary>
        /// <param name="predicate">Predicado de búsqueda</param>
        /// <returns>Colección de entidades que cumplen el predicado</returns>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Obtiene la primera entidad que cumple un predicado o devuelve null
        /// </summary>
        /// <param name="predicate">Predicado de búsqueda</param>
        /// <returns>La primera entidad que cumple el predicado o null</returns>
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Verifica si existe alguna entidad que cumpla un predicado
        /// </summary>
        /// <param name="predicate">Predicado de búsqueda</param>
        /// <returns>True si existe al menos una entidad, False si no existe ninguna</returns>
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Guarda una entidad nueva
        /// </summary>
        /// <param name="entity">Entidad a guardar</param>
        /// <returns>Identificador de la entidad guardada</returns>
        Task<Guid> AddAsync(T entity);

        /// <summary>
        /// Actualiza una entidad existente
        /// </summary>
        /// <param name="entity">Entidad a actualizar</param>
        /// <returns>Task asíncrono</returns>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Elimina una entidad
        /// </summary>
        /// <param name="id">Identificador de la entidad a eliminar</param>
        /// <returns>True si se eliminó, False si no se encontró</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Guarda una entidad nueva o actualiza una existente
        /// </summary>
        /// <param name="entity">Entidad a guardar o actualizar</param>
        /// <returns>Task asíncrono</returns>
        Task SaveAsync(T entity);

        /// <summary>
        /// Verifica si una entidad existe
        /// </summary>
        Task<bool> EntityExistsAsync(Guid id);
    }
} 