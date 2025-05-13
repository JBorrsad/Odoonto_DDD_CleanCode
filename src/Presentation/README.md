# Capa de Presentación

La capa de Presentación proporciona la interfaz de usuario y expone APIs.

## Responsabilidades

- Presentar interfaces de usuario
- Gestionar entrada del usuario
- Proporcionar endpoints APIs
- Manejar validaciones de formularios

## Estructura

### Backend (API REST)

- **Odoonto.UI.Server**: API REST.
  - `Controllers`: Controladores API REST
  - `Middlewares`: Componentes de middleware

### Frontend (React con MVP)

- **Odoonto.UI.Client**: Cliente React.
  - `src/models`: Modelos de datos para la UI (M en MVP)
  - `src/views`: Componentes React puros de presentación (V en MVP)
  - `src/presenters`: Lógica de presentación, estado y coordinación (P en MVP)
  - `src/services`: Servicios para comunicación con API
  - `src/services/api`: Clientes API para comunicación con backend
  - `src/components`: Componentes UI reutilizables
  - `src/utils`: Utilidades y helpers

## Reglas para Backend

1. Los controladores deben ser ligeros, delegando la lógica a los servicios de aplicación
2. Los endpoints deben estar versionados y documentados con Swagger
3. La validación básica se realiza en esta capa
4. Las respuestas HTTP deben seguir estándares REST

## Reglas para Frontend (MVP)

1. **Modelos**: Contienen solo datos y validaciones simples
2. **Vistas**: Componentes de presentación pura, sin estado ni lógica de negocio
3. **Presentadores**: Contienen la lógica de UI, estado y coordinación
4. Comunicación exclusiva con el backend a través de servicios API
5. No acceso directo a Firebase desde el frontend
6. Separación clara de responsabilidades según el patrón MVP 

## TODO

### Directrices para implementación del patrón MVP en frontend:
- Seguir estructura del ejemplo con clara separación de vistas y presentadores
- Implementar interfaces para cada vista para facilitar pruebas
- Mantener las vistas como componentes puros de presentación
- Centralizar toda la lógica de UI en los presentadores
- Utilizar servicios API para comunicación con el backend
- Evitar acceso directo a APIs externas desde presentadores

### Directrices para implementación de API REST:
- Controladores enfocados en traducir peticiones HTTP a operaciones de servicios
- Versionado claro en rutas (ej. /api/v1/patients)
- Documentación completa con Swagger
- Gestión consistente de errores HTTP
- Formatos de respuesta estandarizados

### Backend pendiente por implementar:
- PatientController
  - Endpoints CRUD completos
  - Filtrado y paginación
  - Upload de documentos/imágenes
- DoctorController
  - Gestión de horarios y disponibilidad
  - Calendario y agenda
- AppointmentController
  - Programación y reprogramación
  - Búsqueda por fechas y estados
  - Notificaciones
- TreatmentController
  - Catálogo con precios y duración
  - Categorización
- OdontogramController
  - Visualización y edición
  - Histórico de cambios
- LesionController
  - Catálogo y registro
- Configuración de Swagger
  - Documentación de endpoints
  - Ejemplos de uso
- Middleware de autenticación de usuarios
  - Roles y permisos
  - Tokens JWT

### Frontend pendiente por implementar:
- Modelos:
  - PatientModel
    - Datos básicos del paciente y validaciones
    - Estado local de odontograma
  - DoctorModel
    - Perfil y especialidades
    - Gestión de disponibilidad
  - AppointmentModel
    - Programación y estados
    - Relaciones con doctor y paciente
  - TreatmentModel
    - Catálogo y precios
    - Agrupación por categorías
  - OdontogramModel
    - Representación visual del estado dental
  
- Vistas:
  - PatientsView (listado y detalles)
    - Tabla/lista de pacientes
    - Formularios de creación/edición
    - Filtros de búsqueda
  - DoctorsView (listado y detalles)
    - Directorio de profesionales
    - Calendario de disponibilidad
  - AppointmentsView (calendario y agenda)
    - Vista diaria, semanal y mensual
    - Drag & drop para cambios
  - TreatmentsView (catálogo)
    - Navegación por categorías
    - Precios y descripciones
  - OdontogramView (visualización interactiva)
    - Diagrama interactivo
    - Registro de lesiones y tratamientos
  - DashboardView (panel principal)
    - Resumen de actividad
    - Métricas principales
  - LoginView (autenticación)
    - Login y registro
    - Recuperación de contraseña
  
- Presentadores:
  - PatientsPresenter
    - Lógica de búsqueda y filtrado
    - Validación de formularios
  - DoctorsPresenter
    - Gestión de horarios
    - Visualización de agenda
  - AppointmentsPresenter
    - Verificación de disponibilidad
    - Lógica de programación
  - TreatmentsPresenter
    - Filtrado y búsqueda
    - Selección para citas
  - OdontogramPresenter
    - Lógica interactiva de selección
    - Histórico de cambios
  - DashboardPresenter
    - Cálculo de estadísticas
    - Actualización en tiempo real
  - LoginPresenter
    - Flujo de autenticación
    - Gestión de tokens
  
- Servicios API:
  - PatientApiService
    - Métodos para todas las operaciones CRUD
    - Gestión de caché local
  - DoctorApiService
    - Consulta de disponibilidad
    - Perfil profesional
  - AppointmentApiService
    - Programación y estados
    - Notificaciones
  - TreatmentApiService
    - Catálogo completo
    - Precios y duración
  - OdontogramApiService
    - Sincronización de cambios
    - Histórico dental
  - AuthApiService
    - Login y registro
    - Refresh token
  