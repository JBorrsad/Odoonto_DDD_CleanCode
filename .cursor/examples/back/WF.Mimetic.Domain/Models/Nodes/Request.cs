using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WF.Mimetic.Domain.Core.Models.Exceptions;
using WF.Mimetic.Domain.Interfaces.Rules;
using WF.Mimetic.Domain.Models.Flows;
using WF.Mimetic.Domain.Shared;

namespace WF.Mimetic.Domain.Models.Nodes;

public class Request : Node
{
    public static Request Default => new Request();

    public string Route { get; private set; }
    public string MediaType { get; private set; }
    public string Body { get; private set; }
    public Method Method { get; private set; }

    public override NodeType Type => NodeType.Request;

    private Request() : base(Guid.Empty, Guid.Empty)
    {
        Route = null;
        Body = null;
        Method = Method.None;
        MediaType = "text/plain";
    }

    private Request(Guid id, Guid flowId) : base(id, flowId)
    {
        Route = null;
        Body = null;
        Method = Method.None;
        MediaType = "text/plain";
    }

    internal static Request Create(Guid id, Flow flow)
    {
        if (Guid.Empty.Equals(id))
        {
            throw new InvalidValueException("The gate id cant be empty.");
        }

        if (flow is null)
        {
            throw new WrongOperationException("The flow cant be null.");
        }

        return new Request(id, flow.Id);
    }

    public void SetMediaType(string mediaType)
    {
        if (string.IsNullOrEmpty(mediaType))
        {
            mediaType = "text/plain";
        }

        MediaType = mediaType;
    }

    public void SetRoute(string route)
    {
        if (!RouteValidator.ValidateRoute(route))
        {
            throw new InvalidValueException("The route is not valid.");
        }

        Route = route;
    }

    public void SetBody(string body)
    {
        Body = body;
    }

    public void SetMethod(Method method)
    {
        if (Method.None.Equals(method))
        {
            throw new InvalidValueException("The gate method is not valid.");
        }

        Method = method;
    }

    public override async Task<string> Run(IRulesEngine rulesEngine, string msg)
    {
        try
        {
            if (msg is null)
            {
                throw new WrongOperationException("The msg cant be null.");
            }
            JObject msgJObject = Moustacher.GetJObjectFromJson(msg)?["msg"] as JObject;
            JObject pipeVariables = (msgJObject?["pipeVariables"] as JObject) ?? new JObject();

            HttpResponseMessage httpResponse = await RunHttpRequest(msg);
            string content = await httpResponse.Content.ReadAsStringAsync();

            IDictionary<string, string> headers = new Dictionary<string, string>();

            foreach (var header in httpResponse.Content.Headers)
            {
                headers.Add(header.Key, string.Join(';', header.Value));
            }

            int statusCode = (int)httpResponse.StatusCode;
            object response = ReadContent(content, headers);

            return Moustacher.SetObjectToJson(new { pipeVariables, response, headers, statusCode }, "msg", msg);
        }
        catch (HttpRequestException ex)
        {
            throw new BadGatewayException($"The node {Name} (Id: {Id}) could not send request.", ex);
        }
    }

    private static object ReadContent(string content, IDictionary<string, string> headers)
    {
        if (!headers.ContainsKey("Content-Type"))
        {
            return content;
        }

        if (!headers["Content-Type"].Contains("application/json"))
        {
            return content;
        }

        return JsonConvert.DeserializeObject(content);
    }

    private Task<HttpResponseMessage> RunHttpRequest(string msg)
    {
        return Method switch
        {
            Method.Get => RunGetRequest(msg),
            Method.Post => RunPostRequest(msg),
            Method.Put => RunPutRequest(msg),
            Method.Patch => RunPatchRequest(msg),
            Method.Delete => RunDeleteRequest(msg),
            _ => throw new InvalidValueException("The request method is not valid.")
        };
    }

    private async Task<HttpResponseMessage> RunGetRequest(string msg)
    {
        using (HttpClient client = new HttpClient())
        {
            string route = PrintByTemplate(msg, Route);

            return await client.GetAsync(route);
        }
    }

    private async Task<HttpResponseMessage> RunPostRequest(string msg)
    {
        using (HttpClient client = new HttpClient())
        {
            string route = PrintByTemplate(msg, Route);
            string body = PrintByTemplate(msg, Body);

            StringContent content = new StringContent(body, Encoding.UTF8, MediaType);
            return await client.PostAsync(route, content);
        }
    }

    private async Task<HttpResponseMessage> RunPutRequest(string msg)
    {
        using (HttpClient client = new HttpClient())
        {
            string route = PrintByTemplate(msg, Route);
            string body = PrintByTemplate(msg, Body);

            StringContent content = new StringContent(body, Encoding.UTF8, MediaType);
            return await client.PutAsync(route, content);
        }
    }

    private async Task<HttpResponseMessage> RunPatchRequest(string msg)
    {
        using (HttpClient client = new HttpClient())
        {
            string route = PrintByTemplate(msg, Route);
            string body = PrintByTemplate(msg, Body);

            StringContent content = new StringContent(body, Encoding.UTF8, MediaType);
            return await client.PatchAsync(route, content);
        }
    }

    private async Task<HttpResponseMessage> RunDeleteRequest(string msg)
    {
        using (HttpClient client = new HttpClient())
        {
            string route = PrintByTemplate(msg, Route);

            return await client.DeleteAsync(route);
        }
    }

    private static string PrintByTemplate(string msg, string template)
    {
        if (string.IsNullOrWhiteSpace(template))
        {
            throw new WrongOperationException("The route is not setted.");
        }

        return Moustacher.PrintFromJson(msg, template);
    }
}
