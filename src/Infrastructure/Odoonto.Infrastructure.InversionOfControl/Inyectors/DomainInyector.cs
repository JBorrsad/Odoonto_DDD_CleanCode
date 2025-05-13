using Microsoft.Extensions.DependencyInjection;
using Odoonto.Domain.Models.Appointments;
using System;

namespace Odoonto.Infrastructure.InversionOfControl.Inyectors
{
    /// <summary>
    /// Clase para la inyección de dependencias del dominio
    /// </summary>
    public static class DomainInyector
    {
        /// <summary>
        /// Método para inyectar los servicios del dominio
        /// </summary>
        /// <param name="services">Colección de servicios</param>
        public static void Inyect(IServiceCollection services)
        {
            // En este caso, no hay servicios de dominio para inyectar
            // Si en el futuro se añaden servicios de dominio, se pueden inyectar aquí
            
            // Por ejemplo:
            // services.AddScoped<IDomainService, DomainServiceImplementation>();
        }
    }
} 