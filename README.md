# Odoonto: Proyecto DDD con Clean Code

Este proyecto implementa una aplicación para gestión odontológica siguiendo los principios de Domain-Driven Design (DDD) y Clean Code.

## Arquitectura

El proyecto sigue una arquitectura en capas según DDD:

- **Capa de Dominio**: Entidades, reglas de negocio y abstracciones
- **Capa de Aplicación**: Orquestación, DTOs y casos de uso
- **Capa de Infraestructura**: Implementaciones técnicas y configuración
- **Capa de Datos**: Implementación de repositorios y acceso a Firebase
- **Capa de Presentación**: API REST y cliente React con patrón MVP

## Tecnologías

- **Backend**: ASP.NET Core 7 con C#
- **ORM**: Firebase SDK para .NET
- **Frontend**: React 18 con TypeScript
- **Comunicación**: API REST
- **Autenticación**: JWT

## Estructura del Proyecto

```
src/
├── Domain/                 # Núcleo de la aplicación
│   ├── Odoonto.Domain/
│   └── Odoonto.Domain.Core/
│
├── Application/            # Lógica de aplicación
│   └── Odoonto.Application/
│
├── Infrastructure/         # Servicios transversales
│   ├── Odoonto.Infrastructure.InversionOfControl/
│   └── Odoonto.Infrastructure.ExceptionsHandler/
│
├── Data/                   # Acceso a datos
│   ├── Odoonto.Data/
│   ├── Odoonto.Data.Core/
│   └── Odoonto.Data.Contexts/
│
└── Presentation/           # Interfaces de usuario
    ├── Odoonto.UI.Server/
    └── Odoonto.UI.Client/
```

## Flujo de Comunicación

1. El cliente React se comunica con el backend a través de la API REST
2. Los controladores API llaman a servicios de aplicación
3. Los servicios de aplicación orquestan entidades del dominio
4. Los repositorios persisten los datos en Firebase

## Documentación Adicional

- [Guía de la Capa de Dominio](src/Domain/README.md)
- [Guía de la Capa de Aplicación](src/Application/README.md)
- [Guía de la Capa de Datos](src/Data/README.md)
- [Guía de la Capa de Infraestructura](src/Infrastructure/README.md)
- [Guía de la Capa de Presentación](src/Presentation/README.md)
- [Detalles del Frontend MVP](src/Presentation/Odoonto.UI.Client/src/README.md)

## Reglas de Clean Code

Este proyecto sigue estrictamente los principios SOLID y las mejores prácticas de Clean Code:

- Single Responsibility Principle (SRP)
- Open/Closed Principle (OCP)
- Liskov Substitution Principle (LSP)
- Interface Segregation Principle (ISP)
- Dependency Inversion Principle (DIP)

Para más detalles sobre las reglas de código, consulta la [guía completa](docs/Clean_Code_Guidelines.md).

## Entidades del Dominio

El sistema gestiona las siguientes entidades principales:

- **Patient**: Persona atendida con su información personal
- **Doctor**: Profesional odontológico
- **Treatment**: Procedimientos clínicos ofrecidos
- **Appointment**: Citas programadas
- **Odontogram**: Mapa dental completo

Para más detalles sobre las entidades, consulta la [documentación de entidades](docs/Domain_Entities.md).

