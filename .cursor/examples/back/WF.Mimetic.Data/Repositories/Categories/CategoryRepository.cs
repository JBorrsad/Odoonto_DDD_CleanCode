namespace WF.Mimetic.Data.Repositories.Categories;

using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WF.Mimetic.Data.Core.Contexts;
using WF.Mimetic.Data.Core.Repositories;
using WF.Mimetic.Domain.Core.Models.Exceptions;
using WF.Mimetic.Domain.Models.Categories;
using WF.Mimetic.Domain.Models.Flows;
using WF.Mimetic.Domain.Repositories.Categories;


public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private readonly ApplicationContext _context;

    public CategoryRepository(ApplicationContext context) : base(context)
    {
        _context = context;
    }

    public override Task<Category> GetById(Guid id)
    {
        return _context.Set<Category>()
            .Include(category => category.Flows)
            .FirstOrDefaultAsync(category => category.Id == id);
    }

    //public async Task<Category> GetByIdWithFlowsOrThrow(Guid id)
    //{
    //    Category category = await GetById(id); 

    //    if (category is null)
    //    {
    //        throw new ValueNotFoundException($"The {nameof(Category)} (Id: {id}) not found.");
    //    }

    //    return category;
    //}

    //public Task<Category> GetByIdWithoutFlows(Guid id)
    //{
    //    return _context.Set<Category>()
    //        .AsNoTracking()
    //        .FirstOrDefaultAsync(category => category.Id == id);
    //}

    //public async Task AddFlowToCategory(Guid categoryId, Guid flowId)
    //{
    //    Category category = await _context.Set<Category>()
    //        .Include(c => c.Flows)
    //        .FirstOrDefaultAsync(c => c.Id == categoryId);

    //    if (category is null)
    //    {
    //        throw new ValueNotFoundException($"The {nameof(Category)} (Id: {categoryId}) not found.");
    //    }

    //    Flow flow = await _context.Set<Flow>()
    //        .FirstOrDefaultAsync(f => f.Id == flowId);

    //    if (flow is null)
    //    {
    //        throw new ValueNotFoundException($"The {nameof(Flow)} (Id: {flowId}) not found.");
    //    }

    //    category.AddFlow(flow);
    //    _context.Update(category);
    //    await _context.SaveChangesAsync();
    //}

    //public async Task RemoveFlowFromCategory(Guid categoryId, Guid flowId)
    //{
    //    Category category = await _context.Set<Category>()
    //        .Include(c => c.Flows)
    //        .FirstOrDefaultAsync(c => c.Id == categoryId);

    //    if (category is null)
    //    {
    //        throw new ValueNotFoundException($"The {nameof(Category)} (Id: {categoryId}) not found.");
    //    }

    //    Flow flow = await _context.Set<Flow>()
    //        .FirstOrDefaultAsync(f => f.Id == flowId);

    //    if (flow is null)
    //    {
    //        throw new ValueNotFoundException($"The {nameof(Flow)} (Id: {flowId}) not found.");
    //    }

    //    category.RemoveFlow(flow);
    //    _context.Update(category);
    //    await _context.SaveChangesAsync();
    //}
}

// TODO: REVISAR CATEGORYREPOSITORY
//TODO: CREAR CAPA DE PRESENTACION, ENDPOINTS DE LA API