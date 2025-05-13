namespace WF.Mimetic.Application.Services.Flows;

using global::AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WF.Mimetic.Application.DTO.Flows.Pipelines;
using WF.Mimetic.Application.Interfaces.Flows;
using WF.Mimetic.Domain.Core.Models.Exceptions;
using WF.Mimetic.Domain.Models.Flows;
using WF.Mimetic.Domain.Repositories.Flows;

public class PipelineAppService : IPipelineAppService
{
    private readonly IPipelineRepository _pipelineRepository;
    private readonly IMapper _mapper;

    public PipelineAppService(IPipelineRepository pipelineRepository, IMapper mapper)
    {
        _pipelineRepository = pipelineRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PipelineQueryDto>> GetAll()
    {
        IEnumerable<Pipeline> pipelines = await _pipelineRepository.GetAll();

        return _mapper.Map<IEnumerable<Pipeline>, IEnumerable<PipelineQueryDto>>(pipelines);
    }

    public async Task<IEnumerable<PipelineQueryDto>> GetAllWithRouteAndMethod()
    {
        IEnumerable<Pipeline> pipelines = await _pipelineRepository.GetAllWithGateNode();
        IEnumerable<PipelineQueryDto> pipelineQueryDtos = _mapper.Map< IEnumerable<Pipeline>, IEnumerable<PipelineQueryDto>>(pipelines);

        return pipelineQueryDtos;

    }

    public async Task<PipelineReadDto> GetById(Guid pipelineId)
    {
        Pipeline pipeline = await _pipelineRepository.GetByIdOrThrow(pipelineId);
        return _mapper.Map<Pipeline, PipelineReadDto>(pipeline);
    }

    public Task Create(PipelineCreateDto data)
    {
        if (data is null)
        {
            throw new InvalidValueException("The request is not valid.");
        }

        Pipeline pipeline = Pipeline.Create(data.Id);
        pipeline.SetName(data.Name);
        pipeline.RemoveCategory();
        return _pipelineRepository.Create(pipeline);
    }

    public async Task Edit(Guid pipelineId, PipelineEditDto data)
    {
        Pipeline pipeline = await _pipelineRepository.GetByIdOrThrow(pipelineId);
        pipeline.SetName(data.Name);
        pipeline.SetIsPublic(data.IsPublic);
    }

    public async Task Remove(Guid pipelineId)
    {
        Pipeline pipeline = await _pipelineRepository.GetByIdOrThrow(pipelineId);
        _pipelineRepository.Delete(pipeline);
    }
}
