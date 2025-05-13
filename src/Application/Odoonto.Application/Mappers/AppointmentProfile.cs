using AutoMapper;
using Odoonto.Application.DTOs.Appointments;
using Odoonto.Domain.Models.Appointments;
using Odoonto.Domain.Models.ValueObjects;
using System.Linq;

namespace Odoonto.Application.Mappers
{
    /// <summary>
    /// Perfil de mapeo para entidades relacionadas con citas
    /// </summary>
    public class AppointmentProfile : Profile
    {
        public AppointmentProfile()
        {
            // Mapeo de Appointment a AppointmentDto
            CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.TimeSlot.StartTime))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.TimeSlot.EndTime))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Procedures, opt => opt.MapFrom(src => src.TreatmentPlan != null ? src.TreatmentPlan.Procedures : null));

            // Mapeo de PlannedProcedure a PlannedProcedureDto
            CreateMap<PlannedProcedure, PlannedProcedureDto>()
                .ForMember(dest => dest.ToothSurfaces, opt => opt.MapFrom(src => src.ToothSurfaces != null ? 
                    src.ToothSurfaces.Select(ts => new ToothSurfaceDto 
                    { 
                        ToothNumber = ts.ToothNumber, 
                        Surfaces = ts.Surfaces.Select(s => s.ToString()).ToList() 
                    }) : null));

            // Mapeo inverso para la creación
            CreateMap<CreateAppointmentDto, Appointment>()
                .ForMember(dest => dest.TimeSlot, opt => opt.MapFrom(src => 
                    new TimeSlot(src.StartTime, src.EndTime)))
                .ForMember(dest => dest.TreatmentPlan, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());

            // Mapeo de PlannedProcedureCreateDto a PlannedProcedure
            CreateMap<PlannedProcedureCreateDto, PlannedProcedure>()
                .ForMember(dest => dest.ToothSurfaces, opt => opt.Ignore());

            // Mapeo de ToothSurfaceCreateDto a ToothSurfaces
            CreateMap<ToothSurfaceCreateDto, ToothSurfaces>()
                .ForMember(dest => dest.Surfaces, opt => opt.Ignore());
        }
    }
} 