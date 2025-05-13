@echo off
echo Actualizando nombres de carpetas y archivos de Odoonto...

REM Crear carpetas con el nuevo nombre
mkdir src\Domain\Odoonto.Domain\Models
mkdir src\Domain\Odoonto.Domain\Repositories
mkdir src\Domain\Odoonto.Domain\Services
mkdir src\Domain\Odoonto.Domain\Events
mkdir src\Domain\Odoonto.Domain.Core\Abstractions
mkdir src\Domain\Odoonto.Domain.Core\Exceptions

mkdir src\Application\Odoonto.Application\Services
mkdir src\Application\Odoonto.Application\DTOs
mkdir src\Application\Odoonto.Application\Interfaces
mkdir src\Application\Odoonto.Application\Mappers

mkdir src\Infrastructure\Odoonto.Infrastructure.InversionOfControl\Inyectors
mkdir src\Infrastructure\Odoonto.Infrastructure.ExceptionsHandler

mkdir src\Data\Odoonto.Data\Repositories
mkdir src\Data\Odoonto.Data.Core\Abstractions
mkdir src\Data\Odoonto.Data.Contexts\Contexts
mkdir src\Data\Odoonto.Data.Contexts\Configurations

mkdir src\Presentation\Odoonto.UI.Server\Controllers
mkdir src\Presentation\Odoonto.UI.Server\Middlewares

mkdir src\Presentation\Odoonto.UI.Client\src\models
mkdir src\Presentation\Odoonto.UI.Client\src\views
mkdir src\Presentation\Odoonto.UI.Client\src\presenters
mkdir src\Presentation\Odoonto.UI.Client\src\services
mkdir src\Presentation\Odoonto.UI.Client\src\services\api
mkdir src\Presentation\Odoonto.UI.Client\src\components
mkdir src\Presentation\Odoonto.UI.Client\src\utils

echo Actualización completada. 