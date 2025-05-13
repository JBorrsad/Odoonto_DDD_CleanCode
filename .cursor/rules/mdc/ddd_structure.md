# Estructura Domain-Driven Design

## Info

- **Autor**: Cursor
- **Versión**: 1.0
- **Categoría**: Arquitectura
- **Tags**: #DDD #Arquitectura #Estructura

## Contexto

Este documento describe la estructura recomendada para proyectos que siguen Domain-Driven Design (DDD) con una implementación de Firebase y frontend React/TypeScript.

## Estructura

### Capas DDD

La arquitectura DDD se divide en las siguientes capas:

1. **Capa de Dominio (Domain Layer)**

   - Entidades, reglas de negocio, interfaces de repositorios
   - `src/Domain/{Proyecto}.Domain/Models/`
   - `src/Domain/{Proyecto}.Domain/Repositories/`
   - `src/Domain/{Proyecto}.Domain.Core/`

2. **Capa de Aplicación (Application Layer)**

   - Servicios, DTOs, orquestación de casos de uso
   - `src/Application/{Proyecto}.Application/Services/`
   - `src/Application/{Proyecto}.Application/DTO/`
   - `src/Application/{Proyecto}.Application/Interfaces/`

3. **Capa de Datos (Data Layer)**

   - Implementación de repositorios con Firebase
   - `src/Data/{Proyecto}.Data/Repositories/`
   - `src/Data/{Proyecto}.Data.Core/Firebase/`

4. **Capa de Infraestructura (Infrastructure Layer)**

   - Configuración, inyección de dependencias
   - `src/Infrastructure/{Proyecto}.Infrastructure.InversionOfControl/`

5. **Capa de Presentación (Presentation Layer)**
   - API REST y frontend React
   - `src/Presentation/{Proyecto}.API/Controllers/`
   - `frontend/` (Proyecto separado React/TypeScript)

### Backend y Firebase

- El backend implementa API REST con ASP.NET Core
- Firebase reemplaza a la base de datos tradicional
- Los controladores se comunican con servicios de aplicación
- Los servicios utilizan repositorios para acceder a Firebase

### Frontend (React/TypeScript)

- Proyecto separado que solo se comunica con el backend vía API REST
- Sigue el patrón MVP (Model-View-Presenter)
- Estructura de carpetas recomendada:
  ```
  frontend/
    src/
      models/         # Modelos de datos (dominio)
      services/       # Servicios API (infraestructura)
      presenters/     # Presentadores/Hooks (aplicación)
      views/          # Componentes React (presentación)
  ```

## Reglas Importantes

1. **Dependencias**: Las capas externas pueden depender de las internas, nunca al revés
2. **Dominio Aislado**: La capa de dominio no debe depender de ninguna otra capa
3. **Encapsulamiento**: Las entidades deben encapsular su estado (propiedades con getters públicos y setters privados)
4. **Validación**: Las entidades deben validar su propio estado interno
5. **Repositorios**: Interfaces en el dominio, implementaciones en la capa de datos
6. **Firebase**: Solo la capa de datos debe conocer los detalles de Firebase
7. **Frontend**: Solo debe comunicarse con el backend a través de la API REST

## Ejemplos

Ver archivos de ejemplo completos en `src/DDD_Ejemplos_Codigo/`:

- Entidades: `1_Domain_Entity.cs`, `11_Domain_Core_Entity.cs`
- Repositorios: `2_Domain_Repository_Interface.cs`, `15_Firebase_Repository_Implementation.cs`
- Servicios: `6_Application_Service_Interface.cs`, `7_Application_Service_Implementation.cs`
- Frontend: `18_React_Model.tsx` a `21_React_View.tsx`

## Restricciones

- No acceder a Firebase directamente desde el frontend
- No colocar lógica de negocio en controladores o servicios
- No filtrar entidades de dominio a través de la API (usar DTOs)
- No exponer detalles de implementación de Firebase fuera de la capa de datos
