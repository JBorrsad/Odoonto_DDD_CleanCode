using Microsoft.Extensions.DependencyInjection;
using Odoonto.Infrastructure.InversionOfControl.Inyectors;

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
            services.AddRepositories();
            
            // Inyectar infraestructura
            // Usando la sobrecarga sin parámetros, ya que son opcionales en la definición
            services.AddFirebaseServices();

            return services;
        }
    }
} 