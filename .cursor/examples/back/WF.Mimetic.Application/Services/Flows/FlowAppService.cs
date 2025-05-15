namespace WF.Mimetic.Application.Services.Flows;

using System;
using System.Threading.Tasks;
using WF.Mimetic.Application.DTO.Nodes.Gates;
using WF.Mimetic.Application.DTO.Nodes.Printers;
using WF.Mimetic.Application.DTO.Nodes.Requests;
using WF.Mimetic.Application.DTO.Nodes.Responses;
using WF.Mimetic.Application.DTO.Nodes.Scripters;
using WF.Mimetic.Application.DTO.Nodes.Serializers;
using WF.Mimetic.Application.DTO.Nodes.Switchers;
using WF.Mimetic.Application.Interfaces.Flows;
using WF.Mimetic.Domain.Core.Models.Exceptions;
using WF.Mimetic.Domain.Core.Enums;
using WF.Mimetic.Domain.Interfaces.Routes;
using WF.Mimetic.Domain.Models.Flows;
using WF.Mimetic.Domain.Models.Nodes;
using WF.Mimetic.Domain.Models.Serializers;
using WF.Mimetic.Domain.Repositories.Flows;
using WF.Mimetic.Application.DTO.Nodes.Watchdogs;

public class FlowAppService : IFlowAppService
{
    private readonly IFlowRepository _flowRepository;
    private readonly IRouteExplorer _routesExplorer;

    public FlowAppService(IFlowRepository flowRepository, IRouteExplorer routesExplorer)
    {
        _flowRepository = flowRepository;
        _routesExplorer = routesExplorer;
    }

    public async Task AddGate(Guid flowId, GateCreateDto data)
    {
        if (data is null)
        {
            throw new InvalidValueException("The request is not valid.");
        }

        Method method = EnumsParser.Parse<Method>(data.Method);
        Flow flow = await _flowRepository.GetByIdOrThrow(flowId);

        flow.AddGate(data.Id);
        Gate gate = flow.GetNodeOrThorw<Gate>(data.Id);
        gate.SetName(data.Name);
        gate.SetRoute(data.Route);
        gate.SetMethod(method);
        await _routesExplorer.AddGate(gate);
        _flowRepository.Update(flow);
    }

    public async Task AddRequest(Guid flowId, RequestCreateDto data)
    {
        if (data is null)
        {
            throw new InvalidValueException("The request is not valid.");
        }

        Method method = EnumsParser.Parse<Method>(data.Method);
        Flow flow = await _flowRepository.GetByIdOrThrow(flowId);

        flow.AddRequest(data.Id);
        Request request = flow.GetNodeOrThorw<Request>(data.Id);
        request.SetName(data.Name);
        request.SetRoute(data.Route);
        request.SetBody(data.Body);
        request.SetMediaType(data.MediaType);
        request.SetMethod(method);
        _flowRepository.Update(flow);
    }

    public async Task AddResponse(Guid flowId, ResponseCreateDto data)
    {
        if (data is null)
        {
            throw new InvalidValueException("The request is not valid.");
        }

        Flow flow = await _flowRepository.GetByIdOrThrow(flowId);

        flow.AddResponse(data.Id);
        Response response = flow.GetNodeOrThorw<Response>(data.Id);
        response.SetName(data.Name);
        response.SetStatusCode(data.StatusCode);
        response.SetMediaType(data.MediaType);
        response.SetContent(data.Content);
        _flowRepository.Update(flow);
    }

    public async Task AddWatchdog(Guid flowId, WatchdogCreateDto data)
    {
        if (data is null)
        {
            throw new InvalidValueException("The request is not valid.");
        }

        Flow flow = await _flowRepository.GetByIdOrThrow(flowId);

        flow.AddWatchdog(data.Id);
        Watchdog watchdog = flow.GetNodeOrThorw<Watchdog>(data.Id);
        watchdog.SetName(data.Name);
        watchdog.SetStatusCode(data.StatusCode);
        watchdog.SetMediaType(data.MediaType);
        watchdog.SetContent(data.Content);
        watchdog.SetScript(data.Script);
        _flowRepository.Update(flow);
    }

