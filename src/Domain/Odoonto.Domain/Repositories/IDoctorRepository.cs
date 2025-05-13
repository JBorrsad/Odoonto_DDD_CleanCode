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
        /// Busca doctores por su nombre
        /// </summary>
        /// <param name="name">Texto a buscar en el nombre</param>
        /// <returns>Lista de doctores que coinciden con la búsqueda</returns>
        Task<IEnumerable<Doctor>> FindByNameAsync(string name);

        /// <summary>
        /// Busca doctores por especialidad
        /// </summary>
        /// <param name="specialty">Especialidad a buscar</param>
        /// <returns>Lista de doctores con la especialidad indicada</returns>
        Task<IEnumerable<Doctor>> FindBySpecialtyAsync(string specialty);

        /// <summary>
        /// Busca doctores por número de licencia
        /// </summary>
        /// <param name="licenseNumber">Número de licencia a buscar</param>
        /// <returns>El doctor que coincide con el número de licencia o null si no existe</returns>
        Task<Doctor> FindByLicenseNumberAsync(string licenseNumber);

        /// <summary>
        /// Busca doctores por correo electrónico
        /// </summary>
        /// <param name="email">Correo electrónico a buscar</param>
        /// <returns>El doctor que coincide con el email o null si no existe</returns>
        Task<Doctor> FindByEmailAsync(string email);

        /// <summary>
        /// Obtiene la cantidad total de doctores
        /// </summary>
        /// <returns>El número total de doctores</returns>
        Task<int> GetTotalDoctorsCountAsync();

        /// <summary>
        /// Obtiene doctores con paginación
        /// </summary>
        /// <param name="pageNumber">Número de página (1-based)</param>
        /// <param name="pageSize">Tamaño de página</param>
        /// <returns>Lista paginada de doctores</returns>
        Task<IEnumerable<Doctor>> GetPaginatedAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Busca doctores por cualquier coincidencia en sus datos
        /// </summary>
        /// <param name="searchTerm">Término a buscar</param>
        /// <returns>Lista de doctores que coinciden con la búsqueda</returns>
        Task<IEnumerable<Doctor>> SearchAsync(string searchTerm);

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