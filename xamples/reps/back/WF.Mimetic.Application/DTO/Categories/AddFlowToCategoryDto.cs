using System;

namespace WF.Mimetic.Application.DTO.Categories;

public class AddFlowToCategoryDto
{
    public Guid FlowId { get; set; }
    public Guid CategoryId { get; set; }

    public AddFlowToCategoryDto(Guid categoryId, Guid flowId)
    {
        CategoryId = categoryId;
        FlowId = flowId;
    }
}