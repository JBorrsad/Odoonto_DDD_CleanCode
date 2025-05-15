using Microsoft.AspNetCore.Builder;
using Odoonto.Infrastructure.Authentication.Middlewares;

namespace Odoonto.Infrastructure.Authentication.Extensions
{
    /// <summary>
    /// Extensiones para registrar el middleware de autenticación con Firebase
    /// </summary>
    public static class AuthMiddlewareExtensions
    {
        /// <summary>
        /// Agrega el middleware de autenticación con Firebase
        /// </summary>
        public static IApplicationBuilder UseFirebaseAuthentication(this IApplicationBuilder app)
        {
            return app.UseMiddleware<FirebaseAuthMiddleware>();
        }
    }
} 