using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Odoonto.Application.DTOs.Lesions;
using Odoonto.Application.Interfaces;
using Odoonto.Domain.Models.Lesions;
using Odoonto.Domain.Repositories;

namespace Odoonto.Application.Services
{
    /// <summary>
    /// Implementación del servicio de gestión de lesiones
    /// </summary>
    public class LesionService : ILesionService
    {
        private readonly ILesionRepository _lesionRepository;
        private readonly IMapper _mapper;

        public LesionService(ILesionRepository lesionRepository, IMapper mapper)
        {
            _lesionRepository = lesionRepository ?? throw new ArgumentNullException(nameof(lesionRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<LesionDto>> GetAllLesionsAsync()
        {
            var lesions = await _lesionRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<LesionDto>>(lesions);
        }

        public async Task<IEnumerable<LesionDto>> GetActiveLesionsAsync()
        {
            var lesions = await _lesionRepository.GetActiveAsync();
            return _mapper.Map<IEnumerable<LesionDto>>(lesions);
        }

        public async Task<IEnumerable<LesionDto>> GetLesionsByCategoryAsync(string category)
        {
            var lesions = await _lesionRepository.GetByCategoryAsync(category);
            return _mapper.Map<IEnumerable<LesionDto>>(lesions);
        }

        public async Task<LesionDto> GetLesionByIdAsync(Guid id)
        {
            var lesion = await _lesionRepository.GetByIdAsync(id);
            return _mapper.Map<LesionDto>(lesion);
        }

        public async Task<LesionDto> CreateLesionAsync(CreateLesionDto createLesionDto)
        {
            // Crear la entidad de dominio
            var lesion = Lesion.Create(Guid.NewGuid());

            // Establecer propiedades
            lesion.SetName(createLesionDto.Name);
            lesion.SetDescription(createLesionDto.Description);
            lesion.SetCategory(createLesionDto.Category);

            // Persistir en el repositorio
            await _lesionRepository.AddAsync(lesion);

            // Mapear y devolver el DTO
            return _mapper.Map<LesionDto>(lesion);
        }

        public async Task<LesionDto> UpdateLesionAsync(UpdateLesionDto updateLesionDto)
        {
            var lesion = await _lesionRepository.GetByIdAsync(updateLesionDto.Id);
            if (lesion == null)
            {
                throw new ApplicationException($"Lesión con ID {updateLesionDto.Id} no encontrada.");
            }

            // Actualizar propiedades
            lesion.SetName(updateLesionDto.Name);
            lesion.SetDescription(updateLesionDto.Description);
            lesion.SetCategory(updateLesionDto.Category);

            // Actualizar estado activo/inactivo
            if (updateLesionDto.IsActive)
            {
                lesion.Activate();
            }
            else
            {
                lesion.Deactivate();
            }

            // Persistir cambios
            await _lesionRepository.UpdateAsync(lesion);

            // Mapear y devolver el DTO
            return _mapper.Map<LesionDto>(lesion);
        }

        public async Task<LesionDto> ActivateLesionAsync(Guid id)
        {
            var lesion = await _lesionRepository.GetByIdAsync(id);
            if (lesion == null)
            {
                throw new ApplicationException($"Lesión con ID {id} no encontrada.");
            }

            // Activar la lesión
            lesion.Activate();

            // Persistir cambios
            await _lesionRepository.UpdateAsync(lesion);

            // Mapear y devolver el DTO
            return _mapper.Map<LesionDto>(lesion);
        }

        public async Task<LesionDto> DeactivateLesionAsync(Guid id)
        {
            var lesion = await _lesionRepository.GetByIdAsync(id);
            if (lesion == null)
            {
                throw new ApplicationException($"Lesión con ID {id} no encontrada.");
            }

            // Desactivar la lesión
            lesion.Deactivate();

            // Persistir cambios
            await _lesionRepository.UpdateAsync(lesion);

            // Mapear y devolver el DTO
            return _mapper.Map<LesionDto>(lesion);
        }

        public async Task<bool> LesionExistsAsync(Guid id)
        {
            return await _lesionRepository.ExistsAsync(id);
        }
    }
}