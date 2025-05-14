using AutoMapper;
using Odoonto.Application.DTOs.Lesions;
using Odoonto.Domain.Models.Lesions;

namespace Odoonto.Application.Mappers
{
    /// <summary>
    /// Perfil de AutoMapper para la entidad Lesion
    /// </summary>
    public class LesionProfile : Profile
    {
        public LesionProfile()
        {
            // Mapeo de entidad a DTO
            CreateMap<Lesion, LesionDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.LastModified, opt => opt.MapFrom(src => src.LastModified));

            // Mapeo de DTO de creación a entidad
            CreateMap<CreateLesionDto, Lesion>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));

            // Mapeo de DTO de actualización a entidad
            CreateMap<UpdateLesionDto, Lesion>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));
        }
    }
}