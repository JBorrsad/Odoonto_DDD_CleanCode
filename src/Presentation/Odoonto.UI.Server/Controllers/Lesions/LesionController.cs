using Microsoft.AspNetCore.Mvc;
using Odoonto.Application.DTOs.Lesions;
using Odoonto.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Odoonto.UI.Server.Controllers.Lesions
{
    [ApiController]
    [Route("api/[controller]")]
    public class LesionController : ControllerBase
    {
        private readonly ILesionService _lesionService;

        public LesionController(ILesionService lesionService)
        {
            _lesionService = lesionService ?? throw new ArgumentNullException(nameof(lesionService));
        }

        /// <summary>
        /// Obtiene todas las lesiones
        /// </summary>
        /// <returns>Lista de lesiones</returns>
        [HttpGet(Name = "GetAllLesions")]
        [ProducesResponseType(typeof(IEnumerable<LesionDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<LesionDto>>> GetAll()
        {
            var lesions = await _lesionService.GetAllLesionsAsync();
            return Ok(lesions);
        }

        /// <summary>
        /// Obtiene todas las lesiones activas
        /// </summary>
        /// <returns>Lista de lesiones activas</returns>
        [HttpGet("active", Name = "GetActiveLesions")]
        [ProducesResponseType(typeof(IEnumerable<LesionDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<LesionDto>>> GetActive()
        {
            var lesions = await _lesionService.GetActiveLesionsAsync();
            return Ok(lesions);
        }

        /// <summary>
        /// Obtiene lesiones por categoría
        /// </summary>
        /// <param name="category">Categoría de lesiones</param>
        /// <returns>Lista de lesiones de la categoría especificada</returns>
        [HttpGet("category/{category}", Name = "GetLesionsByCategory")]
        [ProducesResponseType(typeof(IEnumerable<LesionDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<LesionDto>>> GetByCategory(string category)
        {
            var lesions = await _lesionService.GetLesionsByCategoryAsync(category);
            return Ok(lesions);
        }

        /// <summary>
        /// Obtiene una lesión por su identificador
        /// </summary>
        /// <param name="id">Identificador de la lesión</param>
        /// <returns>Información de la lesión</returns>
        [HttpGet("{id}", Name = "GetLesionById")]
        [ProducesResponseType(typeof(LesionDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<LesionDto>> GetById(Guid id)
        {
            var lesion = await _lesionService.GetLesionByIdAsync(id);
            if (lesion == null)
            {
                return NotFound($"Lesión con ID {id} no encontrada.");
            }
            return Ok(lesion);
        }

        /// <summary>
        /// Crea una nueva lesión
        /// </summary>
        /// <param name="lesionDto">Información de la lesión</param>
        /// <returns>Lesión creada</returns>
        [HttpPost(Name = "CreateLesion")]
        [ProducesResponseType(typeof(LesionDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<LesionDto>> Create([FromBody] CreateLesionDto lesionDto)
        {
            var createdLesion = await _lesionService.CreateLesionAsync(lesionDto);
            return CreatedAtAction(nameof(GetById), new { id = createdLesion.Id }, createdLesion);
        }

        /// <summary>
        /// Actualiza una lesión existente
        /// </summary>
        /// <param name="id">Identificador de la lesión</param>
        /// <param name="lesionDto">Información actualizada de la lesión</param>
        /// <returns>Lesión actualizada</returns>
        [HttpPut("{id}", Name = "UpdateLesion")]
        [ProducesResponseType(typeof(LesionDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<LesionDto>> Update(Guid id, [FromBody] UpdateLesionDto lesionDto)
        {
            try
            {
                // Aseguramos que el ID en la ruta y en el cuerpo sean consistentes
                if (id != lesionDto.Id)
                {
                    return BadRequest("El ID en la ruta y en el cuerpo de la solicitud no coinciden.");
                }

                var updatedLesion = await _lesionService.UpdateLesionAsync(lesionDto);
                return Ok(updatedLesion);
            }
            catch (ApplicationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Activa una lesión
        /// </summary>
        /// <param name="id">Identificador de la lesión</param>
        /// <returns>Lesión activada</returns>
        [HttpPatch("{id}/activate", Name = "ActivateLesion")]
        [ProducesResponseType(typeof(LesionDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<LesionDto>> Activate(Guid id)
        {
            try
            {
                var activatedLesion = await _lesionService.ActivateLesionAsync(id);
                return Ok(activatedLesion);
            }
            catch (ApplicationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Desactiva una lesión
        /// </summary>
        /// <param name="id">Identificador de la lesión</param>
        /// <returns>Lesión desactivada</returns>
        [HttpPatch("{id}/deactivate", Name = "DeactivateLesion")]
        [ProducesResponseType(typeof(LesionDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<LesionDto>> Deactivate(Guid id)
        {
            try
            {
                var deactivatedLesion = await _lesionService.DeactivateLesionAsync(id);
                return Ok(deactivatedLesion);
            }
            catch (ApplicationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}