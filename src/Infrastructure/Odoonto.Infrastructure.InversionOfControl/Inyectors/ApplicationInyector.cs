using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using AutoMapper;
using Odoonto.Application.Services.Doctors;
using Odoonto.Application.Services.Patients;
using Odoonto.Application.Services.Appointments;
using Odoonto.Application.Services.Treatments;
using Odoonto.Application.Services.Odontograms;

namespace Odoonto.Infrastructure.InversionOfControl.Inyectors
{
    /// <summary>
    /// Clase para la inyección de dependencias de la capa de aplicación
    /// </summary>
    public static class ApplicationInyector
    {
        /// <summary>
        /// Método para inyectar los servicios de aplicación
        /// </summary>
        /// <param name="services">Colección de servicios</param>
        public static void Inyect(IServiceCollection services)
        {
            // Registrar servicios de aplicación
            RegisterDoctorServices(services);
            RegisterPatientServices(services);
            RegisterAppointmentServices(services);
            RegisterTreatmentServices(services);
            RegisterOdontogramServices(services);
            
            // Registrar perfiles de AutoMapper
            RegisterAutoMapperProfiles(services);
        }

        /// <summary>
        /// Registra los servicios de doctores
        /// </summary>
        private static void RegisterDoctorServices(IServiceCollection services)
        {
            services.AddScoped<IDoctorAppService, DoctorAppService>();
        }

        /// <summary>
        /// Registra los servicios de pacientes
        /// </summary>
        private static void RegisterPatientServices(IServiceCollection services)
        {
            services.AddScoped<IPatientAppService, PatientAppService>();
        }

        /// <summary>
        /// Registra los servicios de citas
        /// </summary>
        private static void RegisterAppointmentServices(IServiceCollection services)
        {
            services.AddScoped<IAppointmentAppService, AppointmentAppService>();
        }

        /// <summary>
        /// Registra los servicios de tratamientos
        /// </summary>
        private static void RegisterTreatmentServices(IServiceCollection services)
        {
            services.AddScoped<ITreatmentAppService, TreatmentAppService>();
        }

        /// <summary>
        /// Registra los servicios de odontogramas
        /// </summary>
        private static void RegisterOdontogramServices(IServiceCollection services)
        {
            services.AddScoped<IOdontogramAppService, OdontogramAppService>();
        }

        /// <summary>
        /// Registra los perfiles de AutoMapper
        /// </summary>
        private static void RegisterAutoMapperProfiles(IServiceCollection services)
        {
            // Obtener todos los perfiles de AutoMapper en el assembly de Application
            var applicationAssembly = Assembly.GetAssembly(typeof(DoctorAppService));
            
            if (applicationAssembly != null)
            {
                // Registrar todos los perfiles
                services.AddAutoMapper(applicationAssembly);
            }
        }
    }
}