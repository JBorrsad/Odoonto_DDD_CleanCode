namespace WF.Mimetic.Application.DTO.Nodes.Responses;

using System;

public class ResponseCreateDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string StatusCode { get; set; }
    public string MediaType { get; set; }
    public string Content { get; set; }
}
