namespace WF.Mimetic.Infraestructure.InversionOfControl.Inyectors;

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using WF.Mimetic.Application.AutoMapper.Flows;
using WF.Mimetic.Application.AutoMapper.Nodes;
using WF.Mimetic.Application.AutoMapper.Parameters;
using WF.Mimetic.Application.Interfaces.Flows;
using WF.Mimetic.Application.Interfaces.Listeners;
using WF.Mimetic.Application.Interfaces.Nodes;
using WF.Mimetic.Application.Interfaces.Rules;
using WF.Mimetic.Application.Services.Flows;
using WF.Mimetic.Application.Services.Listeners;
using WF.Mimetic.Application.Services.Nodes;
using WF.Mimetic.Application.Services.Rules;
using WF.Mimetic.Application.Sidecar.AutoMapper.Partners;
using WF.Mimetic.Application.Sidecar.Interfaces.Partners;
using WF.Mimetic.Application.Sidecar.Services.Partners;
using WF.Mimetic.Application.Interfaces.Categories;
using WF.Mimetic.Application.Services.Categories;
using WF.Mimetic.Domain.Repositories.Categories;
using WF.Mimetic.Data.Repositories.Categories;
using WF.Mimetic.Application.AutoMapper.Categories;

public static class ApplicationInyector
{
    public static void Inyect(IServiceCollection services)
    {
        services.AddScoped<ICategoryRepository, CategoryRepository>(); 
        services.AddScoped<ICategoryAppService, CategoryAppService>();
        services.AddScoped<IEngineAppService, EngineAppService>();
        services.AddScoped<INodeAppService, NodeAppService>();
        services.AddScoped<IListenerAppService, ListenerAppService>();
        services.AddScoped<IPipelineAppService, PipelineAppService>();
        services.AddScoped<IPipelineBulkAppService, PipelineBulkAppService>();
        services.AddScoped<IFlowAppService, FlowAppService>();
        services.AddScoped<IPartnerAppService, PartnerAppService>();
        InyectProfiles(services);
    }

    private static void InyectProfiles(IServiceCollection services)
    {
        List<Type> profiles = new List<Type>();
        profiles.Add(typeof(CategoryMappingProfile));
        profiles.Add(typeof(ParameterMappingProfile));
        profiles.Add(typeof(FlowResultMappingProfile));
        profiles.Add(typeof(PipelineMappingProfile));
        profiles.Add(typeof(NodeMappingProfile));
        profiles.Add(typeof(PartnerMappingProfile));

        services.AddAutoMapper(profiles.ToArray());
    }
}
