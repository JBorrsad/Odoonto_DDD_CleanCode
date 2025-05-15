# Infraestructura de Autenticación

## Propósito

La capa de Infraestructura de Autenticación proporciona los mecanismos necesarios para la autenticación y autorización basados en Firebase. Se encarga de verificar tokens JWT emitidos por Firebase Authentication y convertirlos en identidades de usuario para ASP.NET Core.

## Responsabilidades

- Verificar tokens de autenticación de Firebase
- Convertir reclamaciones de Firebase en ClaimsPrincipal de ASP.NET Core
- Proporcionar middleware para integrar la autenticación en el pipeline HTTP
- Inicializar Firebase Admin SDK

## Estructura

- **Middlewares/**: Contiene los middleware para interceptar solicitudes HTTP
  - `FirebaseAuthMiddleware.cs`: Middleware para autenticación basada en tokens
  
- **Extensions/**: Extensiones para facilitar el registro de middlewares
  - `AuthMiddlewareExtensions.cs`: Extensiones para registrar middleware de autenticación
  
- **Services/**: Servicios para inicialización y gestión de Firebase
  - `FirebaseInitializer.cs`: Inicializador de Firebase Admin SDK

## Funcionamiento

1. El middleware intercepta todas las solicitudes HTTP
2. Extrae el token Bearer del encabezado Authorization
3. Verifica el token con Firebase Admin SDK
4. Si es válido, extrae información del usuario y crea un ClaimsPrincipal
5. Asocia el ClaimsPrincipal al contexto HTTP para su uso en autorización

## Configuración

En `appsettings.json`:

```json
{
  "Firebase": {
    "ProjectId": "tu-proyecto-firebase",
    "CredentialsPath": "ruta-a-credenciales.json",
    "DatabaseUrl": "https://tu-proyecto.firebaseio.com"
  },
  "Authentication": {
    "JwtBearer": {
      "Issuer": "firebase",
      "Audience": "tu-proyecto-firebase",
      "ValidateIssuer": true,
      "ValidateAudience": true,
      "ValidateLifetime": true
    }
  }
}
```

## Uso

Para habilitar la autenticación con Firebase en el pipeline HTTP:

```csharp
// En Program.cs
app.UseFirebaseAuthentication();

// Después configurar autorización
app.UseAuthorization();
```

Para proteger endpoints con autorización:

```csharp
[Authorize]
public class PatientsController : ControllerBase
{
    // ...
}
```

## Dependencias

- FirebaseAdmin: Para verificación de tokens y acceso a Firebase
- Microsoft.AspNetCore.Http.Abstractions: Para middleware de ASP.NET Core
- Microsoft.Extensions.Logging.Abstractions: Para registro de eventos 