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
  - `Repositories`: Interfaces genéricas para repositorios
  - `Exceptions`: Excepciones específicas del dominio

## Reglas

1. Las entidades deben tener identidad y encapsular su comportamiento
2. Las reglas de negocio deben estar en el dominio, no en las capas externas
3. Los repositorios se definen como interfaces en esta capa
4. No debe haber dependencias con frameworks externos 

## Implementación Completada

### Clases base en Odoonto.Domain.Core:
- `Entity`: Clase base para todas las entidades con identidad y propiedades de auditoría
- `SortedEntity`: Clase base para entidades que requieren ordenamiento, con funcionalidad de comparación
- `ValueObject`: Clase base para todos los value objects con igualdad basada en valores
- `IRepository<T>`: Interfaz genérica para repositorios con operaciones CRUD comunes

### Entidades implementadas:
- `Doctor`: Profesional odontológico con especialidad y disponibilidad
- `Treatment`: Procedimientos odontológicos con precio y duración
- `Appointment`: Citas con pacientes, doctores y planes de tratamiento
- `Lesion`: Catálogo de lesiones con categorías
- `Patient`: Pacientes con datos personales y contacto

### Value Objects implementados:
- `Money`: Valor monetario con monto y divisa
- `Date`: Fecha con zona horaria Europe/Madrid
- `TimeSlot`: Período de tiempo con hora de inicio y fin
- `TimeRange`: Rango de tiempo con validaciones
- `WeeklyAvailability`: Disponibilidad semanal por día y rango horario
- `AppointmentStatus`: Estados de una cita (Enum)
- `Gender`: Género del paciente (Enum)
- `ToothNumber`: Número de diente con validación según tipo de dentición
- `ToothSurfaces`: Conjunto de superficies de un diente específico
- `ToothSurface`: Superficie dental individual (Enum)
- `FullName`: Nombre completo con validaciones
- `ContactInfo`: Información de contacto con validaciones
- `TreatmentPlan`: Lista inmutable de procedimientos planificados
- `PlannedProcedure`: Procedimiento planificado para una cita
- `PerformedProcedure`: Registro de procedimiento realizado
- `CompletedProcedures`: Conjunto inmutable de procedimientos completados

### Interfaces de repositorio implementadas:
- `IRepository<T>`: Interfaz genérica para operaciones comunes
- `IAppointmentRepository`: Operaciones específicas para citas
- `IDoctorRepository`: Operaciones específicas para doctores
- `ITreatmentRepository`: Operaciones específicas para tratamientos
- `ILesionRepository`: Operaciones específicas para lesiones
- `IPatientRepository`: Operaciones específicas para pacientes

### Servicios de dominio implementados:
- `AppointmentService`: Lógica de negocio para citas (verificación de disponibilidad, cambios de estado) 