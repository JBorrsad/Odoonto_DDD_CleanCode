using System;
using System.Collections.Generic;
using WF.Mimetic.Application.DTO.Flows.Pipelines;

namespace WF.Mimetic.Application.DTO.Categories;

public class CategoryBulkDto
{
    public string Name { get; set; }
    public IEnumerable<PipelineBulkDto> Pipelines { get; set; }
}
