# Script para probar todos los endpoints de la API REST de Odoonto
# ======================================================================
# Este script:
# 1. Levanta la API REST
# 2. Prueba todos los endpoints CRUD para todas las entidades
# 3. Muestra los resultados detallados de las pruebas
# 4. Termina la ejecución del servidor al finalizar

# Configuración y variables globales
$TestsPassed = 0
$TestsFailed = 0
$AllTests = @()
$BaseUrl = "http://localhost:5000/api"
$IdsCreated = @{
    Patient     = $null
    Doctor      = $null
    Appointment = $null
    Odontogram  = $null
    Lesion      = $null
    Treatment   = $null
    TeethIds    = @()
}

# Colores para mensajes
$Green = [ConsoleColor]::Green
$Yellow = [ConsoleColor]::Yellow
$Red = [ConsoleColor]::Red
$Cyan = [ConsoleColor]::Cyan
$Gray = [ConsoleColor]::Gray
$White = [ConsoleColor]::White

# Función para mostrar mensajes de estado
function Write-Status {
    param (
        [string]$Message,
        [ConsoleColor]$Color = [ConsoleColor]::White
    )
    Write-Host $Message -ForegroundColor $Color
}

# Función para registrar resultados de pruebas
function Register-TestResult {
    param (
        [string]$TestName,
        [bool]$Success,
        [string]$ErrorMessage = ""
    )
    
    $global:AllTests += [PSCustomObject]@{
        Name         = $TestName
        Success      = $Success
        ErrorMessage = $ErrorMessage
        Timestamp    = Get-Date
    }
    
    if ($Success) {
        $global:TestsPassed++
    }
    else {
        $global:TestsFailed++
    }
}

# Función para mostrar resumen de pruebas
function Show-TestResults {
    Write-Status "`n====== RESUMEN DE PRUEBAS ======" -Color $Cyan
    Write-Status "Pruebas exitosas: $TestsPassed" -Color $Green
    Write-Status "Pruebas fallidas: $TestsFailed" -Color $Red
    Write-Status "Total de pruebas: $($TestsPassed + $TestsFailed)" -Color $White
    
    if ($TestsFailed -gt 0) {
        Write-Status "`nDetalle de pruebas fallidas:" -Color $Yellow
        $AllTests | Where-Object { -not $_.Success } | ForEach-Object {
            Write-Status "- $($_.Name): $($_.ErrorMessage)" -Color $Red
        }
    }
    
    # Exportar resultados a JSON si hay errores
    if ($TestsFailed -gt 0) {
        $resultsPath = "test-results-$(Get-Date -Format 'yyyyMMdd-HHmmss').json"
        $AllTests | ConvertTo-Json -Depth 4 | Out-File -FilePath $resultsPath
        Write-Status "Resultados detallados exportados a: $resultsPath" -Color $Cyan
    }
}

# Función para realizar peticiones HTTP a la API
function Invoke-ApiRequest {
    param (
        [string]$Method,
        [string]$Endpoint,
        [string]$TestName,
        [string]$Body = $null,
        [string]$ContentType = "application/json",
        [scriptblock]$ValidationScript = { $true },
        [bool]$ReturnErrorResponse = $false,
        [int]$MaxRetries = 3,
        [int]$RetryDelay = 2
    )
    
    $fullUrl = "$BaseUrl/$Endpoint"
    Write-Status "Probando: $TestName" -Color $Cyan
    Write-Status "  $Method $fullUrl" -Color $Gray
    
    $params = @{
        Method      = $Method
        Uri         = $fullUrl
        ContentType = $ContentType
        Headers     = @{
            "Accept" = "application/json"
        }
    }
    
    if ($Body) {
        $params.Body = $Body
        $bodyPreview = if ($Body.Length -gt 100) { "$($Body.Substring(0, 100))..." } else { $Body }
        Write-Status "  Body: $bodyPreview" -Color $Gray
    }
    
    $retryCount = 0
    $success = $false
    $response = $null
    
    while (-not $success -and $retryCount -lt $MaxRetries) {
        try {
            $response = Invoke-RestMethod @params -ErrorAction Stop
            $success = $true
        }
        catch {
            $retryCount++
            $statusCode = $_.Exception.Response.StatusCode.value__
            $errorMessage = $_.Exception.Message
            
            if ($ReturnErrorResponse -and $retryCount -eq $MaxRetries) {
                Write-Status "  × Error: $statusCode - $errorMessage (esperado para este caso de prueba)" -Color $Yellow
                Register-TestResult -TestName $TestName -Success $true
                return $_
            }
            
            if ($retryCount -lt $MaxRetries) {
                Write-Status "  ! Reintentando ($retryCount/$MaxRetries)..." -Color $Yellow
                Start-Sleep -Seconds $RetryDelay
            }
            else {
                Write-Status "  × Error: $statusCode - $errorMessage" -Color $Red
                Register-TestResult -TestName $TestName -Success $false -ErrorMessage "Error $statusCode - $errorMessage"
                return $null
            }
        }
    }
    
    if ($success) {
        # Validar la respuesta usando el script proporcionado
        $validationResult = & $ValidationScript $response
        
        if ($validationResult -eq $true) {
            Write-Status "  √ Prueba exitosa!" -Color $Green
            Register-TestResult -TestName $TestName -Success $true
            return $response
        }
        else {
            Write-Status "  × Falló la validación: $validationResult" -Color $Red
            Register-TestResult -TestName $TestName -Success $false -ErrorMessage "Validación: $validationResult"
            return $null
        }
    }
    
    return $null
}

