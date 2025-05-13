namespace WF.Mimetic.Application.DTO.Nodes.Gates;

using WF.Mimetic.Application.DTO.Nodes.Nodes;

public class GateReadDto : NodeReadDto
{
    public string Route { get; set; }
    public string Method { get; set; }
}
