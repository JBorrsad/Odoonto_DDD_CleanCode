using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Odoonto.Application.DTOs.Treatments;
using Odoonto.Application.Interfaces;
using Odoonto.UI.Server.Controllers;

namespace Odoonto.UI.Server.Controllers.Treatments
{
    [ApiController]
    [Route("api/treatments")]
    public class TreatmentsController : BaseApiController
    {
        private readonly ITreatmentService _treatmentService;

        public TreatmentsController(
            ITreatmentService treatmentService,
            ILogger<TreatmentsController> logger)
            : base(logger)
        {
            _treatmentService = treatmentService ?? throw new ArgumentNullException(nameof(treatmentService));
        }

        /// <summary>
        /// Obtiene todos los tratamientos
        /// </summary>
        /// <returns>Lista de tratamientos</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TreatmentDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TreatmentDto>>> GetAll()
        {
            return await ExecuteAsync(async () => {
                return await _treatmentService.GetAllAsync();
            }, "Error al obtener todos los tratamientos");
        }

        /// <summary>
        /// Obtiene un tratamiento por su identificador
        /// </summary>
        /// <param name="id">Identificador del tratamiento</param>
        /// <returns>Información del tratamiento</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TreatmentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TreatmentDto>> GetById(Guid id)
        {
            return await ExecuteAsync(async () => {
                var treatment = await _treatmentService.GetByIdAsync(id);
                if (treatment == null)
                {
                    throw new KeyNotFoundException($"No se encontró un tratamiento con ID: {id}");
                }
                return treatment;
            }, $"Error al obtener el tratamiento con ID: {id}");
        }

        /// <summary>
        /// Obtiene los tratamientos de un paciente
        /// </summary>
        /// <param name="patientId">Identificador del paciente</param>
        /// <returns>Lista de tratamientos del paciente</returns>
        [HttpGet("patient/{patientId}")]
        [ProducesResponseType(typeof(IEnumerable<TreatmentDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TreatmentDto>>> GetByPatient(Guid patientId)
        {
            return await ExecuteAsync(async () => {
                return await _treatmentService.GetTreatmentsByPatientAsync(patientId);
            }, $"Error al obtener tratamientos del paciente con ID: {patientId}");
        }

        /// <summary>
        /// Obtiene los tratamientos asignados a un doctor
        /// </summary>
        /// <param name="doctorId">Identificador del doctor</param>
        /// <returns>Lista de tratamientos asignados al doctor</returns>
        [HttpGet("doctor/{doctorId}")]
        [ProducesResponseType(typeof(IEnumerable<TreatmentDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TreatmentDto>>> GetByDoctor(Guid doctorId)
        {
            return await ExecuteAsync(async () => {
                return await _treatmentService.GetTreatmentsByDoctorAsync(doctorId);
            }, $"Error al obtener tratamientos del doctor con ID: {doctorId}");
        }

        /// <summary>
        /// Obtiene tratamientos por categoría
        /// </summary>
        /// <param name="category">Categoría de tratamiento</param>
        /// <returns>Lista de tratamientos por categoría</returns>
        [HttpGet("by-category/{category}")]
        [ProducesResponseType(typeof(IEnumerable<TreatmentDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TreatmentDto>>> GetByCategory(string category)
        {
            return await ExecuteAsync(async () => {
                if (string.IsNullOrWhiteSpace(category))
                {
                    throw new ArgumentException("La categoría no puede estar vacía");
                }
                return await _treatmentService.GetByCategoryAsync(category);
            }, $"Error al obtener tratamientos por categoría: {category}");
        }

        /// <summary>
        /// Crea un nuevo tratamiento
        /// </summary>
        /// <param name="createTreatmentDto">Información del tratamiento</param>
        /// <returns>Tratamiento creado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TreatmentDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TreatmentDto>> Create([FromBody] CreateTreatmentDto createTreatmentDto)
        {
            return await ExecuteAsync(async () => {
                if (!ModelState.IsValid)
                {
                    throw new ArgumentException("Datos de tratamiento inválidos");
                }
                
                var createdTreatment = await _treatmentService.CreateTreatmentAsync(createTreatmentDto);
                return CreatedAtAction(nameof(GetById), new { id = createdTreatment.Id }, createdTreatment).Value;
            }, "Error al crear el tratamiento");
        }

        /// <summary>
        /// Actualiza un tratamiento existente
        /// </summary>
        /// <param name="id">Identificador del tratamiento</param>
        /// <param name="updateTreatmentDto">Información actualizada del tratamiento</param>
        /// <returns>Tratamiento actualizado</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TreatmentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TreatmentDto>> Update(Guid id, [FromBody] UpdateTreatmentDto updateTreatmentDto)
        {
            return await ExecuteAsync(async () => {
                if (!ModelState.IsValid)
                {
                    throw new ArgumentException("Datos de tratamiento inválidos");
                }

                if (id != updateTreatmentDto.Id)
                {
                    throw new ArgumentException("El ID de la ruta no coincide con el ID del tratamiento en el cuerpo de la solicitud");
                }

                var treatment = await _treatmentService.GetByIdAsync(id);
                if (treatment == null)
                {
                    throw new KeyNotFoundException($"No se encontró un tratamiento con ID: {id}");
                }

                return await _treatmentService.UpdateTreatmentAsync(updateTreatmentDto);
            }, $"Error al actualizar el tratamiento con ID: {id}");
        }

        /// <summary>
        /// Elimina un tratamiento
        /// </summary>
        /// <param name="id">Identificador del tratamiento</param>
        /// <returns>Resultado de la operación</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(Guid id)
        {
            return await ExecuteAsync(async () => {
                var treatment = await _treatmentService.GetByIdAsync(id);
                if (treatment == null)
                {
                    throw new KeyNotFoundException($"No se encontró un tratamiento con ID: {id}");
                }

                await _treatmentService.DeleteAsync(id);
                return NoContent();
            }, $"Error al eliminar el tratamiento con ID: {id}");
        }

        /// <summary>
        /// Marca un tratamiento como completado
        /// </summary>
        /// <param name="id">Identificador del tratamiento</param>
        /// <returns>Tratamiento actualizado</returns>
        [HttpPatch("{id}/complete")]
        [ProducesResponseType(typeof(TreatmentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TreatmentDto>> CompleteTreatment(Guid id)
        {
            return await ExecuteAsync(async () => {
                var treatment = await _treatmentService.GetByIdAsync(id);
                if (treatment == null)
                {
                    throw new KeyNotFoundException($"No se encontró un tratamiento con ID: {id}");
                }
                
                return await _treatmentService.CompleteTreatmentAsync(id);
            }, $"Error al marcar como completado el tratamiento con ID: {id}");
        }

        /// <summary>
        /// Cancela un tratamiento
        /// </summary>
        /// <param name="id">Identificador del tratamiento</param>
        /// <returns>Tratamiento actualizado</returns>
        [HttpPatch("{id}/cancel")]
        [ProducesResponseType(typeof(TreatmentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TreatmentDto>> CancelTreatment(Guid id)
        {
            return await ExecuteAsync(async () => {
                var treatment = await _treatmentService.GetByIdAsync(id);
                if (treatment == null)
                {
                    throw new KeyNotFoundException($"No se encontró un tratamiento con ID: {id}");
                }
                
                return await _treatmentService.CancelTreatmentAsync(id);
            }, $"Error al cancelar el tratamiento con ID: {id}");
        }
    }
} 