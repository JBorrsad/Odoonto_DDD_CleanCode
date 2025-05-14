using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Odoonto.Data.Core.Contexts;
using Odoonto.Data.Core.Repositories;
using Odoonto.Data.Mappings;
using Odoonto.Domain.Models.Treatments;
using Odoonto.Domain.Models.ValueObjects;
using Odoonto.Domain.Repositories;

namespace Odoonto.Data.Repositories
{
    /// <summary>
    /// Implementación del repositorio de tratamientos utilizando Firebase
    /// </summary>
    public class TreatmentRepository : BaseRepository<Treatment>, ITreatmentRepository
    {
        public TreatmentRepository(FirestoreContext context)
            : base(context, "treatments")
        {
        }

        protected override Treatment ConvertToEntity(DocumentSnapshot document)
        {
            return TreatmentMapper.ToEntity(document);
        }

        protected override Dictionary<string, object> ConvertFromEntity(Treatment entity)
        {
            return TreatmentMapper.ToFirestore(entity);
        }

        public async Task<IEnumerable<Treatment>> GetByCategoryAsync(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                return Enumerable.Empty<Treatment>();

            category = category.Trim();

            // Crear una consulta con filtro por categoría
            var query = _context.GetCollection(_collectionName)
                .WhereEqualTo("Category", category);

            var querySnapshot = await _context.QueryDocumentsAsync(query);

            return querySnapshot
                .Select(ConvertToEntity)
                .Where(t => t != null);
        }

        public async Task<IEnumerable<Treatment>> GetByMaxPriceAsync(decimal maxPrice, string currency = "EUR")
        {
            if (maxPrice <= 0)
                return Enumerable.Empty<Treatment>();

            // En Firestore, no podemos hacer consultas complejas de manera eficiente con estructuras anidadas como Price
            // Por lo tanto, recuperamos todos los tratamientos y filtramos en memoria
            var treatments = await GetAllAsync();

            return treatments.Where(t =>
                t.Price != null &&
                t.Price.Amount <= maxPrice &&
                string.Equals(t.Price.Currency, currency, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<IEnumerable<Treatment>> GetByMaxDurationAsync(int maxDurationMinutes)
        {
            if (maxDurationMinutes <= 0)
                return Enumerable.Empty<Treatment>();

            // Crear una consulta que filtre por duración
            var query = _context.GetCollection(_collectionName)
                .WhereLessThanOrEqualTo("DurationMinutes", maxDurationMinutes);

            var querySnapshot = await _context.QueryDocumentsAsync(query);

            return querySnapshot
                .Select(ConvertToEntity)
                .Where(t => t != null);
        }

        public async Task<IEnumerable<Treatment>> SearchByNameOrDescriptionAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return Enumerable.Empty<Treatment>();

            searchTerm = searchTerm.ToLower().Trim();

            // En Firestore, debemos buscar en memoria para búsquedas de texto parcial
            var treatments = await GetAllAsync();

            return treatments.Where(t =>
                t.Name.ToLower().Contains(searchTerm) ||
                t.Description.ToLower().Contains(searchTerm));
        }
    }
}