using System;
using System.IO;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Storage;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Odoonto.Infrastructure.Configuration.Firebase
{
    public class FirebaseConfiguration
    {
        private static FirebaseConfiguration? _instance;
        private static readonly object _lock = new object();
        private ILogger<FirebaseConfiguration>? _logger;

        public string ProjectId { get; private set; }
        public string WebApiKey { get; private set; }
        public string AuthDomain { get; private set; }
        public string DatabaseUrl { get; private set; }
        public string StorageBucket { get; private set; }
        public string ServiceAccountKeyPath { get; private set; }

        private FirestoreDb? _firestoreDb;
        private FirebaseAuthProvider? _authProvider;
        private FirebaseClient? _databaseClient;
        private FirebaseStorage? _storage;

        private FirebaseConfiguration(ILogger<FirebaseConfiguration>? logger = null)
        {
            _logger = logger;

            // Valores por defecto - Deben ser sobrescritos con Initialize
            ProjectId = "odoonto-e06a7";
            WebApiKey = "";
            AuthDomain = "odoonto-e06a7.firebaseapp.com";
            DatabaseUrl = "https://odoonto-e06a7.firebaseio.com/";
            StorageBucket = "odoonto-e06a7.appspot.com";

            // Ruta relativa al directorio de ejecución
            ServiceAccountKeyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "firebase-credentials.json");
        }

        public static FirebaseConfiguration Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new FirebaseConfiguration();
                    }
                }
                return _instance;
            }
        }

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

                // Establecer la variable de entorno para el SDK de Firebase
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", ServiceAccountKeyPath);
                _logger?.LogInformation("Firebase configurado con credenciales en: {path}", ServiceAccountKeyPath);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error initializing Firebase: {ex.Message}";
                _logger?.LogError(ex, errorMessage);
                throw new InvalidOperationException(errorMessage, ex);
            }
        }

        public FirestoreDb GetFirestoreDb()
        {
            if (_firestoreDb == null)
            {
                try
                {
                    _firestoreDb = FirestoreDb.Create(ProjectId);
                    _logger?.LogInformation("FirestoreDb creado correctamente para el proyecto {projectId}", ProjectId);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Error al crear FirestoreDb para el proyecto {projectId}", ProjectId);
                    throw;
                }
            }
            return _firestoreDb;
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
    }
}