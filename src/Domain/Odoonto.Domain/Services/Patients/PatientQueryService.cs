using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Odoonto.Domain.Models.Patients;
using Odoonto.Domain.Repositories;
using Odoonto.Domain.Specifications.Patients;

namespace Odoonto.Domain.Services.Patients
{
    /// <summary>
    /// Implementación del servicio de consultas para pacientes
    /// </summary>
    public class PatientQueryService : IPatientQueryService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientQueryService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        }

        public async Task<IEnumerable<Patient>> SearchByNameAsync(string name, int page = 1, int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new List<Patient>();

            var spec = new PatientByNameSpecification(name);
            return await _patientRepository.FindAsync(spec, page, pageSize);
        }

        public async Task<IEnumerable<Patient>> SearchByAgeRangeAsync(int minAge, int maxAge, int page = 1, int pageSize = 10)
        {
            if (minAge < 0)
                minAge = 0;

            if (maxAge < minAge)
                return new List<Patient>();

            var spec = new PatientByAgeRangeSpecification(minAge, maxAge);
            return await _patientRepository.FindAsync(spec, page, pageSize);
        }

        public async Task<Patient> FindByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var spec = new PatientByEmailSpecification(email);
            return await _patientRepository.FirstOrDefaultAsync(spec);
        }

        public async Task<IEnumerable<Patient>> SearchByPhoneNumberAsync(string phoneNumber, int page = 1, int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return new List<Patient>();

            var spec = new PatientByPhoneSpecification(phoneNumber);
            return await _patientRepository.FindAsync(spec, page, pageSize);
        }

        public async Task<IEnumerable<Patient>> SearchPatientsAsync(string searchTerm, int page = 1, int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<Patient>();

            var spec = new PatientSearchSpecification(searchTerm);
            return await _patientRepository.FindAsync(spec, page, pageSize);
        }

        public async Task<IEnumerable<Patient>> GetPaginatedPatientsAsync(int page = 1, int pageSize = 10)
        {
            return await _patientRepository.GetPaginatedAsync(page, pageSize);
        }

        public async Task<int> GetTotalPatientsCountAsync()
        {
            return await _patientRepository.GetTotalPatientsCountAsync();
        }

        public async Task<int> CountSearchResultsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return 0;

            var spec = new PatientSearchSpecification(searchTerm);
            return await _patientRepository.CountAsync(spec);
        }
    }
}