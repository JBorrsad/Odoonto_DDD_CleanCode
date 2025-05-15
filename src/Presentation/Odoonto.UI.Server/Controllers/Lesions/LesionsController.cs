using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Odoonto.Application.DTOs.Lesions;
using Odoonto.Application.Interfaces;
using Odoonto.UI.Server.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Odoonto.UI.Server.Controllers.Lesions
{
    /// <summary>
    /// Controlador para gestionar lesiones dentales
    /// </summary>
    [ApiController]
    [Route("api/lesions")]
    public class LesionsController : BaseApiController
    {
        private readonly ILesionService _lesionService;

        public LesionsController(
            ILesionService lesionService,
            ILogger<LesionsController> logger)
            : base(logger)
        {
            _lesionService = lesionService ?? throw new ArgumentNullException(nameof(lesionService));
        }

        /// <summary>
        /// Obtiene todas las categorías de lesiones
        /// </summary>
        /// <returns>Lista de categorías de lesiones</returns>
        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<LesionCategoryDto>>> GetAllCategories()
        {
            return await ExecuteAsync(async () => {
                return await _lesionService.GetAllCategoriesAsync();
            }, "Error al obtener las categorías de lesiones");
        }

        /// <summary>
        /// Obtiene todas las lesiones
        /// </summary>
        /// <returns>Lista de lesiones</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LesionDto>>> GetAll()
        {
            return await ExecuteAsync(async () => {
                return await _lesionService.GetAllAsync();
            }, "Error al obtener las lesiones");
        }

        /// <summary>
        /// Obtiene una lesión por su identificador
        /// </summary>
        /// <param name="id">Identificador de la lesión</param>
        /// <returns>Lesión</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<LesionDto>> GetById(Guid id)
        {
            return await ExecuteAsync(async () => {
                var lesion = await _lesionService.GetByIdAsync(id);
                if (lesion == null)
                {
                    throw new KeyNotFoundException($"No se encontró la lesión con ID {id}");
                }
                return lesion;
            }, $"Error al obtener la lesión con ID {id}");
        }

        /// <summary>
        /// Obtiene lesiones por categoría
        /// </summary>
        /// <param name="categoryId">ID de la categoría</param>
        /// <returns>Lista de lesiones de la categoría</returns>
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<LesionDto>>> GetByCategory(Guid categoryId)
        {
            return await ExecuteAsync(async () => {
                return await _lesionService.GetByCategoryAsync(categoryId);
            }, $"Error al obtener lesiones de la categoría con ID {categoryId}");
        }

        /// <summary>
        /// Crea una nueva lesión
        /// </summary>
        /// <param name="createLesionDto">Datos de la lesión</param>
        /// <returns>Lesión creada</returns>
        [HttpPost]
        public async Task<ActionResult<LesionDto>> Create([FromBody] CreateLesionDto createLesionDto)
        {
            return await ExecuteAsync(async () => {
                var createdLesion = await _lesionService.CreateAsync(createLesionDto);
                return CreatedAtAction(nameof(GetById), new { id = createdLesion.Id }, createdLesion);
            }, "Error al crear la lesión");
        }

        /// <summary>
        /// Actualiza una lesión existente
        /// </summary>
        /// <param name="id">ID de la lesión</param>
        /// <param name="updateLesionDto">Datos actualizados</param>
        /// <returns>Lesión actualizada</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<LesionDto>> Update(Guid id, [FromBody] UpdateLesionDto updateLesionDto)
        {
            return await ExecuteAsync(async () => {
                if (id != updateLesionDto.Id)
                {
                    throw new ArgumentException("El ID de la ruta no coincide con el ID en el cuerpo de la solicitud");
                }

                var lesion = await _lesionService.GetByIdAsync(id);
                if (lesion == null)
                {
                    throw new KeyNotFoundException($"No se encontró la lesión con ID {id}");
                }

                return await _lesionService.UpdateAsync(updateLesionDto);
            }, $"Error al actualizar la lesión con ID {id}");
        }

        /// <summary>
        /// Elimina una lesión
        /// </summary>
        /// <param name="id">ID de la lesión</param>
        /// <returns>Resultado de la operación</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            return await ExecuteAsync(async () => {
                var lesion = await _lesionService.GetByIdAsync(id);
                if (lesion == null)
                {
                    throw new KeyNotFoundException($"No se encontró la lesión con ID {id}");
                }

                await _lesionService.DeleteAsync(id);
                return NoContent();
            }, $"Error al eliminar la lesión con ID {id}");
        }

        /// <summary>
        /// Crea una nueva categoría de lesiones
        /// </summary>
        /// <param name="createCategoryDto">Datos de la categoría</param>
        /// <returns>Categoría creada</returns>
        [HttpPost("categories")]
        public async Task<ActionResult<LesionCategoryDto>> CreateCategory([FromBody] CreateLesionCategoryDto createCategoryDto)
        {
            return await ExecuteAsync(async () => {
                var createdCategory = await _lesionService.CreateCategoryAsync(createCategoryDto);
                return createdCategory;
            }, "Error al crear la categoría de lesiones");
        }
    }
} 