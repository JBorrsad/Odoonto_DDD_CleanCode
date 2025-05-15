using Microsoft.Extensions.DependencyInjection;
using Odoonto.Domain.Services.Appointments;
using Odoonto.Domain.Services.Doctors;
using Odoonto.Domain.Services.Patients;
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
            // Registrar los servicios de dominio
            RegisterPatientServices(services);
            RegisterAppointmentServices(services);
            RegisterDoctorServices(services);

            // Aquí se pueden añadir más servicios de dominio en el futuro
        }

        /// <summary>
        /// Registra los servicios de pacientes
        /// </summary>
        private static void RegisterPatientServices(IServiceCollection services)
        {
            // Servicios de pacientes
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IPatientQueryService, PatientQueryService>();
        }

        /// <summary>
        /// Registra los servicios de citas
        /// </summary>
        private static void RegisterAppointmentServices(IServiceCollection services)
        {
            // Servicios de citas
            services.AddScoped<IAppointmentOverlapService, AppointmentOverlapService>();
            services.AddScoped<IAppointmentQueryService, AppointmentQueryService>();
            services.AddScoped<IAppointmentSchedulingService, AppointmentSchedulingService>();
        }

        /// <summary>
        /// Registra los servicios de doctores
        /// </summary>
        private static void RegisterDoctorServices(IServiceCollection services)
        {
            // Servicios de doctores
            services.AddScoped<IDoctorAvailabilityService, DoctorAvailabilityService>();
            services.AddScoped<IDoctorQueryService, DoctorQueryService>();
        }
    }
}