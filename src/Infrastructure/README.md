# Capa de Infraestructura

La capa de Infraestructura proporciona implementaciones técnicas para las interfaces del dominio y servicios transversales.

## Responsabilidades

- Configurar inyección de dependencias
- Proporcionar implementaciones técnicas (logging, autenticación, etc.)
- Manejar excepciones globales
- Gestionar aspectos transversales (cross-cutting concerns)

## Estructura

- **Odoonto.Infrastructure.InversionOfControl**: Configura la inyección de dependencias.
  - `Inyectors`: Clases para registrar los servicios en el contenedor de DI

- **Odoonto.Infrastructure.ExceptionsHandler**: Manejo centralizado de excepciones.

## Reglas

1. La capa de Infraestructura conoce todas las demás capas
2. Se encarga de "cablear" las dependencias a través de inyección
3. Implementa aspectos técnicos que no pertenecen al dominio ni a la aplicación
4. Las implementaciones de infraestructura no deben filtrar detalles técnicos al dominio
5. Es responsable de la configuración global de la aplicación 

## TODO

### Directrices para componentes de infraestructura:
- Separar responsabilidades en módulos específicos
- Evitar que los detalles de implementación se filtren al dominio
- Proporcionar configuración centralizada para aspectos transversales
- Implementar mecanismos para diagnóstico y monitoreo
- Facilitar pruebas automatizadas mediante abstracciones

### Componentes pendientes por implementar:
- FirebaseInyector (configuración de inyección para Firebase)
  - Registro de servicio FirebaseApp y FirebaseAuth
  - Configuración de opciones de conexión y timeout
  - Inicialización de índices y reglas de seguridad
- ServicesInyector (registro de servicios de aplicación)
  - Registro de todos los servicios de aplicación
  - Configuración de sus dependencias
  - Ciclo de vida adecuado para cada servicio
- RepositoriesInyector (registro de repositorios)
  - Registro de implementaciones de repositorio
  - Configuración de caché si es necesario
  - Gestión de conexiones y pool de conexiones
- Middleware de autenticación con Firebase
  - Validación de tokens JWT
  - Gestión de roles y permisos
  - Manejo de sesiones y revocación
- Middleware de logging y telemetría
  - Registro de acciones de usuario
  - Monitoreo de rendimiento
  - Alertas y diagnóstico
- Configuración de excepciones globales
  - Traducción de excepciones a respuestas HTTP
  - Ocultación de detalles internos
  - Formato consistente de errores 