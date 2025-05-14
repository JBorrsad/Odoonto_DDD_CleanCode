using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Odoonto.Data.Core.Contexts;
using Odoonto.Infrastructure.Configuration.Firebase;
using System;

namespace Odoonto.Infrastructure.InversionOfControl.Inyectors
{
    public static class FirebaseInyector
    {
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

            services.AddSingleton(serviceProvider =>
            {
                var config = serviceProvider.GetRequiredService<FirebaseConfiguration>();
                return config.GetStorage();
            });

            // Registrar el contexto de Firestore
            services.AddSingleton<FirestoreContext>();

            return services;
        }
    }
}