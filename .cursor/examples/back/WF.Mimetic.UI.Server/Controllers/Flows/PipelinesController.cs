namespace WF.Mimetic.UI.Server.Controllers.Flows;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WF.Mimetic.Application.DTO.Flows.Pipelines;
using WF.Mimetic.Application.DTO.Nodes.Gates;
using WF.Mimetic.Application.DTO.Nodes.Printers;
using WF.Mimetic.Application.DTO.Nodes.Requests;
using WF.Mimetic.Application.DTO.Nodes.Responses;
using WF.Mimetic.Application.DTO.Nodes.Scripters;
using WF.Mimetic.Application.DTO.Nodes.Serializers;
using WF.Mimetic.Application.DTO.Nodes.Switchers;
using WF.Mimetic.Application.DTO.Nodes.Watchdogs;
using WF.Mimetic.Application.Interfaces.Flows;

[Route("api/v0/[controller]")]
[ApiController]
public class PipelinesController : ControllerBase
{
    private readonly IPipelineBulkAppService _pipelineBulkAppService;
    private readonly IPipelineAppService _pipelineAppService;
    private readonly IFlowAppService _flowAppService;

    public PipelinesController(IPipelineBulkAppService pipelineBulkAppService, IPipelineAppService pipelineAppService, IFlowAppService flowAppService)
    {
        _pipelineBulkAppService = pipelineBulkAppService;
        _pipelineAppService = pipelineAppService;
        _flowAppService = flowAppService;
    }

    [HttpGet(Name = "PipelinesGetAll")]
    public Task<IEnumerable<PipelineQueryDto>> GetAll()
    {
        return _pipelineAppService.GetAll();
    }

    [HttpGet("PipelineGetAllWithRouteAndMethod")]
    public Task<IEnumerable<PipelineQueryDto>> GetAllWithRouteAndMethod()
    {
        return _pipelineAppService.GetAllWithRouteAndMethod();
    }

    [HttpGet("pipelines.json", Name = "PipelinesGetAllJsons")]
    public Task<IEnumerable<PipelineBulkDto>> GetAllJsons()
    {
        return _pipelineBulkAppService.ExportPipelines();
    }

    [HttpGet("openapi.json", Name = "PipelinesGetAllOpenApiJson")]
    public async Task<ContentResult> GetAllOpenApi()
    {
        string content = await _pipelineBulkAppService.GetOpenApiJsonDoc();

        return new ContentResult()
        {
            Content = content,
            ContentType = "application/json",
            StatusCode = StatusCodes.Status200OK
        };
    }

    [HttpGet("{id}", Name = "PipelinesGetById")]
    public Task<PipelineReadDto> GetById(Guid id)
    {
        return _pipelineAppService.GetById(id);
    }

    [HttpGet("{id}/pipeline.json", Name = "PipelinesGetJson")]
    public Task<PipelineBulkDto> GetJson(Guid id)
    {
        return _pipelineBulkAppService.ExportPipeline(id);
    }

    [HttpPost(Name = "PipelinesCreate")]
    public Task Create(PipelineCreateDto data)
    {
        return _pipelineAppService.Create(data);
    }

    [HttpPut("pipelines.json", Name = "PipelinesImportPipelines")]
    public Task ImportPipelines(IEnumerable<PipelineBulkDto> data)
    {
        return _pipelineBulkAppService.ImportPipelines(data);
    }

    [HttpPut("{id}/pipeline.json", Name = "PipelinesImportPipeline")]
    public Task ImportPipeline(Guid id, PipelineBulkDto data)
    {
        return _pipelineBulkAppService.ImportPipeline(id, data);
    }

    [HttpPut("{pipelineId}", Name = "PipelinesEdit")]
    public Task Edit(Guid pipelineId, [FromBody] PipelineEditDto data)
    {
        return _pipelineAppService.Edit(pipelineId, data);
    }

    [HttpPost("{id}/Gates", Name = "PipelinesAddGate")]
    public Task AddGate(Guid id, [FromBody] GateCreateDto data)
    {
        return _flowAppService.AddGate(id, data);
    }

    [HttpPost("{id}/Requests", Name = "PipelinesAddRequest")]
    public Task AddRequest(Guid id, [FromBody] RequestCreateDto data)
    {
        return _flowAppService.AddRequest(id, data);
    }

    [HttpPost("{id}/Responses", Name = "PipelinesAddResponse")]
    public Task AddResponse(Guid id, [FromBody] ResponseCreateDto data)
    {
        return _flowAppService.AddResponse(id, data);
    }

