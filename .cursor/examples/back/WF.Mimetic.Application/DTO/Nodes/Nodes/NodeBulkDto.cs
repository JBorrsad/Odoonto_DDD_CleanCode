namespace WF.Mimetic.Application.DTO.Nodes.Nodes;

using Newtonsoft.Json;
using System;
using WF.Mimetic.Application.DTO.Nodes.Gates;
using WF.Mimetic.Application.DTO.Nodes.Printers;
using WF.Mimetic.Application.DTO.Nodes.Requests;
using WF.Mimetic.Application.DTO.Nodes.Responses;
using WF.Mimetic.Application.DTO.Nodes.Scripters;
using WF.Mimetic.Application.DTO.Nodes.Serializers;
using WF.Mimetic.Application.DTO.Nodes.Switchers;
using WF.Mimetic.Application.DTO.Nodes.Watchdogs;

[JsonConverter(typeof(JsonInheritanceConverter), "discriminator")]
[JsonInheritance("SwitcherBulkDto", typeof(SwitcherBulkDto))]
[JsonInheritance("SerializerBulkDto", typeof(SerializerBulkDto))]
[JsonInheritance("ScripterBulkDto", typeof(ScripterBulkDto))]
[JsonInheritance("ResponseBulkDto", typeof(ResponseBulkDto))]
[JsonInheritance("RequestBulkDto", typeof(RequestBulkDto))]
[JsonInheritance("PrinterBulkDto", typeof(PrinterBulkDto))]
[JsonInheritance("GateBulkDto", typeof(GateBulkDto))]
[JsonInheritance("WatchdogBulkDto", typeof(WatchdogBulkDto))]
public class NodeBulkDto
{
    public Guid Id { get; set; }
    public Guid FlowId { get; set; }
    public string Discriminator { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
    public bool IsActived { get; set; }
}
