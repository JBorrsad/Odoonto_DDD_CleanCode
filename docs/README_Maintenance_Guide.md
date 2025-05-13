# Guía de Mantenimiento de README

Este documento explica cómo usar las reglas de Cursor para mantener actualizados los README de cada capa de la arquitectura DDD.

## Reglas implementadas

Hemos implementado las siguientes reglas en Cursor para ayudar a mantener actualizados los README:

1. **Reglas de verificación por capa**:
   - `DDD-check-domain-readme.json`: Verifica el README de la capa Domain
   - `DDD-check-application-readme.json`: Verifica el README de la capa Application
   - `DDD-check-data-readme.json`: Verifica el README de la capa Data
   - `DDD-check-infrastructure-readme.json`: Verifica el README de la capa Infrastructure
   - `DDD-check-presentation-readme.json`: Verifica el README de la capa Presentation

2. **Plantillas para secciones**:
   - `DDD-tpl-readme_section_todo.json`: Plantillas para la sección TODO
   - `DDD-tpl-readme_section_implemented.json`: Plantillas para la sección Implementación Completada

## Cómo funciona

1. **Verificación automática**: Cuando modificas o añades un componente clave (como una entidad, repositorio, servicio, etc.), las reglas detectan este cambio y te muestran un aviso recordándote que actualices el README de la capa correspondiente.

2. **Sugerencias específicas**: El aviso te indicará exactamente qué sección del README debes actualizar y con qué información, basándose en el componente que has creado o modificado.

3. **Plantillas disponibles**: Puedes usar las plantillas predefinidas para añadir secciones completas de "Implementación Completada" o "TODO" a los README cuando sea necesario.

## Cómo actualizar un README

Cuando Cursor te avise de que debes actualizar un README, sigue estos pasos:

1. Abre el README de la capa correspondiente (por ejemplo, `src/Domain/README.md`)
2. Busca la sección "## Implementación Completada" y añade el nuevo componente con una breve descripción
3. Busca la sección "## TODO" y elimina el componente de la lista de "Componentes pendientes por implementar" si estaba allí

### Usando plantillas

Para utilizar las plantillas disponibles:

1. Abre el README que deseas actualizar
2. Posiciona el cursor donde deseas insertar una sección
3. Presiona `Ctrl+Space` (o el atajo configurado en Cursor para mostrar autocompletado)
4. Escribe "readme" para ver las plantillas disponibles
5. Selecciona la plantilla deseada, por ejemplo:
   - `Sección TODO para capa Domain` si necesitas añadir la sección TODO
   - `Sección Implementación Completada para capa Domain` si necesitas añadir la sección de Implementación Completada

## Estructura recomendada para cada README

Cada README debe mantener esta estructura:

```markdown
# Capa de [Nombre de la Capa]

[Descripción general de la capa]

## Responsabilidades

- [Lista de responsabilidades]

## Estructura

- [Descripción de la estructura]

## Reglas

[Lista de reglas]

## Implementación Completada

### Directrices implementadas:
- [Lista de directrices implementadas]

### Componentes implementados:
- [Componente 1] (Descripción breve)
  - [Detalle 1]
  - [Detalle 2]
- [Componente 2] (Descripción breve)
  - [Detalle 1]
  - [Detalle 2]

## TODO

### Componentes pendientes por implementar:
- [Componente pendiente 1] (Descripción breve)
  - [Subtarea 1]
  - [Subtarea 2]
- [Componente pendiente 2] (Descripción breve)
  - [Subtarea 1]
  - [Subtarea 2]

### Consideraciones técnicas:
- [Consideración 1]
- [Consideración 2]
```

## Beneficios

Manteniendo actualizados los README con esta metodología, conseguimos:

1. Documentación siempre al día de los componentes implementados
2. Visibilidad clara de lo que queda por implementar
3. Guía para nuevos miembros del equipo sobre la estructura del proyecto
4. Trazabilidad de la evolución del proyecto 