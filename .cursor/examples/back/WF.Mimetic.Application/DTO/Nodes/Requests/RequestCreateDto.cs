namespace WF.Mimetic.Application.DTO.Nodes.Requests;

using System;

public class RequestCreateDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Route { get; set; }
    public string Body { get; set; }
    public string MediaType { get; set; }
    public string Method { get; set; }
}
