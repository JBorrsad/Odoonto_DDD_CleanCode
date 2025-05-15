using Microsoft.AspNetCore.Builder;
using Odoonto.Infrastructure.Logging.Middlewares;

namespace Odoonto.Infrastructure.Logging.Extensions
{
    /// <summary>
    /// Extensiones para registrar el middleware de logging
    /// </summary>
    public static class LoggingMiddlewareExtensions
    {
        /// <summary>
        /// Agrega el middleware de logging de solicitudes
        /// </summary>
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
} 