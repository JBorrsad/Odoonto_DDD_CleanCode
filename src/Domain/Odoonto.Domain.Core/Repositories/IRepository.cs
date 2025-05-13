namespace Odoonto.Domain.Core.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Odoonto.Domain.Core.Models;

public interface IRepository<T> where T : Entity
{
    Task<IEnumerable<T>> GetAll();
    Task<T> GetById(Guid id);
    Task<T> GetByIdOrThrow(Guid id, string errorMessage = null);
    Task<T> GetByIdOrThrow(Guid id, Func<IQueryable<T>, IQueryable<T>> include, string errorMessage = null);
    Task<bool> Exists(Guid id);
    Task Create(T entity);
    Task Update(T entity);
    Task Delete(T entity);
    Task Delete(Guid id);
}