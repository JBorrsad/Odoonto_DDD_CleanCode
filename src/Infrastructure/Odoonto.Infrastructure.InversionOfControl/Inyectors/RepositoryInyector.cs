using Microsoft.Extensions.DependencyInjection;
using Odoonto.Data.Core.Contexts;
using Odoonto.Data.Repositories;
using Odoonto.Domain.Repositories;

namespace Odoonto.Infrastructure.InversionOfControl.Inyectors
{
    /// <summary>
    /// Registra todos los repositorios para inyección de dependencias
    /// </summary>
    public static class RepositoryInyector
    {
        /// <summary>
        /// Registra los repositorios en el contenedor de servicios
        /// </summary>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // Registrar el contexto de Firestore
            services.AddSingleton<FirestoreContext>();

            // Registrar repositorios implementados
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<ILesionRepository, LesionRepository>();
            services.AddScoped<ITreatmentRepository, TreatmentRepository>();

            return services;
        }
    }
}