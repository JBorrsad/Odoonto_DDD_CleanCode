namespace WF.Mimetic.Domain.Models.ApiDocs;

using System.Collections.Generic;
using System.Linq;
using WF.Mimetic.Domain.Interfaces.ApiDocs;
using WF.Mimetic.Domain.Models.ApiDocs.Parameters;
using WF.Mimetic.Domain.Models.Flows;
using WF.Mimetic.Domain.Models.Nodes;

public class FlowDocBuilder : IFlowDocBuilder
{
    public string OpenApiVersion => "3.0.1";
    public string ApiVersion => "1.0";

    public FlowDocBuilder()
    {
    }


    public ApiDoc CreateDoc(string title, IEnumerable<Flow> flows)
    {
        ApiDoc apiDoc = ApiDoc.Create(OpenApiVersion, title, ApiVersion);

        foreach (Flow flow in flows)
        {
            AddFlowInDoc(apiDoc, flow);
        }

        return apiDoc;
    }

    private void AddFlowInDoc(ApiDoc apiDoc, Flow flow)
    {
        IEnumerable<Gate> gates = flow.Nodes.OfType<Gate>().Where(gate => gate.IsActived);

        if (!gates.Any())
        {
            return;
        }

        IEnumerable<Nodes.Response> responses = flow.Nodes.OfType<Nodes.Response>().Where(gate => gate.IsActived);
        IEnumerable<Watchdog> watchdogs = flow.Nodes.OfType<Watchdog>().Where(gate => gate.IsActived);

        foreach (Gate gate in gates)
        {
            if (!apiDoc.Paths.ContainsKey(gate.Route))
            {
                apiDoc.AddPath(Gate.RouteRoot + gate.Route, Path.Create());
            }

            Path path = apiDoc.Paths[Gate.RouteRoot + gate.Route];
            Method method = CreateMethod(gate, responses, watchdogs);
            path.AddMethod(gate.Method.ToString().ToLower(), method);
        }
    }

    private Method CreateMethod(Gate gate, IEnumerable<Nodes.Response> flowResponses, IEnumerable<Watchdog> watchdogs)
    {
        string tag = GetGateTag(gate);

        Method method = Method.Create();
        method.AddTag(tag);

        IEnumerable<Parameter> parameters = GetGateParameters(gate);

        foreach (Parameter parameter in parameters)
        {
            method.AddParameter(parameter);
        }

        IEnumerable<KeyValuePair<string, Response>> responses = GetResponses(flowResponses, watchdogs);

        foreach (KeyValuePair<string, Response> response in responses)
        {
            method.AddResponse(response.Key, response.Value);
        }

        return method;
    }

    private string GetGateTag(Gate gate)
    {
        string[] tags = gate.Route.Split('/');

        return tags.FirstOrDefault(tag => !string.IsNullOrWhiteSpace(tag), "default");
    }

    private IEnumerable<Parameter> GetGateParameters(Gate gate)
    {
        IList<Parameter> parameters = new List<Parameter>();
        string[] pathParameters = gate.Route.Split('/')
            .Where(p => p.StartsWith("{") && p.EndsWith("}"))
            .Select(p => p.TrimStart('{').TrimEnd('}'))
            .ToArray();

        foreach (string pathParameter in pathParameters)
        {
            PathParameter parameter = PathParameter.Create(pathParameter, typeof(string));
            parameters.Add(parameter);
        }

        return parameters;
    }

    private IEnumerable<KeyValuePair<string, Response>> GetResponses(IEnumerable<Nodes.Response> flowResponses, IEnumerable<Watchdog> watchdogs)
    {
        IList<KeyValuePair<string, Response>> responses = new List<KeyValuePair<string, Response>>();

        foreach (Watchdog watchdog in watchdogs)
        {
            Response response = Response.Create().SetDescription(watchdog.Name);
            responses.Add(new KeyValuePair<string, Response>(watchdog.StatusCode.ToString(), response));
        }

        foreach (Nodes.Response flowResponse in flowResponses)
        {
            Response response = Response.Create().SetDescription(flowResponse.Name);
            responses.Add(new KeyValuePair<string, Response>(flowResponse.StatusCode.ToString(), response));
        }

        return responses;
    }
}
