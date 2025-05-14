using Google.Cloud.Firestore;
using Microsoft.Extensions.Logging;
using Odoonto.Domain.Models.Treatments;
using Odoonto.Domain.Models.ValueObjects;
using Odoonto.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Odoonto.Data.Repositories.Firebase
{
    [FirestoreData]
    public class FirebaseTreatmentRepository : FirebaseBaseRepository<Treatment>, ITreatmentRepository
    {
        private const string COLLECTION_NAME = "treatments";

        public FirebaseTreatmentRepository(ILogger<FirebaseTreatmentRepository> logger)
            : base(logger, COLLECTION_NAME)
        {
        }

        public async Task<IReadOnlyList<Treatment>> SearchByNameAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            try
            {
                // Convertir el término de búsqueda a minúsculas para búsqueda insensible a mayúsculas
                searchTerm = searchTerm.ToLowerInvariant();

                // Recuperar todos los tratamientos y filtrar localmente
                var allTreatments = await GetAllAsync();

                var filteredTreatments = allTreatments
                    .Where(t =>
                        (t.Name?.ToLowerInvariant().Contains(searchTerm) ?? false) ||
                        (t.Description?.ToLowerInvariant().Contains(searchTerm) ?? false))
                    .ToList()
                    .AsReadOnly();

                _logger.LogInformation($"Búsqueda de tratamientos por '{searchTerm}': {filteredTreatments.Count} resultados");
                return filteredTreatments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al buscar tratamientos por nombre '{searchTerm}'");
                throw;
            }
        }

        public async Task<IReadOnlyList<Treatment>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice, string currency = "USD")
        {
            if (minPrice < 0 || maxPrice < 0 || minPrice > maxPrice)
                throw new ArgumentException("El rango de precios es inválido");

            try
            {
                var allTreatments = await GetAllAsync();

                var filteredTreatments = allTreatments
                    .Where(t =>
                        (t.Price?.Amount >= minPrice && t.Price?.Amount <= maxPrice &&
                        (t.Price?.Currency?.Equals(currency, StringComparison.OrdinalIgnoreCase) ?? false)))
                    .OrderBy(t => t.Price?.Amount)
                    .ToList()
                    .AsReadOnly();

                _logger.LogInformation($"Recuperados {filteredTreatments.Count} tratamientos con precio entre {minPrice} y {maxPrice} {currency}");
                return filteredTreatments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al buscar tratamientos con precio entre {minPrice} y {maxPrice} {currency}");
                throw;
            }
        }

        public async Task<IReadOnlyList<Treatment>> GetByDurationAsync(int maxDurationMinutes)
        {
            if (maxDurationMinutes <= 0)
                throw new ArgumentException("La duración máxima debe ser mayor que cero", nameof(maxDurationMinutes));

            try
            {
                var allTreatments = await GetAllAsync();

                var filteredTreatments = allTreatments
                    .Where(t => t.EstimatedDuration <= maxDurationMinutes)
                    .OrderBy(t => t.EstimatedDuration)
                    .ToList()
                    .AsReadOnly();

                _logger.LogInformation($"Recuperados {filteredTreatments.Count} tratamientos con duración menor o igual a {maxDurationMinutes} minutos");
                return filteredTreatments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al buscar tratamientos con duración menor o igual a {maxDurationMinutes} minutos");
                throw;
            }
        }
    }
}