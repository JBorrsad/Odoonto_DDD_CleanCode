using Microsoft.AspNetCore.Mvc;
using Odoonto.Application.DTOs.Treatments;
using Odoonto.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Odoonto.UI.Server.Controllers.Treatments
{
    [ApiController]
    [Route("api/[controller]")]
    public class TreatmentController : ControllerBase
    {
        private readonly ITreatmentService _treatmentService;

        public TreatmentController(ITreatmentService treatmentService)
        {
            _treatmentService = treatmentService ?? throw new ArgumentNullException(nameof(treatmentService));
        }

        /// <summary>
        /// Obtiene todos los tratamientos
        /// </summary>
        /// <returns>Lista de tratamientos</returns>
        [HttpGet(Name = "GetAllTreatments")]
        [ProducesResponseType(typeof(IEnumerable<TreatmentDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<TreatmentDto>>> GetAll()
        {
            var treatments = await _treatmentService.GetAllTreatmentsAsync();
            return Ok(treatments);
        }

        /// <summary>
        /// Obtiene un tratamiento por su identificador
        /// </summary>
        /// <param name="id">Identificador del tratamiento</param>
        /// <returns>Información del tratamiento</returns>
        [HttpGet("{id}", Name = "GetTreatmentById")]
        [ProducesResponseType(typeof(TreatmentDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TreatmentDto>> GetById(Guid id)
        {
            var treatment = await _treatmentService.GetTreatmentByIdAsync(id);
            if (treatment == null)
            {
                return NotFound($"Tratamiento con ID {id} no encontrado.");
            }
            return Ok(treatment);
        }

        /// <summary>
        /// Obtiene los tratamientos de un paciente
        /// </summary>
        /// <param name="patientId">Identificador del paciente</param>
        /// <returns>Lista de tratamientos del paciente</returns>
        [HttpGet("patient/{patientId}", Name = "GetTreatmentsByPatient")]
        [ProducesResponseType(typeof(IEnumerable<TreatmentDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<TreatmentDto>>> GetByPatient(Guid patientId)
        {
            var treatments = await _treatmentService.GetTreatmentsByPatientAsync(patientId);
            return Ok(treatments);
        }

        /// <summary>
        /// Obtiene los tratamientos asignados a un doctor
        /// </summary>
        /// <param name="doctorId">Identificador del doctor</param>
        /// <returns>Lista de tratamientos asignados al doctor</returns>
        [HttpGet("doctor/{doctorId}", Name = "GetTreatmentsByDoctor")]
        [ProducesResponseType(typeof(IEnumerable<TreatmentDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<TreatmentDto>>> GetByDoctor(Guid doctorId)
        {
            var treatments = await _treatmentService.GetTreatmentsByDoctorAsync(doctorId);
            return Ok(treatments);
        }

        /// <summary>
        /// Crea un nuevo tratamiento
        /// </summary>
        /// <param name="treatmentDto">Información del tratamiento</param>
        /// <returns>Tratamiento creado</returns>
        [HttpPost(Name = "CreateTreatment")]
        [ProducesResponseType(typeof(TreatmentDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TreatmentDto>> Create([FromBody] CreateTreatmentDto treatmentDto)
        {
            var createdTreatment = await _treatmentService.CreateTreatmentAsync(treatmentDto);
            return CreatedAtAction(nameof(GetById), new { id = createdTreatment.Id }, createdTreatment);
        }

        /// <summary>
        /// Actualiza un tratamiento existente
        /// </summary>
        /// <param name="id">Identificador del tratamiento</param>
        /// <param name="treatmentDto">Información actualizada del tratamiento</param>
        /// <returns>Tratamiento actualizado</returns>
        [HttpPut("{id}", Name = "UpdateTreatment")]
        [ProducesResponseType(typeof(TreatmentDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TreatmentDto>> Update(Guid id, [FromBody] UpdateTreatmentDto treatmentDto)
        {
            try
            {
                if (id != treatmentDto.Id)
                {
                    return BadRequest("El ID en la ruta y en el cuerpo de la solicitud no coinciden.");
                }

                var updatedTreatment = await _treatmentService.UpdateTreatmentAsync(treatmentDto);
                return Ok(updatedTreatment);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Tratamiento con ID {id} no encontrado.");
            }
        }

        /// <summary>
        /// Marca un tratamiento como completado
        /// </summary>
        /// <param name="id">Identificador del tratamiento</param>
        /// <returns>Tratamiento actualizado</returns>
        [HttpPatch("{id}/complete", Name = "CompleteTreatment")]
        [ProducesResponseType(typeof(TreatmentDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TreatmentDto>> CompleteTreatment(Guid id)
        {
            try
            {
                var updatedTreatment = await _treatmentService.CompleteTreatmentAsync(id);
                return Ok(updatedTreatment);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Tratamiento con ID {id} no encontrado.");
            }
        }

        /// <summary>
        /// Cancela un tratamiento
        /// </summary>
        /// <param name="id">Identificador del tratamiento</param>
        /// <returns>Tratamiento actualizado</returns>
        [HttpPatch("{id}/cancel", Name = "CancelTreatment")]
        [ProducesResponseType(typeof(TreatmentDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TreatmentDto>> CancelTreatment(Guid id)
        {
            try
            {
                var updatedTreatment = await _treatmentService.CancelTreatmentAsync(id);
                return Ok(updatedTreatment);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Tratamiento con ID {id} no encontrado.");
            }
        }
    }
}