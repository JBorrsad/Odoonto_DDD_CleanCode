namespace WF.Mimetic.UI.Server.Controllers.Categories;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WF.Mimetic.Application.DTO.Categories;
using WF.Mimetic.Application.Interfaces.Categories;



public partial class CategoryController : ControllerBase
{
    
    [HttpPost("{categoryId}/flows/{flowId}", Name = "AddFlowToCategory")]
    public Task AddFlowToCategory(Guid categoryId, Guid flowId)
    {
        return _categoryAppService.AddFlowToCategory(categoryId, new AddFlowToCategoryDto(categoryId, flowId));
    }

    
    [HttpDelete("{categoryId}/flows/{flowId}", Name = "RemoveFlowFromCategory")]
    public Task DeleteFlowFromCategory(Guid categoryId, Guid flowId)
    {
        return _categoryAppService.RemoveFlowFromCategory(categoryId, flowId);
    }
}