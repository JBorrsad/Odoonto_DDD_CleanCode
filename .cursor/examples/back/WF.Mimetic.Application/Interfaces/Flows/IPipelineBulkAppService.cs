namespace WF.Mimetic.Application.Interfaces.Flows;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WF.Mimetic.Application.DTO.Flows.Pipelines;

public interface IPipelineBulkAppService
{
    Task<string> GetOpenApiJsonDoc();
    Task<PipelineBulkDto> ExportPipeline(Guid pipelineId);
    Task<IEnumerable<PipelineBulkDto>> ExportPipelines();
    Task ImportPipelines(IEnumerable<PipelineBulkDto> pipelines);
    Task ImportPipeline(Guid id, PipelineBulkDto data);
}
