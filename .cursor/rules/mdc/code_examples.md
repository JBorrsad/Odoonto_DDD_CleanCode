# Ejemplos de Código DDD con Firebase y React

## Info

- **Autor**: Cursor
- **Versión**: 1.0
- **Categoría**: Ejemplos
- **Tags**: #DDD #Ejemplos #Firebase #React #CSharp #TypeScript

## Contexto

Este documento proporciona referencias a ejemplos de código para implementar diferentes componentes en una arquitectura Domain-Driven Design (DDD) con Firebase y React.

## Ejemplos de Dominio

### Entidades

| Archivo                                                                      | Descripción                                                                     |
| ---------------------------------------------------------------------------- | ------------------------------------------------------------------------------- |
| [1_Domain_Entity.cs](src/DDD_Ejemplos_Codigo/1_Domain_Entity.cs)             | Entidad de dominio básica con validaciones, getters públicos y setters privados |
| [11_Domain_Core_Entity.cs](src/DDD_Ejemplos_Codigo/11_Domain_Core_Entity.cs) | Clase base para entidades con propiedades comunes                               |

Características principales:

- Encapsulamiento con setters privados
- Validación en métodos de modificación
- Métodos factory (Create)
- Comportamiento específico del dominio

### Interfaces de Repositorio

| Archivo                                                                                                  | Descripción                                     |
| -------------------------------------------------------------------------------------------------------- | ----------------------------------------------- |
| [2_Domain_Repository_Interface.cs](src/DDD_Ejemplos_Codigo/2_Domain_Repository_Interface.cs)             | Interfaz de repositorio para entidad específica |
| [12_Domain_Core_Repository_Interface.cs](src/DDD_Ejemplos_Codigo/12_Domain_Core_Repository_Interface.cs) | Interfaz base para repositorios                 |

Características principales:

- Métodos CRUD básicos
- Métodos específicos por dominio
- Solo definiciones, sin implementación

## Ejemplos de Aplicación

### DTOs

| Archivo                                                              | Descripción                                                    |
| -------------------------------------------------------------------- | -------------------------------------------------------------- |
| [5_Application_DTO.cs](src/DDD_Ejemplos_Codigo/5_Application_DTO.cs) | DTOs para diferentes operaciones (Create, Read, Update, Query) |

Características principales:

- Clases simples con propiedades
- Anotaciones de validación
- Diferentes tipos por operación

### Interfaces de Servicio

| Archivo                                                                                          | Descripción                             |
| ------------------------------------------------------------------------------------------------ | --------------------------------------- |
| [6_Application_Service_Interface.cs](src/DDD_Ejemplos_Codigo/6_Application_Service_Interface.cs) | Interfaces para servicios de aplicación |

Características principales:

- Métodos asíncronos (Task)
- Operaciones basadas en casos de uso
- Uso de DTOs para entrada/salida

### Implementaciones de Servicio

| Archivo                                                                                                    | Descripción                               |
| ---------------------------------------------------------------------------------------------------------- | ----------------------------------------- |
| [7_Application_Service_Implementation.cs](src/DDD_Ejemplos_Codigo/7_Application_Service_Implementation.cs) | Implementación de servicios de aplicación |

Características principales:

- Inyección de repositorios
- Uso de AutoMapper
- Orquestación entre entidades
- Validación de datos de entrada

### AutoMapper

| Archivo                                                                                            | Descripción                                            |
| -------------------------------------------------------------------------------------------------- | ------------------------------------------------------ |
| [8_Application_AutoMapper_Profile.cs](src/DDD_Ejemplos_Codigo/8_Application_AutoMapper_Profile.cs) | Perfil de AutoMapper para mapeo entre entidades y DTOs |

## Ejemplos de Datos

### Repositorios Estándar

| Archivo                                                                                                        | Descripción                                        |
| -------------------------------------------------------------------------------------------------------------- | -------------------------------------------------- |
| [3_Data_Repository_Implementation.cs](src/DDD_Ejemplos_Codigo/3_Data_Repository_Implementation.cs)             | Implementación de repositorio con Entity Framework |
| [13_Data_Core_Repository_Implementation.cs](src/DDD_Ejemplos_Codigo/13_Data_Core_Repository_Implementation.cs) | Implementación base de repositorio                 |

### Repositorios Firebase

