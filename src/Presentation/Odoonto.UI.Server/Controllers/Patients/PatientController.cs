using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Odoonto.Application.DTOs.Patients;
using Odoonto.Application.Interfaces;
using Odoonto.UI.Server.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Odoonto.UI.Server.Controllers.Patients
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : BaseApiController
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService, ILogger<PatientController> logger)
            : base(logger)
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
            return await ExecuteAsync(async () => await _patientService.GetAllPatientsAsync(),
                "Error al obtener los pacientes.");
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
            return await ExecuteAsync(async () =>
            {
                var patient = await _patientService.GetPatientByIdAsync(id);
                if (patient == null)
                {
                    throw new KeyNotFoundException($"Paciente con ID {id} no encontrado.");
                }
                return patient;
            }, $"Error al obtener el paciente con ID {id}.");
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
            return await ExecuteAsync(async () =>
            {
                var createdPatient = await _patientService.CreatePatientAsync(patientDto);
                return CreatedAtAction(nameof(GetById), new { id = createdPatient.Id }, createdPatient).Value;
            }, "Error al crear el paciente.");
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
        public async Task<ActionResult<PatientDto>> Update(Guid id, [FromBody] CreatePatientDto patientDto)
        {
            return await ExecuteAsync(async () => await _patientService.UpdatePatientAsync(id, patientDto),
                $"Error al actualizar el paciente con ID {id}.");
        }

        /// <summary>
        /// Busca pacientes por nombre o apellido
        /// </summary>
        /// <param name="searchTerm">Término de búsqueda</param>
        /// <returns>Lista de pacientes que coinciden con la búsqueda</returns>
        [HttpGet("search", Name = "SearchPatients")]
        [ProducesResponseType(typeof(IEnumerable<PatientDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<PatientDto>>> Search([FromQuery] string searchTerm)
        {
            return await ExecuteAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    throw new ArgumentException("El término de búsqueda no puede estar vacío.");
                }
                return await _patientService.SearchPatientsAsync(searchTerm);
            }, "Error al buscar pacientes.");
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
            return await ExecuteAsync(async () => await _patientService.UpdateMedicalHistoryAsync(id, medicalHistory),
                $"Error al actualizar el historial médico del paciente con ID {id}.");
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
            return await ExecuteAsync(async () => await _patientService.AddAllergyAsync(id, allergy),
                $"Error al añadir alergia al paciente con ID {id}.");
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
            return await ExecuteAsync(async () => await _patientService.RemoveAllergyAsync(id, allergy),
                $"Error al eliminar alergia del paciente con ID {id}.");
        }
    }
}