# Función para iniciar el servidor API
function Start-ApiServer {
    Write-Status "Iniciando el servidor API..." -Color $Yellow
    
    # Verificar y detener procesos en el puerto 5000
    try {
        $existingProcesses = Get-NetTCPConnection -LocalPort 5000 -ErrorAction SilentlyContinue
        foreach ($conn in $existingProcesses) {
            $processId = $conn.OwningProcess
            if ($processId) {
                $process = Get-Process -Id $processId -ErrorAction SilentlyContinue
                if ($process) {
                    Write-Status "Deteniendo proceso en puerto 5000 (PID: $processId)..." -Color $Yellow
                    Stop-Process -Id $processId -Force -ErrorAction Stop
                    Start-Sleep -Seconds 2
                }
            }
        }
    }
    catch {
        Write-Status "No se pudo detener el proceso en puerto 5000. Ejecute como administrador." -Color $Yellow
    }
    
    # Iniciar el servidor API
    Write-Status "Ejecutando: dotnet run --environment Development" -Color $Gray
    $apiProcess = Start-Process -FilePath "dotnet" -ArgumentList "run --environment Development" -NoNewWindow -PassThru
    
    # Esperar a que el servidor esté listo
    Write-Status "Esperando a que el servidor inicie..." -Color $Yellow
    
    $maxAttempts = 15
    $attempts = 0
    $serverStarted = $false
    
    while (-not $serverStarted -and $attempts -lt $maxAttempts) {
        try {
            $attempts++
            Invoke-RestMethod -Uri "$BaseUrl/health" -Method Get -TimeoutSec 2 -ErrorAction Stop | Out-Null
            $serverStarted = $true
        }
        catch [System.Net.WebException] {
            # Si obtenemos una respuesta HTTP (incluso con error) significa que el servidor está respondiendo
            if ($null -ne $_.Exception.Response) {
                $serverStarted = $true
            }
            else {
                Write-Status "Esperando... ($attempts/$maxAttempts)" -Color $Gray
                Start-Sleep -Seconds 2
            }
        }
        catch {
            Write-Status "Esperando... ($attempts/$maxAttempts)" -Color $Gray
            Start-Sleep -Seconds 2
        }
    }
    
    if (-not $serverStarted) {
        Write-Status "No se pudo iniciar el servidor API después de $attempts intentos." -Color $Red
        if ($apiProcess -and -not $apiProcess.HasExited) {
            Stop-Process -Id $apiProcess.Id -Force -ErrorAction SilentlyContinue
        }
        exit 1
    }
    
    Write-Status "¡Servidor API iniciado correctamente!" -Color $Green
    return $apiProcess
}

