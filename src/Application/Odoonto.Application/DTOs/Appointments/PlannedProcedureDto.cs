using System;
using System.Collections.Generic;

namespace Odoonto.Application.DTOs.Appointments
{
    /// <summary>
    /// DTO para representar un procedimiento planificado en una cita
    /// </summary>
    public class PlannedProcedureDto
    {
        public Guid TreatmentId { get; set; }
        public string TreatmentName { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public List<ToothSurfaceDto> ToothSurfaces { get; set; }
    }

    /// <summary>
    /// DTO para representar una superficie dental afectada
    /// </summary>
    public class ToothSurfaceDto
    {
        public int ToothNumber { get; set; }
        public List<string> Surfaces { get; set; }
    }
} 