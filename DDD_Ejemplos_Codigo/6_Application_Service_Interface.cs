// EJEMPLO DE INTERFAZ DE SERVICIO DE APLICACIÓN (Application Layer)
// Ruta: src/Application/TuProyecto.Application/Interfaces/Categories/ICategoryAppService.cs

namespace TuProyecto.Application.Interfaces.Categories;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TuProyecto.Application.DTO.Categories;

/// <summary>
/// Características clave de una interfaz de servicio de aplicación en DDD:
/// 1. Define operaciones de alto nivel (casos de uso)
/// 2. Utiliza DTOs para entrada y salida de datos
/// 3. Define operaciones asíncronas (Task)
/// 4. Nombrado descriptivo orientado a casos de uso
/// 5. Operaciones CRUD básicas + operaciones específicas
/// </summary>
public interface ICategoryAppService
{
    // Operaciones CRUD básicas
    Task Create(CategoryCreateDto data);
    Task Update(Guid categoryId, CategoryUpdateDto data);
    Task Remove(Guid categoryId);
    Task<CategoryReadDto> GetById(Guid categoryId);
    Task<IEnumerable<CategoryQueryDto>> GetAll();

    // Operaciones específicas para este dominio
    Task AddFlowToCategory(Guid categoryId, AddFlowToCategoryDto data);
    Task RemoveFlowFromCategory(Guid categoryId, Guid flowId);

    // Otras operaciones posibles según casos de uso
    // Task<IEnumerable<CategoryQueryDto>> GetByName(string namePattern);
    // Task<int> GetCategoryFlowCount(Guid categoryId);
}