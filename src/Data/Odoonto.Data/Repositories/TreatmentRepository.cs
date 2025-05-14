using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Odoonto.Data.Contexts;
using Odoonto.Domain.Models.Treatments;
using Odoonto.Domain.Repositories;

namespace Odoonto.Data.Repositories
{
    /// <summary>
    /// Implementación del repositorio de tratamientos
    /// </summary>
    public class TreatmentRepository : ITreatmentRepository
    {
        private readonly OdoontoDbContext _context;

        public TreatmentRepository(OdoontoDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Treatment> GetByIdAsync(Guid id)
        {
            return await _context.Treatments
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Treatment>> GetAllAsync()
        {
            return await _context.Treatments.ToListAsync();
        }

        public async Task<IEnumerable<Treatment>> GetByPatientIdAsync(Guid patientId)
        {
            return await _context.Treatments
                .Where(t => t.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Treatment>> GetByDoctorIdAsync(Guid doctorId)
        {
            return await _context.Treatments
                .Where(t => t.DoctorId == doctorId)
                .ToListAsync();
        }

        public async Task AddAsync(Treatment treatment)
        {
            await _context.Treatments.AddAsync(treatment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Treatment treatment)
        {
            _context.Treatments.Update(treatment);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Treatments.AnyAsync(t => t.Id == id);
        }
    }
}