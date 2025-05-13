using AutoMapper;
using Odoonto.Application.DTOs.Treatments;
using Odoonto.Domain.Models.Treatments;
using Odoonto.Domain.Models.ValueObjects;

namespace Odoonto.Application.Mappers
{
    /// <summary>
    /// Perfil de mapeo para entidades relacionadas con tratamientos
    /// </summary>
    public class TreatmentProfile : Profile
    {
        public TreatmentProfile()
        {
            // Mapeo de Treatment a TreatmentDto
            CreateMap<Treatment, TreatmentDto>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Price.Currency))
                .ForMember(dest => dest.DurationMinutes, opt => opt.MapFrom(src => (int)src.EstimatedDuration.TotalMinutes));

            // Mapeo inverso para la creación
            CreateMap<CreateTreatmentDto, Treatment>()
                .ForMember(dest => dest.Price, opt => opt.Ignore())
                .ForMember(dest => dest.EstimatedDuration, opt => opt.Ignore());
        }
    }
} 