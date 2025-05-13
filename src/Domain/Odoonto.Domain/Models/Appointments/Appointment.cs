using System;
using Odoonto.Domain.Core.Abstractions;
using Odoonto.Domain.Core.Models.Exceptions;
using Odoonto.Domain.Models.ValueObjects;

namespace Odoonto.Domain.Models.Appointments
{
    /// <summary>
    /// Entidad que representa una cita odontológica con un paciente y un doctor
    /// </summary>
    public class Appointment : Entity
    {
        // Propiedades con getters públicos y setters privados
        public Guid PatientId { get; private set; }
        public Guid DoctorId { get; private set; }
        public DateTime Date { get; private set; }
        public TimeSlot TimeSlot { get; private set; }
        public AppointmentStatus Status { get; private set; }
        public TreatmentPlan TreatmentPlan { get; private set; }
        public string Notes { get; private set; }

        // Constructor privado - solo accesible desde método factory
        private Appointment(Guid id) : base(id)
        {
            Notes = string.Empty;
            Status = AppointmentStatus.Scheduled; // Por defecto
        }

        // Método factory para crear instancias válidas
        public static Appointment Create(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new InvalidValueException("El identificador de la cita no puede estar vacío.");
            }

            var appointment = new Appointment(id);
            appointment.UpdateEditDate();
            return appointment;
        }

        // Método para establecer la información básica de la cita
        public void SetBasicInfo(Guid patientId, Guid doctorId, DateTime date, TimeSlot timeSlot)
        {
            if (patientId == Guid.Empty)
            {
                throw new InvalidValueException("El identificador del paciente no puede estar vacío.");
            }

            if (doctorId == Guid.Empty)
            {
                throw new InvalidValueException("El identificador del doctor no puede estar vacío.");
            }

            if (date.Date < DateTime.Today)
            {
                throw new InvalidValueException("La fecha de la cita no puede ser en el pasado.");
            }

            if (timeSlot == null)
            {
                throw new InvalidValueException("El horario de la cita no puede ser nulo.");
            }

            PatientId = patientId;
            DoctorId = doctorId;
            Date = date.Date; // Solo mantener la fecha, no la hora
            TimeSlot = timeSlot;
            UpdateEditDate();
        }

        // Método para establecer el plan de tratamiento
        public void SetTreatmentPlan(TreatmentPlan treatmentPlan)
        {
            TreatmentPlan = treatmentPlan ?? throw new InvalidValueException("El plan de tratamiento no puede ser nulo.");
            UpdateEditDate();
        }

        // Método para establecer notas
        public void SetNotes(string notes)
        {
            Notes = notes?.Trim() ?? string.Empty;
            UpdateEditDate();
        }

        // Métodos para cambiar el estado de la cita
        public void MarkAsWaitingRoom()
        {
            EnsureCanChangeStatus();
            Status = AppointmentStatus.WaitingRoom;
            UpdateEditDate();
        }

        public void MarkAsInProgress()
        {
            EnsureStatusIs(AppointmentStatus.WaitingRoom);
            Status = AppointmentStatus.InProgress;
            UpdateEditDate();
        }

        public void MarkAsCompleted()
        {
            EnsureStatusIs(AppointmentStatus.InProgress);
            Status = AppointmentStatus.Completed;
            UpdateEditDate();
        }

        public void Cancel()
        {
            if (Status == AppointmentStatus.Completed)
            {
                throw new InvalidValueException("No se puede cancelar una cita que ya fue completada.");
            }
            
            Status = AppointmentStatus.Cancelled;
            UpdateEditDate();
        }

        // Método privado para validar cambios de estado
        private void EnsureCanChangeStatus()
        {
            if (Status == AppointmentStatus.Cancelled)
            {
                throw new InvalidValueException("No se puede cambiar el estado de una cita cancelada.");
            }
            
            if (Status == AppointmentStatus.Completed)
            {
                throw new InvalidValueException("No se puede cambiar el estado de una cita completada.");
            }
        }

        // Método privado para validar el estado actual
        private void EnsureStatusIs(AppointmentStatus expectedStatus)
        {
            EnsureCanChangeStatus();
            
            if (Status != expectedStatus)
            {
                throw new InvalidValueException($"La cita debe estar en estado '{expectedStatus}' para realizar esta operación. Estado actual: '{Status}'.");
            }
        }
    }
} 