    public async Task AddPrinter(Guid flowId, PrinterCreateDto data)
    {
        if (data is null)
        {
            throw new InvalidValueException("The request is not valid.");
        }

        Flow flow = await _flowRepository.GetByIdOrThrow(flowId);

        flow.AddPrinter(data.Id);
        Printer printer = flow.GetNodeOrThorw<Printer>(data.Id);
        printer.SetName(data.Name);
        printer.SetScript(data.Script);
        _flowRepository.Update(flow);
    }

    public async Task AddSwitcher(Guid flowId, SwitcherCreateDto data)
    {
        if (data is null)
        {
            throw new InvalidValueException("The request is not valid.");
        }

        Flow flow = await _flowRepository.GetByIdOrThrow(flowId);

        flow.AddSwitcher(data.Id);
        Switcher switcher = flow.GetNodeOrThorw<Switcher>(data.Id);
        switcher.SetName(data.Name);
        _flowRepository.Update(flow);
    }

    public async Task AddScripter(Guid flowId, ScripterCreateDto data)
    {
        if (data is null)
        {
            throw new InvalidValueException("The request is not valid.");
        }

        Flow flow = await _flowRepository.GetByIdOrThrow(flowId);

        flow.AddScripter(data.Id);
        Scripter scripter = flow.GetNodeOrThorw<Scripter>(data.Id);
        scripter.SetName(data.Name);
        scripter.SetScript(data.Script);
        _flowRepository.Update(flow);
    }

    public async Task AddSerializer(Guid flowId, SerializerCreateDto data)
    {
        if (data is null)
        {
            throw new InvalidValueException("The request is not valid.");
        }

        SerializationType serializationType = EnumsParser.Parse<SerializationType>(data.SerializationType);
        Flow flow = await _flowRepository.GetByIdOrThrow(flowId);

        flow.AddSerializer(data.Id);
        Serializer serializer = flow.GetNodeOrThorw<Serializer>(data.Id);
        serializer.SetName(data.Name);
        serializer.SetSerializationType(serializationType);
        _flowRepository.Update(flow);
    }

    public async Task RemoveNode(Guid flowId, Guid nodeId)
    {
        Flow flow = await _flowRepository.GetByIdOrThrow(flowId);
        Node node = flow.GetNode(nodeId);

        if (node is Gate gate)
        {
            await _routesExplorer.RemoveGate(gate);
        }

        flow.RemoveNode(node);
    }

    public async Task EditGate(Guid flowId, Guid gateId, GateEditDto data)
    {
        if (data is null)
        {
            throw new InvalidValueException("The request is not valid.");
        }

        Method method = EnumsParser.Parse<Method>(data.Method);
        Flow flow = await _flowRepository.GetByIdOrThrow(flowId);

        Gate gate = flow.GetNodeOrThorw<Gate>(gateId);
        gate.SetName(data.Name);
        gate.SetRoute(data.Route);
        gate.SetMethod(method);
        gate.SetIsActived(data.IsActived);
        await _routesExplorer.UpdateGate(gate);
        _flowRepository.Update(flow);
    }

    public async Task EditRequest(Guid flowId, Guid requestId, RequestEditDto data)
    {
        if (data is null)
        {
            throw new InvalidValueException("The request is not valid.");
        }

        Method method = EnumsParser.Parse<Method>(data.Method);
        Flow flow = await _flowRepository.GetByIdOrThrow(flowId);
        Request request = flow.GetNodeOrThorw<Request>(requestId);
        request.SetName(data.Name);
        request.SetRoute(data.Route);
        request.SetBody(data.Body);
        request.SetMediaType(data.MediaType);
        request.SetMethod(method);
        request.SetIsActived(data.IsActived);
        _flowRepository.Update(flow);
    }

