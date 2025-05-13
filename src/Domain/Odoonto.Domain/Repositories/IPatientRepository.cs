using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Odoonto.Domain.Core.Repositories;
using Odoonto.Domain.Models.Patients;
using Odoonto.Domain.Models.ValueObjects;

namespace Odoonto.Domain.Repositories
{
    /// <summary>
    /// Repositorio para la entidad Patient
    /// </summary>
    public interface IPatientRepository : IRepository<Patient>
    {
        /// <summary>
        /// Busca pacientes por su nombre
        /// </summary>
        /// <param name="name">Texto a buscar en el nombre</param>
        /// <returns>Lista de pacientes que coinciden con la búsqueda</returns>
        Task<IEnumerable<Patient>> FindByNameAsync(string name);

        /// <summary>
        /// Busca pacientes por rango de edad
        /// </summary>
        /// <param name="minAge">Edad mínima</param>
        /// <param name="maxAge">Edad máxima</param>
        /// <returns>Lista de pacientes dentro del rango de edad</returns>
        Task<IEnumerable<Patient>> FindByAgeRangeAsync(int minAge, int maxAge);

        /// <summary>
        /// Busca pacientes por correo electrónico
        /// </summary>
        /// <param name="email">Correo electrónico a buscar</param>
        /// <returns>El paciente que coincide con el email o null si no existe</returns>
        Task<Patient> FindByEmailAsync(string email);

        /// <summary>
        /// Busca pacientes por número de teléfono
        /// </summary>
        /// <param name="phoneNumber">Número de teléfono a buscar</param>
        /// <returns>Los pacientes que coinciden con el número de teléfono</returns>
        Task<IEnumerable<Patient>> FindByPhoneNumberAsync(string phoneNumber);

        /// <summary>
        /// Obtiene la cantidad total de pacientes
        /// </summary>
        /// <returns>El número total de pacientes</returns>
        Task<int> GetTotalPatientsCountAsync();

        /// <summary>
        /// Obtiene pacientes con paginación
        /// </summary>
        /// <param name="pageNumber">Número de página (1-based)</param>
        /// <param name="pageSize">Tamaño de página</param>
        /// <returns>Lista paginada de pacientes</returns>
        Task<IEnumerable<Patient>> GetPaginatedAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Busca pacientes por cualquier coincidencia en sus datos
        /// </summary>
        /// <param name="searchTerm">Término a buscar</param>
        /// <returns>Lista de pacientes que coinciden con la búsqueda</returns>
        Task<IEnumerable<Patient>> SearchAsync(string searchTerm);
    }
} 