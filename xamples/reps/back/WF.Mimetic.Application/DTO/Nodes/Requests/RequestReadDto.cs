namespace WF.Mimetic.Application.DTO.Nodes.Requests;

using WF.Mimetic.Application.DTO.Nodes.Nodes;

public class RequestReadDto : NodeReadDto
{
    public string Route { get; set; }
    public string Body { get; set; }
    public string MediaType { get; set; }
    public string Method { get; set; }
}
