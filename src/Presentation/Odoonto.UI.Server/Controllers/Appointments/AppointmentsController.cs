using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Odoonto.Application.DTOs.Appointments;
using Odoonto.Application.Services.Appointments;
using Odoonto.UI.Server.Controllers;

namespace Odoonto.UI.Server.Controllers.Appointments
{
    /// <summary>
    /// Controlador para gestión de citas odontológicas
    /// </summary>
    [ApiController]
    [Route("api/appointments")]
    public class AppointmentsController : BaseApiController
    {
        private readonly IAppointmentAppService _appointmentAppService;

        /// <summary>
        /// Constructor con inyección de dependencias
        /// </summary>
        public AppointmentsController(
            IAppointmentAppService appointmentAppService,
            ILogger<AppointmentsController> logger) : base(logger)
        {
            _appointmentAppService = appointmentAppService ?? throw new ArgumentNullException(nameof(appointmentAppService));
        }

        /// <summary>
        /// Obtiene todas las citas
        /// </summary>
        /// <returns>Lista de citas</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AppointmentDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            return await ExecuteAsync(
                () => _appointmentAppService.GetAllAsync(),
                "Error al obtener las citas"
            );
        }

        /// <summary>
        /// Obtiene una cita por su ID
        /// </summary>
        /// <param name="id">ID de la cita</param>
        /// <returns>Datos de la cita</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            return await ExecuteAsync(
                () => _appointmentAppService.GetByIdAsync(id),
                $"Error al obtener la cita con ID {id}"
            );
        }

        /// <summary>
        /// Obtiene las citas de un paciente en un rango de fechas
        /// </summary>
        /// <param name="patientId">ID del paciente</param>
        /// <param name="startDate">Fecha de inicio</param>
        /// <param name="endDate">Fecha de fin</param>
        /// <returns>Lista de citas del paciente</returns>
        [HttpGet("patient/{patientId}")]
        [ProducesResponseType(typeof(IEnumerable<AppointmentDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByPatient(
            Guid patientId, 
            [FromQuery] DateTime startDate, 
            [FromQuery] DateTime endDate)
        {
            return await ExecuteAsync(
                () => _appointmentAppService.GetByPatientAsync(patientId, startDate, endDate),
                "Error al obtener las citas del paciente"
            );
        }

        /// <summary>
        /// Obtiene las citas de un doctor en un rango de fechas
        /// </summary>
        /// <param name="doctorId">ID del doctor</param>
        /// <param name="startDate">Fecha de inicio</param>
        /// <param name="endDate">Fecha de fin</param>
        /// <returns>Lista de citas del doctor</returns>
        [HttpGet("doctor/{doctorId}")]
        [ProducesResponseType(typeof(IEnumerable<AppointmentDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByDoctor(
            Guid doctorId, 
            [FromQuery] DateTime startDate, 
            [FromQuery] DateTime endDate)
        {
            return await ExecuteAsync(
                () => _appointmentAppService.GetByDoctorAsync(doctorId, startDate, endDate),
                "Error al obtener las citas del doctor"
            );
        }

        /// <summary>
        /// Crea una nueva cita
        /// </summary>
        /// <param name="createAppointmentDto">Datos para la creación de la cita</param>
        /// <returns>ID de la cita creada</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentDto createAppointmentDto)
        {
            var id = await ExecuteAsync(
                () => _appointmentAppService.CreateAsync(createAppointmentDto),
                "Error al crear la cita"
            );

            return CreatedAtAction(nameof(GetById), new { id = id.Value }, id.Value);
        }

        /// <summary>
        /// Actualiza una cita existente
        /// </summary>
        /// <param name="id">ID de la cita</param>
        /// <param name="updateAppointmentDto">Datos actualizados de la cita</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAppointmentDto updateAppointmentDto)
        {
            return await ExecuteAsync(
                async () => await _appointmentAppService.UpdateAsync(id, updateAppointmentDto),
                $"Error al actualizar la cita con ID {id}"
            );
        }

        /// <summary>
        /// Elimina una cita
        /// </summary>
        /// <param name="id">ID de la cita</param>
        /// <returns>Resultado de la operación</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await ExecuteAsync(
                async () => await _appointmentAppService.DeleteAsync(id),
                $"Error al eliminar la cita con ID {id}"
            );
        }

        /// <summary>
        /// Cancela una cita
        /// </summary>
        /// <param name="id">ID de la cita</param>
        /// <param name="cancellationReason">Motivo de la cancelación</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPost("{id}/cancel")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Cancel(Guid id, [FromBody] string cancellationReason)
        {
            return await ExecuteAsync(
                async () => await _appointmentAppService.CancelAsync(id, cancellationReason),
                $"Error al cancelar la cita con ID {id}"
            );
        }

        /// <summary>
        /// Marca una cita como paciente en sala de espera
        /// </summary>
        /// <param name="id">ID de la cita</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPost("{id}/waiting-room")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MarkAsWaitingRoom(Guid id)
        {
            return await ExecuteAsync(
                async () => await _appointmentAppService.MarkAsWaitingRoomAsync(id),
                $"Error al marcar la cita {id} como 'en sala de espera'"
            );
        }

        /// <summary>
        /// Marca una cita como en progreso
        /// </summary>
        /// <param name="id">ID de la cita</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPost("{id}/in-progress")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MarkAsInProgress(Guid id)
        {
            return await ExecuteAsync(
                async () => await _appointmentAppService.MarkAsInProgressAsync(id),
                $"Error al marcar la cita {id} como 'en progreso'"
            );
        }

        /// <summary>
        /// Marca una cita como completada
        /// </summary>
        /// <param name="id">ID de la cita</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPost("{id}/complete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MarkAsCompleted(Guid id)
        {
            return await ExecuteAsync(
                async () => await _appointmentAppService.MarkAsCompletedAsync(id),
                $"Error al marcar la cita {id} como 'completada'"
            );
        }
    }
} 