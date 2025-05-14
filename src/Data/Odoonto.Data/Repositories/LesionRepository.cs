using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Odoonto.Data.Core.Contexts;
using Odoonto.Data.Core.Repositories;
using Odoonto.Data.Mappings;
using Odoonto.Domain.Models.Lesions;
using Odoonto.Domain.Repositories;

namespace Odoonto.Data.Repositories
{
    /// <summary>
    /// Implementación del repositorio de lesiones utilizando Firebase
    /// </summary>
    public class LesionRepository : BaseRepository<Lesion>, ILesionRepository
    {
        public LesionRepository(FirestoreContext context)
            : base(context, "lesions")
        {
        }

        protected override Lesion ConvertToEntity(DocumentSnapshot document)
        {
            return LesionMapper.ToEntity(document);
        }

        protected override Dictionary<string, object> ConvertFromEntity(Lesion entity)
        {
            return LesionMapper.ToFirestore(entity);
        }

        public async Task<IEnumerable<Lesion>> GetByCategoryAsync(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                return Enumerable.Empty<Lesion>();

            category = category.Trim();

            // Crear una consulta con filtro por categoría
            var query = _context.GetCollection(_collectionName)
                .WhereEqualTo("Category", category);

            var querySnapshot = await _context.QueryDocumentsAsync(query);

            return querySnapshot
                .Select(ConvertToEntity)
                .Where(l => l != null);
        }

        public async Task<IEnumerable<Lesion>> GetActiveAsync()
        {
            // Crear una consulta que filtre lesiones activas
            var query = _context.GetCollection(_collectionName)
                .WhereEqualTo("IsActive", true);

            var querySnapshot = await _context.QueryDocumentsAsync(query);

            return querySnapshot
                .Select(ConvertToEntity)
                .Where(l => l != null);
        }

        public async Task<IEnumerable<Lesion>> GetInactiveAsync()
        {
            // Crear una consulta que filtre lesiones inactivas
            var query = _context.GetCollection(_collectionName)
                .WhereEqualTo("IsActive", false);

            var querySnapshot = await _context.QueryDocumentsAsync(query);

            return querySnapshot
                .Select(ConvertToEntity)
                .Where(l => l != null);
        }

        public async Task<IEnumerable<Lesion>> SearchByNameOrDescriptionAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return Enumerable.Empty<Lesion>();

            searchTerm = searchTerm.ToLower().Trim();

            // En Firestore, debemos buscar en memoria para búsquedas de texto parcial
            var lesions = await GetAllAsync();

            return lesions.Where(l =>
                l.Name.ToLower().Contains(searchTerm) ||
                l.Description.ToLower().Contains(searchTerm));
        }

        public async Task<bool> ActivateAsync(Guid id)
        {
            var lesion = await GetByIdAsync(id);
            if (lesion == null)
                return false;

            lesion.Activate();
            await UpdateAsync(lesion);
            return true;
        }

        public async Task<bool> DeactivateAsync(Guid id)
        {
            var lesion = await GetByIdAsync(id);
            if (lesion == null)
                return false;

            lesion.Deactivate();
            await UpdateAsync(lesion);
            return true;
        }
    }
}