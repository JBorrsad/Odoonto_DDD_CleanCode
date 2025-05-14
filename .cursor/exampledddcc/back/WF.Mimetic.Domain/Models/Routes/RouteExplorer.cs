namespace WF.Mimetic.Domain.Models.Rutes;

using Microsoft.AspNetCore.Routing.Template;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WF.Mimetic.Domain.Caches.Routes;
using WF.Mimetic.Domain.Core.Models.Exceptions;
using WF.Mimetic.Domain.Interfaces.Routes;
using WF.Mimetic.Domain.Models.Nodes;
using WF.Mimetic.Domain.Models.Routes;
using WF.Mimetic.Domain.Repositories.Nodes;

public class RouteExplorer : IRouteExplorer
{
    private readonly ICacheExpirationHandler _cacheExpirationHandler;
    private readonly INodeRepository _nodeRepository;
    private readonly IRouteCache _routesCache;

    public RouteExplorer(INodeRepository nodeRepository, IRouteCache routesCache, ICacheExpirationHandler cacheExpirationHandler)
    {
        _cacheExpirationHandler = cacheExpirationHandler;
        _nodeRepository = nodeRepository;
        _routesCache = routesCache;
    }

    public async Task<Gate> MatchOrThrow(string uri, Method method)
    {
        Gate gate = await Match(uri, method);

        if (gate is null)
        {
            throw new ValueNotFoundException("Route not found.");
        }

        return gate;
    }

    public async Task<Gate> Match(string uri, Method method)
    {
        Route route = await MatchRoute(uri, method);

        if (route is null)
        {
            return null;
        }

        return await _nodeRepository.GetById<Gate>(route.Id);
    }

    public async Task AddGate(Gate gate)
    {
        if (await IsRouteInUse(gate.Route, gate.Method))
        {
            throw new DuplicatedValueException("The route is already in use.");
        }

        Route route = new Route(gate.Id, gate.Method, gate.Route);
        await _routesCache.Create(route);
        await _nodeRepository.Create(gate);
    }

    public async Task UpdateGate(Gate gate)
    {
        if (!await IsRouteValidToUpdate(gate))
        {
            throw new DuplicatedValueException("The route is already in use.");
        }

        Route route = await _routesCache.GetByIdOrThrow(gate.Id);
        route.SetMethod(gate.Method);
        route.SetUri(gate.Route);
        await _routesCache.Update(route);
    }

    private async Task<bool> IsRouteValidToUpdate(Gate gate)
    {
        Route route = await MatchRoute(gate.Route, gate.Method);

        if (route is null)
        {
            return true;
        }

        return route.Id == gate.Id;
    }

    public async Task RemoveGate(Gate gate)
    {
        Route route = await _routesCache.GetById(gate.Id);

        if (route is not null)
        {
            await _routesCache.Delete(route);
        }

        _nodeRepository.Delete(gate);
    }

    private async Task<bool> IsRouteInUse(string uri, Method method)
    {
        Route route = await MatchRoute(uri, method);
        return route is not null;
    }

    private async Task<Route> MatchRoute(string uri, Method method)
    {
        if (!uri.StartsWith('/'))
        {
            uri = "/" + uri;
        }

        await LoadRoutes();

        IEnumerable<Route> routes = await _routesCache.GetAll((route) => route.Method == method);

        foreach (Route route in routes)
        {
            string convertedTemplate = ConvertTemplateFormat(route.TemplateText);
            RouteTemplate routeTemplate = TemplateParser.Parse(convertedTemplate);
            TemplateMatcher templateMatcher = new TemplateMatcher(routeTemplate, null);
            Microsoft.AspNetCore.Routing.RouteValueDictionary values = new Microsoft.AspNetCore.Routing.RouteValueDictionary();

            if (templateMatcher.TryMatch(uri, values))
            {
                return route;
            }
        }

        return null;
    }

    public string ConvertTemplateFormat(string template)
    {
        return Regex.Replace(template, "{{(.*?)}}", "{$1}");
    }

    private async Task LoadRoutes()
    {
        if (!_cacheExpirationHandler.IsExpired)
        {
            return;
        }

        IEnumerable<Gate> gates = await _nodeRepository.GetAll<Gate>();
        await _routesCache.Clear();

        foreach (Gate gate in gates)
        {
            Route route = new Route(gate.Id, gate.Method, gate.Route);
            await _routesCache.Create(route);
        }
    }
}
