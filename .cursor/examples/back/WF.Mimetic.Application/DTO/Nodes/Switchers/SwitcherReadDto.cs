namespace WF.Mimetic.Application.DTO.Nodes.Switchers;

using System.Collections.Generic;
using WF.Mimetic.Application.DTO.Nodes.Nodes;
using WF.Mimetic.Application.DTO.Parameters.Parameters;

public class SwitcherReadDto : NodeReadDto
{
    public IEnumerable<ParameterReadDto> Parameters { get; set; }
}
