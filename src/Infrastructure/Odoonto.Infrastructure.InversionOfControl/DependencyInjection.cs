using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Odoonto.Infrastructure.InversionOfControl.Inyectors;
using System;

namespace Odoonto.Infrastructure.InversionOfControl
{
    /// <summary>
    /// Clase principal para la inyección de dependencias de la aplicación
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Registra todos los servicios de la aplicación
        /// </summary>
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Registrar servicios de infraestructura
            services.AddInfrastructureServices(configuration);
            
            // Registrar servicios de datos
            services.AddDataServices(configuration);
            
            // Registrar servicios de aplicación
            services.AddAppServices();
            
            // Registrar AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
            return services;
        }

        /// <summary>
        /// Registra los servicios de infraestructura
        /// </summary>
        private static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Registrar servicios de Firebase
            FirebaseInyector.Inyect(services, configuration);
            
            return services;
        }

        /// <summary>
        /// Registra los servicios de datos
        /// </summary>
        private static IServiceCollection AddDataServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Registrar repositorios
            DataInyector.Inyect(services);
            
            return services;
        }

        /// <summary>
        /// Registra los servicios de aplicación
        /// </summary>
        private static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            // Registrar servicios de aplicación por dominio
            ApplicationInyector.Inyect(services);
            
            return services;
        }
    }
} 