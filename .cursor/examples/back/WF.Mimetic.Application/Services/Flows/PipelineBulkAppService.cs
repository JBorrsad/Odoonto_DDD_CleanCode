namespace WF.Mimetic.Application.Services.Flows;

using global::AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WF.Mimetic.Application.DTO.Flows.Pipelines;
using WF.Mimetic.Application.DTO.Nodes.Gates;
using WF.Mimetic.Application.DTO.Nodes.Nodes;
using WF.Mimetic.Application.DTO.Nodes.Printers;
using WF.Mimetic.Application.DTO.Nodes.Requests;
using WF.Mimetic.Application.DTO.Nodes.Responses;
using WF.Mimetic.Application.DTO.Nodes.Scripters;
using WF.Mimetic.Application.DTO.Nodes.Serializers;
using WF.Mimetic.Application.DTO.Nodes.Switchers;
using WF.Mimetic.Application.DTO.Parameters.Parameters;
using WF.Mimetic.Application.Interfaces.Flows;
using WF.Mimetic.Domain.Core.Models.Exceptions;
using WF.Mimetic.Domain.Core.Repositories;
using WF.Mimetic.Domain.Core.Transactions;
using WF.Mimetic.Domain.Core.Enums;
using WF.Mimetic.Domain.Models.Flows;
using WF.Mimetic.Domain.Models.Nodes;
using WF.Mimetic.Domain.Models.Parameters;
using WF.Mimetic.Domain.Models.Serializers;
using WF.Mimetic.Domain.Repositories.Flows;
using WF.Mimetic.Domain.Repositories.Nodes;
using WF.Mimetic.Application.DTO.Nodes.Watchdogs;
using WF.Mimetic.Domain.Interfaces.ApiDocs;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

public class PipelineBulkAppService : IPipelineBulkAppService
{
    private readonly IDataBaseTransaction _dataBaseTransaction;
    private readonly IPipelineRepository _pipelineRepository;
    private readonly INodeRepository _nodeRepository;
    private readonly IFlowDocBuilder _flowDocBuilder;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PipelineBulkAppService(IDataBaseTransaction dataBaseTransaction
        , IPipelineRepository pipelineRepository
        , INodeRepository nodeRepository
        , IFlowDocBuilder flowDocBuilder
        , IUnitOfWork unitOfWork
        , IMapper mapper)
    {
        _dataBaseTransaction = dataBaseTransaction;
        _pipelineRepository = pipelineRepository;
        _nodeRepository = nodeRepository;
        _flowDocBuilder = flowDocBuilder;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<string> GetOpenApiJsonDoc()
    {
        IEnumerable<Pipeline> pipelines = await _pipelineRepository.GetAllWithNodesAndParameters();
        Domain.Models.ApiDocs.ApiDoc apiDoc = _flowDocBuilder.CreateDoc("WF.Mimetic.DynamicApi", pipelines);

        return JsonConvert.SerializeObject(apiDoc
            , Formatting.Indented
            , new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }
        );
    }

    public async Task<PipelineBulkDto> ExportPipeline(Guid pipelineId)
    {
        Pipeline pipeline = await _pipelineRepository.GetByIdOrThrow(pipelineId);
        return _mapper.Map<Pipeline, PipelineBulkDto>(pipeline);
    }

    public async Task<IEnumerable<PipelineBulkDto>> ExportPipelines()
    {
        IEnumerable<Pipeline> pipelines = await _pipelineRepository.GetAllWithNodesAndParameters();
        return _mapper.Map<IEnumerable<Pipeline>, IEnumerable<PipelineBulkDto>>(pipelines);
    }

    public async Task ImportPipelines(IEnumerable<PipelineBulkDto> pipelines)
    {
        await _dataBaseTransaction.Run(async () =>
        {
            foreach (PipelineBulkDto pipeline in pipelines)
            {
                await ImportPipeline(pipeline.Id, pipeline);
                await _unitOfWork.SaveAsync();
            }
        });
    }

    public async Task ImportPipeline(Guid id, PipelineBulkDto data)
    {
        if (await _pipelineRepository.Any(query => query.Where(pipeline => pipeline.Id == id)))
        {
            throw new DuplicatedValueException($"The pipeline (Id: {id}) is already in database.");
        }

        Pipeline pipeline = await BuildPipeline(id, data); // Si se quiere implementar de una manera correcta habria que utilizar el patron commander
        await _pipelineRepository.Create(pipeline);
    }

    private async Task<Pipeline> BuildPipeline(Guid id, PipelineBulkDto data)
    {
        Pipeline pipeline = Pipeline.Create(id);

        pipeline.SetName(data.Name);
        pipeline.SetIsPublic(data.IsPublic);

        foreach (NodeBulkDto nodeData in data.Nodes)
        {
            await AddNode(pipeline, nodeData);
        }

        return pipeline;
    }

