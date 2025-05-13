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