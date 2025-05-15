using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Odoonto.UI.Server.Middlewares
{
    /// <summary>
    /// Middleware para la autenticación basada en tokens JWT
    /// </summary>
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthenticationMiddleware> _logger;

        /// <summary>
        /// Constructor del middleware
        /// </summary>
        public AuthenticationMiddleware(
            RequestDelegate next,
            ILogger<AuthenticationMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Procesa la solicitud HTTP y verifica autenticación
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (!string.IsNullOrEmpty(token))
                {
                    // Procesar token JWT
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = tokenHandler.ReadJwtToken(token);

                    // Crear claims para el usuario
                    var identity = new ClaimsIdentity(jwtToken.Claims, "Bearer");
                    context.User = new ClaimsPrincipal(identity);
                    
                    _logger.LogInformation("Usuario autenticado: {userId}", 
                        context.User.FindFirstValue(ClaimTypes.NameIdentifier));
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error al procesar token de autenticación");
                // Continuar sin autenticación
            }

            // Pasar al siguiente middleware
            await _next(context);
        }
    }
} 