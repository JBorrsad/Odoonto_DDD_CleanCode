using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Odoonto.Data.Core.Contexts;
using Odoonto.Data.Core.Repositories;
using Odoonto.Data.Mappings;
using Odoonto.Domain.Models.Patients;
using Odoonto.Domain.Repositories;

namespace Odoonto.Data.Repositories
{
    /// <summary>
    /// Implementación del repositorio de pacientes utilizando Firebase
    /// </summary>
    public class PatientRepository : BaseRepository<Patient>, IPatientRepository
    {
        public PatientRepository(FirestoreContext context)
            : base(context, "patients")
        {
        }

        protected override Patient ConvertToEntity(DocumentSnapshot document)
        {
            return PatientMapper.ToEntity(document);
        }

        protected override Dictionary<string, object> ConvertFromEntity(Patient entity)
        {
            return PatientMapper.ToFirestore(entity);
        }

        public async Task<int> GetTotalPatientsCountAsync()
        {
            var snapshot = await _context.GetAllDocumentsAsync(_collectionName);
            return snapshot.Count;
        }

        public async Task<IEnumerable<Patient>> GetPaginatedAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                pageNumber = 1;

            if (pageSize < 1)
                pageSize = 10;

            // En Firestore, para implementar paginación necesitamos usar limit() y startAfter()
            // Primero obtenemos los documentos ordenados por nombre
            var query = _context.GetCollection(_collectionName)
                .OrderBy("LastName")
                .OrderBy("FirstName");

            // Si no es la primera página, necesitamos obtener el último documento de la página anterior
            if (pageNumber > 1)
            {
                // Obtener el último documento de la página anterior
                var lastDocOfPreviousPage = await query
                    .Limit((pageNumber - 1) * pageSize)
                    .GetSnapshotAsync();

                if (lastDocOfPreviousPage.Count > 0)
                {
                    var lastDoc = lastDocOfPreviousPage.Documents.Last();
                    query = query.StartAfter(lastDoc);
                }
            }

            // Obtener los documentos de la página actual
            var querySnapshot = await query
                .Limit(pageSize)
                .GetSnapshotAsync();

            return querySnapshot.Documents
                .Select(ConvertToEntity)
                .Where(p => p != null);
        }
    }
}