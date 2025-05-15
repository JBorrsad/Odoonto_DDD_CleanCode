namespace WF.Mimetic.Application.DTO.Nodes.Requests;

public class RequestEditDto
{
    public string Name { get; set; }
    public string Route { get; set; }
    public string Body { get; set; }
    public string MediaType { get; set; }
    public string Method { get; set; }
    public bool IsActived { get; set; }
}
