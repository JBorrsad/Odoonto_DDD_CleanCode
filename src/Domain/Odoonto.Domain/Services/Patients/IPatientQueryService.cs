using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Odoonto.Domain.Models.Patients;

namespace Odoonto.Domain.Services.Patients
{
    /// <summary>
    /// Servicio de dominio para consultas específicas de pacientes
    /// </summary>
    public interface IPatientQueryService
    {
        /// <summary>
        /// Busca pacientes por su nombre
        /// </summary>
        Task<IEnumerable<Patient>> SearchByNameAsync(string name, int page = 1, int pageSize = 10);

        /// <summary>
        /// Busca pacientes por rango de edad
        /// </summary>
        Task<IEnumerable<Patient>> SearchByAgeRangeAsync(int minAge, int maxAge, int page = 1, int pageSize = 10);

        /// <summary>
        /// Busca un paciente por su correo electrónico
        /// </summary>
        Task<Patient> FindByEmailAsync(string email);

        /// <summary>
        /// Busca pacientes por número de teléfono
        /// </summary>
        Task<IEnumerable<Patient>> SearchByPhoneNumberAsync(string phoneNumber, int page = 1, int pageSize = 10);

        /// <summary>
        /// Realiza una búsqueda general en todos los campos del paciente
        /// </summary>
        Task<IEnumerable<Patient>> SearchPatientsAsync(string searchTerm, int page = 1, int pageSize = 10);

        /// <summary>
        /// Obtiene la lista paginada de todos los pacientes ordenados alfabéticamente
        /// </summary>
        Task<IEnumerable<Patient>> GetPaginatedPatientsAsync(int page = 1, int pageSize = 10);

        /// <summary>
        /// Obtiene el total de pacientes registrados
        /// </summary>
        Task<int> GetTotalPatientsCountAsync();

        /// <summary>
        /// Obtiene el total de resultados para una búsqueda específica
        /// </summary>
        Task<int> CountSearchResultsAsync(string searchTerm);
    }
}