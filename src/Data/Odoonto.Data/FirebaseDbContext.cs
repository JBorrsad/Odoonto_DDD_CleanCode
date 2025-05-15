using Google.Cloud.Firestore;
using System;
using System.Threading.Tasks;

namespace Odoonto.Data.Repositories
{
    /// <summary>
    /// Contexto para acceso a Firebase Firestore
    /// </summary>
    public class FirebaseDbContext
    {
        private readonly FirestoreDb _firestoreDb;

        /// <summary>
        /// Constructor que recibe la instancia de FirestoreDb
        /// </summary>
        /// <param name="firestoreDb">Instancia de FirestoreDb</param>
        public FirebaseDbContext(FirestoreDb firestoreDb)
        {
            _firestoreDb = firestoreDb ?? throw new ArgumentNullException(nameof(firestoreDb));
        }

        /// <summary>
        /// Obtiene una colección de Firestore
        /// </summary>
        /// <param name="collectionName">Nombre de la colección</param>
        /// <returns>Referencia a la colección</returns>
        public CollectionReference GetCollection(string collectionName)
        {
            return _firestoreDb.Collection(collectionName);
        }

        /// <summary>
        /// Obtiene un documento por su ID
        /// </summary>
        /// <param name="collectionName">Nombre de la colección</param>
        /// <param name="documentId">ID del documento</param>
        /// <returns>Snapshot del documento</returns>
        public async Task<DocumentSnapshot> GetDocumentAsync(string collectionName, string documentId)
        {
            return await _firestoreDb.Collection(collectionName).Document(documentId).GetSnapshotAsync();
        }

        /// <summary>
        /// Crea o actualiza un documento
        /// </summary>
        /// <param name="collectionName">Nombre de la colección</param>
        /// <param name="documentId">ID del documento</param>
        /// <param name="data">Datos del documento</param>
        /// <returns>Task para operación asíncrona</returns>
        public async Task SetDocumentAsync(string collectionName, string documentId, object data)
        {
            await _firestoreDb.Collection(collectionName).Document(documentId).SetAsync(data);
        }

        /// <summary>
        /// Elimina un documento
        /// </summary>
        /// <param name="collectionName">Nombre de la colección</param>
        /// <param name="documentId">ID del documento</param>
        /// <returns>Task para operación asíncrona</returns>
        public async Task DeleteDocumentAsync(string collectionName, string documentId)
        {
            await _firestoreDb.Collection(collectionName).Document(documentId).DeleteAsync();
        }

        /// <summary>
        /// Ejecuta una transacción
        /// </summary>
        /// <typeparam name="T">Tipo de retorno</typeparam>
        /// <param name="callback">Función de transacción</param>
        /// <returns>Resultado de la transacción</returns>
        public async Task<T> RunTransactionAsync<T>(Func<Transaction, Task<T>> callback)
        {
            return await _firestoreDb.RunTransactionAsync(callback);
        }
    }
} 