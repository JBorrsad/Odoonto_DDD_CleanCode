using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Odoonto.Data.Contexts;
using Odoonto.Domain.Models.Lesions;
using Odoonto.Domain.Repositories;

namespace Odoonto.Data.Repositories
{
    /// <summary>
    /// Implementación del repositorio de lesiones
    /// </summary>
    public class LesionRepository : ILesionRepository
    {
        private readonly OdoontoDbContext _context;

        public LesionRepository(OdoontoDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Lesion> GetByIdAsync(Guid id)
        {
            return await _context.Lesions
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<Lesion>> GetAllAsync()
        {
            return await _context.Lesions.ToListAsync();
        }

        public async Task<IEnumerable<Lesion>> GetActiveAsync()
        {
            return await _context.Lesions
                .Where(l => l.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Lesion>> GetByCategoryAsync(string category)
        {
            return await _context.Lesions
                .Where(l => l.Category == category)
                .ToListAsync();
        }

        public async Task AddAsync(Lesion lesion)
        {
            await _context.Lesions.AddAsync(lesion);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Lesion lesion)
        {
            _context.Lesions.Update(lesion);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Lesions.AnyAsync(l => l.Id == id);
        }
    }
}