| Archivo                                                                                                      | Descripción                                             |
| ------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------- |
| [15_Firebase_Repository_Implementation.cs](src/DDD_Ejemplos_Codigo/15_Firebase_Repository_Implementation.cs) | Implementación de repositorio usando Firebase Firestore |
| [16_Data_Core_Firebase_Context.cs](src/DDD_Ejemplos_Codigo/16_Data_Core_Firebase_Context.cs)                 | Contexto para acceso a Firebase                         |

Características principales:

- Uso del SDK de Firebase para C#
- Mapeo entre documentos y entidades
- Manejo de transacciones
- Gestión de relaciones entre entidades

## Ejemplos de Infraestructura

### Inyección de Dependencias

| Archivo                                                                                                  | Descripción                                                              |
| -------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------ |
| [10_Infrastructure_Inyector.cs](src/DDD_Ejemplos_Codigo/10_Infrastructure_Inyector.cs)                   | Configuración de inyección de dependencias para servicios y repositorios |
| [17_Infrastructure_Firebase_Inyector.cs](src/DDD_Ejemplos_Codigo/17_Infrastructure_Firebase_Inyector.cs) | Configuración específica para Firebase                                   |

## Ejemplos de Presentación

### API Controllers

| Archivo                                                                              | Descripción                 |
| ------------------------------------------------------------------------------------ | --------------------------- |
| [9_Presentation_Controller.cs](src/DDD_Ejemplos_Codigo/9_Presentation_Controller.cs) | Controlador API con Swagger |

Características principales:

- Inyección de servicios de aplicación
- Decoración con atributos para rutas y métodos HTTP
- Documentación con atributos Swagger
- Manejo de excepciones y códigos HTTP

### Frontend React (MVP)

#### Modelos

| Archivo                                                          | Descripción                                      |
| ---------------------------------------------------------------- | ------------------------------------------------ |
| [18_React_Model.tsx](src/DDD_Ejemplos_Codigo/18_React_Model.tsx) | Modelos e interfaces TypeScript para el frontend |

Características principales:

- Interfaces para diferentes operaciones
- Clase con comportamiento
- Validación local

#### Servicios API

| Archivo                                                              | Descripción                              |
| -------------------------------------------------------------------- | ---------------------------------------- |
| [19_React_Service.tsx](src/DDD_Ejemplos_Codigo/19_React_Service.tsx) | Servicios para comunicación con API REST |

Características principales:

- Uso de axios
- Métodos para cada operación CRUD
- Manejo de errores centralizado

#### Presentadores

| Archivo                                                                  | Descripción                              |
| ------------------------------------------------------------------------ | ---------------------------------------- |
| [20_React_Presenter.tsx](src/DDD_Ejemplos_Codigo/20_React_Presenter.tsx) | Custom hooks para lógica de presentación |

Características principales:

- Gestión de estado con useState
- Comunicación con servicios
- Implementación de ciclo de vida
- Manejo de validación y errores

#### Vistas

| Archivo                                                        | Descripción            |
| -------------------------------------------------------------- | ---------------------- |
| [21_React_View.tsx](src/DDD_Ejemplos_Codigo/21_React_View.tsx) | Componentes React (UI) |

Características principales:

- Uso de presentadores
- Manejo de eventos UI locales
- Renderizado condicional
- No contiene lógica de negocio

## Documentación de Arquitectura

| Archivo                                                                                    | Descripción                              |
| ------------------------------------------------------------------------------------------ | ---------------------------------------- |
| [22_ReactFirebase_Integration.md](src/DDD_Ejemplos_Codigo/22_ReactFirebase_Integration.md) | Integración de API REST con Firebase     |
| [23_Frontend_Estructura.md](src/DDD_Ejemplos_Codigo/23_Frontend_Estructura.md)             | Estructura recomendada para frontend     |
| [24_Resumen_Adaptaciones.md](src/DDD_Ejemplos_Codigo/24_Resumen_Adaptaciones.md)           | Resumen de adaptaciones para el proyecto |

## Cómo Usar Estos Ejemplos

1. **Iniciar un nuevo componente**:

   - Identifica qué tipo de componente necesitas crear
   - Consulta el ejemplo correspondiente en este documento
   - Usa las plantillas de las reglas JSON para generar código base

2. **Adaptar el ejemplo**:

   - Modifica el código para tu entidad/caso de uso específico
   - Mantén las mismas convenciones y patrones
   - Asegúrate de seguir las reglas de implementación

3. **Reglas importantes**:
   - Mantén las capas separadas y respeta sus responsabilidades
   - No expongas detalles de implementación fuera de su capa
   - Siempre implementa validaciones
   - Sigue las convenciones de nombrado
