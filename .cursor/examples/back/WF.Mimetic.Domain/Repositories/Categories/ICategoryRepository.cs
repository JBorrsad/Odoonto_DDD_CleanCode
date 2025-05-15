namespace WF.Mimetic.Domain.Repositories.Categories;

using System;
using System.Threading.Tasks;
using WF.Mimetic.Domain.Core.Repositories;
using WF.Mimetic.Domain.Models.Categories;
using WF.Mimetic.Domain.Models.Flows;

public interface ICategoryRepository : IRepository<Category>
{
    //Task<Category> GetByIdWithFlowsOrThrow(Guid id);
    //Task<Category> GetByIdWithoutFlows(Guid id);
    //Task AddFlowToCategory(Guid categoryId, Guid flowId);
    //Task RemoveFlowFromCategory(Guid categoryId, Guid flowId);
}