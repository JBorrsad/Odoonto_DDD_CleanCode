using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Odoonto.Domain.Core.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Odoonto.UI.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [Produces("application/json")]
    public abstract class BaseApiController : ControllerBase
    {
        protected readonly ILogger<BaseApiController> _logger;

        protected BaseApiController(ILogger<BaseApiController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected ActionResult HandleException(Exception ex, string errorMessage = null)
        {
            errorMessage = errorMessage ?? "Ha ocurrido un error al procesar la solicitud.";

            // Log del error con información detallada
            _logger.LogError(ex, "Error en la API: {ErrorMessage}. Detalles: {ExceptionType} - {ExceptionMessage}",
                errorMessage, ex.GetType().Name, ex.Message);

            // Mapeo de tipos de excepción a códigos de estado HTTP
            return ex switch
            {
                // 404 - Not Found
                EntityNotFoundException => NotFound(new { message = ex.Message, errorType = "NotFound" }),
                KeyNotFoundException => NotFound(new { message = ex.Message, errorType = "NotFound" }),
                
                // 400 - Bad Request
                ValidationException => BadRequest(new { message = ex.Message, errorType = "ValidationError" }),
                BusinessRuleException => BadRequest(new { message = ex.Message, errorType = "BusinessRuleViolation" }),
                ArgumentException => BadRequest(new { message = ex.Message, errorType = "InvalidArgument" }),
                FormatException => BadRequest(new { message = ex.Message, errorType = "InvalidFormat" }),
                InvalidOperationException => BadRequest(new { message = ex.Message, errorType = "InvalidOperation" }),
                
                // 401 - Unauthorized / 403 - Forbidden
                UnauthorizedAccessException => Forbid(),
                
                // 409 - Conflict
                DuplicateEntityException => Conflict(new { message = ex.Message, errorType = "DuplicateEntity" }),
                
                // 500 - Server Error (por defecto)
                _ => StatusCode(500, new { message = errorMessage, errorType = "ServerError" })
            };
        }

        protected async Task<ActionResult<T>> ExecuteAsync<T>(Func<Task<T>> action, string errorMessage = null)
        {
            try
            {
                var result = await action();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex, errorMessage);
            }
        }

        protected async Task<ActionResult> ExecuteAsync(Func<Task> action, string errorMessage = null)
        {
            try
            {
                await action();
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex, errorMessage);
            }
        }

        protected ActionResult<T> Execute<T>(Func<T> action, string errorMessage = null)
        {
            try
            {
                var result = action();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex, errorMessage);
            }
        }

        protected ActionResult Execute(Action action, string errorMessage = null)
        {
            try
            {
                action();
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex, errorMessage);
            }
        }
    }
}