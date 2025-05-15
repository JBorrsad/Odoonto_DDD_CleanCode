namespace WF.Mimetic.Application.Interfaces.Flows;

using System;
using System.Threading.Tasks;
using WF.Mimetic.Application.DTO.Nodes.Gates;
using WF.Mimetic.Application.DTO.Nodes.Printers;
using WF.Mimetic.Application.DTO.Nodes.Requests;
using WF.Mimetic.Application.DTO.Nodes.Responses;
using WF.Mimetic.Application.DTO.Nodes.Scripters;
using WF.Mimetic.Application.DTO.Nodes.Serializers;
using WF.Mimetic.Application.DTO.Nodes.Switchers;
using WF.Mimetic.Application.DTO.Nodes.Watchdogs;

public interface IFlowAppService
{
    Task AddGate(Guid flowId, GateCreateDto data);
    Task AddRequest(Guid flowId, RequestCreateDto data);
    Task AddResponse(Guid flowId, ResponseCreateDto data);
    Task AddScripter(Guid flowId, ScripterCreateDto data);
    Task AddPrinter(Guid flowId, PrinterCreateDto data);
    Task AddSwitcher(Guid flowId, SwitcherCreateDto data);
    Task AddSerializer(Guid flowId, SerializerCreateDto data);
    Task AddWatchdog(Guid flowId, WatchdogCreateDto data);
    Task EditGate(Guid flowId, Guid gateId, GateEditDto data);
    Task EditRequest(Guid flowId, Guid requestId, RequestEditDto data);
    Task EditResponse(Guid flowId, Guid responseId, ResponseEditDto data);
    Task EditScripter(Guid flowId, Guid scripterId, ScripterEditDto data);
    Task EditPrinter(Guid flowId, Guid printerId, PrinterEditDto data);
    Task EditSwitcher(Guid flowId, Guid switcherId, SwitcherEditDto data);
    Task EditSerializer(Guid flowId, Guid serializerId, SerializerEditDto data);
    Task EditWatchdog(Guid flowId, Guid watchdogId, WatchdogEditDto data);
    Task RemoveNode(Guid flowId, Guid nodeId);
    Task SetNodeOrder(Guid flowId, Guid nodeId, int order);
}
