using Google.Cloud.Firestore;
using Odoonto.Data.Core.Contexts;
using Odoonto.Domain.Core.Abstractions;
using Odoonto.Domain.Core.Models.Exceptions;
using Odoonto.Domain.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Odoonto.Data.Core.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : Entity
    {
        protected readonly FirestoreContext _context;
        protected readonly string _collectionName;

        protected Repository(FirestoreContext context, string collectionName)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _collectionName = collectionName ?? throw new ArgumentNullException(nameof(collectionName));
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            var documents = await _context.GetAllDocumentsAsync(_collectionName);
            return documents.Select(MapToEntity);
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            var document = await _context.GetDocumentByIdAsync(_collectionName, id.ToString());
            return document.Exists ? MapToEntity(document) : default;
        }

        public virtual async Task<T> GetByIdOrThrowAsync(Guid id, string errorMessage = null)
        {
            var entity = await GetByIdAsync(id);
            
            if (entity == null)
            {
                throw new ValueNotFoundException(errorMessage ?? $"La entidad {typeof(T).Name} con ID {id} no fue encontrada.");
            }
            
            return entity;
        }

        public virtual async Task<bool> ExistsAsync(Guid id)
        {
            var document = await _context.GetDocumentByIdAsync(_collectionName, id.ToString());
            return document.Exists;
        }

        public virtual async Task<bool> EntityExistsAsync(Guid id)
        {
            return await ExistsAsync(id);
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            // Firebase no soporta directamente expresiones LINQ complejas,
            // así que tenemos que obtener todos los documentos y filtrar en memoria
            var allEntities = await GetAllAsync();
            var compiled = predicate.Compile();
            return allEntities.Where(compiled);
        }

        public virtual async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            var entities = await FindAsync(predicate);
            return entities.FirstOrDefault();
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            var entity = await FirstOrDefaultAsync(predicate);
            return entity != null;
        }

        public virtual async Task<Guid> AddAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SetDocumentAsync(_collectionName, entity.Id.ToString(), MapToDocument(entity));
            
            return entity.Id;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (entity.Id == Guid.Empty)
            {
                throw new ArgumentException("La entidad no tiene un ID válido.", nameof(entity));
            }

            var exists = await ExistsAsync(entity.Id);
            
            if (!exists)
            {
                throw new ValueNotFoundException($"La entidad {typeof(T).Name} con ID {entity.Id} no fue encontrada.");
            }

            entity.UpdatedAt = DateTime.UtcNow;
            
            await _context.SetDocumentAsync(_collectionName, entity.Id.ToString(), MapToDocument(entity));
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            var exists = await ExistsAsync(id);
            
            if (!exists)
            {
                return false;
            }
            
            await _context.DeleteDocumentAsync(_collectionName, id.ToString());
            return true;
        }

        public virtual async Task SaveAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (entity.Id == Guid.Empty || !await ExistsAsync(entity.Id))
            {
                await AddAsync(entity);
            }
            else
            {
                await UpdateAsync(entity);
            }
        }

        protected abstract T MapToEntity(DocumentSnapshot document);
        
        protected abstract object MapToDocument(T entity);
    }
} 