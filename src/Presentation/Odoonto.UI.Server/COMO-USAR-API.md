# Guía para levantar y probar la API REST de Odoonto con Firebase

Esta guía explica cómo configurar, levantar y probar la API REST de Odoonto con almacenamiento en Firebase.

## Requisitos previos

- .NET 8.0 SDK
- PowerShell (Windows) o Bash (Linux/Mac)
- Proyecto Firebase (para datos persistentes)
- Postman, VS Code con extensión REST Client o cualquier cliente HTTP

## Pasos para configurar Firebase

1. **Crear un proyecto en Firebase**:

   - Accede a [Firebase Console](https://console.firebase.google.com/)
   - Crea un nuevo proyecto
   - Habilita Firestore (modo nativo)
   - Habilita Authentication si necesitas autenticación

2. **Configurar las credenciales**:

   - En la consola de Firebase, ve a Configuración > Cuentas de servicio
   - Genera una nueva clave privada (se descargará un archivo JSON)
   - Copia este archivo a `src/Presentation/Odoonto.UI.Server/firebase-credentials.json`

3. **Actualizar la configuración en `appsettings.json`**:
   - Abre `src/Presentation/Odoonto.UI.Server/appsettings.json`
   - Actualiza los valores en la sección "Firebase" con los datos de tu proyecto

```json
"Firebase": {
  "ApiKey": "TU_API_KEY",
  "ProjectId": "tu-proyecto-id",
  "AuthDomain": "tu-proyecto-id.firebaseapp.com",
  "StorageBucket": "tu-proyecto-id.appspot.com",
  "MessagingSenderId": "1234567890",
  "AppId": "1:1234567890:web:abcdef1234567890",
  "DatabaseUrl": "https://tu-proyecto-id.firebaseio.com",
  "CredentialsPath": "firebase-credentials.json"
}
```

## Levantar la API

### En Windows con PowerShell

1. Navega a la carpeta del proyecto:

   ```powershell
   cd C:\Repos\Odoonto\src\Presentation\Odoonto.UI.Server
   ```

2. Ejecuta el script para levantar la API:
   ```powershell
   .\run-api.bat
   ```

### En Linux/Mac con Bash

1. Navega a la carpeta del proyecto:

   ```bash
   cd /c/Repos/Odoonto/src/Presentation/Odoonto.UI.Server
   ```

2. Asegúrate de que el script tenga permisos de ejecución:

   ```bash
   chmod +x run-api.sh
   ```

3. Ejecuta el script:
   ```bash
   ./run-api.sh
   ```

## Probar la API manualmente

Una vez que la API esté en ejecución, puedes probarla de varias formas:

### Usando Swagger UI

1. Abre un navegador web
2. Accede a http://localhost:5000
3. Verás la interfaz de Swagger que muestra todos los endpoints disponibles
4. Puedes probar cada endpoint directamente desde esta interfaz

### Usando el archivo HTTP

Se incluye un archivo `test-api.http` que contiene ejemplos de todas las peticiones. Puedes usarlo con:

- VS Code con la extensión REST Client
- JetBrains Rider o IntelliJ IDEA

### Usando el script de prueba automatizado

1. Abre una nueva ventana de PowerShell (no cierres la que está ejecutando la API)
2. Navega a la carpeta del proyecto:

   ```powershell
   cd C:\Repos\Odoonto\src\Presentation\Odoonto.UI.Server
   ```

3. Ejecuta el script de prueba:
   ```powershell
   .\test-api.ps1
   ```

Este script:

- Levanta la API si no está en ejecución
- Crea entidades de prueba (pacientes, doctores, tratamientos, etc.)
- Realiza operaciones CRUD en todas las entidades
- Guarda los datos en Firebase
- Opcionalmente limpia los datos de prueba al finalizar

## Estructura de colecciones en Firebase

El API almacena los datos en las siguientes colecciones de Firestore:

- **patients**: Pacientes registrados
- **doctors**: Doctores/odontólogos
- **appointments**: Citas programadas
- **odontograms**: Odontogramas de los pacientes
- **lesions**: Catálogo de lesiones
- **treatments**: Catálogo de tratamientos

## Solución de problemas

### La API no se inicia

- Asegúrate de que el puerto 5000 no esté siendo usado por otra aplicación
- Verifica que las credenciales de Firebase sean correctas y estén en la ubicación adecuada

### Error al conectar con Firebase

- Verifica que el archivo de credenciales sea válido
- Asegúrate de que el proyecto de Firebase tenga habilitadas las APIs necesarias
- Comprueba que los valores en `appsettings.json` correspondan a tu proyecto de Firebase

### Errores 404 en los endpoints

- Verifica que la ruta sea correcta (sensible a mayúsculas/minúsculas)
- Asegúrate de que el controlador correspondiente esté implementado
- Revisa los logs en la consola donde se ejecuta la API para más información

## Próximos pasos

Para ampliar la funcionalidad de la API:

1. **Implementar autenticación**: Integrar Firebase Authentication
2. **Añadir autorización**: Roles de usuario y permisos
3. **Mejorar consultas**: Implementar paginación y filtrado avanzado
4. **Añadir validaciones**: Reglas de negocio adicionales
5. **Integrar almacenamiento**: Para imágenes y archivos adjuntos
