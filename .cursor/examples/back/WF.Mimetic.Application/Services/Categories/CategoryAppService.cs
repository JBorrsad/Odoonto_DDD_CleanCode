namespace WF.Mimetic.Application.Services.Categories;

using global::AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WF.Mimetic.Application.DTO.Categories;
using WF.Mimetic.Application.DTO.Flows.Pipelines;
using WF.Mimetic.Application.Interfaces.Categories;
using WF.Mimetic.Application.Interfaces.Flows;
using WF.Mimetic.Domain.Core.Models.Exceptions;
using WF.Mimetic.Domain.Models.Flows;
using WF.Mimetic.Domain.Repositories.Categories;
using WF.Mimetic.Domain.Models.Categories;
using WF.Mimetic.Domain.Repositories.Flows;

public class CategoryAppService : ICategoryAppService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IFlowRepository _flowRepository;  
    private readonly IMapper _mapper;

    public CategoryAppService(ICategoryRepository categoryRepository, IFlowRepository flowRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _flowRepository = flowRepository;
        _mapper = mapper;
    }
    public async Task<CategoryReadDto> GetById(Guid CategoryId)
    {
        Category category = await _categoryRepository.GetByIdOrThrow(CategoryId, query => 
            query.Include(c => c.Flows)
                 .ThenInclude(f => f.Nodes));

        return _mapper.Map<Category, CategoryReadDto>(category);
    }

    public async Task<IEnumerable<CategoryQueryDto>> GetAll()
    {
        IEnumerable<Category> categories = await _categoryRepository.GetAll(query =>
            query.Include(c => c.Flows));

        return _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryQueryDto>>(categories);
    }

    public Task Create(CategoryCreateDto data)
    {
        if (data is null)
        {
            throw new InvalidValueException("The request is not valid.");
        }
        Category category = Category.Create(data.Id);
        category.SetName(data.Name);
        //la lista de flows no la inicializamos en el service
        //porque en el constructor ya se inicializa como una lista vacia
        return _categoryRepository.Create(category);
    }

    public async Task Remove(Guid categoryId)
    {
        Category category = await _categoryRepository.GetByIdOrThrow(categoryId);
        _categoryRepository.Delete(category);
    }

    public async Task Update(Guid CategoryId, CategoryUpdateDto data)
    {
        if (data is null)
        {
            throw new InvalidValueException("The request is not valid.");
        }

        Category category = await _categoryRepository.GetByIdOrThrow(CategoryId);
        category.SetName(data.Name);

        _categoryRepository.Update(category);
    }

    public async Task AddFlowToCategory(Guid categoryId, AddFlowToCategoryDto data)
    {
        if (data is null)
        {
            throw new InvalidValueException("The request is not valid.");
        }

        Category category = await _categoryRepository.GetByIdOrThrow(categoryId, query => 
            query.Include(c => c.Flows));
        Flow flow = await _flowRepository.GetByIdOrThrow(data.FlowId);
        category.AddFlow(flow);
        _categoryRepository.Update(category);
    }

    public async Task RemoveFlowFromCategory(Guid categoryId, Guid flowId)
    {
        Category category = await _categoryRepository.GetByIdOrThrow(categoryId, query => 
            query.Include(c => c.Flows));
        Flow flow = await _flowRepository.GetByIdOrThrow(flowId);
        category.RemoveFlow(flow);
        _categoryRepository.Update(category);
    }

    //public async Task<IEnumerable<CategoryBulkDto>> GetFlows(Guid CategoryId) => await _categoryRepository.GetFlows(CategoryId);

} 