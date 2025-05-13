# Capa de Datos

La capa de Datos implementa los repositorios definidos en el dominio y gestiona la persistencia a través de Firebase.

## Responsabilidades

- Implementar repositorios
- Abstraer completamente el acceso a Firebase
- Convertir documentos Firestore a entidades de dominio
- Proporcionar optimizaciones de rendimiento

## Estructura

- **Odoonto.Data**: Implementaciones de repositorios.
  - `Repositories`: Implementaciones concretas de las interfaces de repositorio del dominio

- **Odoonto.Data.Core**: Abstracciones para acceso a datos.
  - `Abstractions`: Clases base e interfaces comunes para acceso a datos

- **Odoonto.Data.Contexts**: Definición de contextos de base de datos.
  - `Contexts`: Contextos de Firebase
  - `Configurations`: Configuraciones para mapeo entre entidades y documentos Firebase

## Reglas

1. Los repositorios implementan interfaces definidas en el dominio
2. La interacción con Firebase se abstrae completamente del resto de la aplicación
3. Las consultas a Firebase se optimizan para minimizar lecturas/escrituras
4. Se utiliza el SDK de Firebase para C#
5. Los documentos Firestore se convierten a entidades del dominio antes de devolverlos
6. Las transacciones complejas se implementan a nivel de repositorio 