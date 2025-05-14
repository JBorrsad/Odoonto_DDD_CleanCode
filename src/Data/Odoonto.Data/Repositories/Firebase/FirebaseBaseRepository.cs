using Google.Cloud.Firestore;
using Microsoft.Extensions.Logging;
using Odoonto.Domain.Core.Abstractions;
using Odoonto.Domain.Core.Repositories;
using Odoonto.Infrastructure.Configuration.Firebase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Odoonto.Data.Repositories.Firebase
{
    public abstract class FirebaseBaseRepository<T> : IRepository<T> where T : Entity
    {
        protected readonly FirestoreDb _firestoreDb;
        protected readonly ILogger<FirebaseBaseRepository<T>> _logger;
        protected readonly string _collectionName;

        protected FirebaseBaseRepository(ILogger<FirebaseBaseRepository<T>> logger, string collectionName)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _collectionName = string.IsNullOrEmpty(collectionName)
                ? throw new ArgumentNullException(nameof(collectionName))
                : collectionName;

            try
            {
                _firestoreDb = FirebaseConfiguration.Instance.GetFirestoreDb();
                _logger.LogInformation($"Inicializado repositorio Firebase para {typeof(T).Name} con colección '{_collectionName}'");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al inicializar repositorio Firebase para {typeof(T).Name}");
                throw;
            }
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            try
            {
                var docRef = _firestoreDb.Collection(_collectionName).Document(id.ToString());
                var snapshot = await docRef.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    var entity = snapshot.ConvertTo<T>();
                    _logger.LogInformation($"Recuperada entidad {typeof(T).Name} con ID {id}");
                    return entity;
                }

                _logger.LogWarning($"No se encontró entidad {typeof(T).Name} con ID {id}");
                throw new KeyNotFoundException($"No se encontró entidad {typeof(T).Name} con ID {id}");
            }
            catch (Exception ex) when (!(ex is KeyNotFoundException))
            {
                _logger.LogError(ex, $"Error al recuperar entidad {typeof(T).Name} con ID {id}");
                throw;
            }
        }

        public virtual async Task<IReadOnlyList<T>> GetAllAsync()
        {
            try
            {
                var query = _firestoreDb.Collection(_collectionName);
                var snapshot = await query.GetSnapshotAsync();

                var entities = snapshot.Documents
                    .Select(doc => doc.ConvertTo<T>())
                    .ToList()
                    .AsReadOnly();

                _logger.LogInformation($"Recuperadas {entities.Count} entidades de tipo {typeof(T).Name}");
                return entities;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al recuperar todas las entidades de tipo {typeof(T).Name}");
                throw;
            }
        }

        public virtual async Task<Guid> AddAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                // Asegurarnos de que la entidad tiene un ID asignado
                if (entity.Id == Guid.Empty)
                {
                    entity.Id = Guid.NewGuid();
                }

                // Asegurarnos de que las fechas están actualizadas
                var now = DateTime.UtcNow;
                entity.CreatedAt = now;
                entity.UpdatedAt = now;

                // Guardar en Firestore
                var docRef = _firestoreDb.Collection(_collectionName).Document(entity.Id.ToString());
                await docRef.SetAsync(entity);

                _logger.LogInformation($"Añadida entidad {typeof(T).Name} con ID {entity.Id}");
                return entity.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al añadir entidad {typeof(T).Name}");
                throw;
            }
        }

        public virtual async Task UpdateAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity.Id == Guid.Empty)
                throw new ArgumentException("La entidad debe tener un ID válido", nameof(entity));

            try
            {
                // Verificar que la entidad existe
                var docRef = _firestoreDb.Collection(_collectionName).Document(entity.Id.ToString());
                var snapshot = await docRef.GetSnapshotAsync();

                if (!snapshot.Exists)
                {
                    throw new KeyNotFoundException($"No se encontró entidad {typeof(T).Name} con ID {entity.Id} para actualizar");
                }

                // Actualizar fecha de modificación
                entity.UpdatedAt = DateTime.UtcNow;

                // Actualizar en Firestore
                await docRef.SetAsync(entity);

                _logger.LogInformation($"Actualizada entidad {typeof(T).Name} con ID {entity.Id}");
            }
            catch (Exception ex) when (!(ex is KeyNotFoundException))
            {
                _logger.LogError(ex, $"Error al actualizar entidad {typeof(T).Name} con ID {entity.Id}");
                throw;
            }
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                // Verificar que la entidad existe
                var docRef = _firestoreDb.Collection(_collectionName).Document(id.ToString());
                var snapshot = await docRef.GetSnapshotAsync();

                if (!snapshot.Exists)
                {
                    throw new KeyNotFoundException($"No se encontró entidad {typeof(T).Name} con ID {id} para eliminar");
                }

                // Eliminar de Firestore
                await docRef.DeleteAsync();

                _logger.LogInformation($"Eliminada entidad {typeof(T).Name} con ID {id}");
            }
            catch (Exception ex) when (!(ex is KeyNotFoundException))
            {
                _logger.LogError(ex, $"Error al eliminar entidad {typeof(T).Name} con ID {id}");
                throw;
            }
        }

        public virtual async Task<bool> ExistsAsync(Guid id)
        {
            if (id == Guid.Empty)
                return false;

            try
            {
                var docRef = _firestoreDb.Collection(_collectionName).Document(id.ToString());
                var snapshot = await docRef.GetSnapshotAsync();
                return snapshot.Exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al verificar existencia de entidad {typeof(T).Name} con ID {id}");
                return false;
            }
        }

        public virtual void Dispose()
        {
            // Firestore client no necesita ser dispuesto explícitamente
            GC.SuppressFinalize(this);
        }
    }
}