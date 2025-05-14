using Google.Cloud.Firestore;
using Microsoft.Extensions.Logging;
using Odoonto.Domain.Models.Patients;
using Odoonto.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Odoonto.Data.Repositories.Firebase
{
    [FirestoreData]
    public class FirebasePatientRepository : FirebaseBaseRepository<Patient>, IPatientRepository
    {
        private const string COLLECTION_NAME = "patients";

        public FirebasePatientRepository(ILogger<FirebasePatientRepository> logger)
            : base(logger, COLLECTION_NAME)
        {
        }

        public async Task<IReadOnlyList<Patient>> SearchByNameAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            try
            {
                // Convertir el término de búsqueda a minúsculas para búsqueda insensible a mayúsculas
                searchTerm = searchTerm.ToLowerInvariant();

                // Recuperar todos los pacientes y filtrar localmente
                // Nota: Firestore no soporta directamente búsquedas insensibles a mayúsculas/minúsculas o LIKE
                var allPatients = await GetAllAsync();

                var filteredPatients = allPatients
                    .Where(p =>
                        (p.FirstName?.ToLowerInvariant().Contains(searchTerm) ?? false) ||
                        (p.LastName?.ToLowerInvariant().Contains(searchTerm) ?? false) ||
                        (p.ContactInfo?.Email?.ToLowerInvariant().Contains(searchTerm) ?? false) ||
                        (p.ContactInfo?.Phone?.ToLowerInvariant().Contains(searchTerm) ?? false))
                    .ToList()
                    .AsReadOnly();

                _logger.LogInformation($"Búsqueda de pacientes por '{searchTerm}': {filteredPatients.Count} resultados");
                return filteredPatients;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al buscar pacientes por nombre '{searchTerm}'");
                throw;
            }
        }

        public async Task<Patient?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email no puede estar vacío", nameof(email));

            try
            {
                // En una base de datos relacional usaríamos una consulta directa,
                // pero en Firestore tenemos que hacer una consulta más compleja o filtrar localmente
                var allPatients = await GetAllAsync();

                var patient = allPatients
                    .FirstOrDefault(p =>
                        p.ContactInfo?.Email?.Equals(email, StringComparison.OrdinalIgnoreCase) ?? false);

                if (patient != null)
                {
                    _logger.LogInformation($"Paciente encontrado por email '{email}': ID {patient.Id}");
                }
                else
                {
                    _logger.LogInformation($"No se encontró paciente con email '{email}'");
                }

                return patient;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al buscar paciente por email '{email}'");
                throw;
            }
        }

        public async Task<IReadOnlyList<Patient>> GetPatientsWithUpcomingAppointmentsAsync(int daysAhead)
        {
            try
            {
                // Para implementar esto correctamente, necesitaríamos integrar con el repositorio
                // de citas. Por ahora, simplemente devolvemos todos los pacientes.
                _logger.LogWarning("GetPatientsWithUpcomingAppointmentsAsync: No implementado completamente. Devolviendo todos los pacientes.");
                return await GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener pacientes con citas próximas en {daysAhead} días");
                throw;
            }
        }
    }
}