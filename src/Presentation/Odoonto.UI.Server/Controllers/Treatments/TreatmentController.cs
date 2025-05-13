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
            return Ok(treatment);
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
        /// Obtiene tratamientos por categoría
        /// </summary>
        /// <param name="category">Categoría a buscar</param>
        /// <returns>Lista de tratamientos en la categoría especificada</returns>
        [HttpGet("category/{category}", Name = "GetTreatmentsByCategory")]
        [ProducesResponseType(typeof(IEnumerable<TreatmentDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<TreatmentDto>>> GetByCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                return BadRequest("La categoría no puede estar vacía");
            }

            var treatments = await _treatmentService.GetTreatmentsByCategoryAsync(category);
            return Ok(treatments);
        }

        /// <summary>
        /// Busca tratamientos por término de búsqueda (nombre o descripción)
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda</param>
        /// <returns>Lista de tratamientos que coinciden con el término</returns>
        [HttpGet("search", Name = "SearchTreatments")]
        [ProducesResponseType(typeof(IEnumerable<TreatmentDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<TreatmentDto>>> Search([FromQuery] string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest("El término de búsqueda no puede estar vacío");
            }

            var treatments = await _treatmentService.SearchTreatmentsAsync(searchTerm);
            return Ok(treatments);
        }

        /// <summary>
        /// Busca tratamientos por precio máximo
        /// </summary>
        /// <param name="maxPrice">Precio máximo</param>
        /// <param name="currency">Moneda (por defecto EUR)</param>
        /// <returns>Lista de tratamientos con precio menor o igual al especificado</returns>
        [HttpGet("price", Name = "GetTreatmentsByMaxPrice")]
        [ProducesResponseType(typeof(IEnumerable<TreatmentDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<TreatmentDto>>> GetByMaxPrice(
            [FromQuery] decimal maxPrice, 
            [FromQuery] string currency = "EUR")
        {
            if (maxPrice < 0)
            {
                return BadRequest("El precio máximo no puede ser negativo");
            }

            var treatments = await _treatmentService.GetTreatmentsByMaxPriceAsync(maxPrice, currency);
            return Ok(treatments);
        }

        /// <summary>
        /// Busca tratamientos por duración máxima
        /// </summary>
        /// <param name="maxDuration">Duración máxima en minutos</param>
        /// <returns>Lista de tratamientos con duración menor o igual a la especificada</returns>
        [HttpGet("duration", Name = "GetTreatmentsByMaxDuration")]
        [ProducesResponseType(typeof(IEnumerable<TreatmentDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<TreatmentDto>>> GetByMaxDuration([FromQuery] int maxDuration)
        {
            if (maxDuration <= 0)
            {
                return BadRequest("La duración máxima debe ser mayor que cero");
            }

            var treatments = await _treatmentService.GetTreatmentsByMaxDurationAsync(maxDuration);
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
        public async Task<ActionResult<TreatmentDto>> Update(Guid id, [FromBody] CreateTreatmentDto treatmentDto)
        {
            var updatedTreatment = await _treatmentService.UpdateTreatmentAsync(id, treatmentDto);
            return Ok(updatedTreatment);
        }

        /// <summary>
        /// Elimina un tratamiento
        /// </summary>
        /// <param name="id">Identificador del tratamiento</param>
        /// <returns>Confirmación de eliminación</returns>
        [HttpDelete("{id}", Name = "DeleteTreatment")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _treatmentService.DeleteTreatmentAsync(id);
            return NoContent();
        }
    }
} 