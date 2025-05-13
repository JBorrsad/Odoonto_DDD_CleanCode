// EJEMPLO DE INTERFAZ DE REPOSITORIO (Domain Layer)
// Ruta: src/Domain/TuProyecto.Domain/Repositories/Categories/ICategoryRepository.cs

namespace TuProyecto.Domain.Repositories.Categories;

using System;
using System.Threading.Tasks;
using TuProyecto.Domain.Core.Repositories;
using TuProyecto.Domain.Models.Categories;

/// <summary>
/// Características clave de una interfaz de repositorio en DDD:
/// 1. Se define en la capa de dominio
/// 2. Hereda de la interfaz genérica IRepository<T>
/// 3. Define métodos específicos para la entidad
/// 4. No incluye detalles de implementación o infraestructura
/// </summary>
public interface ICategoryRepository : IRepository<Category>
{
    // Los métodos básicos ya vienen de IRepository<T>:
    // - Task<Category> GetById(Guid id)
    // - Task<IEnumerable<Category>> GetAll()
    // - Task Create(Category entity)
    // - Task Update(Category entity)
    // - Task Delete(Category entity)
    // - Task<bool> Exists(Guid id)
    // - etc.

    // Método específico para obtener por Id con relaciones
    Task<Category> GetByIdWithFlowsOrThrow(Guid id);

    // Método específico para cargar categoría sin relaciones
    Task<Category> GetByIdWithoutFlows(Guid id);

    // Operaciones específicas para esta entidad
    Task AddFlowToCategory(Guid categoryId, Guid flowId);
    Task RemoveFlowFromCategory(Guid categoryId, Guid flowId);
}