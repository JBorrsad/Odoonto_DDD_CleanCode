using Microsoft.AspNetCore.Builder;
using Odoonto.UI.Server.Middlewares;

namespace Odoonto.UI.Server.Extensions
{
    /// <summary>
    /// Extensiones para registrar middlewares personalizados
    /// </summary>
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// Agrega el middleware de registro de solicitudes
        /// </summary>
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestLoggingMiddleware>();
        }

        /// <summary>
        /// Agrega el middleware de manejo global de excepciones
        /// </summary>
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app, bool isDevelopment)
        {
            return app.UseMiddleware<GlobalExceptionMiddleware>(isDevelopment);
        }

        /// <summary>
        /// Agrega el middleware de autenticación JWT
        /// </summary>
        public static IApplicationBuilder UseCustomJwtAuthentication(this IApplicationBuilder app)
        {
            return app.UseMiddleware<AuthenticationMiddleware>();
        }
    }
} 