using AutoMapper;
using Odoonto.Application.DTOs.Patients;
using Odoonto.Domain.Models.Patients;
using Odoonto.Domain.Models.ValueObjects;
using System;

namespace Odoonto.Application.Mappers
{
    /// <summary>
    /// Perfil de AutoMapper para mapeo entre entidad Patient y DTOs
    /// </summary>
    public class PatientProfile : Profile
    {
        public PatientProfile()
        {
            // Mapeo de Patient -> PatientDto
            CreateMap<Patient, PatientDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Name.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Name.LastName))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.Value))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.CalculateAge()))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Contact.Address))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Contact.PhoneNumber))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Contact.Email));

            // Mapeo de CreatePatientDto -> Patient
            CreateMap<CreatePatientDto, Patient>()
                .ConstructUsing((src, ctx) => new Patient(
                    new FullName(src.FirstName, src.LastName),
                    new Date(src.DateOfBirth),
                    (Gender)src.Gender,
                    new ContactInfo(src.Address, src.PhoneNumber, src.Email)))
                .ForMember(dest => dest.MedicalHistory, opt => opt.MapFrom(src => src.MedicalHistory))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ForMember(dest => dest.Allergies, opt => opt.MapFrom(src => src.Allergies))
                .AfterMap((src, dest) => {
                    // Limpiar y añadir alergias
                    if (src.Allergies != null)
                    {
                        foreach (var allergy in src.Allergies)
                        {
                            if (!string.IsNullOrWhiteSpace(allergy))
                            {
                                dest.AddAllergy(allergy);
                            }
                        }
                    }
                });
        }
    }
} 