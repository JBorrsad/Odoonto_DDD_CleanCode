namespace WF.Mimetic.Application.Services.Nodes;

using global::AutoMapper;
using System;
using System.Threading.Tasks;
using WF.Mimetic.Application.DTO.Nodes.Nodes;
using WF.Mimetic.Application.DTO.Parameters.Parameters;
using WF.Mimetic.Application.Interfaces.Nodes;
using WF.Mimetic.Domain.Core.Enums;
using WF.Mimetic.Domain.Models.Nodes;
using WF.Mimetic.Domain.Models.Parameters;
using WF.Mimetic.Domain.Repositories.Nodes;

public class NodeAppService : INodeAppService
{
    private readonly INodeRepository _nodeRepository;
    private readonly IMapper _mapper;

    public NodeAppService(INodeRepository nodeRepository, IMapper mapper)
    {
        _nodeRepository = nodeRepository;
        _mapper = mapper;
    }

    public async Task<NodeReadDto> GetById(Guid id)
    {
        Node node = await _nodeRepository.GetByIdOrThrow(id);
        return _mapper.Map<Node, NodeReadDto>(node);
    }

    public async Task AddParameter(Guid nodeId, ParameterCreateDto data)
    {
        Node node = await _nodeRepository.GetByIdOrThrow(nodeId);
        ParameterType parameterType = EnumsParser.Parse<ParameterType>(data.Type);

        node.AddParameter(data.Id, parameterType);
        Parameter parameter = node.GetParameterOrThrow(data.Id);
        parameter.SetName(data.Name);
        parameter.SetDefaultValue(data.DefaultValue);
        parameter.SetIsNullable(data.IsNullable);
        parameter.SetTarget(data.Target);
        _nodeRepository.Update(node);
    }

    public async Task EditParameter(Guid nodeId, Guid parameterId, ParameterEditDto data)
    {
        Node node = await _nodeRepository.GetByIdOrThrow(nodeId);
        Parameter parameter = node.GetParameterOrThrow(parameterId);

        parameter.SetName(data.Name);
        parameter.SetDefaultValue(data.DefaultValue);
        parameter.SetIsNullable(data.IsNullable);
        parameter.SetTarget(data.Target);
        _nodeRepository.Update(node);
    }

    public async Task RemoveParameter(Guid nodeId, Guid parameterId)
    {
        Node node = await _nodeRepository.GetByIdOrThrow(nodeId);
        Parameter parameter = node.GetParameterOrThrow(parameterId);

        node.Remove(parameter);
        _nodeRepository.Update(node);
    }

    public Task<bool> IsUniqueGateRoute(string id, string route)
    {
        return _nodeRepository.IsUniqueGateRoute(id, route);
    }
}
