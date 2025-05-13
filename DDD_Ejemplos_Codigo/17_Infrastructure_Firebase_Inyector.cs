// EJEMPLO DE INYECTOR DE DEPENDENCIAS PARA FIREBASE (Infrastructure Layer)
// Ruta: src/Infraestructure/TuProyecto.Infraestructure.InversionOfControl/Inyectors/FirebaseInyector.cs

namespace TuProyecto.Infraestructure.InversionOfControl.Inyectors;

using Microsoft.Extensions.DependencyInjection;
using TuProyecto.Data.Core.Firebase;
using TuProyecto.Data.Repositories.Categories;
using TuProyecto.Data.Repositories.Flows;
using TuProyecto.Data.Repositories.Nodes;
using TuProyecto.Domain.Repositories.Categories;
using TuProyecto.Domain.Repositories.Flows;
using TuProyecto.Domain.Repositories.Nodes;

/// <summary>
/// Características clave de un inyector de Firebase en DDD:
/// 1. Registra el contexto de Firebase
/// 2. Registra los repositorios que dependen de Firebase
/// 3. Configura el ciclo de vida de los servicios (Singleton para el contexto)
/// 4. Se integra con el resto de inyectores de la aplicación
/// </summary>
public static class FirebaseInyector
{
    public static void Inyect(IServiceCollection services)
    {
        // Registro del contexto Firebase como Singleton para toda la aplicación
        services.AddSingleton<FirebaseDbContext>();

        // Registro de repositorios que dependen de Firebase
        // Las interfaces están en Domain, las implementaciones en Data
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IFlowRepository, FlowRepository>();
        services.AddScoped<INodeRepository, NodeRepository>();
    }
}