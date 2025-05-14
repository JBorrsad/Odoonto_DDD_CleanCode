using Google.Cloud.Firestore;
using Microsoft.Extensions.Logging;
using Odoonto.Domain.Models.Odontograms;
using Odoonto.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Odoonto.Data.Repositories.Firebase
{
    [FirestoreData]
    public class FirebaseOdontogramRepository : FirebaseBaseRepository<Odontogram>, IOdontogramRepository
    {
        private const string COLLECTION_NAME = "odontograms";

        public FirebaseOdontogramRepository(ILogger<FirebaseOdontogramRepository> logger)
            : base(logger, COLLECTION_NAME)
        {
        }

        public async Task<Odontogram?> GetByPatientIdAsync(Guid patientId)
        {
            if (patientId == Guid.Empty)
                throw new ArgumentException("El ID del paciente no puede estar vacío", nameof(patientId));

            try
            {
                var allOdontograms = await GetAllAsync();

                var odontogram = allOdontograms
                    .FirstOrDefault(o => o.PatientId == patientId);

                if (odontogram != null)
                {
                    _logger.LogInformation($"Odontograma encontrado para paciente con ID {patientId}: ID {odontogram.Id}");
                }
                else
                {
                    _logger.LogInformation($"No se encontró odontograma para paciente con ID {patientId}");
                }

                return odontogram;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al buscar odontograma para paciente con ID {patientId}");
                throw;
            }
        }

        public async Task SaveOdontogramAsync(Odontogram odontogram)
        {
            if (odontogram == null)
                throw new ArgumentNullException(nameof(odontogram));

            try
            {
                // Si el odontograma ya existe, lo actualizamos
                if (await ExistsAsync(odontogram.Id))
                {
                    await UpdateAsync(odontogram);
                }
                // Si no existe, lo añadimos
                else
                {
                    await AddAsync(odontogram);
                }

                _logger.LogInformation($"Odontograma guardado con ID {odontogram.Id} para paciente con ID {odontogram.PatientId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al guardar odontograma con ID {odontogram.Id}");
                throw;
            }
        }

        public async Task AddToothRecordAsync(Guid odontogramId, ToothRecord toothRecord)
        {
            if (odontogramId == Guid.Empty)
                throw new ArgumentException("El ID del odontograma no puede estar vacío", nameof(odontogramId));

            if (toothRecord == null)
                throw new ArgumentNullException(nameof(toothRecord));

            try
            {
                var odontogram = await GetByIdAsync(odontogramId);

                odontogram.AddToothRecord(toothRecord);
                await UpdateAsync(odontogram);

                _logger.LogInformation($"Registro dental añadido para diente {toothRecord.ToothNumber} en odontograma con ID {odontogramId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al añadir registro dental para diente {toothRecord?.ToothNumber} en odontograma con ID {odontogramId}");
                throw;
            }
        }

        public async Task AddLesionRecordAsync(Guid odontogramId, int toothNumber, LesionRecord lesionRecord)
        {
            if (odontogramId == Guid.Empty)
                throw new ArgumentException("El ID del odontograma no puede estar vacío", nameof(odontogramId));

            if (lesionRecord == null)
                throw new ArgumentNullException(nameof(lesionRecord));

            try
            {
                var odontogram = await GetByIdAsync(odontogramId);

                odontogram.AddLesionRecord(toothNumber, lesionRecord);
                await UpdateAsync(odontogram);

                _logger.LogInformation($"Registro de lesión añadido para diente {toothNumber} en odontograma con ID {odontogramId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al añadir registro de lesión para diente {toothNumber} en odontograma con ID {odontogramId}");
                throw;
            }
        }

        public async Task AddPerformedProcedureAsync(Guid odontogramId, int toothNumber, PerformedProcedure procedure)
        {
            if (odontogramId == Guid.Empty)
                throw new ArgumentException("El ID del odontograma no puede estar vacío", nameof(odontogramId));

            if (procedure == null)
                throw new ArgumentNullException(nameof(procedure));

            try
            {
                var odontogram = await GetByIdAsync(odontogramId);

                odontogram.AddPerformedProcedure(toothNumber, procedure);
                await UpdateAsync(odontogram);

                _logger.LogInformation($"Procedimiento realizado añadido para diente {toothNumber} en odontograma con ID {odontogramId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al añadir procedimiento realizado para diente {toothNumber} en odontograma con ID {odontogramId}");
                throw;
            }
        }
    }
}