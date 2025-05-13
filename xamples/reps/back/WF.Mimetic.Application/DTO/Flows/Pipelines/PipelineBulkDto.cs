namespace WF.Mimetic.Application.DTO.Flows.Pipelines;

using System;
using System.Collections.Generic;
using WF.Mimetic.Application.DTO.Nodes.Nodes;

public class PipelineBulkDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public bool IsPublic { get; set; }
    public IEnumerable<NodeBulkDto> Nodes { get; set; }
}