    private async Task AddNode(Pipeline pipeline, NodeBulkDto data)
    {
        if (await _nodeRepository.Any(query => query.Where(node => node.Id == data.Id)))
        {
            throw new DuplicatedValueException($"The node (Id: {data.Id}) is already in database.");
        }

        if (data is GateBulkDto gateData)
        {
            AddGate(pipeline, gateData);
        }
        else if (data is PrinterBulkDto printerData)
        {
            AddPrinter(pipeline, printerData);
        }
        else if (data is RequestBulkDto requestData)
        {
            AddRequest(pipeline, requestData);
        }
        else if (data is ResponseBulkDto responseData)
        {
            AddResponse(pipeline, responseData);
        }
        else if (data is ScripterBulkDto scripterData)
        {
            AddScripter(pipeline, scripterData);
        }
        else if (data is SerializerBulkDto serializerData)
        {
            AddSerializer(pipeline, serializerData);
        }
        else if (data is SwitcherBulkDto switcherData)
        {
            AddSwitcher(pipeline, switcherData);
        }
        else if (data is WatchdogBulkDto watchdogData)
        {
            AddWatchdog(pipeline, watchdogData);
        }
        else
        {
            throw new InvalidValueException($"The node type ({data.Discriminator}) is not valid.");
        }
    }

    private void AddSwitcher(Pipeline pipeline, SwitcherBulkDto data)
    {
        pipeline.AddSwitcher(data.Id);
        Switcher switcher = pipeline.GetNode<Switcher>(data.Id);
        switcher.SetName(data.Name);
        switcher.SetIsActived(data.IsActived);

        foreach (ParameterBulkDto parameterData in data.Parameters)
        {
            AddParameter(switcher, parameterData);
        }
    }

    private void AddSerializer(Pipeline pipeline, SerializerBulkDto data)
    {
        pipeline.AddSerializer(data.Id);
        Serializer serializer = pipeline.GetNode<Serializer>(data.Id);
        SerializationType serializationType = EnumsParser.Parse<SerializationType>(data.SerializationType);
        serializer.SetName(data.Name);
        serializer.SetIsActived(data.IsActived);
        serializer.SetSerializationType(serializationType);
    }

    private void AddScripter(Pipeline pipeline, ScripterBulkDto data)
    {
        pipeline.AddScripter(data.Id);
        Scripter scripter = pipeline.GetNode<Scripter>(data.Id);
        scripter.SetName(data.Name);
        scripter.SetIsActived(data.IsActived);
        scripter.SetScript(data.Script);
    }

    private void AddResponse(Pipeline pipeline, ResponseBulkDto data)
    {
        pipeline.AddResponse(data.Id);
        Response response = pipeline.GetNode<Response>(data.Id);
        response.SetName(data.Name);
        response.SetIsActived(data.IsActived);
        response.SetMediaType(data.MediaType);
        response.SetStatusCode(data.StatusCode);
        response.SetContent(data.Content);
    }

    private void AddWatchdog(Pipeline pipeline, WatchdogBulkDto data)
    {
        pipeline.AddWatchdog(data.Id);
        Watchdog watchdog = pipeline.GetNode<Watchdog>(data.Id);
        watchdog.SetName(data.Name);
        watchdog.SetIsActived(data.IsActived);
        watchdog.SetMediaType(data.MediaType);
        watchdog.SetStatusCode(data.StatusCode);
        watchdog.SetContent(data.Content);
        watchdog.SetScript(data.Script);
    }

    private void AddRequest(Pipeline pipeline, RequestBulkDto data)
    {
        pipeline.AddRequest(data.Id);
        Request request = pipeline.GetNode<Request>(data.Id);
        Method method = EnumsParser.Parse<Method>(data.Method);
        request.SetName(data.Name);
        request.SetIsActived(data.IsActived);
        request.SetBody(data.Body);
        request.SetMediaType(data.MediaType);
        request.SetMethod(method);
        request.SetRoute(data.Route);
    }

    private void AddPrinter(Pipeline pipeline, PrinterBulkDto data)
    {
        pipeline.AddPrinter(data.Id);
        Printer printer = pipeline.GetNode<Printer>(data.Id);
        printer.SetName(data.Name);
        printer.SetIsActived(data.IsActived);
        printer.SetScript(data.Script);
    }

    private void AddGate(Pipeline pipeline, GateBulkDto data)
    {
        pipeline.AddGate(data.Id);
        Gate gate = pipeline.GetNode<Gate>(data.Id);
        Method method = EnumsParser.Parse<Method>(data.Method);
        gate.SetIsActived(data.IsActived);
        gate.SetMethod(method);
        gate.SetName(data.Name);
        gate.SetRoute(data.Route);
    }

    private void AddParameter(Node node, ParameterBulkDto data)
    {
        ParameterType type = EnumsParser.Parse<ParameterType>(data.Type);

        switch (type)
        {
            case ParameterType.Boolean:
                node.AddBoolean(data.Id);
                break;
            case ParameterType.Date:
                node.AddDate(data.Id);
                break;
            case ParameterType.Numeric:
                node.AddNumeric(data.Id);
                break;
            case ParameterType.Text:
                node.AddText(data.Id);
                break;
            default:
                throw new InvalidValueException($"The parameter type ({type}) is not valid.");
        }

        Parameter parameter = node.GetParameter(data.Id);
        parameter.SetTarget(data.Target);
        parameter.SetDefaultValue(data.DefaultValue);
        parameter.SetIsNullable(data.IsNullable);
        parameter.SetName(data.Name);
    }
}
