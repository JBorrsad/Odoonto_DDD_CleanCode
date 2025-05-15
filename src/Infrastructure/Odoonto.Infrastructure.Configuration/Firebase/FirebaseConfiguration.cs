using System;
using System.IO;
using Firebase.Auth;
using Firebase.Database;
// Comentamos temporalmente la referencia a Firebase.Storage
// using Firebase.Storage;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Odoonto.Infrastructure.Configuration.Firebase
{
    /// <summary>
    /// Configuración para Firebase
    /// </summary>
    public class FirebaseConfiguration
    {
        // Instancia singleton
        private static readonly FirebaseConfiguration _instance = new FirebaseConfiguration();
        
        /// <summary>
        /// Obtiene la instancia singleton
        /// </summary>
        public static FirebaseConfiguration Instance => _instance;
        
        // Cliente Firestore
        private FirestoreDb _firestoreDb;
        private ILogger<FirebaseConfiguration>? _logger;
        
        public string ProjectId { get; private set; }
        public string WebApiKey { get; private set; }
        public string AuthDomain { get; private set; }
        public string DatabaseUrl { get; private set; }
        public string StorageBucket { get; private set; }
        public string ServiceAccountKeyPath { get; private set; }

        private FirebaseAuthProvider? _authProvider;
        private FirebaseClient? _databaseClient;
        // Comentamos temporalmente la propiedad de Storage
        // private FirebaseStorage? _storage;
        
        private FirebaseConfiguration()
        {
            // Constructor privado para singleton
        }
        
        /// <summary>
        /// Obtiene la instancia de FirestoreDb
        /// </summary>
        /// <returns>Instancia de FirestoreDb</returns>
        public FirestoreDb GetFirestoreDb() => _firestoreDb;
        
        /// <summary>
        /// Inicializa la configuración de Firebase
        /// </summary>
        /// <param name="webApiKey">API Key de Firebase para autenticación web</param>
        /// <param name="servicePath">Ruta al archivo de credenciales</param>
        /// <param name="logger">Logger para registrar eventos</param>
        public void Initialize(string? webApiKey = null, string? servicePath = null, ILogger<FirebaseConfiguration>? logger = null)
        {
            if (logger != null)
            {
                _logger = logger;
            }

            if (!string.IsNullOrEmpty(webApiKey))
            {
                WebApiKey = webApiKey;
            }

            if (!string.IsNullOrEmpty(servicePath))
            {
                ServiceAccountKeyPath = servicePath;
            }

            // Verificar que el archivo de credenciales existe
            if (!File.Exists(ServiceAccountKeyPath))
            {
                var errorMessage = $"Firebase service account key file not found at {ServiceAccountKeyPath}";
                _logger?.LogError(errorMessage);
                throw new FileNotFoundException(errorMessage);
            }

            try
            {
                // Leer el contenido del archivo para verificar que es un JSON válido
                var jsonContent = File.ReadAllText(ServiceAccountKeyPath);
                var credentialData = JsonConvert.DeserializeObject<dynamic>(jsonContent);

                // Verificar que el ProjectId coincide con el especificado en el archivo
                if (credentialData?.project_id != null)
                {
                    ProjectId = credentialData.project_id.ToString();
                    _logger?.LogInformation("Usando ProjectId {projectId} de las credenciales", ProjectId);
                }
                else
                {
                    throw new InvalidOperationException("No se pudo encontrar project_id en el archivo de credenciales");
                }

                // Establecer la variable de entorno para el SDK de Firebase
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", ServiceAccountKeyPath);
                
                // Inicializar Firestore
                _firestoreDb = FirestoreDb.Create(ProjectId);
                
                _logger?.LogInformation("Firebase configurado con éxito. ProjectId: {projectId}", ProjectId);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error initializing Firebase: {ex.Message}";
                _logger?.LogError(ex, errorMessage);
                throw new InvalidOperationException(errorMessage, ex);
            }
        }

        public FirebaseAuthProvider GetAuthProvider()
        {
            if (_authProvider == null)
            {
                if (string.IsNullOrEmpty(WebApiKey))
                {
                    var errorMessage = "Firebase WebApiKey no está configurado";
                    _logger?.LogError(errorMessage);
                    throw new InvalidOperationException(errorMessage);
                }

                _authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));
            }
            return _authProvider;
        }

        public FirebaseClient GetDatabaseClient()
        {
            if (_databaseClient == null)
            {
                if (string.IsNullOrEmpty(DatabaseUrl))
                {
                    var errorMessage = "Firebase DatabaseUrl no está configurado";
                    _logger?.LogError(errorMessage);
                    throw new InvalidOperationException(errorMessage);
                }

                _databaseClient = new FirebaseClient(DatabaseUrl);
            }
            return _databaseClient;
        }

        // Comentamos temporalmente el método de Storage
        /*
        public FirebaseStorage GetStorage()
        {
            if (_storage == null)
            {
                if (string.IsNullOrEmpty(StorageBucket))
                {
                    var errorMessage = "Firebase StorageBucket no está configurado";
                    _logger?.LogError(errorMessage);
                    throw new InvalidOperationException(errorMessage);
                }

                _storage = new FirebaseStorage(StorageBucket);
            }
            return _storage;
        }
        */
    }
}