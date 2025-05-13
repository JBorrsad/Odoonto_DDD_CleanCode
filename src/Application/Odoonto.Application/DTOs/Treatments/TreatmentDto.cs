using System;

namespace Odoonto.Application.DTOs.Treatments
{
    /// <summary>
    /// DTO para transferir datos de un tratamiento odontológico
    /// </summary>
    public class TreatmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public int DurationMinutes { get; set; }
        public string Category { get; set; }
    }
} 