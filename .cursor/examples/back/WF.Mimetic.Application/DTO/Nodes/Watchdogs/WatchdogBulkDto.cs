namespace WF.Mimetic.Application.DTO.Nodes.Watchdogs;

using WF.Mimetic.Application.DTO.Nodes.Nodes;

public class WatchdogBulkDto : NodeBulkDto
{
    public string StatusCode { get; set; }
    public string MediaType { get; set; }
    public string Content { get; set; }
    public string Script { get; set; }
}
