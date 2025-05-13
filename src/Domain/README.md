# Capa de Dominio

La capa de Dominio es el núcleo de la aplicación y contiene:

- Entidades y agregados (Models)
- Interfaces de repositorios (Repositories)
- Reglas y validaciones de negocio
- Eventos de dominio (Events)

Esta capa no tiene dependencias con otras capas y define las abstracciones que serán implementadas por las capas externas.

## Estructura

- **Odoonto.Domain**: Contiene las entidades principales, interfaces de repositorios y lógica de dominio.
  - `Models`: Entidades y agregados del dominio
  - `Repositories`: Interfaces de repositorio
  - `Services`: Servicios de dominio
  - `Events`: Eventos de dominio

- **Odoonto.Domain.Core**: Contiene abstracciones comunes, base para entidades y excepciones.
  - `Abstractions`: Clases base e interfaces comunes
  - `Exceptions`: Excepciones específicas del dominio

## Reglas

1. Las entidades deben tener identidad y encapsular su comportamiento
2. Las reglas de negocio deben estar en el dominio, no en las capas externas
3. Los repositorios se definen como interfaces en esta capa
4. No debe haber dependencias con frameworks externos 