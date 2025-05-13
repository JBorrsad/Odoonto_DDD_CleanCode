namespace WF.Mimetic.Application.AutoMapper.Nodes;

using global::AutoMapper;
using WF.Mimetic.Application.DTO.Nodes.Gates;
using WF.Mimetic.Application.DTO.Nodes.Nodes;
using WF.Mimetic.Application.DTO.Nodes.Printers;
using WF.Mimetic.Application.DTO.Nodes.Requests;
using WF.Mimetic.Application.DTO.Nodes.Responses;
using WF.Mimetic.Application.DTO.Nodes.Scripters;
using WF.Mimetic.Application.DTO.Nodes.Serializers;
using WF.Mimetic.Application.DTO.Nodes.Switchers;
using WF.Mimetic.Application.DTO.Nodes.Watchdogs;
using WF.Mimetic.Domain.Models.Nodes;

public class NodeMappingProfile : Profile
{
    public NodeMappingProfile()
    {
        CreateMap<Node, NodeReadDto>()
            .ForMember(dest => dest.Discriminator, opt => opt.MapFrom((org, dest) => dest.GetType().Name))
            .Include<Gate, GateReadDto>()
            .Include<Request, RequestReadDto>()
            .Include<Response, ResponseReadDto>()
            .Include<Printer, PrinterReadDto>()
            .Include<Scripter, ScripterReadDto>()
            .Include<Switcher, SwitcherReadDto>()
            .Include<Serializer, SerializerReadDto>()
            .Include<Watchdog, WatchdogReadDto>();

        CreateMap<Gate, GateReadDto>();
        CreateMap<Request, RequestReadDto>();
        CreateMap<Response, ResponseReadDto>();
        CreateMap<Printer, PrinterReadDto>();
        CreateMap<Scripter, ScripterReadDto>();
        CreateMap<Switcher, SwitcherReadDto>();
        CreateMap<Serializer, SerializerReadDto>();
        CreateMap<Watchdog, WatchdogReadDto>();

        CreateMap<Node, NodeBulkDto>()
            .ForMember(dest => dest.Discriminator, opt => opt.MapFrom((org, dest) => dest.GetType().Name))
            .Include<Gate, GateBulkDto>()
            .Include<Request, RequestBulkDto>()
            .Include<Response, ResponseBulkDto>()
            .Include<Printer, PrinterBulkDto>()
            .Include<Scripter, ScripterBulkDto>()
            .Include<Switcher, SwitcherBulkDto>()
            .Include<Serializer, SerializerBulkDto>()
            .Include<Watchdog, WatchdogBulkDto>()
            .ReverseMap();

        CreateMap<Gate, GateBulkDto>().ReverseMap();
        CreateMap<Request, RequestBulkDto>().ReverseMap();
        CreateMap<Response, ResponseBulkDto>().ReverseMap();
        CreateMap<Printer, PrinterBulkDto>().ReverseMap();
        CreateMap<Scripter, ScripterBulkDto>().ReverseMap();
        CreateMap<Switcher, SwitcherBulkDto>().ReverseMap();
        CreateMap<Serializer, SerializerBulkDto>().ReverseMap();
        CreateMap<Watchdog, WatchdogBulkDto>().ReverseMap();
    }
}
