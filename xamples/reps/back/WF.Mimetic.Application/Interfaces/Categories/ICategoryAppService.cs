namespace WF.Mimetic.Application.Interfaces.Categories;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WF.Mimetic.Application.DTO.Categories;

public interface ICategoryAppService
{
    Task Create(CategoryCreateDto data);
    Task Update(Guid categoryId, CategoryUpdateDto data);
    Task Remove(Guid categoryId);
    Task <CategoryReadDto>GetById(Guid categoryId);
    Task <IEnumerable<CategoryQueryDto>>GetAll();
    Task AddFlowToCategory(Guid categoryId, AddFlowToCategoryDto data);
    Task RemoveFlowFromCategory(Guid categoryId, Guid flowId);
} 