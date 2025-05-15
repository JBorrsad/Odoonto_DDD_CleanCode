using Microsoft.AspNetCore.Builder;
using Odoonto.Infrastructure.ExceptionsHandler.Middlewares;

namespace Odoonto.Infrastructure.ExceptionsHandler.Extensions
{
    /// <summary>
    /// Extensiones para registrar el middleware de manejo de excepciones
    /// </summary>
    public static class ExceptionMiddlewareExtensions
    {
        /// <summary>
        /// Agrega el middleware de manejo global de excepciones
        /// </summary>
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
} 