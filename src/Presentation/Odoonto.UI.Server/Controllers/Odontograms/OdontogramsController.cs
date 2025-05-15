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
    [ApiController]
    [Route("api/odontograms")]
    public class OdontogramsController : BaseApiController
    {
        private readonly IOdontogramService _odontogramService;

        public OdontogramsController(
            IOdontogramService odontogramService,
            ILogger<OdontogramsController> logger)
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
            return await ExecuteAsync(async () => {
                var odontogram = await _odontogramService.GetByPatientIdAsync(patientId);
                if (odontogram == null)
                    throw new KeyNotFoundException($"No se encontró odontograma para el paciente con ID {patientId}");

                return odontogram;
            }, $"Error al obtener el odontograma del paciente {patientId}");
        }

        /// <summary>
        /// Crea un nuevo odontograma para un paciente
        /// </summary>
        /// <param name="patientId">ID del paciente</param>
        /// <returns>Odontograma creado</returns>
        [HttpPost("patient/{patientId}")]
        public async Task<IActionResult> Create(Guid patientId)
        {
            return await ExecuteAsync(async () => {
                var odontogram = await _odontogramService.CreateOdontogramAsync(patientId);
                return Created($"/api/odontograms/patient/{patientId}", odontogram);
            }, $"Error al crear odontograma para el paciente {patientId}");
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
            return await ExecuteAsync(async () => {
                var odontogram = await _odontogramService.AddToothRecordAsync(odontogramId, toothRecordDto);
                return Ok(odontogram);
            }, $"Error al agregar registro dental al odontograma {odontogramId}");
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
            return await ExecuteAsync(async () => {
                var odontogram = await _odontogramService.AddLesionRecordAsync(odontogramId, toothNumber, lesionRecordDto);
                return Ok(odontogram);
            }, $"Error al agregar lesión al diente {toothNumber} del odontograma {odontogramId}");
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
            return await ExecuteAsync(async () => {
                var odontogram = await _odontogramService.AddPerformedProcedureAsync(odontogramId, toothNumber, procedureDto);
                return Ok(odontogram);
            }, $"Error al agregar procedimiento al diente {toothNumber} del odontograma {odontogramId}");
        }
    }
} 