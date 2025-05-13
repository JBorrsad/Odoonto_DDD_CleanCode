// EJEMPLO DE MAPEO PARA ENTITY FRAMEWORK (Data Layer)
// Ruta: src/Data/TuProyecto.Data/Mappings/Categories/CategoryMapping.cs

namespace TuProyecto.Data.Mappings.Categories;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TuProyecto.Data.Core.Mappings;
using TuProyecto.Domain.Models.Categories;

/// <summary>
/// Características clave de una clase de mapeo EF Core en DDD:
/// 1. Hereda de una clase base EntityMapping<T>
/// 2. Define la configuración de la tabla en la base de datos
/// 3. Configura las propiedades de la entidad (requerido, longitud, etc.)
/// 4. Configura las relaciones entre entidades
/// 5. Mapea campos privados a propiedades de navegación
/// </summary>
public class CategoryMapping : EntityMapping<Category>
{
    // Define el nombre de la tabla en la base de datos
    protected override string TableName => "TuProyecto_Categories";

    // Configura el mapeo de la entidad
    protected override void ConfigureMapping(EntityTypeBuilder<Category> builder)
    {
        // Configuración de propiedades
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);

        // Mapeo de campo privado a propiedad de navegación
        builder.Navigation(category => category.Flows)
            .HasField("_flows")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        // Configuración de relaciones (ejemplo)
        // builder.HasMany(c => c.Flows)
        //     .WithOne()
        //     .HasForeignKey("CategoryId")
        //     .OnDelete(DeleteBehavior.SetNull);
    }
}