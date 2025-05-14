using Odoonto.Application.DTOs.Odontograms;
using System;
using System.Threading.Tasks;

namespace Odoonto.Application.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de odontogramas
    /// </summary>
    public interface IOdontogramService
    {
        /// <summary>
        /// Obtiene el odontograma de un paciente
        /// </summary>
        /// <param name="patientId">ID del paciente</param>
        /// <returns>Odontograma del paciente o null si no existe</returns>
        Task<OdontogramDto?> GetByPatientIdAsync(Guid patientId);

        /// <summary>
        /// Crea un nuevo odontograma para un paciente
        /// </summary>
        /// <param name="patientId">ID del paciente</param>
        /// <returns>Odontograma creado</returns>
        Task<OdontogramDto> CreateOdontogramAsync(Guid patientId);

        /// <summary>
        /// Agrega un registro dental al odontograma
        /// </summary>
        /// <param name="odontogramId">ID del odontograma</param>
        /// <param name="toothRecordDto">Datos del registro dental</param>
        /// <returns>Odontograma actualizado</returns>
        Task<OdontogramDto> AddToothRecordAsync(Guid odontogramId, CreateToothRecordDto toothRecordDto);

        /// <summary>
        /// Registra una lesión en un diente específico
        /// </summary>
        /// <param name="odontogramId">ID del odontograma</param>
        /// <param name="toothNumber">Número del diente</param>
        /// <param name="lesionRecordDto">Datos de la lesión</param>
        /// <returns>Odontograma actualizado</returns>
        Task<OdontogramDto> AddLesionRecordAsync(Guid odontogramId, int toothNumber, CreateLesionRecordDto lesionRecordDto);

        /// <summary>
        /// Registra un procedimiento realizado en un diente específico
        /// </summary>
        /// <param name="odontogramId">ID del odontograma</param>
        /// <param name="toothNumber">Número del diente</param>
        /// <param name="procedureDto">Datos del procedimiento</param>
        /// <returns>Odontograma actualizado</returns>
        Task<OdontogramDto> AddPerformedProcedureAsync(Guid odontogramId, int toothNumber, CreatePerformedProcedureDto procedureDto);
    }
}