using Microsoft.AspNetCore.Mvc;
using Odoonto.Application.DTOs.Patients;
using Odoonto.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Odoonto.UI.Server.Controllers.Patients
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
        }

        /// <summary>
        /// Obtiene todos los pacientes
        /// </summary>
        /// <returns>Lista de pacientes</returns>
        [HttpGet(Name = "GetAllPatients")]
        [ProducesResponseType(typeof(IEnumerable<PatientDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetAll()
        {
            var patients = await _patientService.GetAllPatientsAsync();
            return Ok(patients);
        }

        /// <summary>
        /// Obtiene un paciente por su identificador
        /// </summary>
        /// <param name="id">Identificador del paciente</param>
        /// <returns>Información del paciente</returns>
        [HttpGet("{id}", Name = "GetPatientById")]
        [ProducesResponseType(typeof(PatientDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PatientDto>> GetById(Guid id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return NotFound($"Paciente con ID {id} no encontrado.");
            }
            return Ok(patient);
        }

        /// <summary>
        /// Crea un nuevo paciente
        /// </summary>
        /// <param name="patientDto">Información del paciente</param>
        /// <returns>Paciente creado</returns>
        [HttpPost(Name = "CreatePatient")]
        [ProducesResponseType(typeof(PatientDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PatientDto>> Create([FromBody] CreatePatientDto patientDto)
        {
            var createdPatient = await _patientService.CreatePatientAsync(patientDto);
            return CreatedAtAction(nameof(GetById), new { id = createdPatient.Id }, createdPatient);
        }

        /// <summary>
        /// Actualiza un paciente existente
        /// </summary>
        /// <param name="id">Identificador del paciente</param>
        /// <param name="patientDto">Información actualizada del paciente</param>
        /// <returns>Paciente actualizado</returns>
        [HttpPut("{id}", Name = "UpdatePatient")]
        [ProducesResponseType(typeof(PatientDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PatientDto>> Update(Guid id, [FromBody] UpdatePatientDto patientDto)
        {
            try
            {
                var updatedPatient = await _patientService.UpdatePatientAsync(id, patientDto);
                return Ok(updatedPatient);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Paciente con ID {id} no encontrado.");
            }
        }

        /// <summary>
        /// Busca pacientes por nombre o apellido
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda</param>
        /// <returns>Lista de pacientes que coinciden con la búsqueda</returns>
        [HttpGet("search", Name = "SearchPatients")]
        [ProducesResponseType(typeof(IEnumerable<PatientDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<PatientDto>>> Search([FromQuery] string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest("El término de búsqueda no puede estar vacío.");
            }

            var patients = await _patientService.SearchPatientsAsync(searchTerm);
            return Ok(patients);
        }

        /// <summary>
        /// Actualiza el historial médico de un paciente
        /// </summary>
        /// <param name="id">Identificador del paciente</param>
        /// <param name="medicalHistory">Historial médico actualizado</param>
        /// <returns>Paciente actualizado</returns>
        [HttpPatch("{id}/medical-history", Name = "UpdateMedicalHistory")]
        [ProducesResponseType(typeof(PatientDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PatientDto>> UpdateMedicalHistory(Guid id, [FromBody] string medicalHistory)
        {
            try
            {
                var updatedPatient = await _patientService.UpdateMedicalHistoryAsync(id, medicalHistory);
                return Ok(updatedPatient);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Paciente con ID {id} no encontrado.");
            }
        }

        /// <summary>
        /// Añade una alergia al paciente
        /// </summary>
        /// <param name="id">Identificador del paciente</param>
        /// <param name="allergy">Nombre de la alergia</param>
        /// <returns>Paciente actualizado</returns>
        [HttpPost("{id}/allergies", Name = "AddAllergy")]
        [ProducesResponseType(typeof(PatientDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PatientDto>> AddAllergy(Guid id, [FromBody] string allergy)
        {
            try
            {
                var updatedPatient = await _patientService.AddAllergyAsync(id, allergy);
                return Ok(updatedPatient);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Paciente con ID {id} no encontrado.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Elimina una alergia del paciente
        /// </summary>
        /// <param name="id">Identificador del paciente</param>
        /// <param name="allergy">Nombre de la alergia</param>
        /// <returns>Paciente actualizado</returns>
        [HttpDelete("{id}/allergies", Name = "RemoveAllergy")]
        [ProducesResponseType(typeof(PatientDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PatientDto>> RemoveAllergy(Guid id, [FromBody] string allergy)
        {
            try
            {
                var updatedPatient = await _patientService.RemoveAllergyAsync(id, allergy);
                return Ok(updatedPatient);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Paciente con ID {id} no encontrado.");
            }
        }
    }
}