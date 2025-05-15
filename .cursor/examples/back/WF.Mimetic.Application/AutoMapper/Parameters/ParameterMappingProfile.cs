namespace WF.Mimetic.Application.AutoMapper.Parameters;

using global::AutoMapper;
using WF.Mimetic.Application.DTO.Parameters.Parameters;
using WF.Mimetic.Domain.Models.Parameters;

public class ParameterMappingProfile : Profile
{
    public ParameterMappingProfile()
    {
        CreateMap<Parameter, ParameterReadDto>();
        CreateMap<Parameter, ParameterBulkDto>().ReverseMap();
    }
}
