using System.Threading.Tasks;

namespace Odoonto.Infrastructure.Configuration.Auth
{
    /// <summary>
    /// Interfaz para el servicio de autenticación
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Autentica al usuario con email y contraseña
        /// </summary>
        Task<AuthResult> SignInWithEmailAndPasswordAsync(string email, string password);

        /// <summary>
        /// Registra un nuevo usuario
        /// </summary>
        Task<AuthResult> RegisterUserAsync(string email, string password, string displayName);

        /// <summary>
        /// Verifica la validez de un token JWT
        /// </summary>
        bool VerifyToken(string token);
    }

    /// <summary>
    /// Resultado de una operación de autenticación
    /// </summary>
    public class AuthResult
    {
        /// <summary>
        /// Indica si la operación fue exitosa
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Token JWT generado (solo si Success es true)
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// ID del usuario autenticado (solo si Success es true)
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Email del usuario autenticado (solo si Success es true)
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// Fecha de expiración del token (solo si Success es true)
        /// </summary>
        public System.DateTime ExpiresAt { get; set; }

        /// <summary>
        /// Mensaje de error (solo si Success es false)
        /// </summary>
        public string ErrorMessage { get; set; }
    }
} 