    public async Task EditResponse(Guid flowId, Guid responseId, ResponseEditDto data)
    {
        if (data is null)
        {
            throw new InvalidValueException("The request is not valid.");
        }

        Flow flow = await _flowRepository.GetByIdOrThrow(flowId);
        Response response = flow.GetNodeOrThorw<Response>(responseId);
        response.SetName(data.Name);
        response.SetStatusCode(data.StatusCode);
        response.SetMediaType(data.MediaType);
        response.SetContent(data.Content);
        response.SetIsActived(data.IsActived);
        _flowRepository.Update(flow);
    }

    public async Task EditWatchdog(Guid flowId, Guid watchdogId, WatchdogEditDto data)
    {
        if (data is null)
        {
            throw new InvalidValueException("The request is not valid.");
        }

        Flow flow = await _flowRepository.GetByIdOrThrow(flowId);
        Watchdog watchdog = flow.GetNodeOrThorw<Watchdog>(watchdogId);
        watchdog.SetName(data.Name);
        watchdog.SetStatusCode(data.StatusCode);
        watchdog.SetMediaType(data.MediaType);
        watchdog.SetContent(data.Content);
        watchdog.SetIsActived(data.IsActived);
        watchdog.SetScript(data.Script);
        _flowRepository.Update(flow);
    }

    public async Task EditScripter(Guid flowId, Guid scripterId, ScripterEditDto data)
    {
        if (data is null)
        {
            throw new InvalidValueException("The request is not valid.");
        }

        Flow flow = await _flowRepository.GetByIdOrThrow(flowId);
        Scripter scripter = flow.GetNodeOrThorw<Scripter>(scripterId);
        scripter.SetName(data.Name);
        scripter.SetScript(data.Script);
        scripter.SetIsActived(data.IsActived);
        _flowRepository.Update(flow);
    }

    public async Task EditPrinter(Guid flowId, Guid printerId, PrinterEditDto data)
    {
        if (data is null)
        {
            throw new InvalidValueException("The request is not valid.");
        }

        Flow flow = await _flowRepository.GetByIdOrThrow(flowId);
        Printer printer = flow.GetNodeOrThorw<Printer>(printerId);
        printer.SetName(data.Name);
        printer.SetScript(data.Script);
        printer.SetIsActived(data.IsActived);
        _flowRepository.Update(flow);
    }

    public async Task EditSwitcher(Guid flowId, Guid switcherId, SwitcherEditDto data)
    {
        if (data is null)
        {
            throw new InvalidValueException("The request is not valid.");
        }

        Flow flow = await _flowRepository.GetByIdOrThrow(flowId);
        Switcher switcher = flow.GetNodeOrThorw<Switcher>(switcherId);
        switcher.SetName(data.Name);
        switcher.SetIsActived(data.IsActived);
        _flowRepository.Update(flow);
    }

    public async Task EditSerializer(Guid flowId, Guid serializerId, SerializerEditDto data)
    {
        if (data is null)
        {
            throw new InvalidValueException("The request is not valid.");
        }

        SerializationType serializationType = EnumsParser.Parse<SerializationType>(data.SerializationType);
        Flow flow = await _flowRepository.GetByIdOrThrow(flowId);
        Serializer serializer = flow.GetNodeOrThorw<Serializer>(serializerId);
        serializer.SetName(data.Name);
        serializer.SetSerializationType(serializationType);
        serializer.SetIsActived(data.IsActived);
        _flowRepository.Update(flow);
    }

    public async Task SetNodeOrder(Guid flowId, Guid nodeId, int order)
    {
        Flow flow = await _flowRepository.GetByIdOrThrow(flowId);
        Node node = flow.GetNodeOrThorw(nodeId);

        flow.MoveNode(node, order);
        _flowRepository.Update(flow);
    }
}