# Función para probar endpoints de Pacientes
function Test-Patients {
    Write-Status "`n=== Probando endpoints de Pacientes ===" -Color $White
    
    # Crear paciente
    $newPatient = @{
        firstName      = "Juan"
        lastName       = "Pérez"
        birthDate      = (Get-Date).AddYears(-30).ToString("yyyy-MM-dd")
        gender         = "Male"
        contactInfo    = @{
            email   = "juan.test@ejemplo.com"
            phone   = "555-123-4567"
            address = "Calle Test 123"
        }
        medicalHistory = "Sin antecedentes médicos relevantes"
        allergies      = @("Ninguna")
    } | ConvertTo-Json -Depth 3
    
    $createdPatient = Invoke-ApiRequest -Method "POST" -Endpoint "patient" -TestName "Crear paciente" -Body $newPatient -ValidationScript {
        param($response)
        if (-not $response.id) { return "No se devolvió el ID del paciente" }
        if ($response.firstName -ne "Juan") { return "El nombre no coincide" }
        return $true
    }
    
    if (-not $createdPatient) { return $false }
    $IdsCreated.Patient = $createdPatient.id
    
    # Obtener todos los pacientes
    $allPatients = Invoke-ApiRequest -Method "GET" -Endpoint "patient" -TestName "Obtener todos los pacientes" -ValidationScript {
        param($response)
        if (-not ($response -is [array])) { return "La respuesta no es un array" }
        return $true
    }
    
    if (-not $allPatients) { return $false }
    
    # Obtener paciente por ID
    $patient = Invoke-ApiRequest -Method "GET" -Endpoint "patient/$($IdsCreated.Patient)" -TestName "Obtener paciente por ID" -ValidationScript {
        param($response)
        if ($response.id -ne $IdsCreated.Patient) { return "El ID del paciente no coincide" }
        return $true
    }
    
    if (-not $patient) { return $false }
    
    # Buscar pacientes por nombre
    Invoke-ApiRequest -Method "GET" -Endpoint "patient/search?searchTerm=Juan" -TestName "Buscar pacientes por nombre" -ValidationScript {
        param($response)
        if (-not ($response -is [array])) { return "La respuesta no es un array" }
        return $true
    }
    
    # Actualizar paciente
    $updatePatient = @{
        firstName      = "Juan Carlos"
        lastName       = "Pérez"
        birthDate      = (Get-Date).AddYears(-30).ToString("yyyy-MM-dd")
        gender         = "Male"
        contactInfo    = @{
            email   = "juancarlos.test@ejemplo.com"
            phone   = "555-123-4567"
            address = "Avenida Test 456"
        }
        medicalHistory = "Sin antecedentes médicos relevantes"
        allergies      = @("Ninguna")
    } | ConvertTo-Json -Depth 3
    
    $updatedPatient = Invoke-ApiRequest -Method "PUT" -Endpoint "patient/$($IdsCreated.Patient)" -TestName "Actualizar paciente" -Body $updatePatient -ValidationScript {
        param($response)
        if ($response.firstName -ne "Juan Carlos") { return "El nombre no se actualizó correctamente" }
        return $true
    }
    
    if (-not $updatedPatient) { return $false }
    
    return $true
}

