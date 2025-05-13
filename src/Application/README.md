# Capa de Aplicación

La capa de Aplicación orquesta el flujo de la aplicación, coordina los objetos del dominio y proporciona APIs para acceder al sistema.

## Responsabilidades

- Definir casos de uso (Services)
- Definir DTOs para transferencia de datos
- Implementar mapeo entre entidades y DTOs (AutoMapper)
- Coordinar operaciones complejas entre entidades

## Estructura

- **Odoonto.Application**: Contiene servicios, DTOs y mapeadores.
  - `Services`: Implementación de servicios de aplicación
  - `DTOs`: Objetos de transferencia de datos
  - `Interfaces`: Interfaces para los servicios de aplicación
  - `Mappers`: Perfiles de AutoMapper para conversión entre entidades y DTOs

## Reglas

1. Los servicios de aplicación implementan casos de uso específicos
2. La capa de Aplicación depende de la capa de Dominio
3. No debe contener lógica de negocio compleja, solo orquestación
4. Utiliza DTOs para comunicarse con las capas externas
5. Mantiene los servicios ligeros, delegando en el dominio la lógica de negocio 

## TODO

### Directrices para implementación de servicios:
- Seguir el patrón de servicios de aplicación del ejemplo (FlowAppService)
- Cada operación del servicio debe ser un método separado y específico
- Los servicios deben ser ligeros, delegando la lógica de negocio a las entidades
- La responsabilidad principal es orquestar, no implementar lógica de negocio
- Utilizar repositorios para obtener/guardar entidades
- Validar datos de entrada y gestionar excepciones de forma consistente

### Servicios pendientes por implementar:
- PatientService (CRUD y gestión de pacientes)
  - Métodos específicos para crear, actualizar, eliminar y consultar pacientes
  - Operaciones para gestionar el historial dental
- DoctorService (CRUD y gestión de doctores)
  - Métodos para administrar horarios y disponibilidad
  - Validación de superposición de citas
- AppointmentService (programación y gestión de citas)
  - Verificación de disponibilidad de doctor
  - Prevención de superposición de citas
  - Gestión del estado de la cita
- TreatmentService (catálogo de tratamientos)
  - Gestión del catálogo de tratamientos disponibles
  - Validación de precios y duración
- OdontogramService (gestión de odontogramas)
  - Registro de lesiones y tratamientos por diente
  - Visualización histórica de cambios
- LesionService (catálogo y registro de lesiones)
  - Mantenimiento del catálogo de lesiones

### DTOs pendientes por implementar:
- PatientDTO (CreatePatientDTO, UpdatePatientDTO, PatientDetailsDTO)
- DoctorDTO (CreateDoctorDTO, UpdateDoctorDTO, DoctorScheduleDTO)
- AppointmentDTO (CreateAppointmentDTO, UpdateAppointmentDTO, AppointmentDetailsDTO)
- TreatmentDTO (CreateTreatmentDTO, UpdateTreatmentDTO, TreatmentDetailsDTO)
- OdontogramDTO (OdontogramDetailsDTO, ToothStatusDTO)
- ToothRecordDTO (CreateToothRecordDTO, ToothHistoryDTO)
- LesionDTO (CreateLesionDTO, UpdateLesionDTO, LesionDetailsDTO) 