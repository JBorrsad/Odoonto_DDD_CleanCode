namespace WF.Mimetic.Application.DTO.Nodes.Gates;

using System;

public class GateCreateDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Route { get; set; }
    public string Method { get; set; }
}
