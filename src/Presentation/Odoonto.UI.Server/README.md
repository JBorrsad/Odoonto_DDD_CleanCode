# Odoonto API

API REST para el sistema de gestión odontológica Odoonto.

## Requisitos

- .NET 8.0 SDK
- Cuenta de Firebase (Firestore)

## Configuración de Firebase

1. **Crear proyecto en Firebase**:

   - Ve a la [consola de Firebase](https://console.firebase.google.com/)
   - Crea un nuevo proyecto o usa uno existente
   - Activa Firestore en tu proyecto

2. **Configurar credenciales**:

   - En la consola de Firebase, ve a Configuración del proyecto > Cuentas de servicio
   - Genera una nueva clave privada (descargará un archivo JSON)
   - Renombra el archivo descargado a `firebase-credentials.json`
   - Coloca este archivo en la raíz del proyecto `src/Presentation/Odoonto.UI.Server/`

3. **Configurar appsettings**:
   - Abre `appsettings.json` y `appsettings.Development.json`
   - Actualiza el valor de `Firebase:ApiKey` con tu Web API Key de Firebase
   - Puedes encontrar esta clave en Configuración del proyecto > Configuración general

## Configuración

1. **Base de datos**:

   - La conexión a la base de datos está configurada en `appsettings.json` y `appsettings.Development.json`.
   - Por defecto, usa SQL Server LocalDB.

2. **Migraciones**:
   - Para crear las migraciones, ejecutar:
     ```
     dotnet ef migrations add InitialMigration -p src/Data/Odoonto.Data.Contexts -s src/Presentation/Odoonto.UI.Server
     ```
   - Para aplicar las migraciones a la base de datos, ejecutar:
     ```
     dotnet ef database update -p src/Data/Odoonto.Data.Contexts -s src/Presentation/Odoonto.UI.Server
     ```

## Ejecución

Para ejecutar la aplicación:

```
cd src/Presentation/Odoonto.UI.Server
dotnet run
```

La API estará disponible en:

- HTTP: http://localhost:5000
- HTTPS: https://localhost:5001

## Pruebas con Swagger

La interfaz de Swagger estará disponible en:

- HTTP: http://localhost:5000/swagger
- HTTPS: https://localhost:5001/swagger

## Endpoints disponibles

La API expone las siguientes entidades:

1. **Pacientes** (`/api/Patient`)
2. **Doctores** (`/api/Doctor`)
3. **Citas** (`/api/Appointment`)
4. **Tratamientos** (`/api/Treatment`)
5. **Lesiones** (`/api/Lesion`)

Cada entidad tiene endpoints para las operaciones CRUD básicas, y algunos adicionales específicos para cada entidad.

## Estructura de datos en Firestore

Las colecciones en Firestore están estructuradas de la siguiente manera:

- **patients**: Almacena la información de los pacientes
- **doctors**: Almacena la información de los doctores
- **appointments**: Almacena las citas médicas
- **treatments**: Almacena los tratamientos
- **lesions**: Almacena el catálogo de lesiones/patologías

Cada documento tiene un ID único que corresponde al Guid generado en la aplicación.