# Función para probar endpoints de Doctores
function Test-Doctors {
    Write-Status "`n=== Probando endpoints de Doctores ===" -Color $White
    
    # Crear doctor
    $newDoctor = @{
        firstName      = "María"
        lastName       = "González"
        specialization = "Odontología General"
        licenseNumber  = "ODO-12345-TEST"
        contactInfo    = @{
            email   = "maria.test@odoonto.com"
            phone   = "555-987-6543"
            address = "Centro Médico Test, Consultorio 201"
        }
    } | ConvertTo-Json -Depth 3
    
    $createdDoctor = Invoke-ApiRequest -Method "POST" -Endpoint "doctor" -TestName "Crear doctor" -Body $newDoctor -ValidationScript {
        param($response)
        if (-not $response.id) { return "No se devolvió el ID del doctor" }
        if ($response.firstName -ne "María") { return "El nombre no coincide" }
        return $true
    }
    
    if (-not $createdDoctor) { return $false }
    $IdsCreated.Doctor = $createdDoctor.id
    
    # Obtener todos los doctores
    $allDoctors = Invoke-ApiRequest -Method "GET" -Endpoint "doctor" -TestName "Obtener todos los doctores" -ValidationScript {
        param($response)
        if (-not ($response -is [array])) { return "La respuesta no es un array" }
        return $true
    }
    
    if (-not $allDoctors) { return $false }
    
    # Obtener doctor por ID
    $doctor = Invoke-ApiRequest -Method "GET" -Endpoint "doctor/$($IdsCreated.Doctor)" -TestName "Obtener doctor por ID" -ValidationScript {
        param($response)
        if ($response.id -ne $IdsCreated.Doctor) { return "El ID del doctor no coincide" }
        return $true
    }
    
    if (-not $doctor) { return $false }
    
    # Actualizar doctor
    $updateDoctor = @{
        firstName      = "María Elena"
        lastName       = "González"
        specialization = "Ortodoncia"
        licenseNumber  = "ODO-12345-TEST"
        contactInfo    = @{
            email   = "mariaelena.test@odoonto.com"
            phone   = "555-987-6543"
            address = "Centro Médico Test, Consultorio 301"
        }
    } | ConvertTo-Json -Depth 3
    
    $updatedDoctor = Invoke-ApiRequest -Method "PUT" -Endpoint "doctor/$($IdsCreated.Doctor)" -TestName "Actualizar doctor" -Body $updateDoctor -ValidationScript {
        param($response)
        if ($response.specialization -ne "Ortodoncia") { return "La especialización no se actualizó correctamente" }
        return $true
    }
    
    if (-not $updatedDoctor) { return $false }
    
    return $true
}

# Función para probar endpoints de Lesiones
function Test-Lesions {
    Write-Status "`n=== Probando endpoints de Lesiones ===" -Color $White
    
    # Crear lesión
    $newLesion = @{
        name        = "Caries TEST"
        description = "Lesión de prueba en etapa temprana"
        severity    = "Low"
        color       = "#FFCC00"
    } | ConvertTo-Json
    
    $createdLesion = Invoke-ApiRequest -Method "POST" -Endpoint "lesion" -TestName "Crear lesión" -Body $newLesion -ValidationScript {
        param($response)
        if (-not $response.id) { return "No se devolvió el ID de la lesión" }
        if ($response.name -ne "Caries TEST") { return "El nombre no coincide" }
        return $true
    }
    
    if (-not $createdLesion) { return $false }
    $IdsCreated.Lesion = $createdLesion.id
    
    # Obtener todas las lesiones
    $allLesions = Invoke-ApiRequest -Method "GET" -Endpoint "lesion" -TestName "Obtener todas las lesiones" -ValidationScript {
        param($response)
        if (-not ($response -is [array])) { return "La respuesta no es un array" }
        return $true
    }
    
    if (-not $allLesions) { return $false }
    
    # Obtener lesión por ID
    $lesion = Invoke-ApiRequest -Method "GET" -Endpoint "lesion/$($IdsCreated.Lesion)" -TestName "Obtener lesión por ID" -ValidationScript {
        param($response)
        if ($response.id -ne $IdsCreated.Lesion) { return "El ID de la lesión no coincide" }
        return $true
    }
    
    if (-not $lesion) { return $false }
    
    # Actualizar lesión
    $updateLesion = @{
        name        = "Caries Moderada TEST"
        description = "Lesión de prueba en etapa media"
        severity    = "Medium"
        color       = "#FF9900"
    } | ConvertTo-Json
    
    $updatedLesion = Invoke-ApiRequest -Method "PUT" -Endpoint "lesion/$($IdsCreated.Lesion)" -TestName "Actualizar lesión" -Body $updateLesion -ValidationScript {
        param($response)
        if ($response.name -ne "Caries Moderada TEST") { return "El nombre no se actualizó correctamente" }
        if ($response.severity -ne "Medium") { return "La severidad no se actualizó correctamente" }
        return $true
    }
    
    if (-not $updatedLesion) { return $false }
    
    return $true
}

