# Infraestructura de Logging

## Propósito

La capa de Infraestructura de Logging proporciona mecanismos para el registro detallado de eventos, solicitudes HTTP y excepciones en la aplicación, facilitando la observabilidad, diagnóstico y monitoreo del sistema.

## Responsabilidades

- Registrar todas las solicitudes HTTP con información detallada
- Medir tiempos de respuesta y reportar solicitudes lentas
- Generar IDs de correlación para seguimiento de solicitudes
- Permitir análisis de patrones de uso y rendimiento
- Facilitar diagnóstico de problemas

## Estructura

- **Middlewares/**: Contiene los middleware para registro de solicitudes
  - `RequestLoggingMiddleware.cs`: Middleware para logging de solicitudes HTTP
  
- **Extensions/**: Extensiones para facilitar el registro de middlewares
  - `LoggingMiddlewareExtensions.cs`: Extensiones para registrar middleware de logging

## Funcionamiento

1. El middleware intercepta todas las solicitudes HTTP al inicio del pipeline
2. Genera un ID único para cada solicitud
3. Registra información inicial (método, ruta, IP, etc.)
4. Mide el tiempo de procesamiento
5. Registra el resultado (código de estado, tiempo, tamaño)
6. Detecta y alerta sobre solicitudes lentas (>500ms)

## Información Registrada

Para cada solicitud HTTP, se registra:

- ID único de solicitud (para correlación)
- Método HTTP (GET, POST, etc.)
- Ruta y query string
- IP del cliente
- User-Agent
- Tiempo de procesamiento en milisegundos
- Código de estado HTTP de respuesta
- Tamaño de la respuesta
- Si se produjo un error, la excepción asociada

## Configuración

En `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Odoonto.Infrastructure.Logging": "Debug"
    },
    "Console": {
      "IncludeScopes": true
    }
  }
}
```

## Uso

Para habilitar el logging de solicitudes en el pipeline HTTP:

```csharp
// En Program.cs - después del middleware de excepciones
app.UseRequestLogging();
```

## Dependencias

- Microsoft.AspNetCore.Http.Abstractions: Para middleware de ASP.NET Core
- Microsoft.Extensions.Logging.Abstractions: Para registro de eventos

## Ejemplos de Logs

```
info: Odoonto.Infrastructure.Logging.Middlewares.RequestLoggingMiddleware[0]
      Solicitud 3fa85f64-5717-4562-b3fc-2c963f66afa6 iniciada: GET /api/patients?page=1&pageSize=10 desde IP 127.0.0.1
      
info: Odoonto.Infrastructure.Logging.Middlewares.RequestLoggingMiddleware[0]
      Solicitud 3fa85f64-5717-4562-b3fc-2c963f66afa6 completada con estado 200 en 125ms
      
warning: Odoonto.Infrastructure.Logging.Middlewares.RequestLoggingMiddleware[0]
      Solicitud 7b2e8f64-9217-4262-a3fc-4c963f66cbd8 excedió el umbral de rendimiento: 752ms
``` 