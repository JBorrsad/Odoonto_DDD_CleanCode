using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Odoonto.Data.Core.Contexts;
using Odoonto.Data.Core.Repositories;
using Odoonto.Data.Mappings;
using Odoonto.Domain.Models.Odontograms;
using Odoonto.Domain.Repositories;

namespace Odoonto.Data.Repositories
{
    /// <summary>
    /// Implementación del repositorio de odontogramas utilizando Firebase
    /// </summary>
    public class OdontogramRepository : BaseRepository<Odontogram>, IOdontogramRepository
    {
        public OdontogramRepository(FirestoreContext context)
            : base(context, "odontograms")
        {
        }

        protected override Odontogram ConvertToEntity(DocumentSnapshot document)
        {
            return OdontogramMapper.ToEntity(document);
        }

        protected override Dictionary<string, object> ConvertFromEntity(Odontogram entity)
        {
            return OdontogramMapper.ToFirestore(entity);
        }

        public async Task<Odontogram> GetByPatientIdAsync(Guid patientId)
        {
            var query = _context.GetCollection(_collectionName)
                .WhereEqualTo("PatientId", patientId.ToString());

            var snapshot = await query.GetSnapshotAsync();

            return snapshot.Documents.Select(ConvertToEntity).FirstOrDefault();
        }

        public async Task<Guid> SaveOdontogramAsync(Odontogram odontogram)
        {
            if (odontogram == null)
                throw new ArgumentNullException(nameof(odontogram));

            var data = ConvertFromEntity(odontogram);

            await _context.SetDocumentAsync(
                _collectionName,
                odontogram.Id.ToString(),
                data);

            return odontogram.Id;
        }

        public async Task UpdateToothRecordAsync(Guid odontogramId, ToothRecord toothRecord)
        {
            if (toothRecord == null)
                throw new ArgumentNullException(nameof(toothRecord));

            // Obtener el odontograma
            var odontogram = await GetByIdAsync(odontogramId);
            if (odontogram == null)
                throw new KeyNotFoundException($"No se encontró el odontograma con ID {odontogramId}");

            // Actualizar el registro dental
            odontogram.UpdateToothRecord(toothRecord);

            // Guardar el odontograma actualizado
            await UpdateAsync(odontogram);
        }

        public async Task AddLesionRecordAsync(Guid odontogramId, int toothNumber, LesionRecord lesionRecord)
        {
            if (lesionRecord == null)
                throw new ArgumentNullException(nameof(lesionRecord));

            // Obtener el odontograma
            var odontogram = await GetByIdAsync(odontogramId);
            if (odontogram == null)
                throw new KeyNotFoundException($"No se encontró el odontograma con ID {odontogramId}");

            // Obtener el registro dental correspondiente
            var toothRecord = odontogram.GetToothRecord(toothNumber);
            if (toothRecord == null)
            {
                // Si no existe el registro dental, lo creamos
                toothRecord = new ToothRecord(toothNumber);
                odontogram.AddToothRecord(toothRecord);
            }

            // Añadir el registro de lesión
            toothRecord.AddLesionRecord(lesionRecord);

            // Actualizar el registro dental en el odontograma
            odontogram.UpdateToothRecord(toothRecord);

            // Guardar el odontograma actualizado
            await UpdateAsync(odontogram);
        }

        public async Task AddPerformedProcedureAsync(Guid odontogramId, int toothNumber, PerformedProcedure performedProcedure)
        {
            if (performedProcedure == null)
                throw new ArgumentNullException(nameof(performedProcedure));

            // Obtener el odontograma
            var odontogram = await GetByIdAsync(odontogramId);
            if (odontogram == null)
                throw new KeyNotFoundException($"No se encontró el odontograma con ID {odontogramId}");

            // Obtener el registro dental correspondiente
            var toothRecord = odontogram.GetToothRecord(toothNumber);
            if (toothRecord == null)
            {
                // Si no existe el registro dental, lo creamos
                toothRecord = new ToothRecord(toothNumber);
                odontogram.AddToothRecord(toothRecord);
            }

            // Añadir el procedimiento realizado
            toothRecord.AddPerformedProcedure(performedProcedure);

            // Actualizar el registro dental en el odontograma
            odontogram.UpdateToothRecord(toothRecord);

            // Guardar el odontograma actualizado
            await UpdateAsync(odontogram);
        }
    }
}