using System.Collections.Concurrent;
using Odoonto.Models;

namespace Odoonto.Services
{
    public interface IDataService<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(Guid id, T entity);
        Task<bool> DeleteAsync(Guid id);
    }

    public class InMemoryDataService<T> : IDataService<T> where T : class
    {
        private readonly ConcurrentDictionary<Guid, T> _items = new();
        private readonly Func<T, Guid> _idSelector;

        public InMemoryDataService(Func<T, Guid> idSelector)
        {
            _idSelector = idSelector ?? throw new ArgumentNullException(nameof(idSelector));
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<T>>(_items.Values);
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            if (_items.TryGetValue(id, out var item))
            {
                return Task.FromResult(item);
            }

            return Task.FromResult<T>(null);
        }

        public Task<T> CreateAsync(T entity)
        {
            var id = _idSelector(entity);
            if (_items.TryAdd(id, entity))
            {
                return Task.FromResult(entity);
            }

            throw new InvalidOperationException($"Entity with id {id} already exists.");
        }

        public Task<T> UpdateAsync(Guid id, T entity)
        {
            if (_items.TryGetValue(id, out _))
            {
                _items[id] = entity;
                return Task.FromResult(entity);
            }

            throw new KeyNotFoundException($"Entity with id {id} not found.");
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            var result = _items.TryRemove(id, out _);
            return Task.FromResult(result);
        }
    }

    public class DoctorService : InMemoryDataService<Doctor>
    {
        public DoctorService() : base(d => d.Id)
        {
            // Add some initial data
            var doctor1 = new Doctor { Id = Guid.NewGuid(), Name = "Dr. Smith", Specialty = "Odontología General", Email = "smith@example.com", Phone = "123-456-7890" };
            var doctor2 = new Doctor { Id = Guid.NewGuid(), Name = "Dr. Johnson", Specialty = "Cirugía Bucal", Email = "johnson@example.com", Phone = "098-765-4321" };
            
            CreateAsync(doctor1).Wait();
            CreateAsync(doctor2).Wait();
        }
    }

    public class PatientService : InMemoryDataService<Patient>
    {
        public PatientService() : base(p => p.Id)
        {
            // Add some initial data
            var patient1 = new Patient { Id = Guid.NewGuid(), Name = "Juan Pérez", Email = "juan@example.com", Phone = "555-123-4567", DateOfBirth = new DateTime(1985, 5, 15), Address = "Calle Principal 123" };
            var patient2 = new Patient { Id = Guid.NewGuid(), Name = "María García", Email = "maria@example.com", Phone = "555-987-6543", DateOfBirth = new DateTime(1990, 8, 22), Address = "Avenida Central 456" };
            
            CreateAsync(patient1).Wait();
            CreateAsync(patient2).Wait();
        }
    }

    public class AppointmentService : InMemoryDataService<Appointment>
    {
        public AppointmentService() : base(a => a.Id)
        {
            // Add some initial data
            var appointment1 = new Appointment
            {
                Id = Guid.NewGuid(),
                DoctorId = Guid.NewGuid(), // Would be replaced with actual doctor ID in real app
                PatientId = Guid.NewGuid(), // Would be replaced with actual patient ID in real app
                AppointmentDate = DateTime.Today.AddDays(2),
                StartTime = new TimeSpan(10, 0, 0),
                EndTime = new TimeSpan(10, 30, 0),
                Notes = "Revisión general",
                Status = AppointmentStatus.Scheduled
            };
            
            CreateAsync(appointment1).Wait();
        }
    }

    public class TreatmentService : InMemoryDataService<Treatment>
    {
        public TreatmentService() : base(t => t.Id)
        {
            // Add some initial data
            var treatment1 = new Treatment { Id = Guid.NewGuid(), Name = "Limpieza Dental", Description = "Limpieza dental básica", Price = 50.00m, DurationInMinutes = 30 };
            var treatment2 = new Treatment { Id = Guid.NewGuid(), Name = "Empaste", Description = "Empaste de resina compuesta", Price = 80.00m, DurationInMinutes = 45 };
            var treatment3 = new Treatment { Id = Guid.NewGuid(), Name = "Extracción", Description = "Extracción dental simple", Price = 100.00m, DurationInMinutes = 60 };
            
            CreateAsync(treatment1).Wait();
            CreateAsync(treatment2).Wait();
            CreateAsync(treatment3).Wait();
        }
    }
} 