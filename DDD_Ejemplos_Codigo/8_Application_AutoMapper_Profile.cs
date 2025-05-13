// EJEMPLO DE PERFIL DE AUTOMAPPER (Application Layer)
// Ruta: src/Application/TuProyecto.Application/AutoMapper/Categories/CategoryMappingProfile.cs

namespace TuProyecto.Application.AutoMapper.Categories;

using global::AutoMapper;
using System.Linq;
using TuProyecto.Application.DTO.Categories;
using TuProyecto.Domain.Models.Categories;
using TuProyecto.Domain.Models.Flows;

/// <summary>
/// Características clave de un perfil de AutoMapper en DDD:
/// 1. Hereda de Profile de AutoMapper
/// 2. Define mapeos entre entidades y DTOs en ambas direcciones
/// 3. Configura mapeos personalizados para propiedades que requieren transformación
/// 4. Gestiona relaciones entre entidades
/// 5. Se registra en la configuración de IoC
/// </summary>
public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        // Mapeo de entidad a DTO de lectura completa
        CreateMap<Category, CategoryReadDto>()
            .ForMember(
                dest => dest.Flows,
                opt => opt.MapFrom(src => src.Flows))
            .ForMember(
                dest => dest.CreatedDate,
                opt => opt.MapFrom(src => src.CreationDate))
            .ForMember(
                dest => dest.LastEditDate,
                opt => opt.MapFrom(src => src.EditDate));

        // Mapeo de entidad a DTO de consulta (listado)
        CreateMap<Category, CategoryQueryDto>()
            .ForMember(
                dest => dest.FlowCount,
                opt => opt.MapFrom(src => src.Flows.Count()));

        // Mapeo de DTO de creación a entidad (normalmente no se usa directamente)
        CreateMap<CategoryCreateDto, Category>();

        // Mapeo de entidad relacionada a DTO relacionado
        CreateMap<Flow, FlowInCategoryDto>()
            .ForMember(
                dest => dest.NodeCount,
                opt => opt.MapFrom(src => src.Nodes.Count()));
    }
}