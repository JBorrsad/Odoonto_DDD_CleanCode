using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Odoonto.Domain.Core.Exceptions;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Odoonto.UI.Server.Middlewares
{
    /// <summary>
    /// Middleware para manejo global de excepciones
    /// </summary>
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly bool _isDevelopment;

        /// <summary>
        /// Constructor del middleware
        /// </summary>
        public GlobalExceptionMiddleware(
            RequestDelegate next, 
            ILogger<GlobalExceptionMiddleware> logger,
            bool isDevelopment)
        {
            _next = next;
            _logger = logger;
            _isDevelopment = isDevelopment;
        }

        /// <summary>
        /// Procesa la solicitud HTTP
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no controlado: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            
            // Mapear excepciones del dominio a códigos HTTP
            var statusCode = exception switch
            {
                NotFoundException => HttpStatusCode.NotFound,
                ValidationException => HttpStatusCode.BadRequest,
                BusinessRuleViolationException => HttpStatusCode.UnprocessableEntity,
                UnauthorizedException => HttpStatusCode.Unauthorized,
                AuthorizationException => HttpStatusCode.Forbidden,
                _ => HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                status = statusCode.ToString(),
                message = exception.Message,
                stackTrace = _isDevelopment ? exception.StackTrace : null,
                errors = exception is ValidationException validationEx ? validationEx.Errors : null
            };

            var options = new JsonSerializerOptions 
            { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
            };
            
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }
} 