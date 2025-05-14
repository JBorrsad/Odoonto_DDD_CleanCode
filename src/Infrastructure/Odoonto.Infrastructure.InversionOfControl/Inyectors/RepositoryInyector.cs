using Microsoft.Extensions.DependencyInjection;
using Odoonto.Data.Core.Contexts;
using Odoonto.Data.Mappings;
using Odoonto.Data.Repositories;
using Odoonto.Data.Repositories.Firebase;
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

            // Registrar los repositorios base de Firebase
            services.AddFirebaseRepositories();

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

        private static IServiceCollection AddFirebaseRepositories(this IServiceCollection services)
        {
            // Registrar implementaciones específicas de repositorios
            services.AddScoped<IPatientRepository, FirebasePatientRepository>();
            services.AddScoped<IDoctorRepository, FirebaseDoctorRepository>();
            services.AddScoped<IAppointmentRepository, FirebaseAppointmentRepository>();
            services.AddScoped<IOdontogramRepository, FirebaseOdontogramRepository>();
            services.AddScoped<ILesionRepository, FirebaseLesionRepository>();
            services.AddScoped<ITreatmentRepository, FirebaseTreatmentRepository>();

            return services;
        }
    }
}