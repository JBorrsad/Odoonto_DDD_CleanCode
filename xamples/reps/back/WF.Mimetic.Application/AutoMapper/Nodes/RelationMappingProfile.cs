namespace WF.Mimetic.Application.AutoMapper.Nodes;

using global::AutoMapper;
using WF.Mimetic.Application.DTO.Nodes.Relations;
using WF.Mimetic.Domain.Models.Nodes;

public class RelationMappingProfile : Profile
{
    public RelationMappingProfile()
    {
        CreateMap<Relation, RelationReadDto>();
    }
}
