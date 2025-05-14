using AutoMapper;
using Odoonto.Application.DTOs.Odontograms;
using Odoonto.Domain.Models.Odontograms;
using System;
using System.Linq;

namespace Odoonto.Application.Mappers
{
    /// <summary>
    /// Perfil de AutoMapper para mapeo entre entidades de odontograma y DTOs
    /// </summary>
    public class OdontogramProfile : Profile
    {
        public OdontogramProfile()
        {
            // Mapeo de Odontogram -> OdontogramDto
            CreateMap<Odontogram, OdontogramDto>()
                .ForMember(dest => dest.ToothRecords, opt => opt.MapFrom(src => src.ToothRecords));

            // Mapeo de ToothRecord -> ToothRecordDto
            CreateMap<ToothRecord, ToothRecordDto>()
                .ForMember(dest => dest.Lesions, opt => opt.MapFrom(src => src.RecordedLesions))
                .ForMember(dest => dest.CompletedProcedures, opt => opt.MapFrom(src => src.CompletedProcedures));

            // Mapeo de LesionRecord -> LesionRecordDto
            CreateMap<LesionRecord, LesionRecordDto>()
                .ForMember(dest => dest.AffectedSurfaces, opt => opt.MapFrom(src => src.AffectedSurfaces.ToList()))
                // Nota: LesionName requeriría una búsqueda adicional, se maneja en el servicio
                .ForMember(dest => dest.LesionName, opt => opt.Ignore());

            // Mapeo de PerformedProcedure -> PerformedProcedureDto
            CreateMap<PerformedProcedure, PerformedProcedureDto>()
                .ForMember(dest => dest.TreatedSurfaces, opt => opt.MapFrom(src => src.TreatedSurfaces.ToList()))
                // Nota: TreatmentName requeriría una búsqueda adicional, se maneja en el servicio
                .ForMember(dest => dest.TreatmentName, opt => opt.Ignore());

            // Mapeo de CreateToothRecordDto -> ToothRecord
            CreateMap<CreateToothRecordDto, ToothRecord>()
                .ConstructUsing(src => new ToothRecord(src.ToothNumber));

            // Mapeo de CreateLesionRecordDto -> LesionRecord
            CreateMap<CreateLesionRecordDto, LesionRecord>()
                .ConstructUsing(src => new LesionRecord(src.LesionId, src.AffectedSurfaces, DateTime.Now));

            // Mapeo de CreatePerformedProcedureDto -> PerformedProcedure
            CreateMap<CreatePerformedProcedureDto, PerformedProcedure>()
                .ConstructUsing(src => new PerformedProcedure(src.TreatmentId, src.TreatedSurfaces, src.CompletionDate));
        }
    }
}