# Función para probar endpoints de Tratamientos
function Test-Treatments {
    Write-Status "`n=== Probando endpoints de Tratamientos ===" -Color $White
    
    # Crear tratamiento
    $newTreatment = @{
        name              = "Empaste TEST"
        description       = "Tratamiento de prueba con composite"
        price             = @{
            amount   = 150.00
            currency = "USD"
        }
        estimatedDuration = 30
    } | ConvertTo-Json -Depth 3
    
    $createdTreatment = Invoke-ApiRequest -Method "POST" -Endpoint "treatment" -TestName "Crear tratamiento" -Body $newTreatment -ValidationScript {
        param($response)
        if (-not $response.id) { return "No se devolvió el ID del tratamiento" }
        if ($response.name -ne "Empaste TEST") { return "El nombre no coincide" }
        return $true
    }
    
    if (-not $createdTreatment) { return $false }
    $IdsCreated.Treatment = $createdTreatment.id
    
    # Obtener todos los tratamientos
    $allTreatments = Invoke-ApiRequest -Method "GET" -Endpoint "treatment" -TestName "Obtener todos los tratamientos" -ValidationScript {
        param($response)
        if (-not ($response -is [array])) { return "La respuesta no es un array" }
        return $true
    }
    
    if (-not $allTreatments) { return $false }
    
    # Obtener tratamiento por ID
    $treatment = Invoke-ApiRequest -Method "GET" -Endpoint "treatment/$($IdsCreated.Treatment)" -TestName "Obtener tratamiento por ID" -ValidationScript {
        param($response)
        if ($response.id -ne $IdsCreated.Treatment) { return "El ID del tratamiento no coincide" }
        return $true
    }
    
    if (-not $treatment) { return $false }
    
    # Actualizar tratamiento
    $updateTreatment = @{
        name              = "Empaste Compuesto TEST"
        description       = "Tratamiento de prueba con composite de alta duración"
        price             = @{
            amount   = 180.00
            currency = "USD"
        }
        estimatedDuration = 45
    } | ConvertTo-Json -Depth 3
    
    $updatedTreatment = Invoke-ApiRequest -Method "PUT" -Endpoint "treatment/$($IdsCreated.Treatment)" -TestName "Actualizar tratamiento" -Body $updateTreatment -ValidationScript {
        param($response)
        if ($response.name -ne "Empaste Compuesto TEST") { return "El nombre no se actualizó correctamente" }
        if ($response.estimatedDuration -ne 45) { return "La duración no se actualizó correctamente" }
        return $true
    }
    
    if (-not $updatedTreatment) { return $false }
    
    return $true
}

