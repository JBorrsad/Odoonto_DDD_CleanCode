namespace WF.Mimetic.Application.Interfaces.Nodes;

using System;
using System.Threading.Tasks;
using WF.Mimetic.Application.DTO.Nodes.Nodes;
using WF.Mimetic.Application.DTO.Parameters.Parameters;

public interface INodeAppService
{
    Task<NodeReadDto> GetById(Guid id);
    Task AddParameter(Guid nodeId, ParameterCreateDto data);
    Task EditParameter(Guid nodeId, Guid parameterId, ParameterEditDto data);
    Task RemoveParameter(Guid nodeId, Guid parameterId);
    Task<bool> IsUniqueGateRoute(string id, string route);
}
