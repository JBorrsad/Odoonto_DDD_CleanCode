namespace WF.Mimetic.Application.AutoMapper.Categories;

using global::AutoMapper;
using System.Linq;
using WF.Mimetic.Application.DTO.Categories;
using WF.Mimetic.Application.DTO.Flows.Pipelines;
using WF.Mimetic.Domain.Models.Categories;
using WF.Mimetic.Domain.Models.Flows;

public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        // Mapeo detallado para GetById
        CreateMap<Category, CategoryReadDto>()
            .ForMember(dest => dest.Flows, opt => opt
                .MapFrom(src => src.Flows.Where(f => f.Type == FlowType.Pipeline)));

        // Mapeo básico para GetAll
        CreateMap<Category, CategoryQueryDto>()
            .ForMember(dest => dest.Pipelines, opt => opt
                .MapFrom(src => src.Flows.Where(f => f.Type == FlowType.Pipeline)));

        // Mapeos de Pipeline
        CreateMap<Pipeline, PipelineReadDto>();
        CreateMap<Pipeline, PipelineQueryDto>();
    }
}
