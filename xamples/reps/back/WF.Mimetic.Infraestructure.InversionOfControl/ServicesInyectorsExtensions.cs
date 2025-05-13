namespace WF.Mimetic.Infraestructure.InversionOfControl;

using Microsoft.Extensions.DependencyInjection;
using WF.Mimetic.Infraestructure.InversionOfControl.Inyectors;

public static class ServicesInyectorsExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        ApplicationInyector.Inyect(services);
        DomainInyector.Inyect(services);
        DataInyector.Inyect(services);

        return services;
    }
}
