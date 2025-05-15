using System;
using System.IO;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Odoonto.Infrastructure.Authentication.Services
{
    /// <summary>
    /// Servicio para inicializar Firebase Authentication
    /// </summary>
    public static class FirebaseInitializer
    {
        /// <summary>
        /// Inicializa Firebase Admin SDK con las credenciales proporcionadas
        /// </summary>
        public static IServiceCollection AddFirebaseAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var logger = services.BuildServiceProvider().GetService<ILogger<FirebaseApp>>();

            try
            {
                // Obtener configuración
                var credentialsPath = configuration["Firebase:CredentialsPath"];
                
                if (string.IsNullOrEmpty(credentialsPath))
                {
                    logger?.LogWarning("Firebase:CredentialsPath no está configurado. La autenticación con Firebase no funcionará correctamente.");
                    return services;
                }

                if (!File.Exists(credentialsPath))
                {
                    logger?.LogError("El archivo de credenciales de Firebase no existe en la ruta especificada: {Path}", credentialsPath);
                    return services;
                }

                // Inicializar Firebase Admin si no está ya inicializado
                if (FirebaseApp.DefaultInstance == null)
                {
                    FirebaseApp.Create(new AppOptions
                    {
                        Credential = GoogleCredential.FromFile(credentialsPath)
                    });

                    logger?.LogInformation("Firebase Authentication inicializado correctamente.");
                }
                else
                {
                    logger?.LogInformation("Firebase Authentication ya estaba inicializado.");
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Error al inicializar Firebase Authentication: {Message}", ex.Message);
            }

            return services;
        }
    }
} 