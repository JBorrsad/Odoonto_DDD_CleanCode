using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Odoonto.Application.Services;
using Odoonto.Application.Interfaces;
using Odoonto.Application.Services.Doctors;
using Odoonto.Application.Services.Patients;
using Odoonto.Domain.Repositories;
using Odoonto.Data.Repositories;
using Odoonto.Infrastructure.InversionOfControl.Inyectors;

namespace Odoonto.Infrastructure.InversionOfControl
{
    /// <summary>
    /// Extensiones para registrar todos los servicios de la aplicación
    /// </summary>
    public static class ServicesInyectorsExtensions
    {
        /// <summary>
        /// Registra todos los servicios de la aplicación en el contenedor IoC
        /// </summary>
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Inyectar servicios de dominio
            DomainInyector.Inyect(services);
            
            // Inyectar servicios de aplicación
            ApplicationInyector.Inyect(services);
            
            // Inyectar servicios de Firebase
            FirebaseInyector.Inyect(services, configuration);
            
            // Inyectar repositorios
            RepositoryInyector.Inyect(services);
            
            // Inyectar DataInyector
            DataInyector.Inyect(services);
            
            // Inyectar middleware y componentes transversales
            MiddlewareInyector.Inyect(services, configuration);
            
            // Inyectar servicios de aplicación
            ServicesInyector.Inyect(services);
            
            return services;
        }
    }
} 