using System;
using System.Collections.Generic;
using WF.Mimetic.Application.DTO.Flows.Pipelines;

namespace WF.Mimetic.Application.DTO.Categories;

public class CategoryQueryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<PipelineQueryDto> Pipelines { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime EditDate { get; set; }
}