namespace WF.Mimetic.Application.DTO.Nodes.Nodes;

using System;
using System.Collections.Generic;
using WF.Mimetic.Application.DTO.Nodes.Relations;

public class NodeReadDto
{
    public Guid Id { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime EditDate { get; set; }
    public Guid FlowId { get; set; }
    public string Discriminator { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
    public bool IsActived { get; set; }
    public IEnumerable<RelationReadDto> Relations { get; set; }
}
