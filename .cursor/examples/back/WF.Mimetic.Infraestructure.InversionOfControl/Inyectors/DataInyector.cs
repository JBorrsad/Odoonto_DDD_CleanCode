namespace WF.Mimetic.Infraestructure.InversionOfControl.Inyectors;

using Microsoft.Extensions.DependencyInjection;
using WF.Mimetic.Data.Cache.Caches.Routes;
using WF.Mimetic.Data.Cache.Contexts.Routes;
using WF.Mimetic.Data.Core.Repositories;
using WF.Mimetic.Data.Core.Transactions;
using WF.Mimetic.Data.Repositories.Flows;
using WF.Mimetic.Data.Repositories.Nodes;
using WF.Mimetic.Data.Sidecar.Repositories.Partners;
using WF.Mimetic.Domain.Caches.Routes;
using WF.Mimetic.Domain.Core.Repositories;
using WF.Mimetic.Domain.Core.Transactions;
using WF.Mimetic.Domain.Repositories.Flows;
using WF.Mimetic.Domain.Repositories.Nodes;
using WF.Mimetic.Domain.Sidecar.Repositories.Partners;

public static class DataInyector
{
    public static void Inyect(IServiceCollection services)
    {
        services.AddScoped<IDataBaseTransaction, DataBaseTransaction>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<INodeRepository, NodeRepository>();
        services.AddScoped<IGraphRepository, GraphRepository>();
        services.AddScoped<IPipelineRepository, PipelineRepository>();
        services.AddScoped<IFlowRepository, FlowRepository>();
        services.AddScoped<IPartnerRepository, PartnerRepository>();

        services.AddSingleton<IRouteCacheContext, RouteCacheContext>();
        services.AddScoped<IRouteCache, RouteCache>();
    }
}
