using AutoMapper;
using Odoonto.Application.DTOs.Doctors;
using Odoonto.Domain.Models.Doctors;
using Odoonto.Domain.Models.ValueObjects;
using System;
using System.Linq;

namespace Odoonto.Application.Mappers
{
    /// <summary>
    /// Perfil de AutoMapper para mapeo entre entidad Doctor y DTOs
    /// </summary>
    public class DoctorProfile : Profile
    {
        public DoctorProfile()
        {
            // Mapeo de Doctor -> DoctorDto
            CreateMap<Doctor, DoctorDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FullName.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.FullName.LastName))
                .ForMember(dest => dest.Specialty, opt => opt.MapFrom(src => src.Specialty))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.ContactInfo.PhoneNumber))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ContactInfo.Email))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => ""))
                .ForMember(dest => dest.Schedule, opt => opt.MapFrom(src => src.Availability != null ?
                    src.Availability.GetSchedule().Select(s => new ScheduleDto
                    {
                        DayOfWeek = s.Key.ToString(),
                        TimeSlots = s.Value.Select(ts => $"{ts.StartTime:hh\\:mm}-{ts.EndTime:hh\\:mm}").ToList()
                    }).ToList() : new System.Collections.Generic.List<ScheduleDto>()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));

            // Mapeo de CreateDoctorDto -> Doctor
            CreateMap<CreateDoctorDto, Doctor>()
                .ConstructUsing((src, ctx) =>
                {
                    var doctor = Doctor.Create(Guid.NewGuid());
                    doctor.SetFullName(src.FirstName, src.LastName);
                    doctor.SetSpecialty(src.Specialty);
                    doctor.SetContactInfo("", src.PhoneNumber, src.Email);

                    // Crear disponibilidad si hay horarios
                    if (src.Schedule != null && src.Schedule.Any())
                    {
                        var availability = new WeeklyAvailability();
                        foreach (var schedule in src.Schedule)
                        {
                            if (Enum.TryParse<DayOfWeek>(schedule.DayOfWeek, out var day))
                            {
                                foreach (var timeSlotStr in schedule.TimeSlots)
                                {
                                    var parts = timeSlotStr.Split('-');
                                    if (parts.Length == 2)
                                    {
                                        if (TimeSpan.TryParse(parts[0], out var start) &&
                                            TimeSpan.TryParse(parts[1], out var end))
                                        {
                                            availability.AddTimeSlot(day, new TimeSlot(start, end));
                                        }
                                    }
                                }
                            }
                        }
                        doctor.SetAvailability(availability);
                    }

                    return doctor;
                });
        }
    }
}