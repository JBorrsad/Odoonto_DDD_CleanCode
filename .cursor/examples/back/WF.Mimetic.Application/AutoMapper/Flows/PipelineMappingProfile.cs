namespace WF.Mimetic.Application.AutoMapper.Flows;

using global::AutoMapper;
using WF.Mimetic.Application.DTO.Flows.Pipelines;
using WF.Mimetic.Domain.Models.Flows;

public class PipelineMappingProfile : Profile
{
    public PipelineMappingProfile()
    {
        CreateMap<Pipeline, PipelineReadDto>();
        CreateMap<Pipeline, PipelineQueryDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.Route, opt => opt.MapFrom(src => src.GetNodeGateRoute()))
            .ForMember(dest => dest.Method, opt => opt.MapFrom(src => src.GetNodeGateMethod().ToString()));
        CreateMap<Pipeline, PipelineBulkDto>().ReverseMap();
    }
}
