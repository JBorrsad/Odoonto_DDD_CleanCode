namespace WF.Mimetic.Application.DTO.Parameters.Parameters;

public class ParameterEditDto
{
    public string Name { get; set; }
    public bool IsNullable { get; set; }
    public string DefaultValue { get; set; }
    public string Target { get; set; }
}
