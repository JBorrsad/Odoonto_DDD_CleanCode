// EJEMPLO DE IMPLEMENTACIÓN DE REPOSITORIO BASE (Data Core Layer)
// Ruta: src/Data/TuProyecto.Data.Core/Repositories/Repository.cs

namespace TuProyecto.Data.Core.Repositories;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TuProyecto.Data.Core.Contexts;
using TuProyecto.Domain.Core.Models;
using TuProyecto.Domain.Core.Models.Exceptions;
using TuProyecto.Domain.Core.Repositories;

/// <summary>
/// Características clave de una implementación base de repositorio en DDD:
/// 1. Implementa la interfaz genérica IRepository<T>
/// 2. Proporciona implementaciones predeterminadas para operaciones comunes
/// 3. Utiliza Entity Framework Core para acceso a datos
/// 4. Puede ser extendida por repositorios específicos
/// 5. Maneja excepciones y casos de error comunes
/// </summary>
public class Repository<T> : IRepository<T> where T : Entity
{
    protected readonly ApplicationContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(ApplicationContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    // Implementación de métodos de consulta
    public virtual async Task<T> GetById(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<T> GetByIdOrThrow(Guid id)
    {
        T entity = await GetById(id);

        if (entity is null)
        {
            throw new ValueNotFoundException($"The {typeof(T).Name} (Id: {id}) not found.");
        }

        return entity;
    }

    public virtual async Task<IEnumerable<T>> GetAll()
    {
        return await _dbSet.ToListAsync();
    }

    // Implementación de métodos con modificación de consulta
    public virtual async Task<T> GetByIdOrThrow(Guid id, Func<IQueryable<T>, IQueryable<T>> includeFunc)
    {
        IQueryable<T> query = _dbSet;

        if (includeFunc != null)
        {
            query = includeFunc(query);
        }

        T entity = await query.FirstOrDefaultAsync(e => e.Id == id);

        if (entity is null)
        {
            throw new ValueNotFoundException($"The {typeof(T).Name} (Id: {id}) not found.");
        }

        return entity;
    }

    public virtual async Task<IEnumerable<T>> GetAll(Func<IQueryable<T>, IQueryable<T>> includeFunc)
    {
        IQueryable<T> query = _dbSet;

        if (includeFunc != null)
        {
            query = includeFunc(query);
        }

        return await query.ToListAsync();
    }

    // Implementación de métodos de filtrado
    public virtual async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public virtual async Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    public virtual async Task<bool> Any(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    // Implementación de métodos de manipulación
    public virtual async Task Create(T entity)
    {
        await _dbSet.AddAsync(entity);
        await SaveChanges();
    }

    public virtual void Update(T entity)
    {
        _dbSet.Update(entity);
        _context.Entry(entity).State = EntityState.Modified;
        SaveChanges().Wait();
    }

    public virtual void Delete(T entity)
    {
        _dbSet.Remove(entity);
        SaveChanges().Wait();
    }

    public virtual async Task<bool> Exists(Guid id)
    {
        return await _dbSet.AnyAsync(e => e.Id == id);
    }

    // Implementación de guardado de cambios
    public virtual async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}