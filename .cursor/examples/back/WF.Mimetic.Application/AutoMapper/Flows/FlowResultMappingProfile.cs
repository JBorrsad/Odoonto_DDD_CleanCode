namespace WF.Mimetic.Application.AutoMapper.Flows;

using global::AutoMapper;
using WF.Mimetic.Application.DTO.Flows.FlowResults;
using WF.Mimetic.Domain.Models.Flows;

public class FlowResultMappingProfile : Profile
{
    public FlowResultMappingProfile()
    {
        CreateMap<FlowResult, FlowResultReadDto>();
    }
}
