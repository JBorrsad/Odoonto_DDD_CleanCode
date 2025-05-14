using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Odoonto.Application.DTOs.Odontograms;
using Odoonto.Application.Interfaces;
using Odoonto.UI.Server.Controllers;
using System;
using System.Threading.Tasks;

namespace Odoonto.UI.Server.Controllers.Odontograms
{
    /// <summary>
    /// Controlador para gestionar odontogramas
    /// </summary>
    public class OdontogramController : BaseApiController
    {
        private readonly IOdontogramService _odontogramService;

        public OdontogramController(
            IOdontogramService odontogramService,
            ILogger<OdontogramController> logger)
            : base(logger)
        {
            _odontogramService = odontogramService ?? throw new ArgumentNullException(nameof(odontogramService));
        }

        /// <summary>
        /// Obtiene el odontograma de un paciente
        /// </summary>
        /// <param name="patientId">ID del paciente</param>
        /// <returns>Odontograma del paciente</returns>
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatientId(Guid patientId)
        {
            try
            {
                var odontogram = await _odontogramService.GetByPatientIdAsync(patientId);
                if (odontogram == null)
                    return NotFound($"No se encontró odontograma para el paciente con ID {patientId}");

                return Ok(odontogram);
            }
            catch (Exception ex)
            {
                return HandleException(ex, $"Error al obtener el odontograma del paciente {patientId}");
            }
        }

        /// <summary>
        /// Crea un nuevo odontograma para un paciente
        /// </summary>
        /// <param name="patientId">ID del paciente</param>
        /// <returns>Odontograma creado</returns>
        [HttpPost("patient/{patientId}")]
        public async Task<IActionResult> Create(Guid patientId)
        {
            try
            {
                var odontogram = await _odontogramService.CreateOdontogramAsync(patientId);
                return Created($"/api/odontogram/patient/{patientId}", odontogram);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return HandleException(ex, $"Error al crear odontograma para el paciente {patientId}");
            }
        }

        /// <summary>
        /// Agrega un registro dental al odontograma
        /// </summary>
        /// <param name="odontogramId">ID del odontograma</param>
        /// <param name="toothRecordDto">Datos del registro dental</param>
        /// <returns>Odontograma actualizado</returns>
        [HttpPost("{odontogramId}/tooth")]
        public async Task<IActionResult> AddToothRecord(Guid odontogramId, [FromBody] CreateToothRecordDto toothRecordDto)
        {
            try
            {
                var odontogram = await _odontogramService.AddToothRecordAsync(odontogramId, toothRecordDto);
                return Ok(odontogram);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return HandleException(ex, $"Error al agregar registro dental al odontograma {odontogramId}");
            }
        }

        /// <summary>
        /// Registra una lesión en un diente específico
        /// </summary>
        /// <param name="odontogramId">ID del odontograma</param>
        /// <param name="toothNumber">Número del diente</param>
        /// <param name="lesionRecordDto">Datos de la lesión</param>
        /// <returns>Odontograma actualizado</returns>
        [HttpPost("{odontogramId}/tooth/{toothNumber}/lesion")]
        public async Task<IActionResult> AddLesionRecord(Guid odontogramId, int toothNumber, [FromBody] CreateLesionRecordDto lesionRecordDto)
        {
            try
            {
                var odontogram = await _odontogramService.AddLesionRecordAsync(odontogramId, toothNumber, lesionRecordDto);
                return Ok(odontogram);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return HandleException(ex, $"Error al agregar lesión al diente {toothNumber} del odontograma {odontogramId}");
            }
        }

        /// <summary>
        /// Registra un procedimiento realizado en un diente específico
        /// </summary>
        /// <param name="odontogramId">ID del odontograma</param>
        /// <param name="toothNumber">Número del diente</param>
        /// <param name="procedureDto">Datos del procedimiento</param>
        /// <returns>Odontograma actualizado</returns>
        [HttpPost("{odontogramId}/tooth/{toothNumber}/procedure")]
        public async Task<IActionResult> AddPerformedProcedure(Guid odontogramId, int toothNumber, [FromBody] CreatePerformedProcedureDto procedureDto)
        {
            try
            {
                var odontogram = await _odontogramService.AddPerformedProcedureAsync(odontogramId, toothNumber, procedureDto);
                return Ok(odontogram);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return HandleException(ex, $"Error al agregar procedimiento al diente {toothNumber} del odontograma {odontogramId}");
            }
        }
    }
}