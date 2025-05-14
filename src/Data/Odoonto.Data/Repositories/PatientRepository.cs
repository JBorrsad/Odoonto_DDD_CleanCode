using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Odoonto.Data.Core.Contexts;
using Odoonto.Data.Core.Repositories;
using Odoonto.Data.Mappings;
using Odoonto.Domain.Core.Specifications;
using Odoonto.Domain.Models.Patients;
using Odoonto.Domain.Repositories;
using Odoonto.Domain.Specifications.Patients;

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
            return await GetPaginatedAsync(pageNumber, pageSize, null);
        }
        
        /// <summary>
        /// Obtiene pacientes paginados con un token de paginación
        /// </summary>
        public async Task<(IEnumerable<Patient> Items, string NextPageToken)> GetPaginatedWithTokenAsync(
            int pageSize = 20, 
            string pageToken = null,
            ISpecification<Patient> specification = null)
        {
            if (pageSize < 1)
                pageSize = 20;
                
            // Limitamos a un máximo de 200 elementos por consulta para evitar problemas de rendimiento
            pageSize = Math.Min(pageSize, 200);

            // Construir la consulta base ordenada
            var query = _context.GetCollection(_collectionName)
                .OrderBy("LastName")
                .OrderBy("FirstName")
                .Limit(pageSize);

            // Si tenemos un token de paginación, lo aplicamos
            if (!string.IsNullOrEmpty(pageToken))
            {
                var startAfterDoc = await _context.GetDocumentByIdAsync(_collectionName, pageToken);
                if (startAfterDoc.Exists)
                {
                    query = query.StartAfter(startAfterDoc);
                }
            }
            
            // Aplicar filtros adicionales si hay especificación
            if (specification is PatientByNameSpecification nameSpec)
            {
                // Firestore no soporta directamente búsqueda LIKE o contains(), así que hacemos
                // una aproximación con filtros básicos y luego refinamos en memoria
                string normalizedName = nameSpec.SearchTerm.ToLower();
                
                // Aplicamos filtros que Firestore permita
                if (normalizedName.Length > 0)
                {
                    // Por ejemplo, filtrar por inicial si es posible
                    var initialChar = normalizedName[0].ToString();
                    query = query.WhereGreaterThanOrEqualTo("FirstName", initialChar)
                                 .WhereLessThanOrEqualTo("FirstName", initialChar + '\uf8ff');
                    
                    // Nota: Esto es una aproximación y luego aplicaremos el filtro específico en memoria
                }
            }
            else if (specification is PatientByEmailSpecification emailSpec)
            {
                query = query.WhereEqualTo("ContactInfo.Email", emailSpec.Email);
            }
            else if (specification is PatientByPhoneSpecification phoneSpec)
            {
                query = query.WhereEqualTo("ContactInfo.Phone", phoneSpec.Phone);
            }
            // Nota: PatientByAgeRangeSpecification no puede aplicarse directamente en Firestore
            // ya que requiere un cálculo en base a la fecha de nacimiento
            
            // Ejecutar la consulta
            var querySnapshot = await query.GetSnapshotAsync();
            
            if (querySnapshot.Count == 0)
            {
                return (Enumerable.Empty<Patient>(), null);
            }
            
            // Convertir los resultados a entidades y aplicar filtros adicionales en memoria
            var items = querySnapshot.Documents
                .Select(ConvertToEntity)
                .Where(p => p != null)
                .ToList();
                
            // Aplicar filtrado adicional en memoria para especificaciones que no se pueden aplicar en Firestore
            if (specification != null)
            {
                items = items.Where(specification.IsSatisfiedBy).ToList();
            }
            
            // Determinar el token para la siguiente página
            string nextPageToken = items.Count < pageSize 
                ? null 
                : querySnapshot.Documents.Last().Id;
                
            return (items, nextPageToken);
        }
        
        /// <summary>
        /// Implementación tradicional de paginación para compatibilidad
        /// </summary>
        private async Task<IEnumerable<Patient>> GetPaginatedAsync(
            int pageNumber, 
            int pageSize, 
            ISpecification<Patient> specification = null)
        {
            if (pageNumber < 1)
                pageNumber = 1;

            if (pageSize < 1)
                pageSize = 10;
                
            // Limitar el tamaño de página a un máximo razonable
            pageSize = Math.Min(pageSize, 100);

            // Construcción de la query base
            var query = _context.GetCollection(_collectionName)
                .OrderBy("LastName")
                .OrderBy("FirstName");

            // Si no es la primera página, necesitamos obtener el último documento de la página anterior
            if (pageNumber > 1)
            {
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

            var items = querySnapshot.Documents
                .Select(ConvertToEntity)
                .Where(p => p != null)
                .ToList();
                
            // Aplicar filtrado adicional en memoria si es necesario
            if (specification != null)
            {
                items = items.Where(specification.IsSatisfiedBy).ToList();
            }
            
            return items;
        }
    }
}