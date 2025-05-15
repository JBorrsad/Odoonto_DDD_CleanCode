namespace WF.Mimetic.Application.DTO.Nodes.Responses;

using WF.Mimetic.Application.DTO.Nodes.Nodes;

public class ResponseBulkDto : NodeBulkDto
{
    public string StatusCode { get; set; }
    public string MediaType { get; set; }
    public string Content { get; set; }
}
