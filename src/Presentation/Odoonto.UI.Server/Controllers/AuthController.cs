using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Odoonto.UI.Server.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : BaseApiController
    {
        private readonly IConfiguration _configuration;

        public AuthController(ILogger<AuthController> logger, IConfiguration configuration)
            : base(logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Verifica el estado de la API y la conexión a Firebase
        /// </summary>
        /// <returns>Estado de la API</returns>
        [HttpGet("health")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(500)]
        public ActionResult<object> HealthCheck()
        {
            return Execute(() =>
            {
                return new
                {
                    status = "ok",
                    timestamp = DateTime.UtcNow,
                    version = "1.0.0",
                    environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"
                };
            }, "Error al verificar el estado de la API.");
        }
        
        /// <summary>
        /// Obtiene la configuración de Firebase para el cliente
        /// </summary>
        /// <returns>Configuración de Firebase para el cliente</returns>
        [HttpGet("firebase-config")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(500)]
        public ActionResult<object> GetFirebaseConfig()
        {
            return Execute(() =>
            {
                return new
                {
                    apiKey = _configuration["Firebase:ApiKey"],
                    authDomain = _configuration["Firebase:AuthDomain"],
                    projectId = _configuration["Firebase:ProjectId"],
                    storageBucket = _configuration["Firebase:StorageBucket"],
                    messagingSenderId = _configuration["Firebase:MessagingSenderId"],
                    appId = _configuration["Firebase:AppId"]
                };
            }, "Error al obtener la configuración de Firebase.");
        }
    }
} 