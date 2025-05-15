using System.Linq;
using AutoMapper;
using Odoonto.Application.DTOs.Common;
using Odoonto.Application.DTOs.Doctors;
using Odoonto.Domain.Models.Doctors;
using Odoonto.Domain.Models.ValueObjects;

namespace Odoonto.Application.Mappings
{
    /// <summary>
    /// Perfil de mapeo para doctores
    /// </summary>
    public class DoctorMappingProfile : Profile
    {
        public DoctorMappingProfile()
        {
            // Mapeo de Doctor a DoctorDto
            CreateMap<Doctor, DoctorDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => 
                    src.FullName != null ? src.FullName.FullNameWithFirstNameFirst : string.Empty))
                .ForMember(dest => dest.ContactInfo, opt => opt.MapFrom(src => src.ContactInfo))
                .ForMember(dest => dest.Availability, opt => opt.MapFrom(src => 
                    src.Availability != null 
                        ? src.Availability.GetDaysWithAvailability()
                            .SelectMany(day => src.Availability.GetTimeSlots(day)
                                .Select(timeSlot => new AvailabilityDto
                                {
                                    DayOfWeek = day,
                                    StartTime = timeSlot.StartTime,
                                    EndTime = timeSlot.EndTime
                                }))
                        : Enumerable.Empty<AvailabilityDto>()));

            // Mapeo de ContactInfo a ContactInfoDto
            CreateMap<ContactInfo, ContactInfoDto>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
            
            // Mapeo inverso para referencias
            CreateMap<ContactInfoDto, ContactInfo>()
                .ConstructUsing(src => new ContactInfo(src.Address, src.PhoneNumber, src.Email));
        }
    }
} 