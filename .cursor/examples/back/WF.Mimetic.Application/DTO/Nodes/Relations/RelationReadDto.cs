namespace WF.Mimetic.Application.DTO.Nodes.Relations;

using System;

public class RelationReadDto
{
    public Guid Id { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime EditDate { get; set; }
    public Guid OriginId { get; set; }
    public Guid DestinationId { get; set; }
    public string Script { get; set; }
}
