using Google.Cloud.Firestore;
using Microsoft.Extensions.Logging;
using Odoonto.Domain.Models.Lesions;
using Odoonto.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Odoonto.Data.Repositories.Firebase
{
    [FirestoreData]
    public class FirebaseLesionRepository : FirebaseBaseRepository<Lesion>, ILesionRepository
    {
        private const string COLLECTION_NAME = "lesions";

        public FirebaseLesionRepository(ILogger<FirebaseLesionRepository> logger)
            : base(logger, COLLECTION_NAME)
        {
        }

        public async Task<IReadOnlyList<Lesion>> SearchByNameAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            try
            {
                // Convertir el término de búsqueda a minúsculas para búsqueda insensible a mayúsculas
                searchTerm = searchTerm.ToLowerInvariant();

                // Recuperar todas las lesiones y filtrar localmente
                var allLesions = await GetAllAsync();

                var filteredLesions = allLesions
                    .Where(l =>
                        (l.Name?.ToLowerInvariant().Contains(searchTerm) ?? false) ||
                        (l.Description?.ToLowerInvariant().Contains(searchTerm) ?? false))
                    .ToList()
                    .AsReadOnly();

                _logger.LogInformation($"Búsqueda de lesiones por '{searchTerm}': {filteredLesions.Count} resultados");
                return filteredLesions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al buscar lesiones por nombre '{searchTerm}'");
                throw;
            }
        }

        public async Task<IReadOnlyList<Lesion>> GetBySeverityAsync(LesionSeverity severity)
        {
            try
            {
                var allLesions = await GetAllAsync();

                var filteredLesions = allLesions
                    .Where(l => l.Severity == severity)
                    .ToList()
                    .AsReadOnly();

                _logger.LogInformation($"Recuperadas {filteredLesions.Count} lesiones con severidad {severity}");
                return filteredLesions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al buscar lesiones con severidad {severity}");
                throw;
            }
        }
    }
}