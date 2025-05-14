using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Odoonto.Data.Core.Contexts;
using Odoonto.Domain.Core.Abstractions;
using Odoonto.Domain.Core.Models.Exceptions;
using Odoonto.Domain.Core.Repositories;
using Odoonto.Domain.Core.Specifications;

namespace Odoonto.Data.Core.Repositories
{
    /// <summary>
    /// Implementación base para repositorios que utilizan Firebase
    /// </summary>
    /// <typeparam name="T">Tipo de entidad</typeparam>
    public abstract class BaseRepository<T> : IRepository<T> where T : Entity
    {
        protected readonly FirestoreContext _context;
        protected readonly string _collectionName;

        protected BaseRepository(FirestoreContext context, string collectionName)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _collectionName = collectionName ?? throw new ArgumentNullException(nameof(collectionName));
        }

        /// <summary>
        /// Método para convertir un DocumentSnapshot a una entidad
        /// </summary>
        protected abstract T ConvertToEntity(DocumentSnapshot document);

        /// <summary>
        /// Método para convertir una entidad a un Dictionary para Firebase
        /// </summary>
        protected abstract Dictionary<string, object> ConvertFromEntity(T entity);

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            var snapshot = await _context.GetAllDocumentsAsync(_collectionName);
            return snapshot.Select(ConvertToEntity).Where(entity => entity != null);
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                return null;

            var document = await _context.GetDocumentByIdAsync(_collectionName, id.ToString());
            return document.Exists ? ConvertToEntity(document) : null;
        }

        public virtual async Task<T> GetByIdOrThrowAsync(Guid id, string errorMessage = null)
        {
            var entity = await GetByIdAsync(id);

            if (entity == null)
            {
                var message = errorMessage ?? $"Entidad {typeof(T).Name} con ID {id} no encontrada.";
                throw new EntityNotFoundException(message);
            }

            return entity;
        }

        public virtual async Task<bool> ExistsAsync(Guid id)
        {
            if (id == Guid.Empty)
                return false;

            var document = await _context.GetDocumentByIdAsync(_collectionName, id.ToString());
            return document.Exists;
        }

        public virtual async Task<Guid> AddAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity.Id == Guid.Empty)
                throw new ArgumentException("La entidad debe tener un ID asignado.");

            var data = ConvertFromEntity(entity);
            await _context.SetDocumentAsync(_collectionName, entity.Id.ToString(), data);
            return entity.Id;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity.Id == Guid.Empty)
                throw new ArgumentException("La entidad debe tener un ID asignado.");

            var data = ConvertFromEntity(entity);
            await _context.SetDocumentAsync(_collectionName, entity.Id.ToString(), data);
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                return false;

            if (!await ExistsAsync(id))
                return false;

            await _context.DeleteDocumentAsync(_collectionName, id.ToString());
            return true;
        }

        public virtual async Task SaveAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity.Id == Guid.Empty)
                throw new ArgumentException("La entidad debe tener un ID asignado.");

            var exists = await ExistsAsync(entity.Id);

            if (exists)
                await UpdateAsync(entity);
            else
                await AddAsync(entity);
        }

        public virtual async Task<bool> EntityExistsAsync(Guid id)
        {
            return await ExistsAsync(id);
        }

        public virtual async Task<IEnumerable<T>> FindAsync(ISpecification<T> spec, int page = 1, int pageSize = 10)
        {
            if (spec == null)
                throw new ArgumentNullException(nameof(spec));

            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            // En Firestore, debemos obtener todos los documentos y filtrar en memoria
            var allEntities = await GetAllAsync();
            var filteredEntities = allEntities.Where(e => spec.IsSatisfiedBy(e));

            // Aplicamos paginación en memoria
            return filteredEntities
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public virtual async Task<int> CountAsync(ISpecification<T> spec)
        {
            if (spec == null)
                throw new ArgumentNullException(nameof(spec));

            var allEntities = await GetAllAsync();
            return allEntities.Count(e => spec.IsSatisfiedBy(e));
        }

        public virtual async Task<T> FirstOrDefaultAsync(ISpecification<T> spec)
        {
            if (spec == null)
                throw new ArgumentNullException(nameof(spec));

            var allEntities = await GetAllAsync();
            return allEntities.FirstOrDefault(e => spec.IsSatisfiedBy(e));
        }

        public virtual async Task<bool> AnyAsync(ISpecification<T> spec)
        {
            if (spec == null)
                throw new ArgumentNullException(nameof(spec));

            var allEntities = await GetAllAsync();
            return allEntities.Any(e => spec.IsSatisfiedBy(e));
        }
    }
}