using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Odoonto.Infrastructure.Authentication.Services;

namespace Odoonto.Infrastructure.InversionOfControl.Inyectors
{
    /// <summary>
    /// Inyector para middlewares y componentes de infraestructura transversales
    /// </summary>
    public static class MiddlewareInyector
    {
        /// <summary>
        /// Registra los componentes de middleware en el contenedor IoC
        /// </summary>
        public static void Inyect(IServiceCollection services, IConfiguration configuration)
        {
            // Configurar servicios de autenticación de Firebase
            services.AddFirebaseAuthentication(configuration);
            
            // Registrar servicios de logging personalizados (si es necesario)
            services.AddLogging();
            
            // Registrar servicios para el manejo global de excepciones
            // (Los middleware en sí mismos se registran en el pipeline de la aplicación,
            // pero aquí podríamos registrar servicios adicionales que requieran)
        }
    }
} 