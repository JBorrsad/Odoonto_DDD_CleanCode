using Google.Cloud.Firestore;
using Microsoft.Extensions.Logging;
using Odoonto.Domain.Models.Doctors;
using Odoonto.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Odoonto.Data.Repositories.Firebase
{
    [FirestoreData]
    public class FirebaseDoctorRepository : FirebaseBaseRepository<Doctor>, IDoctorRepository
    {
        private const string COLLECTION_NAME = "doctors";

        public FirebaseDoctorRepository(ILogger<FirebaseDoctorRepository> logger)
            : base(logger, COLLECTION_NAME)
        {
        }

        public async Task<IReadOnlyList<Doctor>> SearchByNameAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            try
            {
                // Convertir el término de búsqueda a minúsculas para búsqueda insensible a mayúsculas
                searchTerm = searchTerm.ToLowerInvariant();

                // Recuperar todos los doctores y filtrar localmente
                var allDoctors = await GetAllAsync();

                var filteredDoctors = allDoctors
                    .Where(d =>
                        (d.FirstName?.ToLowerInvariant().Contains(searchTerm) ?? false) ||
                        (d.LastName?.ToLowerInvariant().Contains(searchTerm) ?? false) ||
                        (d.Specialization?.ToLowerInvariant().Contains(searchTerm) ?? false) ||
                        (d.ContactInfo?.Email?.ToLowerInvariant().Contains(searchTerm) ?? false))
                    .ToList()
                    .AsReadOnly();

                _logger.LogInformation($"Búsqueda de doctores por '{searchTerm}': {filteredDoctors.Count} resultados");
                return filteredDoctors;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al buscar doctores por nombre '{searchTerm}'");
                throw;
            }
        }

        public async Task<Doctor?> GetByLicenseNumberAsync(string licenseNumber)
        {
            if (string.IsNullOrWhiteSpace(licenseNumber))
                throw new ArgumentException("El número de licencia no puede estar vacío", nameof(licenseNumber));

            try
            {
                var allDoctors = await GetAllAsync();

                var doctor = allDoctors
                    .FirstOrDefault(d =>
                        d.LicenseNumber?.Equals(licenseNumber, StringComparison.OrdinalIgnoreCase) ?? false);

                if (doctor != null)
                {
                    _logger.LogInformation($"Doctor encontrado por licencia '{licenseNumber}': ID {doctor.Id}");
                }
                else
                {
                    _logger.LogInformation($"No se encontró doctor con licencia '{licenseNumber}'");
                }

                return doctor;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al buscar doctor por licencia '{licenseNumber}'");
                throw;
            }
        }

        public async Task<IReadOnlyList<Doctor>> GetBySpecializationAsync(string specialization)
        {
            if (string.IsNullOrWhiteSpace(specialization))
                return await GetAllAsync();

            try
            {
                // Convertir la especialización a minúsculas para búsqueda insensible a mayúsculas
                specialization = specialization.ToLowerInvariant();

                var allDoctors = await GetAllAsync();

                var filteredDoctors = allDoctors
                    .Where(d =>
                        (d.Specialization?.ToLowerInvariant().Contains(specialization) ?? false))
                    .ToList()
                    .AsReadOnly();

                _logger.LogInformation($"Búsqueda de doctores por especialización '{specialization}': {filteredDoctors.Count} resultados");
                return filteredDoctors;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al buscar doctores por especialización '{specialization}'");
                throw;
            }
        }
    }
}