# Función para probar endpoints de Citas
function Test-Appointments {
    Write-Status "`n=== Probando endpoints de Citas ===" -Color $White
    
    if (-not $IdsCreated.Patient -or -not $IdsCreated.Doctor) {
        Write-Status "No se pueden probar las citas sin IDs válidos de paciente y doctor" -Color $Yellow
        return $false
    }
    
    # Crear cita
    $tomorrow = (Get-Date).AddDays(1)
    $startTime = Get-Date -Date $tomorrow -Hour 10 -Minute 0 -Second 0
    
    $newAppointment = @{
        patientId = $IdsCreated.Patient
        doctorId  = $IdsCreated.Doctor
        dateTime  = $startTime.ToString("yyyy-MM-ddTHH:mm:ss")
        duration  = 60
        status    = "Scheduled"
        type      = "Regular"
        notes     = "Cita de prueba automatizada"
    } | ConvertTo-Json
    
    $createdAppointment = Invoke-ApiRequest -Method "POST" -Endpoint "appointment" -TestName "Crear cita" -Body $newAppointment -ValidationScript {
        param($response)
        if (-not $response.id) { return "No se devolvió el ID de la cita" }
        if ($response.patientId -ne $IdsCreated.Patient) { return "El ID del paciente no coincide" }
        if ($response.doctorId -ne $IdsCreated.Doctor) { return "El ID del doctor no coincide" }
        return $true
    }
    
    if (-not $createdAppointment) { return $false }
    $IdsCreated.Appointment = $createdAppointment.id
    
    # Obtener cita por ID
    $appointment = Invoke-ApiRequest -Method "GET" -Endpoint "appointment/$($IdsCreated.Appointment)" -TestName "Obtener cita por ID" -ValidationScript {
        param($response)
        if ($response.id -ne $IdsCreated.Appointment) { return "El ID de la cita no coincide" }
        return $true
    }
    
    if (-not $appointment) { return $false }
    
    # Obtener citas por paciente
    Invoke-ApiRequest -Method "GET" -Endpoint "appointment/patient/$($IdsCreated.Patient)" -TestName "Obtener citas por paciente" -ValidationScript {
        param($response)
        if (-not ($response -is [array])) { return "La respuesta no es un array" }
        return $true
    }
    
    # Obtener citas por doctor
    Invoke-ApiRequest -Method "GET" -Endpoint "appointment/doctor/$($IdsCreated.Doctor)" -TestName "Obtener citas por doctor" -ValidationScript {
        param($response)
        if (-not ($response -is [array])) { return "La respuesta no es un array" }
        return $true
    }
    
    # Actualizar cita
    $updateAppointment = @{
        patientId = $IdsCreated.Patient
        doctorId  = $IdsCreated.Doctor
        dateTime  = $startTime.AddHours(1).ToString("yyyy-MM-ddTHH:mm:ss")
        duration  = 45
        status    = "Confirmed"
        type      = "Regular"
        notes     = "Cita de prueba actualizada"
    } | ConvertTo-Json
    
    $updatedAppointment = Invoke-ApiRequest -Method "PUT" -Endpoint "appointment/$($IdsCreated.Appointment)" -TestName "Actualizar cita" -Body $updateAppointment -ValidationScript {
        param($response)
        if ($response.status -ne "Confirmed") { return "El estado no se actualizó correctamente" }
        if ($response.duration -ne 45) { return "La duración no se actualizó correctamente" }
        return $true
    }
    
    if (-not $updatedAppointment) { return $false }
    
    # Marcar cita como en sala de espera
    Invoke-ApiRequest -Method "PATCH" -Endpoint "appointment/$($IdsCreated.Appointment)/waiting-room" -TestName "Marcar cita como en sala de espera" -ValidationScript {
        param($response)
        if ($response.status -ne "WaitingRoom") { return "El estado no se actualizó correctamente" }
        return $true
    }
    
    # Marcar cita como en progreso
    Invoke-ApiRequest -Method "PATCH" -Endpoint "appointment/$($IdsCreated.Appointment)/in-progress" -TestName "Marcar cita como en progreso" -ValidationScript {
        param($response)
        if ($response.status -ne "InProgress") { return "El estado no se actualizó correctamente" }
        return $true
    }
    
    # Marcar cita como completada
    Invoke-ApiRequest -Method "PATCH" -Endpoint "appointment/$($IdsCreated.Appointment)/completed" -TestName "Marcar cita como completada" -ValidationScript {
        param($response)
        if ($response.status -ne "Completed") { return "El estado no se actualizó correctamente" }
        return $true
    }
    
    return $true
}

