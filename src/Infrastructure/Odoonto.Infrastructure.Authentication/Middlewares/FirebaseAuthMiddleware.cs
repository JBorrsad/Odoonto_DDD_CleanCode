using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Odoonto.Infrastructure.Authentication.Middlewares
{
    /// <summary>
    /// Middleware para autenticación con Firebase
    /// </summary>
    public class FirebaseAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<FirebaseAuthMiddleware> _logger;

        public FirebaseAuthMiddleware(RequestDelegate next, ILogger<FirebaseAuthMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string authHeader = context.Request.Headers["Authorization"];

            // Si no hay header de autorización o no es un Bearer token, continuar sin autenticar
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                await _next(context);
                return;
            }

            string token = authHeader.Substring("Bearer ".Length);

            try
            {
                // Verificar el token con Firebase Admin SDK
                FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
                
                // Crear identidad para el usuario autenticado
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, decodedToken.Uid),
                    new Claim(ClaimTypes.Email, decodedToken.Claims.TryGetValue("email", out var email) ? email.ToString() : ""),
                    new Claim(ClaimTypes.Name, decodedToken.Claims.TryGetValue("name", out var name) ? name.ToString() : "")
                };

                // Agregar roles si están presentes en el token
                if (decodedToken.Claims.TryGetValue("role", out var role))
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                }

                // Agregar reclamaciones personalizadas
                foreach (var claim in decodedToken.Claims)
                {
                    if (!claim.Key.Equals("email") && !claim.Key.Equals("name") && !claim.Key.Equals("role"))
                    {
                        claims.Add(new Claim(claim.Key, claim.Value.ToString()));
                    }
                }

                var identity = new ClaimsIdentity(claims, "Firebase");
                context.User = new ClaimsPrincipal(identity);

                _logger.LogInformation("Usuario autenticado: {UserId}", decodedToken.Uid);
            }
            catch (FirebaseAuthException ex)
            {
                _logger.LogWarning(ex, "Error al validar token de Firebase: {Message}", ex.Message);
                // No interrumpir el pipeline, simplemente continuar sin autenticar
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado durante autenticación: {Message}", ex.Message);
                // No interrumpir el pipeline, simplemente continuar sin autenticar
            }

            // Continuar con el siguiente middleware
            await _next(context);
        }
    }
} 