// EJEMPLO DE IMPLEMENTACIÓN DE REPOSITORIO (Data Layer)
// Ruta: src/Data/TuProyecto.Data/Repositories/Categories/CategoryRepository.cs

namespace TuProyecto.Data.Repositories.Categories;

using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TuProyecto.Data.Core.Contexts;
using TuProyecto.Data.Core.Repositories;
using TuProyecto.Domain.Core.Models.Exceptions;
using TuProyecto.Domain.Models.Categories;
using TuProyecto.Domain.Models.Flows;
using TuProyecto.Domain.Repositories.Categories;

/// <summary>
/// Características clave de una implementación de repositorio en DDD:
/// 1. Implementa la interfaz definida en el dominio
/// 2. Hereda de una clase base Repository<T> que implementa operaciones comunes
/// 3. Utiliza Entity Framework Core para acceso a datos
/// 4. Maneja detalles específicos de carga y persistencia
/// 5. Maneja excepciones específicas del dominio
/// </summary>
public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private readonly ApplicationContext _context;

    public CategoryRepository(ApplicationContext context) : base(context)
    {
        _context = context;
    }

    // Sobrescribimos el método base para incluir relaciones
    public override Task<Category> GetById(Guid id)
    {
        return _context.Set<Category>()
            .Include(category => category.Flows)
            .FirstOrDefaultAsync(category => category.Id == id);
    }

    // Implementación de método específico definido en la interfaz
    public async Task<Category> GetByIdWithFlowsOrThrow(Guid id)
    {
        Category category = await GetById(id);

        if (category is null)
        {
            throw new ValueNotFoundException($"The {nameof(Category)} (Id: {id}) not found.");
        }

        return category;
    }

    // Método para obtener sin incluir las relaciones
    public Task<Category> GetByIdWithoutFlows(Guid id)
    {
        return _context.Set<Category>()
            .AsNoTracking()
            .FirstOrDefaultAsync(category => category.Id == id);
    }

    // Implementación de operación específica
    public async Task AddFlowToCategory(Guid categoryId, Guid flowId)
    {
        Category category = await _context.Set<Category>()
            .Include(c => c.Flows)
            .FirstOrDefaultAsync(c => c.Id == categoryId);

        if (category is null)
        {
            throw new ValueNotFoundException($"The {nameof(Category)} (Id: {categoryId}) not found.");
        }

        Flow flow = await _context.Set<Flow>()
            .FirstOrDefaultAsync(f => f.Id == flowId);

        if (flow is null)
        {
            throw new ValueNotFoundException($"The {nameof(Flow)} (Id: {flowId}) not found.");
        }

        category.AddFlow(flow);
        _context.Update(category);
        await _context.SaveChangesAsync();
    }

    // Implementación de operación específica
    public async Task RemoveFlowFromCategory(Guid categoryId, Guid flowId)
    {
        Category category = await _context.Set<Category>()
            .Include(c => c.Flows)
            .FirstOrDefaultAsync(c => c.Id == categoryId);

        if (category is null)
        {
            throw new ValueNotFoundException($"The {nameof(Category)} (Id: {categoryId}) not found.");
        }

        Flow flow = await _context.Set<Flow>()
            .FirstOrDefaultAsync(f => f.Id == flowId);

        if (flow is null)
        {
            throw new ValueNotFoundException($"The {nameof(Flow)} (Id: {flowId}) not found.");
        }

        category.RemoveFlow(flow);
        _context.Update(category);
        await _context.SaveChangesAsync();
    }
}