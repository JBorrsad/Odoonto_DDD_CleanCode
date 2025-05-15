using Microsoft.Extensions.DependencyInjection;
using Odoonto.Data.Core.Contexts;
using Odoonto.Data.Repositories;
using Odoonto.Domain.Repositories;

namespace Odoonto.Infrastructure.InversionOfControl.Inyectors
{
    /// <summary>
    /// Clase para la inyección de dependencias de la capa de datos
    /// </summary>
    public static class DataInyector
    {
        /// <summary>
        /// Método para inyectar los repositorios
        /// </summary>
        /// <param name="services">Colección de servicios</param>
        public static void Inyect(IServiceCollection services)
        {
            // Registrar contexto de datos para Firebase
            // Nota: FirestoreContext ya se registró en FirebaseInyector
            
            // Registrar repositorios
            RegisterDoctorRepositories(services);
            RegisterPatientRepositories(services);
            RegisterAppointmentRepositories(services);
            RegisterTreatmentRepositories(services);
            RegisterOdontogramRepositories(services);
        }

        /// <summary>
        /// Registra los repositorios de doctores
        /// </summary>
        private static void RegisterDoctorRepositories(IServiceCollection services)
        {
            services.AddScoped<IDoctorRepository, DoctorRepository>();
        }

        /// <summary>
        /// Registra los repositorios de pacientes
        /// </summary>
        private static void RegisterPatientRepositories(IServiceCollection services)
        {
            services.AddScoped<IPatientRepository, PatientRepository>();
        }

        /// <summary>
        /// Registra los repositorios de citas
        /// </summary>
        private static void RegisterAppointmentRepositories(IServiceCollection services)
        {
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        }

        /// <summary>
        /// Registra los repositorios de tratamientos
        /// </summary>
        private static void RegisterTreatmentRepositories(IServiceCollection services)
        {
            services.AddScoped<ITreatmentRepository, TreatmentRepository>();
        }

        /// <summary>
        /// Registra los repositorios de odontogramas
        /// </summary>
        private static void RegisterOdontogramRepositories(IServiceCollection services)
        {
            services.AddScoped<IOdontogramRepository, OdontogramRepository>();
        }
    }
} 