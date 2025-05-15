using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Odoonto.UI.Server.Middlewares
{
    /// <summary>
    /// Middleware para registro de solicitudes HTTP
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        /// <summary>
        /// Constructor del middleware
        /// </summary>
        public RequestLoggingMiddleware(
            RequestDelegate next, 
            ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Procesa la solicitud HTTP y registra información relevante
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            var requestId = Guid.NewGuid().ToString();
            var requestInfo = $"{context.Request.Method} {context.Request.Path}";
            var stopwatch = Stopwatch.StartNew();

            // Registrar inicio de solicitud
            _logger.LogInformation($"Inicio solicitud {requestId}: {requestInfo}");

            // Agregar headers de correlación para seguimiento
            context.Response.Headers.Add("X-Request-Id", requestId);
            
            try
            {
                // Pasar al siguiente middleware
                await _next(context);
                
                // Registrar finalización exitosa
                stopwatch.Stop();
                _logger.LogInformation($"Fin solicitud {requestId}: {context.Response.StatusCode} en {stopwatch.ElapsedMilliseconds}ms");
            }
            catch (Exception ex)
            {
                // Registrar error (pero dejar que el middleware de excepciones lo maneje)
                stopwatch.Stop();
                _logger.LogError(ex, $"Error en solicitud {requestId} después de {stopwatch.ElapsedMilliseconds}ms: {ex.Message}");
                throw; // Re-lanzar para que lo maneje el middleware de excepciones
            }
        }
    }
} 