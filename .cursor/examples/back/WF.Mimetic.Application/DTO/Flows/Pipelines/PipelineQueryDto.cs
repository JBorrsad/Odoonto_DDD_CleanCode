namespace WF.Mimetic.Application.DTO.Flows.Pipelines;

using System;

public class PipelineQueryDto
{
    public Guid Id { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime EditDate { get; set; }
    public string Name { get; set; }
    public bool IsPublic { get; set; }
    public string Type { get; set; }
    public string Route { get; set; }
    public string Method { get; set; }
}
