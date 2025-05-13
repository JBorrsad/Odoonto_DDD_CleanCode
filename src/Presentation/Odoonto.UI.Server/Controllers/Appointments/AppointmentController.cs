using Microsoft.AspNetCore.Mvc;
using Odoonto.Application.DTOs.Appointments;
using Odoonto.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Odoonto.UI.Server.Controllers.Appointments
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService ?? throw new ArgumentNullException(nameof(appointmentService));
        }

        /// <summary>
        /// Obtiene una cita por su identificador
        /// </summary>
        /// <param name="id">Identificador de la cita</param>
        /// <returns>Información de la cita</returns>
        [HttpGet("{id}", Name = "GetAppointmentById")]
        [ProducesResponseType(typeof(AppointmentDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<AppointmentDto>> GetById(Guid id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            return Ok(appointment);
        }

        /// <summary>
        /// Obtiene las citas de un paciente en un rango de fechas
        /// </summary>
        /// <param name="patientId">Identificador del paciente</param>
        /// <param name="startDate">Fecha de inicio (opcional)</param>
        /// <param name="endDate">Fecha de fin (opcional)</param>
        /// <returns>Lista de citas</returns>
        [HttpGet("patient/{patientId}", Name = "GetAppointmentsByPatient")]
        [ProducesResponseType(typeof(IEnumerable<AppointmentDto>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetByPatient(
            Guid patientId, 
            [FromQuery] DateTime? startDate = null, 
            [FromQuery] DateTime? endDate = null)
        {
            // Si no se proporcionan fechas, usar fechas predeterminadas
            var start = startDate ?? DateTime.Today;
            var end = endDate ?? DateTime.Today.AddMonths(3);

            var appointments = await _appointmentService.GetAppointmentsByPatientAsync(patientId, start, end);
            return Ok(appointments);
        }

        /// <summary>
        /// Obtiene las citas de un doctor en un rango de fechas
        /// </summary>
        /// <param name="doctorId">Identificador del doctor</param>
        /// <param name="startDate">Fecha de inicio (opcional)</param>
        /// <param name="endDate">Fecha de fin (opcional)</param>
        /// <returns>Lista de citas</returns>
        [HttpGet("doctor/{doctorId}", Name = "GetAppointmentsByDoctor")]
        [ProducesResponseType(typeof(IEnumerable<AppointmentDto>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetByDoctor(
            Guid doctorId, 
            [FromQuery] DateTime? startDate = null, 
            [FromQuery] DateTime? endDate = null)
        {
            // Si no se proporcionan fechas, usar fechas predeterminadas
            var start = startDate ?? DateTime.Today;
            var end = endDate ?? DateTime.Today.AddDays(7);

            var appointments = await _appointmentService.GetAppointmentsByDoctorAsync(doctorId, start, end);
            return Ok(appointments);
        }

        /// <summary>
        /// Crea una nueva cita
        /// </summary>
        /// <param name="appointmentDto">Información de la cita</param>
        /// <returns>Cita creada</returns>
        [HttpPost(Name = "CreateAppointment")]
        [ProducesResponseType(typeof(AppointmentDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<AppointmentDto>> Create([FromBody] CreateAppointmentDto appointmentDto)
        {
            var createdAppointment = await _appointmentService.CreateAppointmentAsync(appointmentDto);
            return CreatedAtAction(nameof(GetById), new { id = createdAppointment.Id }, createdAppointment);
        }

        /// <summary>
        /// Actualiza una cita existente
        /// </summary>
        /// <param name="id">Identificador de la cita</param>
        /// <param name="appointmentDto">Información actualizada de la cita</param>
        /// <returns>Cita actualizada</returns>
        [HttpPut("{id}", Name = "UpdateAppointment")]
        [ProducesResponseType(typeof(AppointmentDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<AppointmentDto>> Update(Guid id, [FromBody] CreateAppointmentDto appointmentDto)
        {
            var updatedAppointment = await _appointmentService.UpdateAppointmentAsync(id, appointmentDto);
            return Ok(updatedAppointment);
        }

        /// <summary>
        /// Cancela una cita
        /// </summary>
        /// <param name="id">Identificador de la cita</param>
        /// <returns>Confirmación de cancelación</returns>
        [HttpDelete("{id}", Name = "CancelAppointment")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> Cancel(Guid id)
        {
            await _appointmentService.CancelAppointmentAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Marca una cita como paciente en sala de espera
        /// </summary>
        /// <param name="id">Identificador de la cita</param>
        /// <returns>Cita actualizada</returns>
        [HttpPatch("{id}/waiting-room", Name = "MarkAsWaitingRoom")]
        [ProducesResponseType(typeof(AppointmentDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<AppointmentDto>> MarkAsWaitingRoom(Guid id)
        {
            var updatedAppointment = await _appointmentService.MarkAsWaitingRoomAsync(id);
            return Ok(updatedAppointment);
        }

        /// <summary>
        /// Marca una cita como en progreso
        /// </summary>
        /// <param name="id">Identificador de la cita</param>
        /// <returns>Cita actualizada</returns>
        [HttpPatch("{id}/in-progress", Name = "MarkAsInProgress")]
        [ProducesResponseType(typeof(AppointmentDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<AppointmentDto>> MarkAsInProgress(Guid id)
        {
            var updatedAppointment = await _appointmentService.MarkAsInProgressAsync(id);
            return Ok(updatedAppointment);
        }

        /// <summary>
        /// Marca una cita como completada
        /// </summary>
        /// <param name="id">Identificador de la cita</param>
        /// <returns>Cita actualizada</returns>
        [HttpPatch("{id}/completed", Name = "MarkAsCompleted")]
        [ProducesResponseType(typeof(AppointmentDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<AppointmentDto>> MarkAsCompleted(Guid id)
        {
            var updatedAppointment = await _appointmentService.MarkAsCompletedAsync(id);
            return Ok(updatedAppointment);
        }
    }
} 