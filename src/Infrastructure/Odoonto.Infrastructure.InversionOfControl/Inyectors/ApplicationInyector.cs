using Microsoft.Extensions.DependencyInjection;
using Odoonto.Application.Interfaces;
using Odoonto.Application.Mappers;
using Odoonto.Application.Services;
using System;
using System.Collections.Generic;

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
            // Inyección de servicios de aplicación
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<ITreatmentService, TreatmentService>();
            
            // Configuración de AutoMapper
            InyectProfiles(services);
        }

        /// <summary>
        /// Método para inyectar los perfiles de AutoMapper
        /// </summary>
        /// <param name="services">Colección de servicios</param>
        private static void InyectProfiles(IServiceCollection services)
        {
            List<Type> profiles = new List<Type>();
            
            // Añadir perfiles de mapeo
            profiles.Add(typeof(AppointmentProfile));
            profiles.Add(typeof(TreatmentProfile));
            
            // Si existe un perfil para pacientes, añadirlo
            if (Type.GetType("Odoonto.Application.Mappers.PatientProfile") != null)
            {
                profiles.Add(typeof(PatientProfile));
            }
            
            // Si existe un perfil para doctores, añadirlo
            if (Type.GetType("Odoonto.Application.Mappers.DoctorProfile") != null)
            {
                profiles.Add(typeof(DoctorProfile));
            }

            // Configurar AutoMapper con los perfiles
            services.AddAutoMapper(profiles.ToArray());
        }
    }
} 