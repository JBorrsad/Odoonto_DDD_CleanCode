// EJEMPLO DE INTERFAZ DE REPOSITORIO BASE (Domain Core Layer)
// Ruta: src/Domain/TuProyecto.Domain.Core/Repositories/IRepository.cs

namespace TuProyecto.Domain.Core.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TuProyecto.Domain.Core.Models;

/// <summary>
/// Características clave de una interfaz de repositorio genérico en DDD:
/// 1. Define operaciones CRUD comunes para todas las entidades
/// 2. Es genérica para ser reutilizable con cualquier entidad
/// 3. Incluye métodos de consulta y manipulación
/// 4. Define métodos asíncronos (Task)
/// 5. Permite filtrado y modificación de consultas
/// </summary>
public interface IRepository<T> where T : Entity
{
    // Métodos de consulta básicos
    Task<T> GetById(Guid id);
    Task<T> GetByIdOrThrow(Guid id);
    Task<IEnumerable<T>> GetAll();

    // Método de consulta con modificación de query
    Task<T> GetByIdOrThrow(Guid id, Func<IQueryable<T>, IQueryable<T>> includeFunc);
    Task<IEnumerable<T>> GetAll(Func<IQueryable<T>, IQueryable<T>> includeFunc);

    // Métodos para filtrar entidades
    Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
    Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate);
    Task<bool> Any(Expression<Func<T, bool>> predicate);

    // Métodos para manipulación de entidades
    Task Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<bool> Exists(Guid id);

    // Métodos de guardado
    Task SaveChanges();
}