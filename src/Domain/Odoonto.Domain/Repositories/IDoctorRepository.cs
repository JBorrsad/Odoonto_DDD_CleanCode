using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Odoonto.Domain.Core.Repositories;
using Odoonto.Domain.Models.Doctors;
using Odoonto.Domain.Models.ValueObjects;

namespace Odoonto.Domain.Repositories
{
    /// <summary>
    /// Interfaz que define las operaciones de repositorio para la entidad Doctor
    /// </summary>
    public interface IDoctorRepository : IRepository<Doctor>
    {
        /// <summary>
        /// Busca doctores por especialidad
        /// </summary>
        /// <param name="specialty">Especialidad a buscar</param>
        /// <returns>Lista de doctores con la especialidad especificada</returns>
        Task<IEnumerable<Doctor>> GetBySpecialtyAsync(string specialty);
        
        /// <summary>
        /// Verifica la disponibilidad de un doctor en un día y horario específico
        /// </summary>
        /// <param name="doctorId">Identificador del doctor</param>
        /// <param name="date">Fecha a verificar</param>
        /// <param name="timeSlot">Franja horaria a verificar</param>
        /// <returns>True si el doctor está disponible, False si no está disponible</returns>
        Task<bool> IsAvailableAsync(Guid doctorId, DateTime date, TimeSlot timeSlot);
        
        /// <summary>
        /// Busca doctores por nombre o apellido
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda para nombre o apellido</param>
        /// <returns>Lista de doctores que coinciden con el término de búsqueda</returns>
        Task<IEnumerable<Doctor>> SearchByNameAsync(string searchTerm);
    }
} 