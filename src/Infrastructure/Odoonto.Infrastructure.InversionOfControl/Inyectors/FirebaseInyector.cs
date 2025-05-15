using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Odoonto.Data.Core.Contexts;
using Odoonto.Infrastructure.Configuration.Firebase;
using System;
using System.IO;

namespace Odoonto.Infrastructure.InversionOfControl.Inyectors
{
    /// <summary>
    /// Clase para la inyección de dependencias de Firebase
    /// </summary>
    public static class FirebaseInyector
    {
        /// <summary>
        /// Método para inyectar los servicios de Firebase
        /// </summary>
        /// <param name="services">Colección de servicios</param>
        /// <param name="configuration">Configuración de la aplicación</param>
        public static void Inyect(IServiceCollection services, IConfiguration configuration)
        {
            var firebaseSection = configuration.GetSection("Firebase");
            if (!firebaseSection.Exists())
            {
                throw new InvalidOperationException("La sección 'Firebase' no existe en la configuración");
            }

            string apiKey = firebaseSection["ApiKey"] ?? throw new InvalidOperationException("ApiKey no configurado");
            string credentialsPath = firebaseSection["CredentialsPath"] ?? "firebase-credentials.json";
            
            // Obtener la ruta completa del archivo de credenciales
            var contentRootPath = configuration["ContentRootPath"] ?? AppDomain.CurrentDomain.BaseDirectory;
            var credentialsFullPath = Path.Combine(contentRootPath, credentialsPath);

            // Comprobar si existe el archivo de credenciales
            if (!File.Exists(credentialsFullPath))
            {
                throw new FileNotFoundException($"Archivo de credenciales de Firebase no encontrado en: {credentialsFullPath}");
            }

            services.AddFirebaseServices(apiKey, credentialsFullPath);
        }

        public static IServiceCollection AddFirebaseServices(this IServiceCollection services, string webApiKey = null, string credentialsPath = null)
        {
            // Registrar FirebaseConfiguration como singleton
            services.AddSingleton<FirebaseConfiguration>(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<FirebaseConfiguration>>();
                var instance = FirebaseConfiguration.Instance;
                instance.Initialize(webApiKey, credentialsPath, logger);
                return instance;
            });

            // Registrar servicios de Firebase
            services.AddSingleton(serviceProvider =>
            {
                var config = serviceProvider.GetRequiredService<FirebaseConfiguration>();
                return config.GetFirestoreDb();
            });

            services.AddSingleton(serviceProvider =>
            {
                var config = serviceProvider.GetRequiredService<FirebaseConfiguration>();
                return config.GetAuthProvider();
            });

            services.AddSingleton(serviceProvider =>
            {
                var config = serviceProvider.GetRequiredService<FirebaseConfiguration>();
                return config.GetDatabaseClient();
            });

            // Comentado temporalmente el servicio de Storage
            /*
            services.AddSingleton(serviceProvider =>
            {
                var config = serviceProvider.GetRequiredService<FirebaseConfiguration>();
                return config.GetStorage();
            });
            */

            // Registrar el contexto de Firestore
            services.AddSingleton<FirestoreContext>();

            return services;
        }
    }
}