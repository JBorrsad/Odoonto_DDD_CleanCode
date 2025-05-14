namespace WF.Mimetic.UI.Server.Controllers.Nodes;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WF.Mimetic.Application.DTO.Nodes.Nodes;
using WF.Mimetic.Application.DTO.Parameters.Parameters;
using WF.Mimetic.Application.Interfaces.Nodes;

[Route("api/v0/[controller]")]
[ApiController]
public class NodesController : ControllerBase
{
    private readonly INodeAppService _nodeAppService;

    public NodesController(INodeAppService nodeAppService)
    {
        _nodeAppService = nodeAppService;
    }

    [HttpGet("uniqueGateRoute/{id}/{route}", Name = "UniqueGateRoute")]
    public Task<bool> IsUniqueGateRoute(string id, string route)
    {
        return _nodeAppService.IsUniqueGateRoute(id, route.Replace("*", "/"));
    }

    [HttpGet("{id}", Name = "NodesGetById")]
    public Task<NodeReadDto> Get(Guid id)
    {
        return _nodeAppService.GetById(id); 
    }

    [HttpPost("{nodeId}/Parameters", Name = "NodesAddParameter")]
    public Task CreateParameter(Guid nodeId, ParameterCreateDto data)
    {
        return _nodeAppService.AddParameter(nodeId, data);
    }

    [HttpPut("{nodeId}/Parameters/{parameterId}", Name = "NodesEditParameter")]
    public Task EditParameter(Guid nodeId, Guid parameterId, ParameterEditDto data)
    {
        return _nodeAppService.EditParameter(nodeId, parameterId, data);
    }

    [HttpDelete("{nodeId}/Parameters/{parameterId}", Name = "NodesRemoveParameter")]
    public Task DeleteParameter(Guid nodeId, Guid parameterId)
    {
        return _nodeAppService.RemoveParameter(nodeId, parameterId);
    }
}
