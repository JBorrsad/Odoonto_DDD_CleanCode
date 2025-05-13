namespace Odoonto.Domain.Models.ValueObjects
{
    /// <summary>
    /// Enum que representa las superficies dentales
    /// O: Oclusal (superficie de masticación)
    /// M: Mesial (superficie hacia la línea media)
    /// D: Distal (superficie alejada de la línea media)
    /// P: Palatina/Lingual (superficie hacia el paladar/lengua)
    /// V: Vestibular/Bucal (superficie hacia el exterior/mejillas)
    /// </summary>
    public enum ToothSurface
    {
        Occlusal, // O
        Mesial,   // M
        Distal,   // D
        Palatine, // P (Palatina/Lingual)
        Vestibular // V (Bucal)
    }
} 