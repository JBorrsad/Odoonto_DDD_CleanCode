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

## TODO

### Directrices para implementación de repositorios:
- Seguir el patrón del repositorio ejemplo (FlowRepository)
- Implementar métodos específicos como GetByIdOrThrow para manejo seguro
- Asegurar conversión correcta entre documentos Firestore y entidades
- Proporcionar métodos para consultas optimizadas por caso de uso
- Implementar transacciones para operaciones que afectan a múltiples entidades
- Considerar implementaciones de caché para mejorar rendimiento

### Repositorios pendientes por implementar:
- IPatientRepository y PatientRepository
  - Métodos para búsqueda por nombre, fecha de nacimiento, etc.
  - Carga eficiente del odontograma (parcial o completo)
- IDoctorRepository y DoctorRepository
  - Métodos para consulta de disponibilidad
  - Optimización para búsqueda de horarios 
- ITreatmentRepository y TreatmentRepository
  - Métodos para búsqueda por categoría, precio, etc.
  - Soporte para catálogo de tratamientos
- IAppointmentRepository y AppointmentRepository
  - Consultas por rango de fechas
  - Filtrado por doctor, paciente o estado
  - Validación de superposición de citas
- IOdontogramRepository y OdontogramRepository
  - Soporte para carga/guardado incremental
  - Histórico de cambios por diente
- ILesionRepository y LesionRepository
  - Catálogo y gestión de tipos de lesiones

### Contextos pendientes por implementar:
- FirebaseContext (configuración principal)
  - Inicialización y configuración segura de Firebase
  - Gestión de autenticación y permisos
- Configuraciones de mapeo para todas las entidades
  - Conversión bidireccional entidad-documento
  - Estrategias de serialización para value objects 