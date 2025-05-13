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

- **Odoonto.Infrastructure.Configuration**: Configuraciones para servicios externos.
  - `Firebase`: Configuración y credenciales para Firebase

## Reglas

1. La capa de Infraestructura conoce todas las demás capas
2. Se encarga de "cablear" las dependencias a través de inyección
3. Implementa aspectos técnicos que no pertenecen al dominio ni a la aplicación
4. Las implementaciones de infraestructura no deben filtrar detalles técnicos al dominio
5. Es responsable de la configuración global de la aplicación 

## Implementación Completada

### Componentes implementados:
- Estructura base para inyección de dependencias
- Configuración inicial para manejo de excepciones
- Estructura para registro de servicios
- FirebaseConfiguration (configuración para acceso a Firebase)
- FirebaseInyector (registro de servicios de Firebase)
- RepositoryInyector (registro de repositorios implementados)
  - Registro de PatientRepository, AppointmentRepository y DoctorRepository
  - Configuración del ciclo de vida Scoped para los repositorios
  - Registro del FirestoreContext como Singleton
- Configuración segura para credenciales de Firebase

## TODO

### Componentes pendientes por implementar:
- ServicesInyector (registro de servicios de aplicación)
  - Registro de todos los servicios de aplicación
  - Configuración de sus dependencias
  - Ciclo de vida adecuado para cada servicio
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