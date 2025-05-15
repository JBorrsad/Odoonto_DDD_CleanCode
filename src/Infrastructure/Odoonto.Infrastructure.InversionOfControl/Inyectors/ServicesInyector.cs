using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Odoonto.Application.DTOs.Patients;
using Odoonto.Application.DTOs.Doctors;
using Odoonto.Application.DTOs.Appointments;
using Odoonto.Application.DTOs.Treatments;
using Odoonto.Application.DTOs.Odontograms;
using Odoonto.Application.DTOs.Lesions;
using Odoonto.Application.Interfaces;
using Odoonto.Application.Services;
using Odoonto.Application.Mappings;

namespace Odoonto.Infrastructure.InversionOfControl.Inyectors
{
    /// <summary>
    /// Inyector para los servicios de aplicación
    /// </summary>
    public static class ServicesInyector
    {
        /// <summary>
        /// Registra los servicios de aplicación en el contenedor IoC
        /// </summary>
        public static void Inyect(IServiceCollection services)
        {
            // Registrar servicios de pacientes
            services.AddScoped<IPatientService, PatientService>();
            
            // Registrar servicios de doctores
            services.AddScoped<IDoctorService, DoctorService>();
            
            // Registrar servicios de citas
            services.AddScoped<IAppointmentService, AppointmentService>();
            
            // Registrar servicios de tratamientos
            services.AddScoped<ITreatmentService, TreatmentService>();
            
            // Registrar servicios de odontogramas
            services.AddScoped<IOdontogramService, OdontogramService>();
            
            // Registrar servicios de lesiones
            services.AddScoped<ILesionService, LesionService>();
            
            // Registrar todos los perfiles de AutoMapper
            RegisterAutoMapperProfiles(services);
        }
        
        /// <summary>
        /// Registra los perfiles de AutoMapper
        /// </summary>
        private static void RegisterAutoMapperProfiles(IServiceCollection services)
        {
            // Crear lista de todos los perfiles
            List<Type> profiles = new List<Type>
            {
                // Perfiles de pacientes
                typeof(PatientMappingProfile),
                
                // Perfiles de doctores
                typeof(DoctorMappingProfile),
                
                // Perfiles de citas
                typeof(AppointmentMappingProfile),
                
                // Perfiles de tratamientos
                typeof(TreatmentMappingProfile),
                
                // Perfiles de odontogramas
                typeof(OdontogramMappingProfile),
                
                // Perfiles de lesiones
                typeof(LesionMappingProfile)
            };
            
            // Registrar todos los perfiles
            services.AddAutoMapper(profiles.ToArray());
        }
    }
} 