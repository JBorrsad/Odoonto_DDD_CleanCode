using AutoMapper;
using Odoonto.Application.DTOs.Odontograms;
using Odoonto.Application.Interfaces;
using Odoonto.Domain.Core.Models.Exceptions;
using Odoonto.Domain.Models.Odontograms;
using Odoonto.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Odoonto.Application.Services
{
    /// <summary>
    /// Implementación del servicio de odontogramas
    /// </summary>
    public class OdontogramService : IOdontogramService
    {
        private readonly IOdontogramRepository _odontogramRepository;
        private readonly ILesionRepository _lesionRepository;
        private readonly ITreatmentRepository _treatmentRepository;
        private readonly IMapper _mapper;

        public OdontogramService(
            IOdontogramRepository odontogramRepository,
            ILesionRepository lesionRepository,
            ITreatmentRepository treatmentRepository,
            IMapper mapper)
        {
            _odontogramRepository = odontogramRepository ?? throw new ArgumentNullException(nameof(odontogramRepository));
            _lesionRepository = lesionRepository ?? throw new ArgumentNullException(nameof(lesionRepository));
            _treatmentRepository = treatmentRepository ?? throw new ArgumentNullException(nameof(treatmentRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<OdontogramDto?> GetByPatientIdAsync(Guid patientId)
        {
            try
            {
                var odontogram = await _odontogramRepository.GetByPatientIdAsync(patientId);
                var odontogramDto = _mapper.Map<OdontogramDto>(odontogram);

                // Completar información adicional para lesiones y tratamientos
                await EnrichOdontogramDtoAsync(odontogramDto);

                return odontogramDto;
            }
            catch (Exception)
            {
                // En caso de error (por ejemplo, si no existe el odontograma), devolvemos null
                return null;
            }
        }

        public async Task<OdontogramDto> CreateOdontogramAsync(Guid patientId)
        {
            try
            {
                // Verificar si ya existe un odontograma para este paciente
                var existingOdontogram = await _odontogramRepository.GetByPatientIdAsync(patientId);

                // Si llegamos aquí, es que existe el odontograma
                throw new InvalidOperationException($"Ya existe un odontograma para el paciente con ID {patientId}");
            }
            catch (Exception ex) when (!(ex is InvalidOperationException))
            {
                // Solo continuamos si la excepción es porque no existe el odontograma
                // Crear nuevo odontograma
                var odontogram = Odontogram.Create(patientId);

                // Guardar en repositorio
                await _odontogramRepository.SaveOdontogramAsync(odontogram);

                // Mapear a DTO
                return _mapper.Map<OdontogramDto>(odontogram);
            }
        }

        public async Task<OdontogramDto> AddToothRecordAsync(Guid odontogramId, CreateToothRecordDto toothRecordDto)
        {
            // Validar datos
            if (toothRecordDto == null)
                throw new ArgumentNullException(nameof(toothRecordDto));

            try
            {
                // Obtener odontograma
                var odontogram = await _odontogramRepository.GetByIdAsync(odontogramId);

                // Convertir DTO a entidad
                var toothRecord = _mapper.Map<ToothRecord>(toothRecordDto);

                // Añadir registro dental al odontograma
                odontogram.AddToothRecord(toothRecord);

                // Actualizar odontograma
                await _odontogramRepository.UpdateAsync(odontogram);

                // Obtener odontograma actualizado
                var updatedOdontogram = await _odontogramRepository.GetByIdAsync(odontogramId);
                var odontogramDto = _mapper.Map<OdontogramDto>(updatedOdontogram);

                // Completar información adicional para lesiones y tratamientos
                await EnrichOdontogramDtoAsync(odontogramDto);

                return odontogramDto;
            }
            catch (Exception ex)
            {
                throw new KeyNotFoundException($"No se encontró el odontograma con ID {odontogramId}", ex);
            }
        }

        public async Task<OdontogramDto> AddLesionRecordAsync(Guid odontogramId, int toothNumber, CreateLesionRecordDto lesionRecordDto)
        {
            // Validar datos
            if (lesionRecordDto == null)
                throw new ArgumentNullException(nameof(lesionRecordDto));

            try
            {
                // Verificar que la lesión existe
                var lesion = await _lesionRepository.GetByIdAsync(lesionRecordDto.LesionId);

                // Crear registro de lesión
                var lesionRecord = _mapper.Map<LesionRecord>(lesionRecordDto);

                // Añadir al odontograma
                await _odontogramRepository.AddLesionRecordAsync(odontogramId, toothNumber, lesionRecord);

                // Obtener odontograma actualizado
                var updatedOdontogram = await _odontogramRepository.GetByIdAsync(odontogramId);
                var odontogramDto = _mapper.Map<OdontogramDto>(updatedOdontogram);

                // Completar información adicional para lesiones y tratamientos
                await EnrichOdontogramDtoAsync(odontogramDto);

                return odontogramDto;
            }
            catch (Exception ex)
            {
                throw new KeyNotFoundException($"No se encontró el odontograma con ID {odontogramId} o la lesión con ID {lesionRecordDto.LesionId}", ex);
            }
        }

        public async Task<OdontogramDto> AddPerformedProcedureAsync(Guid odontogramId, int toothNumber, CreatePerformedProcedureDto procedureDto)
        {
            // Validar datos
            if (procedureDto == null)
                throw new ArgumentNullException(nameof(procedureDto));

            try
            {
                // Verificar que el tratamiento existe
                var treatment = await _treatmentRepository.GetByIdAsync(procedureDto.TreatmentId);

                // Crear registro de procedimiento
                var performedProcedure = _mapper.Map<PerformedProcedure>(procedureDto);

                // Añadir al odontograma
                await _odontogramRepository.AddPerformedProcedureAsync(odontogramId, toothNumber, performedProcedure);

                // Obtener odontograma actualizado
                var updatedOdontogram = await _odontogramRepository.GetByIdAsync(odontogramId);
                var odontogramDto = _mapper.Map<OdontogramDto>(updatedOdontogram);

                // Completar información adicional para lesiones y tratamientos
                await EnrichOdontogramDtoAsync(odontogramDto);

                return odontogramDto;
            }
            catch (Exception ex)
            {
                throw new KeyNotFoundException($"No se encontró el odontograma con ID {odontogramId} o el tratamiento con ID {procedureDto.TreatmentId}", ex);
            }
        }

        /// <summary>
        /// Completa la información adicional en el DTO de odontograma
        /// (nombres de lesiones y tratamientos)
        /// </summary>
        private async Task EnrichOdontogramDtoAsync(OdontogramDto odontogramDto)
        {
            if (odontogramDto == null || odontogramDto.ToothRecords == null)
                return;

            foreach (var toothRecord in odontogramDto.ToothRecords)
            {
                // Completar nombres de lesiones
                if (toothRecord.Lesions != null)
                {
                    foreach (var lesion in toothRecord.Lesions)
                    {
                        try
                        {
                            var lesionEntity = await _lesionRepository.GetByIdAsync(lesion.LesionId);
                            lesion.LesionName = lesionEntity.Name;
                        }
                        catch
                        {
                            // Si no se encuentra la lesión, continuamos con el siguiente
                            lesion.LesionName = "Lesión desconocida";
                        }
                    }
                }

                // Completar nombres de tratamientos
                if (toothRecord.CompletedProcedures != null)
                {
                    foreach (var procedure in toothRecord.CompletedProcedures)
                    {
                        try
                        {
                            var treatmentEntity = await _treatmentRepository.GetByIdAsync(procedure.TreatmentId);
                            procedure.TreatmentName = treatmentEntity.Name;
                        }
                        catch
                        {
                            // Si no se encuentra el tratamiento, continuamos con el siguiente
                            procedure.TreatmentName = "Tratamiento desconocido";
                        }
                    }
                }
            }
        }
    }
}