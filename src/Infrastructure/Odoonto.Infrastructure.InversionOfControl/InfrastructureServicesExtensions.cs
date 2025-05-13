using Microsoft.Extensions.DependencyInjection;

namespace Odoonto.Infrastructure.InversionOfControl
{
    /// <summary>
    /// Clase de extensión para configurar todos los servicios de infraestructura
    /// </summary>
    public static class InfrastructureServicesExtensions
    {
        /// <summary>
        /// Método de extensión para configurar todos los servicios de la aplicación
        /// </summary>
        /// <param name="services">Colección de servicios</param>
        /// <returns>Colección de servicios modificada</returns>
        public static IServiceCollection AddOdoontoServices(this IServiceCollection services)
        {
            // Inyectar servicios de aplicación
            Inyectors.ApplicationInyector.Inyect(services);
            
            // Inyectar repositorios de datos
            Inyectors.RepositoryInyector.Inyect(services);
            
            // Inyectar infraestructura
            Inyectors.FirebaseInyector.Inyect(services);

            return services;
        }
    }
} 