using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Odoonto.Domain.Core.Abstractions;
using Odoonto.Domain.Core.Specifications;

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
        /// Busca entidades que cumplan una especificación
        /// </summary>
        /// <param name="spec">Especificación a aplicar</param>
        /// <param name="page">Número de página (1-based)</param>
        /// <param name="pageSize">Tamaño de página</param>
        /// <returns>Colección de entidades que cumplen la especificación</returns>
        Task<IEnumerable<T>> FindAsync(ISpecification<T> spec, int page = 1, int pageSize = 10);

        /// <summary>
        /// Cuenta el número de entidades que cumplen una especificación
        /// </summary>
        /// <param name="spec">Especificación a aplicar</param>
        /// <returns>Número de entidades</returns>
        Task<int> CountAsync(ISpecification<T> spec);

        /// <summary>
        /// Obtiene la primera entidad que cumple una especificación o devuelve null
        /// </summary>
        /// <param name="spec">Especificación a aplicar</param>
        /// <returns>La primera entidad que cumple la especificación o null</returns>
        Task<T> FirstOrDefaultAsync(ISpecification<T> spec);

        /// <summary>
        /// Verifica si existe alguna entidad que cumpla una especificación
        /// </summary>
        /// <param name="spec">Especificación a aplicar</param>
        /// <returns>True si existe al menos una entidad, False si no existe ninguna</returns>
        Task<bool> AnyAsync(ISpecification<T> spec);

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
        /// Guarda o actualiza una entidad
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