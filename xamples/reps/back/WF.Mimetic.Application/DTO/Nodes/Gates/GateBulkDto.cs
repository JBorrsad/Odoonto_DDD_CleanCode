namespace WF.Mimetic.Application.DTO.Nodes.Gates;

using WF.Mimetic.Application.DTO.Nodes.Nodes;

public class GateBulkDto : NodeBulkDto
{
    public string Route { get; set; }
    public string Method { get; set; }
}
