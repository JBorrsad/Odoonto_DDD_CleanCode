using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Odoonto.Infrastructure.Logging.Middlewares
{
    /// <summary>
    /// Middleware para logging de solicitudes HTTP
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Generar ID único para la solicitud
            string requestId = Guid.NewGuid().ToString();
            context.TraceIdentifier = requestId;
            
            // Añadir a los headers de respuesta para correlación
            context.Response.Headers.Append("X-Request-ID", requestId);

            // Obtener información básica de la solicitud
            string method = context.Request.Method;
            string path = context.Request.Path;
            string query = context.Request.QueryString.ToString();
            string userAgent = context.Request.Headers["User-Agent"].ToString();
            string clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "desconocida";

            // Registrar inicio de la solicitud
            _logger.LogInformation("Solicitud {RequestId} iniciada: {Method} {Path}{Query} desde IP {ClientIp}",
                requestId, method, path, query, clientIp);

            // Medir tiempo de respuesta
            Stopwatch sw = Stopwatch.StartNew();

            // Capturar cuerpo de respuesta
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            // Continuar con el pipeline
            try
            {
                await _next(context);
                sw.Stop();

                // Registrar finalización exitosa
                _logger.LogInformation("Solicitud {RequestId} completada con estado {StatusCode} en {ElapsedMs}ms",
                    requestId, context.Response.StatusCode, sw.ElapsedMilliseconds);

                // Información adicional para solicitudes lentas
                if (sw.ElapsedMilliseconds > 500)
                {
                    _logger.LogWarning("Solicitud {RequestId} excedió el umbral de rendimiento: {ElapsedMs}ms",
                        requestId, sw.ElapsedMilliseconds);
                }

                // Capturar y analizar tamaño de respuesta
                responseBody.Position = 0;
                var responseContent = await new StreamReader(responseBody).ReadToEndAsync();
                
                // Si la respuesta es demasiado grande, sólo loguear su tamaño
                if (responseContent.Length > 1000)
                {
                    _logger.LogInformation("Solicitud {RequestId} generó una respuesta de {Size} bytes",
                        requestId, responseContent.Length);
                }

                // Copiar contenido de la respuesta al stream original
                responseBody.Position = 0;
                await responseBody.CopyToAsync(originalBodyStream);
            }
            catch (Exception ex)
            {
                sw.Stop();
                // Registrar error en la solicitud
                _logger.LogError(ex, "Solicitud {RequestId} falló después de {ElapsedMs}ms: {ErrorMessage}",
                    requestId, sw.ElapsedMilliseconds, ex.Message);
                
                // Permitir que el middleware de excepciones lo maneje
                throw;
            }
            finally
            {
                // Restaurar el stream original del cuerpo si no se hizo antes
                if (context.Response.Body != originalBodyStream)
                {
                    context.Response.Body = originalBodyStream;
                }
            }
        }
    }
} 