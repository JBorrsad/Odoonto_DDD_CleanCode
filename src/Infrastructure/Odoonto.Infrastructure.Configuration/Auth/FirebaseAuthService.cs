using Firebase.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Odoonto.Infrastructure.Configuration.Auth
{
    /// <summary>
    /// Servicio para la autenticación utilizando Firebase Auth
    /// </summary>
    public class FirebaseAuthService : IAuthService
    {
        private readonly FirebaseAuthProvider _authProvider;
        private readonly ILogger<FirebaseAuthService> _logger;
        private readonly string _jwtSecret;

        /// <summary>
        /// Constructor del servicio de autenticación
        /// </summary>
        public FirebaseAuthService(
            FirebaseAuthProvider authProvider,
            ILogger<FirebaseAuthService> logger,
            string jwtSecret)
        {
            _authProvider = authProvider ?? throw new ArgumentNullException(nameof(authProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _jwtSecret = jwtSecret ?? throw new ArgumentNullException(nameof(jwtSecret));
        }

        /// <summary>
        /// Autentica al usuario con email y contraseña
        /// </summary>
        public async Task<AuthResult> SignInWithEmailAndPasswordAsync(string email, string password)
        {
            try
            {
                // Autenticar usuario en Firebase
                var authData = await _authProvider.SignInWithEmailAndPasswordAsync(email, password);
                
                // Generar token JWT para uso en el sistema
                var token = GenerateJwtToken(authData);
                
                return new AuthResult
                {
                    Success = true,
                    Token = token,
                    UserId = authData.User.LocalId,
                    UserEmail = authData.User.Email,
                    ExpiresAt = DateTime.UtcNow.AddHours(1)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al autenticar usuario con email {email}", email);
                return new AuthResult
                {
                    Success = false,
                    ErrorMessage = "Credenciales inválidas"
                };
            }
        }

        /// <summary>
        /// Registra un nuevo usuario
        /// </summary>
        public async Task<AuthResult> RegisterUserAsync(string email, string password, string displayName)
        {
            try
            {
                // Crear usuario en Firebase
                var authData = await _authProvider.CreateUserWithEmailAndPasswordAsync(email, password, displayName);
                
                // Generar token JWT para uso en el sistema
                var token = GenerateJwtToken(authData);
                
                return new AuthResult
                {
                    Success = true,
                    Token = token,
                    UserId = authData.User.LocalId,
                    UserEmail = authData.User.Email,
                    ExpiresAt = DateTime.UtcNow.AddHours(1)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar usuario con email {email}", email);
                return new AuthResult
                {
                    Success = false,
                    ErrorMessage = "No se pudo crear el usuario"
                };
            }
        }

        /// <summary>
        /// Verifica la validez de un token JWT
        /// </summary>
        public bool VerifyToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSecret);
                
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Genera un token JWT a partir de los datos de autenticación de Firebase
        /// </summary>
        private string GenerateJwtToken(FirebaseAuthLink authData)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, authData.User.LocalId),
                new Claim(ClaimTypes.Email, authData.User.Email),
                new Claim(ClaimTypes.Name, authData.User.DisplayName ?? string.Empty)
            };
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
} 