# Función para probar endpoints de Odontogramas
function Test-Odontograms {
    Write-Status "`n=== Probando endpoints de Odontogramas ===" -Color $White
    
    if (-not $IdsCreated.Patient) {
        Write-Status "No se pueden probar los odontogramas sin un ID válido de paciente" -Color $Yellow
        return $false
    }
    
    # Crear odontograma
    $createdOdontogram = Invoke-ApiRequest -Method "POST" -Endpoint "odontogram/patient/$($IdsCreated.Patient)" -TestName "Crear odontograma" -ValidationScript {
        param($response)
        if (-not $response.id) { return "No se devolvió el ID del odontograma" }
        if ($response.patientId -ne $IdsCreated.Patient) { return "El ID del paciente no coincide" }
        return $true
    }
    
    if (-not $createdOdontogram) { return $false }
    $IdsCreated.Odontogram = $createdOdontogram.id
    
    # Obtener odontograma por ID de paciente
    $odontogram = Invoke-ApiRequest -Method "GET" -Endpoint "odontogram/patient/$($IdsCreated.Patient)" -TestName "Obtener odontograma por ID de paciente" -ValidationScript {
        param($response)
        if (-not $response.id) { return "No se devolvió un odontograma válido" }
        if ($response.id -ne $IdsCreated.Odontogram) { return "El ID del odontograma no coincide" }
        return $true
    }
    
    if (-not $odontogram) { return $false }
    
    # Agregar un registro dental
    $teethNumbers = @(11, 21, 36, 46)
    $toothRecords = @()
    
    foreach ($toothNumber in $teethNumbers) {
        $toothRecord = @{
            toothNumber  = $toothNumber
            observations = "Diente de prueba $toothNumber"
            status       = "Healthy"
        } | ConvertTo-Json
        
        $updatedOdontogram = Invoke-ApiRequest -Method "POST" -Endpoint "odontogram/$($IdsCreated.Odontogram)/tooth" -TestName "Agregar registro dental para diente $toothNumber" -Body $toothRecord -ValidationScript {
            param($response)
            if (-not $response.toothRecords -or $response.toothRecords.Count -eq 0) { 
                return "No se agregó el registro dental" 
            }
            $tooth = $response.toothRecords | Where-Object { $_.toothNumber -eq $toothNumber }
            if (-not $tooth) {
                return "No se encontró el registro del diente $toothNumber"
            }
            return $true
        }
        
        if (-not $updatedOdontogram) { continue }
        
        $tooth = $updatedOdontogram.toothRecords | Where-Object { $_.toothNumber -eq $toothNumber }
        if ($tooth) {
            $toothRecords += $tooth
            $IdsCreated.TeethIds += $tooth.id
        }
    }
    
    # Si tenemos un ID de lesión y al menos un diente, agregar una lesión al diente
    if ($IdsCreated.Lesion -and $toothRecords.Count -gt 0) {
        $firstToothNumber = $toothRecords[0].toothNumber
        
        $lesionRecord = @{
            lesionId     = $IdsCreated.Lesion
            location     = "Distal"
            observations = "Lesión de prueba en diente $firstToothNumber"
        } | ConvertTo-Json
        
        Invoke-ApiRequest -Method "POST" -Endpoint "odontogram/$($IdsCreated.Odontogram)/tooth/$firstToothNumber/lesion" -TestName "Agregar lesión a diente $firstToothNumber" -Body $lesionRecord -ValidationScript {
            param($response)
            if (-not $response.toothRecords) { return "No se devolvieron registros dentales" }
            $tooth = $response.toothRecords | Where-Object { $_.toothNumber -eq $firstToothNumber }
            if (-not $tooth) { return "No se encontró el diente $firstToothNumber" }
            if (-not $tooth.lesions -or $tooth.lesions.Count -eq 0) { 
                return "No se agregó la lesión al diente" 
            }
            return $true
        }
    }
    
    # Si tenemos un ID de tratamiento y al menos dos dientes, agregar un procedimiento al segundo diente
    if ($IdsCreated.Treatment -and $toothRecords.Count -gt 1) {
        $secondToothNumber = $toothRecords[1].toothNumber
        
        $procedureRecord = @{
            treatmentId  = $IdsCreated.Treatment
            date         = (Get-Date).ToString("yyyy-MM-ddTHH:mm:ss")
            observations = "Procedimiento de prueba en diente $secondToothNumber"
        } | ConvertTo-Json
        
        Invoke-ApiRequest -Method "POST" -Endpoint "odontogram/$($IdsCreated.Odontogram)/tooth/$secondToothNumber/procedure" -TestName "Agregar procedimiento a diente $secondToothNumber" -Body $procedureRecord -ValidationScript {
            param($response)
            if (-not $response.toothRecords) { return "No se devolvieron registros dentales" }
            $tooth = $response.toothRecords | Where-Object { $_.toothNumber -eq $secondToothNumber }
            if (-not $tooth) { return "No se encontró el diente $secondToothNumber" }
            if (-not $tooth.procedures -or $tooth.procedures.Count -eq 0) { 
                return "No se agregó el procedimiento al diente" 
            }
            return $true
        }
    }
    
    return $true
}

