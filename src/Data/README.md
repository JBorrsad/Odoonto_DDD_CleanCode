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
  - `Contexts`: Contextos para acceso a la base de datos
  - `Repositories`: Clases base para implementaciones de repositorios

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

## Implementación Completada

### Componentes implementados:
- Estructura base para repositorios
- Configuración inicial de Firebase
- Clases base para conversión de documentos-entidades
- FirestoreContext para interactuar con Firestore
- Repository<T> clase base para todos los repositorios
- PatientRepository (Implementación concreta)
  - Métodos para búsqueda por nombre, fecha de nacimiento, etc.
  - Búsqueda por datos de contacto y rango de edad
- AppointmentRepository (Implementación concreta)
  - Consultas por rango de fechas
  - Filtrado por doctor, paciente o estado
  - Validación de superposición de citas
- Configuraciones de mapeo:
  - AppointmentConfiguration: Conversión bidireccional entre documentos Firestore y entidades Appointment
  - Estrategias de serialización para objetos Date y TimeSlot

## TODO

### Repositorios pendientes por implementar:
- IDoctorRepository y DoctorRepository
  - Métodos para consulta de disponibilidad
  - Optimización para búsqueda de horarios 
- ITreatmentRepository y TreatmentRepository
  - Métodos para búsqueda por categoría, precio, etc.
  - Soporte para catálogo de tratamientos
- IOdontogramRepository y OdontogramRepository
  - Soporte para carga/guardado incremental
  - Histórico de cambios por diente
- ILesionRepository y LesionRepository
  - Catálogo y gestión de tipos de lesiones

### Contextos pendientes por implementar:
- Configuraciones de mapeo para el resto de entidades
  - PatientConfiguration
  - DoctorConfiguration
  - TreatmentConfiguration 