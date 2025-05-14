namespace WF.Mimetic.Infraestructure.InversionOfControl.Inyectors;

using Microsoft.ClearScript.V8;
using Microsoft.Extensions.DependencyInjection;
using WF.Mimetic.Domain.Core.Mediators;
using WF.Mimetic.Domain.Interfaces.ApiDocs;
using WF.Mimetic.Domain.Interfaces.Routes;
using WF.Mimetic.Domain.Interfaces.Rules;
using WF.Mimetic.Domain.Models.ApiDocs;
using WF.Mimetic.Domain.Models.Routes;
using WF.Mimetic.Domain.Models.Rules;
using WF.Mimetic.Domain.Models.Rutes;

public static class DomainInyector
{
    public static void Inyect(IServiceCollection services)
    {
        services.AddSingleton<ICacheExpirationHandler, CacheExpirationHandler>();

        services.AddScoped<IMasterCommander, MasterCommander>();
        //services.AddScoped<IRulesEngine, JintEngine>();
        services.AddScoped<IRulesEngine, V8ClearScriptEngine>();
        services.AddScoped<IRouteExplorer, RouteExplorer>();
        services.AddScoped<IFlowDocBuilder, FlowDocBuilder>();
    }
}
