using Microsoft.Extensions.DependencyInjection;
using Odoonto.Data.Core.Contexts;
using Odoonto.Data.Mappings;
using Odoonto.Data.Repositories;
using Odoonto.Domain.Repositories;
using Odoonto.Domain.Services.Appointments;
using Odoonto.Domain.Services.Doctors;
using Odoonto.Domain.Services.Patients;

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

            // Registrar mappers
            RegisterMappers(services);

            // Registrar repositorios implementados
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<ILesionRepository, LesionRepository>();
            services.AddScoped<ITreatmentRepository, TreatmentRepository>();

            return services;
        }

        /// <summary>
        /// Registra los mappers necesarios para los repositorios
        /// </summary>
        private static void RegisterMappers(IServiceCollection services)
        {
            // Si los mappers son clases estáticas, no necesitan registro
            // Si los mappers son instancias, registrarlos aquí:

            // Ejemplo de registro de mappers si fueran clases no estáticas:
            // services.AddSingleton<IPatientMapper, PatientMapper>();
            // services.AddSingleton<IDoctorMapper, DoctorMapper>();
            // services.AddSingleton<IAppointmentMapper, AppointmentMapper>();
            // services.AddSingleton<ITreatmentMapper, TreatmentMapper>();
            // services.AddSingleton<ILesionMapper, LesionMapper>();
        }
    }
}