# Función para limpiar los datos de prueba
function Remove-TestData {
    Write-Status "`n=== Limpiando datos de prueba ===" -Color $Yellow
    
    # Eliminar cita
    if ($IdsCreated.Appointment) {
        Invoke-ApiRequest -Method "DELETE" -Endpoint "appointment/$($IdsCreated.Appointment)" -TestName "Eliminar cita" -ValidationScript {
            param($response)
            return $true
        }
    }
    
    # No podemos eliminar el odontograma directamente, 
    # se eliminará al eliminar el paciente asociado
    
    # Eliminar lesión
    if ($IdsCreated.Lesion) {
        Invoke-ApiRequest -Method "DELETE" -Endpoint "lesion/$($IdsCreated.Lesion)" -TestName "Eliminar lesión" -ValidationScript {
            param($response)
            return $true
        }
    }
    
    # Eliminar tratamiento
    if ($IdsCreated.Treatment) {
        Invoke-ApiRequest -Method "DELETE" -Endpoint "treatment/$($IdsCreated.Treatment)" -TestName "Eliminar tratamiento" -ValidationScript {
            param($response)
            return $true
        }
    }
    
    # Eliminar doctor
    if ($IdsCreated.Doctor) {
        Invoke-ApiRequest -Method "DELETE" -Endpoint "doctor/$($IdsCreated.Doctor)" -TestName "Eliminar doctor" -ValidationScript {
            param($response)
            return $true
        }
    }
    
    # Eliminar paciente (también eliminará su odontograma)
    if ($IdsCreated.Patient) {
        Invoke-ApiRequest -Method "DELETE" -Endpoint "patient/$($IdsCreated.Patient)" -TestName "Eliminar paciente" -ValidationScript {
            param($response)
            return $true
        }
    }
}

# Función principal que ejecuta todas las pruebas
function Start-EndpointTest {
    $startTime = Get-Date
    $apiProcess = $null
    
    try {
        Write-Status "=== INICIANDO PRUEBAS DE LA API REST DE ODOONTO ===" -Color $Cyan
        Write-Status "Fecha y hora: $startTime" -Color $Gray
        
        # Iniciar el servidor API
        $apiProcess = Start-ApiServer
        
        # Ejecutar las pruebas
        $testsSuccessful = $true
        
        # Pruebas de Pacientes
        $patientSuccess = Test-Patients
        $testsSuccessful = $testsSuccessful -and $patientSuccess
        
        # Pruebas de Doctores
        $doctorSuccess = Test-Doctors
        $testsSuccessful = $testsSuccessful -and $doctorSuccess
        
        # Pruebas de Lesiones
        $lesionSuccess = Test-Lesions
        $testsSuccessful = $testsSuccessful -and $lesionSuccess
        
        # Pruebas de Tratamientos
        $treatmentSuccess = Test-Treatments
        $testsSuccessful = $testsSuccessful -and $treatmentSuccess
        
        # Pruebas de Citas
        if ($patientSuccess -and $doctorSuccess) {
            $appointmentSuccess = Test-Appointments
            $testsSuccessful = $testsSuccessful -and $appointmentSuccess
        }
        else {
            Write-Status "Se omiten las pruebas de citas debido a errores previos" -Color $Yellow
        }
        
        # Pruebas de Odontogramas
        if ($patientSuccess) {
            $odontogramSuccess = Test-Odontograms
            $testsSuccessful = $testsSuccessful -and $odontogramSuccess
        }
        else {
            Write-Status "Se omiten las pruebas de odontogramas debido a errores previos" -Color $Yellow
        }
        
        # Limpiar los datos de prueba
        Remove-TestData
        
        # Mostrar resultados
        Show-TestResults
        
        $endTime = Get-Date
        $duration = New-TimeSpan -Start $startTime -End $endTime
        
        Write-Status "`n=== RESUMEN DE EJECUCIÓN ===" -Color $Cyan
        Write-Status "Duración total: $($duration.Minutes) minutos, $($duration.Seconds) segundos" -Color $Gray
        
        if ($testsSuccessful) {
            Write-Status "Estado general: EXITOSO - Todas las pruebas pasaron correctamente" -Color $Green
        }
        else {
            Write-Status "Estado general: CON ERRORES - Algunas pruebas fallaron" -Color $Red
        }
    }
    catch {
        Write-Status "Error crítico en la ejecución de las pruebas: $_" -Color $Red
        Write-Status $_.ScriptStackTrace -Color $Gray
    }
    finally {
        if ($apiProcess -and -not $apiProcess.HasExited) {
            Write-Status "`nDeteniendo el servidor API..." -Color $Yellow
            try {
                Stop-Process -Id $apiProcess.Id -Force -ErrorAction Stop
                Write-Status "Servidor API detenido correctamente." -Color $Green
            }
            catch {
                Write-Status "No se pudo detener el servidor API: $_" -Color $Red
            }
        }
        
        Write-Status "Pruebas finalizadas." -Color $Cyan
    }
}

# Ejecutar las pruebas
Start-EndpointTest 