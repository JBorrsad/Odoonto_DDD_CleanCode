namespace WF.Mimetic.Application.Interfaces.Flows;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WF.Mimetic.Application.DTO.Flows.Pipelines;

public interface IPipelineAppService
{
    Task<IEnumerable<PipelineQueryDto>> GetAll();
    Task<PipelineReadDto> GetById(Guid pipelineId);
    Task<IEnumerable<PipelineQueryDto>> GetAllWithRouteAndMethod();
    Task Create(PipelineCreateDto data);
    Task Edit(Guid pipelineId, PipelineEditDto data);
    Task Remove(Guid pipelineId);
}
