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