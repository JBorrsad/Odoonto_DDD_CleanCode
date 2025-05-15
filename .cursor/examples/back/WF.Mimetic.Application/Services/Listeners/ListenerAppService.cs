namespace WF.Mimetic.Application.Services.Listeners;

using global::AutoMapper;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WF.Mimetic.Application.DTO.Flows.FlowResults;
using WF.Mimetic.Application.Interfaces.Listeners;
using WF.Mimetic.Domain.Core.Models.Exceptions;
using WF.Mimetic.Domain.Interfaces.Routes;
using WF.Mimetic.Domain.Interfaces.Rules;
using WF.Mimetic.Domain.Models.Flows;
using WF.Mimetic.Domain.Models.Nodes;
using WF.Mimetic.Domain.Repositories.Flows;

public class ListenerAppService : IListenerAppService
{
    private readonly IConfiguration _configuration;
    private readonly IPipelineRepository _pipelineRepository;
    private readonly IRouteExplorer _routesExplorer;
    private readonly IRulesEngine _rulesEngine;
    private readonly IMapper _mapper;
    private readonly bool _executePrivatePipeline;

    public ListenerAppService(IPipelineRepository pipelineRepository
        , IConfiguration configuration
        , IRouteExplorer routesExplorer
        , IRulesEngine rulesEngine
        , IMapper mapper)
    {
        _configuration = configuration;
        _pipelineRepository = pipelineRepository;
        _routesExplorer = routesExplorer;
        _rulesEngine = rulesEngine;
        _mapper = mapper;
        bool.TryParse(_configuration.GetSection("ControlAccess:ExecutePrivatePipeline").Value, out _executePrivatePipeline);
    }

    public Task<FlowResultReadDto> ListenGet(IDictionary<string, string> query, params string[] segments)
    {
        return RunListenner(Method.Get, query, segments);
    }

    public Task<FlowResultReadDto> ListenPost(string body, IDictionary<string, string> query, params string[] segments)
    {
        return RunListenner(Method.Post, body, query, segments);
    }

    public Task<FlowResultReadDto> ListenPut(string body, IDictionary<string, string> query, params string[] segments)
    {
        return RunListenner(Method.Put, body, query, segments);
    }

    public Task<FlowResultReadDto> ListenDelete(IDictionary<string, string> query, params string[] segments)
    {
        return RunListenner(Method.Delete, query, segments);
    }

    private Task<FlowResultReadDto> RunListenner(Method method, IDictionary<string, string> query, string[] segments)
    {
        return RunListenner(method, "", query, segments);
    }

    private async Task<FlowResultReadDto> RunListenner(Method method, string body, IDictionary<string, string> query, string[] segments)
    {
        string route = CreateRute(segments);
        Gate gate = await _routesExplorer.MatchOrThrow(route, method);
        IDictionary<string, string> routeParameters = GetRouteParameters(gate.Route, route);
        Pipeline pipelines = await _pipelineRepository.GetByIdOrThrow(gate.FlowId);

        if (!pipelines.IsPublic && !_executePrivatePipeline) 
        {
            throw new NotAllowedOperationException("Getaway not found");
        }

        string parsedBody = body.Trim('\n', '\t', ' ');
        object msgObject = new { msg = parsedBody, route = routeParameters, query = query, body = parsedBody };
        string msg = JsonConvert.SerializeObject(msgObject);

        FlowResult result = await pipelines.Run(_rulesEngine, msg);
        return _mapper.Map<FlowResult, FlowResultReadDto>(result);
    }

    private static string CreateRute(params string[] segments)
    {
        return segments.Aggregate("", (accumulate, p) => accumulate + "/" + p);
    }

    private IDictionary<string, string> GetRouteParameters(string template, string route)
    {

        RouteTemplate routeTemplate = TemplateParser.Parse(_routesExplorer.ConvertTemplateFormat(template));
        TemplateMatcher templateMatcher = new TemplateMatcher(routeTemplate, null);
        RouteValueDictionary values = new RouteValueDictionary();

        if (!templateMatcher.TryMatch(route, values))
        {
            throw new WrongOperationException("The route template is not correct.");
        }

        IEnumerable<KeyValuePair<string, string>> keyValues = values.Select(value => new KeyValuePair<string, string>(value.Key, value.Value.ToString()));
        return new Dictionary<string, string>(keyValues);
    }
}
