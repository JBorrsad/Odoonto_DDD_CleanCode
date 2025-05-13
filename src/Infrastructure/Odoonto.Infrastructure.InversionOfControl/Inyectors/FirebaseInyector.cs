using Microsoft.Extensions.DependencyInjection;
using Odoonto.Infrastructure.Configuration.Firebase;
using System;

namespace Odoonto.Infrastructure.InversionOfControl.Inyectors
{
    public static class FirebaseInyector
    {
        public static IServiceCollection AddFirebaseServices(this IServiceCollection services, string webApiKey = null, string credentialsPath = null)
        {
            // Inicializar la configuración de Firebase
            FirebaseConfiguration.Instance.Initialize(webApiKey, credentialsPath);

            // Registrar servicios de Firebase
            services.AddSingleton(_ => FirebaseConfiguration.Instance);
            services.AddSingleton(_ => FirebaseConfiguration.Instance.GetFirestoreDb());
            services.AddSingleton(_ => FirebaseConfiguration.Instance.GetAuthProvider());
            services.AddSingleton(_ => FirebaseConfiguration.Instance.GetDatabaseClient());
            services.AddSingleton(_ => FirebaseConfiguration.Instance.GetStorage());

            return services;
        }
    }
} 