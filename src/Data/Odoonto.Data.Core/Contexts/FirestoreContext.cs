using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Odoonto.Data.Core.Contexts
{
    public class FirestoreContext
    {
        private readonly FirestoreDb _firestoreDb;

        public FirestoreContext(FirestoreDb firestoreDb)
        {
            _firestoreDb = firestoreDb ?? throw new ArgumentNullException(nameof(firestoreDb));
        }

        /// <summary>
        /// Obtiene una referencia a una colección de Firestore
        /// </summary>
        public CollectionReference GetCollection(string collectionName)
        {
            return _firestoreDb.Collection(collectionName);
        }

        /// <summary>
        /// Obtiene un documento por su ID
        /// </summary>
        public async Task<DocumentSnapshot> GetDocumentByIdAsync(string collectionName, string documentId)
        {
            var docRef = _firestoreDb.Collection(collectionName).Document(documentId);
            return await docRef.GetSnapshotAsync();
        }

        /// <summary>
        /// Crea un nuevo documento con ID generado
        /// </summary>
        public async Task<DocumentReference> AddDocumentAsync(string collectionName, object data)
        {
            return await _firestoreDb.Collection(collectionName).AddAsync(data);
        }

        /// <summary>
        /// Crea o actualiza un documento con ID específico
        /// </summary>
        public async Task SetDocumentAsync(string collectionName, string documentId, object data, SetOptions options = null)
        {
            var docRef = _firestoreDb.Collection(collectionName).Document(documentId);
            
            if (options != null)
            {
                await docRef.SetAsync(data, options);
            }
            else
            {
                await docRef.SetAsync(data);
            }
        }

        /// <summary>
        /// Actualiza un documento existente
        /// </summary>
        public async Task UpdateDocumentAsync(string collectionName, string documentId, Dictionary<string, object> updates)
        {
            var docRef = _firestoreDb.Collection(collectionName).Document(documentId);
            await docRef.UpdateAsync(updates);
        }

        /// <summary>
        /// Elimina un documento
        /// </summary>
        public async Task DeleteDocumentAsync(string collectionName, string documentId)
        {
            var docRef = _firestoreDb.Collection(collectionName).Document(documentId);
            await docRef.DeleteAsync();
        }

        /// <summary>
        /// Obtiene todos los documentos de una colección
        /// </summary>
        public async Task<IReadOnlyList<DocumentSnapshot>> GetAllDocumentsAsync(string collectionName)
        {
            var collection = _firestoreDb.Collection(collectionName);
            var snapshot = await collection.GetSnapshotAsync();
            return snapshot.Documents;
        }

        /// <summary>
        /// Ejecuta una consulta en una colección
        /// </summary>
        public async Task<IReadOnlyList<DocumentSnapshot>> QueryDocumentsAsync(Query query)
        {
            var snapshot = await query.GetSnapshotAsync();
            return snapshot.Documents;
        }

        /// <summary>
        /// Ejecuta una transacción en Firestore
        /// </summary>
        public async Task<T> RunTransactionAsync<T>(Func<Transaction, Task<T>> callback)
        {
            return await _firestoreDb.RunTransactionAsync(callback);
        }
    }
} 