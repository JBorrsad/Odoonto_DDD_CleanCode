using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Odoonto.Application.DTO.Doctors;
using Odoonto.Application.Interfaces.Doctors;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Odoonto.UI.Server.Controllers.Doctors
{
    /// <summary>
    /// Controlador para gestión de doctores
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DoctorsController : BaseApiController
    {
        private readonly IDoctorAppService _doctorAppService;

        /// <summary>
        /// Constructor con inyección de dependencias
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="doctorAppService">Servicio de aplicación para doctores</param>
        public DoctorsController(ILogger<DoctorsController> logger, IDoctorAppService doctorAppService)
            : base(logger)
        {
            _doctorAppService = doctorAppService ?? throw new ArgumentNullException(nameof(doctorAppService));
        }

        /// <summary>
        /// Obtiene todos los doctores
        /// </summary>
        /// <returns>Lista de doctores</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DoctorDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<DoctorDto>>> GetAll()
        {
            return await ExecuteAsync(async () =>
            {
                return await _doctorAppService.GetAllAsync();
            }, "Error al obtener el listado de doctores");
        }

        /// <summary>
        /// Obtiene un doctor por su ID
        /// </summary>
        /// <param name="id">ID del doctor</param>
        /// <returns>Doctor encontrado</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DoctorDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<DoctorDto>> GetById(Guid id)
        {
            return await ExecuteAsync(async () =>
            {
                return await _doctorAppService.GetByIdAsync(id);
            }, $"Error al obtener el doctor con ID {id}");
        }

        /// <summary>
        /// Crea un nuevo doctor
        /// </summary>
        /// <param name="doctorDto">Datos del doctor</param>
        /// <returns>ID del nuevo doctor</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Guid>> Create(CreateDoctorDto doctorDto)
        {
            var id = await ExecuteAsync(async () =>
            {
                return await _doctorAppService.CreateAsync(doctorDto);
            }, "Error al crear el doctor");

            return CreatedAtAction(nameof(GetById), new { id = id.Value }, id.Value);
        }

        /// <summary>
        /// Actualiza un doctor existente
        /// </summary>
        /// <param name="id">ID del doctor</param>
        /// <param name="doctorDto">Datos actualizados</param>
        /// <returns>Sin contenido</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> Update(Guid id, UpdateDoctorDto doctorDto)
        {
            await ExecuteAsync(async () =>
            {
                await _doctorAppService.UpdateAsync(id, doctorDto);
            }, $"Error al actualizar el doctor con ID {id}");

            return NoContent();
        }

        /// <summary>
        /// Elimina un doctor
        /// </summary>
        /// <param name="id">ID del doctor</param>
        /// <returns>Sin contenido</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> Delete(Guid id)
        {
            await ExecuteAsync(async () =>
            {
                await _doctorAppService.DeleteAsync(id);
            }, $"Error al eliminar el doctor con ID {id}");

            return NoContent();
        }

        /// <summary>
        /// Obtiene doctores por especialidad
        /// </summary>
        /// <param name="specialty">Especialidad médica</param>
        /// <returns>Lista de doctores con la especialidad especificada</returns>
        [HttpGet("specialty/{specialty}")]
        [ProducesResponseType(typeof(IEnumerable<DoctorDto>), 200)]
        public async Task<ActionResult<IEnumerable<DoctorDto>>> GetBySpecialty(string specialty)
        {
            return await ExecuteAsync(async () => {
                var doctors = await _doctorAppService.GetBySpecialtyAsync(specialty);
                return doctors;
            }, $"Error al obtener doctores por especialidad: {specialty}");
        }

        /// <summary>
        /// Verifica disponibilidad de un doctor en una fecha y hora específicas
        /// </summary>
        /// <param name="id">ID del doctor</param>
        /// <param name="date">Fecha (formato: yyyy-MM-dd)</param>
        /// <param name="startHour">Hora de inicio (formato: HH:mm)</param>
        /// <param name="endHour">Hora de fin (formato: HH:mm)</param>
        /// <returns>True si está disponible, False si no</returns>
        [HttpGet("{id}/availability")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<bool>> CheckAvailability(
            Guid id, 
            [FromQuery] DateTime date, 
            [FromQuery] string startHour, 
            [FromQuery] string endHour)
        {
            return await ExecuteAsync(async () => {
                if (!TimeOnly.TryParse(startHour, out var startTime) || 
                    !TimeOnly.TryParse(endHour, out var endTime))
                {
                    return BadRequest(new { message = "Formato de hora inválido. Use HH:mm" });
                }

                var isAvailable = await _doctorAppService.CheckAvailabilityAsync(id, date, startTime, endTime);
                return isAvailable;
            }, $"Error al verificar disponibilidad del doctor con ID: {id}");
        }

        /// <summary>
        /// Establece un nuevo horario de disponibilidad para un doctor
        /// </summary>
        /// <param name="id">ID del doctor</param>
        /// <param name="day">Día de la semana (0=Domingo, 1=Lunes, ...)</param>
        /// <param name="startHour">Hora de inicio (formato: HH:mm)</param>
        /// <param name="endHour">Hora de fin (formato: HH:mm)</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPost("{id}/availability")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> SetAvailability(
            Guid id,
            [FromQuery] int day,
            [FromQuery] string startHour,
            [FromQuery] string endHour)
        {
            return await ExecuteAsync(async () => {
                if (day < 0 || day > 6)
                {
                    return BadRequest(new { message = "Día inválido. Debe ser un valor entre 0 (Domingo) y 6 (Sábado)" });
                }

                if (!TimeOnly.TryParse(startHour, out var startTime) ||
                    !TimeOnly.TryParse(endHour, out var endTime))
                {
                    return BadRequest(new { message = "Formato de hora inválido. Use HH:mm" });
                }

                await _doctorAppService.SetAvailabilityAsync(id, (DayOfWeek)day, startTime, endTime);
                return NoContent();
            }, $"Error al establecer disponibilidad del doctor con ID: {id}");
        }
    }
} 