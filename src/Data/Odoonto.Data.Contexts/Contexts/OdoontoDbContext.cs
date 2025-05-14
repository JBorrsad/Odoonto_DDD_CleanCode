using Microsoft.EntityFrameworkCore;
using Odoonto.Domain.Models.Appointments;
using Odoonto.Domain.Models.Doctors;
using Odoonto.Domain.Models.Lesions;
using Odoonto.Domain.Models.Patients;
using Odoonto.Domain.Models.Treatments;

namespace Odoonto.Data.Contexts
{
    /// <summary>
    /// Contexto de base de datos para la aplicación Odoonto
    /// </summary>
    public class OdoontoDbContext : DbContext
    {
        public OdoontoDbContext(DbContextOptions<OdoontoDbContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<Lesion> Lesions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplicar todas las configuraciones de entidades
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OdoontoDbContext).Assembly);

            // Configuraciones específicas que no están en archivos separados
            ConfigurePatientEntity(modelBuilder);
            ConfigureDoctorEntity(modelBuilder);
            ConfigureAppointmentEntity(modelBuilder);
            ConfigureTreatmentEntity(modelBuilder);
            ConfigureLesionEntity(modelBuilder);
        }

        private void ConfigurePatientEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("Patients");
                entity.HasKey(e => e.Id);

                // Configurar valores complejos
                entity.OwnsOne(p => p.Name);
                entity.OwnsOne(p => p.DateOfBirth);
                entity.OwnsOne(p => p.Contact);

                // Configurar propiedades de colección
                entity.Property(p => p.Allergies)
                    .HasConversion(
                        v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
            });
        }

        private void ConfigureDoctorEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.ToTable("Doctors");
                entity.HasKey(e => e.Id);

                // Configurar valores complejos
                entity.OwnsOne(d => d.Name);
                entity.OwnsOne(d => d.Contact);

                // Configurar propiedades de colección
                entity.Property(d => d.Specialties)
                    .HasConversion(
                        v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
            });
        }

        private void ConfigureAppointmentEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.ToTable("Appointments");
                entity.HasKey(e => e.Id);

                // Configurar relaciones
                entity.HasOne<Patient>()
                    .WithMany()
                    .HasForeignKey(a => a.PatientId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Doctor>()
                    .WithMany()
                    .HasForeignKey(a => a.DoctorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureTreatmentEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Treatment>(entity =>
            {
                entity.ToTable("Treatments");
                entity.HasKey(e => e.Id);

                // Configurar relaciones
                entity.HasOne<Patient>()
                    .WithMany()
                    .HasForeignKey(t => t.PatientId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Doctor>()
                    .WithMany()
                    .HasForeignKey(t => t.DoctorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureLesionEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lesion>(entity =>
            {
                entity.ToTable("Lesions");
                entity.HasKey(e => e.Id);
            });
        }
    }
}