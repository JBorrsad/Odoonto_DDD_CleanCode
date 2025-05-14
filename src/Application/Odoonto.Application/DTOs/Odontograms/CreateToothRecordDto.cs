using System.ComponentModel.DataAnnotations;

namespace Odoonto.Application.DTOs.Odontograms
{
    /// <summary>
    /// DTO para crear un registro dental
    /// </summary>
    public class CreateToothRecordDto
    {
        /// <summary>
        /// Número del diente (1-32 para adultos, 51-85 para niños)
        /// </summary>
        [Required(ErrorMessage = "El número de diente es obligatorio")]
        [Range(1, 85, ErrorMessage = "El número de diente debe estar entre 1-32 (adultos) o 51-85 (niños)")]
        public int ToothNumber { get; set; }
    }
}