    [HttpPost("{id}/Watchdogs", Name = "PipelinesAddWatchdog")]
    public Task AddResponse(Guid id, [FromBody] WatchdogCreateDto data)
    {
        return _flowAppService.AddWatchdog(id, data);
    }

    [HttpPost("{id}/Printers", Name = "PipelinesAddPrinter")]
    public Task AddPrinter(Guid id, [FromBody] PrinterCreateDto data)
    {
        return _flowAppService.AddPrinter(id, data);
    }

    [HttpPost("{id}/Scripters", Name = "PipelinesAddScripter")]
    public Task AddScripter(Guid id, [FromBody] ScripterCreateDto data)
    {
        return _flowAppService.AddScripter(id, data);
    }

    [HttpPost("{id}/Switchers", Name = "PipelinesAddSwitcher")]
    public Task AddSwitcher(Guid id, [FromBody] SwitcherCreateDto data)
    {
        return _flowAppService.AddSwitcher(id, data);
    }

    [HttpPost("{id}/Serializers", Name = "PipelinesAddSerializer")]
    public Task AddSerializer(Guid id, [FromBody] SerializerCreateDto data)
    {
        return _flowAppService.AddSerializer(id, data);
    }

    [HttpPut("{pipelineId}/Gates/{gateId}", Name = "PipelinesEditGate")]
    public Task EditGate(Guid pipelineId, Guid gateId, [FromBody] GateEditDto data)
    {
        return _flowAppService.EditGate(pipelineId, gateId, data);
    }

    [HttpPut("{pipelineId}/Requests/{requestId}", Name = "PipelinesEditRequest")]
    public Task EditRequest(Guid pipelineId, Guid requestId, [FromBody] RequestEditDto data)
    {
        return _flowAppService.EditRequest(pipelineId, requestId, data);
    }

    [HttpPut("{pipelineId}/Responses/{responseId}", Name = "PipelinesEditResponse")]
    public Task EditResponse(Guid pipelineId, Guid responseId, [FromBody] ResponseEditDto data)
    {
        return _flowAppService.EditResponse(pipelineId, responseId, data);
    }

    [HttpPut("{pipelineId}/Watchdogs/{watchdogId}", Name = "PipelinesEditWatchdog")]
    public Task EditResponse(Guid pipelineId, Guid watchdogId, [FromBody] WatchdogEditDto data)
    {
        return _flowAppService.EditWatchdog(pipelineId, watchdogId, data);
    }

    [HttpPut("{pipelineId}/Printers/{printerId}", Name = "PipelinesEditPrinter")]
    public Task EditPrinter(Guid pipelineId, Guid printerId, [FromBody] PrinterEditDto data)
    {
        return _flowAppService.EditPrinter(pipelineId, printerId, data);
    }

    [HttpPut("{pipelineId}/Scripters/{scripterId}", Name = "PipelinesEditScripter")]
    public Task EditScripter(Guid pipelineId, Guid scripterId, [FromBody] ScripterEditDto data)
    {
        return _flowAppService.EditScripter(pipelineId, scripterId, data);
    }

    [HttpPut("{pipelineId}/Switchers/{switcherId}", Name = "PipelinesEditSwitcher")]
    public Task EditSwitcher(Guid pipelineId, Guid switcherId, [FromBody] SwitcherEditDto data)
    {
        return _flowAppService.EditSwitcher(pipelineId, switcherId, data);
    }

    [HttpPut("{pipelineId}/Serializers/{serializerId}", Name = "PipelinesEditSerializer")]
    public Task EditSerializer(Guid pipelineId, Guid serializerId, [FromBody] SerializerEditDto data)
    {
        return _flowAppService.EditSerializer(pipelineId, serializerId, data);
    }

    [HttpPut("{pipelineId}/Nodes/{nodeId}/Order/{order}", Name = "PipelinesSetNodeOrder")]
    public Task SetNodeOrder(Guid pipelineId, Guid nodeId, int order)
    {
        return _flowAppService.SetNodeOrder(pipelineId, nodeId, order);
    }

    [HttpDelete("{id}", Name = "PipelinesRemove")]
    public Task Delete(Guid id)
    {
        return _pipelineAppService.Remove(id);
    }

    [HttpDelete("{pipelineId}/Nodes/{nodeId}", Name = "PipelinesRemoveNode")]
    public Task DeleteNode(Guid pipelineId, Guid nodeId)
    {
        return _flowAppService.RemoveNode(pipelineId, nodeId);
    }
}
