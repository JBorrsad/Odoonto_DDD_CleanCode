# Instrucciones para Usar las Reglas de Cursor DDD-Firebase-React

## Info

- **Autor**: Cursor
- **Versión**: 1.0
- **Categoría**: Instrucciones
- **Tags**: #DDD #Cursor #Instrucciones

## Introducción

Este proyecto incluye un conjunto de reglas y documentación para Cursor que te ayudará a implementar Domain-Driven Design (DDD) en un proyecto con backend .NET, Firebase y frontend React/TypeScript. Las reglas proporcionan ayuda contextual, ejemplos de código y plantillas.

## Estructura de las Reglas

Las reglas se dividen en dos tipos principales:

1. **Archivos MDC** (Markdown Cursor):

   - Proporcionan documentación contextual
   - Muestran ejemplos de código
   - Se activan automáticamente según el tipo de archivo o patrón

2. **Reglas JSON**:
   - Proporcionan plantillas para generar código
   - Definen patrones para detectar intenciones
   - Incluyen referencias a ejemplos de código

## Cómo Usar las Reglas

### Al Crear Nuevos Archivos

1. Cursor detectará automáticamente qué tipo de componente estás creando basado en:

   - La ruta del archivo
   - El nombre del archivo
   - El tipo de archivo (.cs, .ts, .tsx)

2. Te ofrecerá plantillas y ejemplos relevantes. Por ejemplo:
   - Si creas un archivo en `src/Domain/TuProyecto.Domain/Models/`, Cursor mostrará la plantilla para entidades de dominio
   - Si creas un archivo `ProductDto.cs` en cualquier carpeta, Cursor reconocerá que es un DTO

### Al Editar Archivos Existentes

1. Cursor proporciona documentación contextual basada en el archivo que estás editando
2. Puedes ver ejemplos de implementación similares
3. Recibirás advertencias si tu código se desvía de las reglas DDD

### Para Obtener Ayuda Contextual

1. Escribe un comentario en tu código con una pregunta
2. Cursor detectará el contexto y te mostrará la documentación relevante
3. Para preguntas generales sobre DDD, puedes escribir `// #DDD` en tu comentario

## Ejemplos de Uso

### Crear una Nueva Entidad de Dominio

1. Crea un archivo en `src/Domain/TuProyecto.Domain/Models/Products/Product.cs`
2. Cursor reconocerá que estás creando una entidad de dominio
3. Te mostrará la documentación de `domain_entity.mdc`
4. Puedes solicitar una plantilla escribiendo `// crear entidad de dominio`

### Crear DTOs para una Entidad

1. Crea archivos en `src/Application/TuProyecto.Application/DTO/Products/`
2. Nombra los archivos como `ProductCreateDto.cs`, `ProductReadDto.cs`, etc.
3. Cursor reconocerá que estás creando DTOs
4. Te mostrará la documentación de `dto.mdc`

### Crear Componentes React

1. Crea archivos en el fronted:
   - `frontend/src/models/products/Product.model.ts`
   - `frontend/src/services/products/product.service.ts`
   - `frontend/src/presenters/products/useProductPresenter.tsx`
   - `frontend/src/views/products/ProductList.tsx`
2. Cursor detectará cada tipo de componente
3. Te mostrará la documentación relevante de `react_components.mdc`

## Reglas Disponibles

### Backend (.NET/C#)

- **Entidades de Dominio**: `/mdc/domain_entity.mdc` + `/rules/domain_entity.json`
- **Interfaces de Repositorio**: `/mdc/ddd_rules.md` (sección) + `/rules/domain_repository_interface.json`
- **Repositorios Firebase**: `/mdc/ddd_rules.md` (sección) + `/rules/firebase_repository_implementation.json`
- **DTOs**: `/mdc/dto.mdc` + `/rules/dto.json`
- **Servicios de Aplicación**: `/mdc/application_service.mdc` + `/rules/application_service.json`
- **Controladores API**: `/mdc/ddd_rules.md` (sección) + `/rules/controller.json`

### Frontend (React/TypeScript)

- **Componentes React (General)**: `/mdc/react_components.mdc` + `/rules/react_components.json`
- **Modelos**: Sección en react_components.mdc
- **Servicios API**: Sección en react_components.mdc
- **Presentadores**: Sección en react_components.mdc
- **Vistas**: Sección en react_components.mdc

### Documentación General

- **Estructura DDD**: `/mdc/ddd_structure.md`
- **Flujos de Comunicación**: `/mdc/ddd_communication.md`
- **Reglas Generales**: `/mdc/ddd_rules.md`
- **Ejemplos de Código**: `/mdc/code_examples.md`

## Ejemplos de Referencia

Todos los ejemplos de código se encuentran en la carpeta `src/DDD_Ejemplos_Codigo/`, organizados por tipo de componente.

## Configuración Avanzada

Si necesitas ajustar o extender las reglas:

1. Edita los archivos `.cursor/mdc/*.mdc` para actualizar la documentación
2. Modifica `.cursor/mdc.json` para ajustar patrones de detección
3. Edita los archivos `.cursor/rules/*.json` para cambiar plantillas

## Consejos Finales

- Usa `// #DDD` en tus comentarios para obtener ayuda general sobre DDD
- Escribe comentarios específicos como `// crear entidad dominio` para activar plantillas
- Revisa los ejemplos completos antes de implementar componentes complejos
- Sigue siempre el principio de "Dependencia hacia el núcleo" en tu arquitectura
