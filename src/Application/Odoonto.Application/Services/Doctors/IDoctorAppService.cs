using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Odoonto.Application.DTOs.Doctors;

namespace Odoonto.Application.Services.Doctors
{
    /// <summary>
    /// Interfaz para el servicio de aplicación de doctores
    /// </summary>
    public interface IDoctorAppService
    {
        /// <summary>
        /// Obtiene todos los doctores
        /// </summary>
        Task<IEnumerable<DoctorDto>> GetAllAsync();

        /// <summary>
        /// Obtiene un doctor por su ID
        /// </summary>
        Task<DoctorDto> GetByIdAsync(Guid id);

        /// <summary>
        /// Obtiene doctores por especialidad
        /// </summary>
        Task<IEnumerable<DoctorDto>> GetBySpecialtyAsync(string specialty);

        /// <summary>
        /// Crea un nuevo doctor
        /// </summary>
        Task<Guid> CreateAsync(CreateDoctorDto doctorDto);

        /// <summary>
        /// Actualiza un doctor existente
        /// </summary>
        Task UpdateAsync(Guid id, UpdateDoctorDto doctorDto);

        /// <summary>
        /// Elimina un doctor
        /// </summary>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Verifica disponibilidad de un doctor en una fecha y hora específicas
        /// </summary>
        Task<bool> CheckAvailabilityAsync(Guid id, DateTime date, TimeOnly startTime, TimeOnly endTime);

        /// <summary>
        /// Establece disponibilidad para un doctor en un día específico
        /// </summary>
        Task SetAvailabilityAsync(Guid id, DayOfWeek day, TimeOnly startTime, TimeOnly endTime);
    }
} 