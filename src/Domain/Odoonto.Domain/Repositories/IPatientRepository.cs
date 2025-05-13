using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Odoonto.Domain.Core.Repositories;
using Odoonto.Domain.Models.Patients;
using Odoonto.Domain.Models.ValueObjects;

namespace Odoonto.Domain.Repositories
{
    /// <summary>
    /// Interfaz que define las operaciones de repositorio para la entidad Patient
    /// </summary>
    public interface IPatientRepository : IRepository<Patient>
    {
        /// <summary>
        /// Busca pacientes por nombre o apellido
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda</param>
        /// <returns>Lista de pacientes que coinciden con el término de búsqueda</returns>
        Task<IEnumerable<Patient>> SearchByNameAsync(string searchTerm);
        
        /// <summary>
        /// Busca pacientes por rango de edad
        /// </summary>
        /// <param name="minAge">Edad mínima</param>
        /// <param name="maxAge">Edad máxima</param>
        /// <returns>Lista de pacientes en el rango de edad especificado</returns>
        Task<IEnumerable<Patient>> GetByAgeRangeAsync(int minAge, int maxAge);
        
        /// <summary>
        /// Busca pacientes por género
        /// </summary>
        /// <param name="gender">Género a buscar</param>
        /// <returns>Lista de pacientes del género especificado</returns>
        Task<IEnumerable<Patient>> GetByGenderAsync(Gender gender);
        
        /// <summary>
        /// Busca pacientes por email
        /// </summary>
        /// <param name="email">Email a buscar</param>
        /// <returns>Paciente con el email especificado o null si no existe</returns>
        Task<Patient> GetByEmailAsync(string email);
        
        /// <summary>
        /// Busca pacientes por número de teléfono
        /// </summary>
        /// <param name="phoneNumber">Número de teléfono a buscar</param>
        /// <returns>Lista de pacientes con el número de teléfono especificado</returns>
        Task<IEnumerable<Patient>> GetByPhoneNumberAsync(string phoneNumber);
        
        /// <summary>
        /// Obtiene los pacientes activos
        /// </summary>
        /// <returns>Lista de pacientes activos</returns>
        Task<IEnumerable<Patient>> GetActiveAsync();
        
        /// <summary>
        /// Obtiene los pacientes inactivos
        /// </summary>
        /// <returns>Lista de pacientes inactivos</returns>
        Task<IEnumerable<Patient>> GetInactiveAsync();
    }
} 