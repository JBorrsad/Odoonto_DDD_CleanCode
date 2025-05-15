using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Odoonto.Domain.Core.Models.Exceptions;

namespace Odoonto.Infrastructure.ExceptionsHandler.Middlewares
{
    /// <summary>
    /// Middleware para manejo centralizado de excepciones
    /// </summary>
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger,
            IWebHostEnvironment env)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = env ?? throw new ArgumentNullException(nameof(env));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "Error no controlado: {Message}", exception.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)GetStatusCode(exception);

            var response = CreateErrorResponse(exception);
            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions 
            { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = _env.IsDevelopment()
            });

            await context.Response.WriteAsync(json);
        }

        private HttpStatusCode GetStatusCode(Exception exception)
        {
            // Mapear tipos de excepciones a códigos de estado HTTP
            return exception switch
            {
                EntityNotFoundException => HttpStatusCode.NotFound,
                ValidationException => HttpStatusCode.BadRequest,
                BusinessRuleException => HttpStatusCode.Conflict,
                InvalidOperationException => HttpStatusCode.BadRequest,
                UnauthorizedAccessException => HttpStatusCode.Forbidden,
                ArgumentException => HttpStatusCode.BadRequest,
                // Si no se encuentra el tipo, devolver error interno
                _ => HttpStatusCode.InternalServerError
            };
        }

        private object CreateErrorResponse(Exception exception)
        {
            // En entorno de desarrollo incluir detalles adicionales
            if (_env.IsDevelopment())
            {
                return new
                {
                    message = exception.Message,
                    type = exception.GetType().Name,
                    stackTrace = exception.StackTrace,
                    innerException = exception.InnerException?.Message
                };
            }

            // En entorno de producción, mensajes simplificados
            return new
            {
                message = GetSafeErrorMessage(exception),
                type = exception.GetType().Name
            };
        }

        private string GetSafeErrorMessage(Exception exception)
        {
            // Personalizar mensajes según el tipo de excepción
            return exception switch
            {
                EntityNotFoundException ex => ex.Message,
                ValidationException ex => ex.Message,
                BusinessRuleException ex => ex.Message,
                // Para otros tipos, usar mensajes genéricos en producción para no revelar detalles internos
                _ => "Se ha producido un error interno. Por favor, inténtelo de nuevo más tarde."
            };
        }
    }
} 