using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Odoonto.UI.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        protected readonly ILogger<BaseApiController> _logger;

        protected BaseApiController(ILogger<BaseApiController> logger)
        {
            _logger = logger;
        }

        protected ActionResult HandleException(Exception ex, string errorMessage = null)
        {
            errorMessage = errorMessage ?? "Ha ocurrido un error al procesar la solicitud.";

            // Log del error
            _logger.LogError(ex, errorMessage);

            if (ex is KeyNotFoundException)
            {
                return NotFound(new { message = ex.Message });
            }

            if (ex is ArgumentException || ex is FormatException || ex is InvalidOperationException)
            {
                return BadRequest(new { message = ex.Message });
            }

            // Error por defecto
            return StatusCode(500, new { message = errorMessage });
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