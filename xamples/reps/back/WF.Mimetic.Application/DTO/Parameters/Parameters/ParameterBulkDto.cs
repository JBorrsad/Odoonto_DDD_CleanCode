namespace WF.Mimetic.Application.DTO.Parameters.Parameters;

using System;

public class ParameterBulkDto
{
    public Guid Id { get; set; }
    public Guid FatherId { get; set; }
    public string Name { get; set; }
    public bool IsNullable { get; set; }
    public string DefaultValue { get; set; }
    public string Type { get; set; }
    public string Target { get; set; }
}
