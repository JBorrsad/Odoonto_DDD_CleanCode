using AutoMapper;
using Odoonto.Application.DTOs.Doctors;
using Odoonto.Domain.Models.Doctors;
using Odoonto.Domain.Models.ValueObjects;
using System;
using System.Linq;

namespace Odoonto.Application.Mappings
{
    /// <summary>
    /// Perfil de mapeo para entidades relacionadas con doctores
    /// </summary>
    public class DoctorMappingProfile : Profile
    {
        public DoctorMappingProfile()
        {
            // Mapeo de Doctor a DoctorDto
            CreateMap<Doctor, DoctorDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FullName.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.FullName.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ContactInfo.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.ContactInfo.PhoneNumber))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.Schedule, opt => opt.MapFrom(src => 
                    src.Availability != null 
                        ? MapAvailabilityToScheduleDtos(src.Availability) 
                        : new System.Collections.Generic.List<ScheduleDto>()));

            // Mapeo de TimeRange a ScheduleDto
            CreateMap<(DayOfWeek DayOfWeek, TimeRange TimeRange), ScheduleDto>()
                .ForMember(dest => dest.DayOfWeek, opt => opt.MapFrom(src => (int)src.DayOfWeek))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.TimeRange.StartTime))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.TimeRange.EndTime))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
        }

        private System.Collections.Generic.List<ScheduleDto> MapAvailabilityToScheduleDtos(WeeklyAvailability availability)
        {
            var scheduleDtos = new System.Collections.Generic.List<ScheduleDto>();

            if (availability != null)
            {
                foreach (var day in availability.Availability)
                {
                    foreach (var timeRange in day.Value)
                    {
                        var scheduleDto = new ScheduleDto
                        {
                            DayOfWeek = (int)day.Key,
                            StartTime = timeRange.StartTime,
                            EndTime = timeRange.EndTime,
                            IsActive = true
                        };
                        scheduleDtos.Add(scheduleDto);
                    }
                }
            }

            return scheduleDtos;
        }
    }
} 