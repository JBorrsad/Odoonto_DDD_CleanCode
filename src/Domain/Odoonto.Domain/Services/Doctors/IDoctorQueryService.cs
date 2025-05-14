using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Odoonto.Domain.Models.Doctors;

namespace Odoonto.Domain.Services.Doctors
{
    /// <summary>
    /// Servicio de dominio para consultas específicas de doctores
    /// </summary>
    public interface IDoctorQueryService
    {
        /// <summary>
        /// Busca doctores por nombre
        /// </summary>
        /// <param name="name">Nombre a buscar</param>
        /// <param name="page">Número de página (1-based)</param>
        /// <param name="pageSize">Tamaño de página</param>
        /// <returns>Lista paginada de doctores</returns>
        Task<IEnumerable<Doctor>> SearchByNameAsync(string name, int page = 1, int pageSize = 10);

        /// <summary>
        /// Busca doctores por especialidad
        /// </summary>
        /// <param name="specialty">Especialidad a buscar</param>
        /// <param name="page">Número de página (1-based)</param>
        /// <param name="pageSize">Tamaño de página</param>
        /// <returns>Lista paginada de doctores</returns>
        Task<IEnumerable<Doctor>> SearchBySpecialtyAsync(string specialty, int page = 1, int pageSize = 10);

        /// <summary>
        /// Busca un doctor por su correo electrónico
        /// </summary>
        /// <param name="email">Correo electrónico a buscar</param>
        /// <returns>El doctor que coincide con el email o null si no existe</returns>
        Task<Doctor> FindByEmailAsync(string email);

        /// <summary>
        /// Busca doctores por término de búsqueda general
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda</param>
        /// <param name="page">Número de página (1-based)</param>
        /// <param name="pageSize">Tamaño de página</param>
        /// <returns>Lista paginada de doctores</returns>
        Task<IEnumerable<Doctor>> SearchAsync(string searchTerm, int page = 1, int pageSize = 10);

        /// <summary>
        /// Obtiene la lista paginada de todos los doctores
        /// </summary>
        /// <param name="page">Número de página (1-based)</param>
        /// <param name="pageSize">Tamaño de página</param>
        /// <returns>Lista paginada de doctores</returns>
        Task<IEnumerable<Doctor>> GetPaginatedAsync(int page = 1, int pageSize = 10);

        /// <summary>
        /// Obtiene el total de doctores registrados
        /// </summary>
        /// <returns>Número total de doctores</returns>
        Task<int> GetTotalDoctorsCountAsync();

        /// <summary>
        /// Obtiene el total de resultados para una búsqueda específica
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda</param>
        /// <returns>Número total de resultados</returns>
        Task<int> CountSearchResultsAsync(string searchTerm);
    }
}