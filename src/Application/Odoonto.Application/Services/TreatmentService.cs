using AutoMapper;
using Odoonto.Application.DTOs.Treatments;
using Odoonto.Application.Interfaces;
using Odoonto.Domain.Core.Models.Exceptions;
using Odoonto.Domain.Models.Treatments;
using Odoonto.Domain.Models.ValueObjects;
using Odoonto.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Odoonto.Application.Services
{
    /// <summary>
    /// Implementación del servicio de tratamientos
    /// </summary>
    public class TreatmentService : ITreatmentService
    {
        private readonly ITreatmentRepository _treatmentRepository;
        private readonly IMapper _mapper;

        public TreatmentService(ITreatmentRepository treatmentRepository, IMapper mapper)
        {
            _treatmentRepository = treatmentRepository ?? throw new ArgumentNullException(nameof(treatmentRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<TreatmentDto> CreateTreatmentAsync(CreateTreatmentDto treatmentDto)
        {
            // Validar el DTO
            if (treatmentDto == null)
                throw new ArgumentNullException(nameof(treatmentDto));

            // Crear el tratamiento
            var treatment = Treatment.Create(Guid.NewGuid());
            treatment.SetName(treatmentDto.Name);
            
            if (!string.IsNullOrWhiteSpace(treatmentDto.Description))
            {
                treatment.SetDescription(treatmentDto.Description);
            }
            
            treatment.SetPrice(treatmentDto.Price, treatmentDto.Currency);
            treatment.SetEstimatedDurationInMinutes(treatmentDto.DurationMinutes);
            
            if (!string.IsNullOrWhiteSpace(treatmentDto.Category))
            {
                treatment.SetCategory(treatmentDto.Category);
            }

            // Guardar el tratamiento
            await _treatmentRepository.AddAsync(treatment);

            // Mapear y retornar el DTO
            return _mapper.Map<TreatmentDto>(treatment);
        }

        public async Task<bool> DeleteTreatmentAsync(Guid id)
        {
            return await _treatmentRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<TreatmentDto>> GetAllTreatmentsAsync()
        {
            var treatments = await _treatmentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TreatmentDto>>(treatments);
        }

        public async Task<TreatmentDto> GetTreatmentByIdAsync(Guid id)
        {
            var treatment = await _treatmentRepository.GetByIdOrThrowAsync(id);
            return _mapper.Map<TreatmentDto>(treatment);
        }

        public async Task<IEnumerable<TreatmentDto>> GetTreatmentsByCategoryAsync(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                throw new InvalidValueException("La categoría no puede estar vacía.");

            var treatments = await _treatmentRepository.GetByCategoryAsync(category);
            return _mapper.Map<IEnumerable<TreatmentDto>>(treatments);
        }

        public async Task<IEnumerable<TreatmentDto>> GetTreatmentsByMaxDurationAsync(int maxDurationMinutes)
        {
            if (maxDurationMinutes <= 0)
                throw new InvalidValueException("La duración máxima debe ser mayor que cero.");

            var treatments = await _treatmentRepository.GetByMaxDurationAsync(maxDurationMinutes);
            return _mapper.Map<IEnumerable<TreatmentDto>>(treatments);
        }

        public async Task<IEnumerable<TreatmentDto>> GetTreatmentsByMaxPriceAsync(decimal maxPrice, string currency = "EUR")
        {
            if (maxPrice < 0)
                throw new InvalidValueException("El precio máximo no puede ser negativo.");

            if (string.IsNullOrWhiteSpace(currency))
                throw new InvalidValueException("La moneda no puede estar vacía.");

            var treatments = await _treatmentRepository.GetByMaxPriceAsync(maxPrice, currency);
            return _mapper.Map<IEnumerable<TreatmentDto>>(treatments);
        }

        public async Task<IEnumerable<TreatmentDto>> SearchTreatmentsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                throw new InvalidValueException("El término de búsqueda no puede estar vacío.");

            var treatments = await _treatmentRepository.SearchByNameOrDescriptionAsync(searchTerm);
            return _mapper.Map<IEnumerable<TreatmentDto>>(treatments);
        }

        public async Task<TreatmentDto> UpdateTreatmentAsync(Guid id, CreateTreatmentDto treatmentDto)
        {
            // Validar el DTO
            if (treatmentDto == null)
                throw new ArgumentNullException(nameof(treatmentDto));

            // Obtener el tratamiento existente
            var treatment = await _treatmentRepository.GetByIdOrThrowAsync(id);

            // Actualizar los campos
            treatment.SetName(treatmentDto.Name);
            treatment.SetDescription(treatmentDto.Description);
            treatment.SetPrice(treatmentDto.Price, treatmentDto.Currency);
            treatment.SetEstimatedDurationInMinutes(treatmentDto.DurationMinutes);
            treatment.SetCategory(treatmentDto.Category);

            // Guardar los cambios
            await _treatmentRepository.UpdateAsync(treatment);

            // Mapear y retornar el DTO
            return _mapper.Map<TreatmentDto>(treatment);
        }
    }
} 