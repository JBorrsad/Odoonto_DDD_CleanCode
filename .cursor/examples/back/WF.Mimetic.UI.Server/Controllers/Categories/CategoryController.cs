namespace WF.Mimetic.UI.Server.Controllers.Categories;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WF.Mimetic.Application.DTO.Categories;
using WF.Mimetic.Application.DTO.Flows.Pipelines;
using WF.Mimetic.Application.Interfaces.Categories;
using WF.Mimetic.Application.Interfaces.Flows;

[ApiController]
[Route("api/[controller]")]
public partial class CategoryController : ControllerBase
{
    private readonly ICategoryAppService _categoryAppService;
    private readonly IPipelineBulkAppService _pipelineAppService;

    public CategoryController(ICategoryAppService categoryAppService, IPipelineBulkAppService pipelineAppService)
    {
        _categoryAppService = categoryAppService;
        _pipelineAppService = pipelineAppService;
    }

   
    [HttpGet("{id}", Name = "CategoryGetById")]
    public Task<CategoryReadDto> Get(Guid id)
    {
        return _categoryAppService.GetById(id);
    }


    [HttpGet(Name = "CategoryGetAll")]
    public Task<IEnumerable<CategoryQueryDto>> GetAll()
    {
        return _categoryAppService.GetAll();
    }

    [HttpGet("/json", Name = "CategoryGetAll.json")]
    public async Task<IEnumerable<CategoryBulkDto>> GetAllInJson()
    {
        IEnumerable<CategoryQueryDto> categories = await _categoryAppService.GetAll();
        IEnumerable<PipelineBulkDto> pipelines = await _pipelineAppService.ExportPipelines();

        return categories.Select((category) =>
        {
            var cat = new CategoryBulkDto();
            cat.Name = category.Name;

            var filtredPipelines = categories.FirstOrDefault(c => c.Id == category.Id).Pipelines;
            cat.Pipelines = pipelines.Where(pipeline => filtredPipelines.Any(fp => fp.Id == pipeline.Id)).ToList();
            return cat;
        });

    }

  
    [HttpPost(Name = "CategoryPost")]
    public Task CreateCategory(CategoryCreateDto categoryCreateDto)
    {
        return _categoryAppService.Create(categoryCreateDto);
    }

    
    [HttpPut("{categoryId}", Name = "CategoryUpdate")]
    public Task UpdateCategory(Guid categoryId, CategoryUpdateDto categoryUpdateDto)
    {
        return _categoryAppService.Update(categoryId, categoryUpdateDto);
    }

    
    [HttpDelete("{categoryId}", Name = "CategoryRemove")]
    public Task DeleteCategory(Guid categoryId)
    {
        return _categoryAppService.Remove(categoryId);
    }


}