using Microsoft.AspNetCore.Mvc;
using Odoonto.Application.DTOs.Doctors;
using Odoonto.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Odoonto.UI.Server.Controllers.Doctors
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService ?? throw new ArgumentNullException(nameof(doctorService));
        }

        /// <summary>
        /// Obtiene todos los doctores
        /// </summary>
        /// <returns>Lista de doctores</returns>
        [HttpGet(Name = "GetAllDoctors")]
        [ProducesResponseType(typeof(IEnumerable<DoctorDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<DoctorDto>>> GetAll()
        {
            var doctors = await _doctorService.GetAllDoctorsAsync();
            return Ok(doctors);
        }

        /// <summary>
        /// Obtiene un doctor por su identificador
        /// </summary>
        /// <param name="id">Identificador del doctor</param>
        /// <returns>Información del doctor</returns>
        [HttpGet("{id}", Name = "GetDoctorById")]
        [ProducesResponseType(typeof(DoctorDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<DoctorDto>> GetById(Guid id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            if (doctor == null)
            {
                return NotFound($"Doctor con ID {id} no encontrado.");
            }
            return Ok(doctor);
        }

        /// <summary>
        /// Crea un nuevo doctor
        /// </summary>
        /// <param name="doctorDto">Información del doctor</param>
        /// <returns>Doctor creado</returns>
        [HttpPost(Name = "CreateDoctor")]
        [ProducesResponseType(typeof(DoctorDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<DoctorDto>> Create([FromBody] CreateDoctorDto doctorDto)
        {
            var createdDoctor = await _doctorService.CreateDoctorAsync(doctorDto);
            return CreatedAtAction(nameof(GetById), new { id = createdDoctor.Id }, createdDoctor);
        }

        /// <summary>
        /// Actualiza un doctor existente
        /// </summary>
        /// <param name="id">Identificador del doctor</param>
        /// <param name="doctorDto">Información actualizada del doctor</param>
        /// <returns>Doctor actualizado</returns>
        [HttpPut("{id}", Name = "UpdateDoctor")]
        [ProducesResponseType(typeof(DoctorDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<DoctorDto>> Update(Guid id, [FromBody] UpdateDoctorDto doctorDto)
        {
            try
            {
                var updatedDoctor = await _doctorService.UpdateDoctorAsync(id, doctorDto);
                return Ok(updatedDoctor);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Doctor con ID {id} no encontrado.");
            }
        }

        /// <summary>
        /// Busca doctores por nombre o especialidad
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda</param>
        /// <returns>Lista de doctores que coinciden con la búsqueda</returns>
        [HttpGet("search", Name = "SearchDoctors")]
        [ProducesResponseType(typeof(IEnumerable<DoctorDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<DoctorDto>>> Search([FromQuery] string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest("El término de búsqueda no puede estar vacío.");
            }

            var doctors = await _doctorService.SearchDoctorsAsync(searchTerm);
            return Ok(doctors);
        }

        /// <summary>
        /// Obtiene doctores por especialidad
        /// </summary>
        /// <param name="specialty">Especialidad médica</param>
        /// <returns>Lista de doctores con la especialidad indicada</returns>
        [HttpGet("specialty/{specialty}", Name = "GetDoctorsBySpecialty")]
        [ProducesResponseType(typeof(IEnumerable<DoctorDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<DoctorDto>>> GetBySpecialty(string specialty)
        {
            var doctors = await _doctorService.GetDoctorsBySpecialtyAsync(specialty);
            return Ok(doctors);
        }

        /// <summary>
        /// Actualiza la disponibilidad de un doctor
        /// </summary>
        /// <param name="id">Identificador del doctor</param>
        /// <param name="isAvailable">Estado de disponibilidad</param>
        /// <returns>Doctor actualizado</returns>
        [HttpPatch("{id}/availability", Name = "UpdateAvailability")]
        [ProducesResponseType(typeof(DoctorDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<DoctorDto>> UpdateAvailability(Guid id, [FromBody] bool isAvailable)
        {
            try
            {
                var updatedDoctor = await _doctorService.UpdateAvailabilityAsync(id, isAvailable);
                return Ok(updatedDoctor);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Doctor con ID {id} no encontrado.");
            }
        }
    }
}