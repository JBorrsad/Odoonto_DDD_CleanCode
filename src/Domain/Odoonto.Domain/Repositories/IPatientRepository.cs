using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Odoonto.Domain.Core.Repositories;
using Odoonto.Domain.Models.Patients;

namespace Odoonto.Domain.Repositories
{
    /// <summary>
    /// Repositorio para la entidad Patient
    /// </summary>
    public interface IPatientRepository : IRepository<Patient>
    {
        /// <summary>
        /// Obtiene la cantidad total de pacientes
        /// </summary>
        /// <returns>El número total de pacientes</returns>
        Task<int> GetTotalPatientsCountAsync();

        /// <summary>
        /// Obtiene pacientes con paginación simple ordenados alfabéticamente
        /// </summary>
        /// <param name="pageNumber">Número de página (1-based)</param>
        /// <param name="pageSize">Tamaño de página</param>
        /// <returns>Lista paginada de pacientes</returns>
        Task<IEnumerable<Patient>> GetPaginatedAsync(int pageNumber, int pageSize);
    }
}