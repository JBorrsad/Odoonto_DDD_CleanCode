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

## Implementación Completada

### Directrices implementadas:
- Estructura base de servicios de aplicación
- Estructura base para DTOs
- Configuración inicial de AutoMapper
- Servicio de pacientes (PatientService)
- DTOs para pacientes (PatientDto)
- Interfaz de servicio (IPatientService)
- Servicio de doctores (DoctorService)
- DTOs para doctores (DoctorDto, CreateDoctorDto, ScheduleDto)
- Interfaz de servicio (IDoctorService)
- Perfiles de mapeo (DoctorMappingProfile)

## TODO

### Servicios pendientes por implementar:
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
- AppointmentDTO (CreateAppointmentDTO, UpdateAppointmentDTO, AppointmentDetailsDTO)
- TreatmentDTO (CreateTreatmentDTO, UpdateTreatmentDTO, TreatmentDetailsDTO)
- OdontogramDTO (OdontogramDetailsDTO, ToothStatusDTO)
- ToothRecordDTO (CreateToothRecordDTO, ToothHistoryDTO)
- LesionDTO (CreateLesionDTO, UpdateLesionDTO, LesionDetailsDTO) 