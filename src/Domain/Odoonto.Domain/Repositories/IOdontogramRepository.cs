using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Odoonto.Domain.Core.Repositories;
using Odoonto.Domain.Models.Odontograms;

namespace Odoonto.Domain.Repositories
{
    /// <summary>
    /// Repositorio para la entidad Odontogram
    /// </summary>
    public interface IOdontogramRepository : IRepository<Odontogram>
    {
        /// <summary>
        /// Obtiene el odontograma de un paciente
        /// </summary>
        /// <param name="patientId">Identificador del paciente</param>
        /// <returns>Odontograma del paciente</returns>
        Task<Odontogram> GetByPatientIdAsync(Guid patientId);

        /// <summary>
        /// Guarda un odontograma completo
        /// </summary>
        /// <param name="odontogram">Odontograma a guardar</param>
        /// <returns>Id del odontograma guardado</returns>
        Task<Guid> SaveOdontogramAsync(Odontogram odontogram);

        /// <summary>
        /// Actualiza un registro dental específico
        /// </summary>
        /// <param name="odontogramId">Id del odontograma</param>
        /// <param name="toothRecord">Registro dental a actualizar</param>
        /// <returns>Tarea asíncrona</returns>
        Task UpdateToothRecordAsync(Guid odontogramId, ToothRecord toothRecord);

        /// <summary>
        /// Agrega una lesión a un diente específico
        /// </summary>
        /// <param name="odontogramId">Id del odontograma</param>
        /// <param name="toothNumber">Número del diente</param>
        /// <param name="lesionRecord">Registro de lesión a agregar</param>
        /// <returns>Tarea asíncrona</returns>
        Task AddLesionRecordAsync(Guid odontogramId, int toothNumber, LesionRecord lesionRecord);

        /// <summary>
        /// Agrega un procedimiento realizado a un diente específico
        /// </summary>
        /// <param name="odontogramId">Id del odontograma</param>
        /// <param name="toothNumber">Número del diente</param>
        /// <param name="performedProcedure">Procedimiento realizado a agregar</param>
        /// <returns>Tarea asíncrona</returns>
        Task AddPerformedProcedureAsync(Guid odontogramId, int toothNumber, PerformedProcedure performedProcedure);
    }
}