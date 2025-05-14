namespace WF.Mimetic.Application.DTO.Flows.Pipelines;

using System;
using System.Collections.Generic;
using WF.Mimetic.Application.DTO.Nodes.Nodes;

public class PipelineReadDto
{
    public Guid Id { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime EditDate { get; set; }
    public string Name { get; set; }
    public bool IsPublic { get; set; }
    public string Type { get; set; }
    public IEnumerable<NodeReadDto> Nodes { get; set; }
}
