# Odoonto API Server

Esta API REST implementa los servicios backend para la aplicación Odoonto de gestión odontológica, siguiendo una arquitectura DDD (Domain Driven Design) y Clean Architecture.

## Configuración de Firebase

Para configurar Firebase, sigue estos pasos:

1. Crea un proyecto en Firebase Console (https://console.firebase.google.com/)
2. Habilita Firestore Database en tu proyecto
3. Genera una clave de cuenta de servicio:
   - Ve a Configuración del proyecto > Cuentas de servicio
   - Selecciona Firebase Admin SDK
   - Haz clic en "Generar nueva clave privada"
   - Guarda el archivo JSON descargado como `firebase-credentials.json` en la raíz de este proyecto

4. Actualiza `appsettings.json` con la configuración correcta:
   ```json
   "Firebase": {
     "ApiKey": "[Tu API Key]",
     "CredentialsPath": "firebase-credentials.json",
     "ProjectId": "[Tu Project ID]"
   }
   ```

## Estructura de la Base de Datos

Los datos se almacenan en colecciones de Firestore con la siguiente estructura:

- `doctors`: Información de doctores
- `patients`: Información de pacientes
- `appointments`: Citas programadas
- `treatments`: Catálogo de tratamientos
- `odontograms`: Odontogramas por paciente

## Ejecución de la API

Para ejecutar la API localmente:

```bash
dotnet run
```

La API estará disponible en https://localhost:5001 y http://localhost:5000

## Documentación de la API

La documentación de la API está disponible a través de Swagger en la ruta raíz de la aplicación:

- Local: https://localhost:5001/

## Variables de Entorno

Puedes configurar las siguientes variables de entorno en lugar de usar `appsettings.json`:

- `Firebase__ApiKey`: Clave API de Firebase
- `Firebase__CredentialsPath`: Ruta al archivo de credenciales
- `Firebase__ProjectId`: ID del proyecto Firebase 