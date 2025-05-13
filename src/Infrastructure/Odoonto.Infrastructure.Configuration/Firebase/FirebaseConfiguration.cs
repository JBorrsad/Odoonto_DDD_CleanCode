using System;
using System.IO;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Storage;
using Google.Cloud.Firestore;
using Newtonsoft.Json;

namespace Odoonto.Infrastructure.Configuration.Firebase
{
    public class FirebaseConfiguration
    {
        private static FirebaseConfiguration _instance;
        private static readonly object _lock = new object();

        public string ProjectId { get; private set; }
        public string WebApiKey { get; private set; }
        public string AuthDomain { get; private set; }
        public string DatabaseUrl { get; private set; }
        public string StorageBucket { get; private set; }
        public string ServiceAccountKeyPath { get; private set; }

        private FirestoreDb _firestoreDb;
        private FirebaseAuthProvider _authProvider;
        private FirebaseClient _databaseClient;
        private FirebaseStorage _storage;

        private FirebaseConfiguration()
        {
            // Valores por defecto - Deben ser sobrescritos con Initialize
            ProjectId = "odoonto-e06a7";
            WebApiKey = "";
            AuthDomain = "odoonto-e06a7.firebaseapp.com";
            DatabaseUrl = "https://odoonto-e06a7.firebaseio.com/";
            StorageBucket = "odoonto-e06a7.appspot.com";
            
            // Ruta relativa al directorio de ejecución
            ServiceAccountKeyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, 
                "Infrastructure", "Configuration", "Firebase", "firebase-credentials.json");
        }

        public static FirebaseConfiguration Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new FirebaseConfiguration();
                        }
                    }
                }
                return _instance;
            }
        }

        public void Initialize(string webApiKey = null, string servicePath = null)
        {
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
                throw new FileNotFoundException($"Firebase service account key file not found at {ServiceAccountKeyPath}");
            }

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", ServiceAccountKeyPath);
        }

        public FirestoreDb GetFirestoreDb()
        {
            if (_firestoreDb == null)
            {
                _firestoreDb = FirestoreDb.Create(ProjectId);
            }
            return _firestoreDb;
        }

        public FirebaseAuthProvider GetAuthProvider()
        {
            if (_authProvider == null)
            {
                _authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));
            }
            return _authProvider;
        }

        public FirebaseClient GetDatabaseClient()
        {
            if (_databaseClient == null)
            {
                _databaseClient = new FirebaseClient(DatabaseUrl);
            }
            return _databaseClient;
        }

        public FirebaseStorage GetStorage()
        {
            if (_storage == null)
            {
                _storage = new FirebaseStorage(StorageBucket);
            }
            return _storage;
        }
    }
} 